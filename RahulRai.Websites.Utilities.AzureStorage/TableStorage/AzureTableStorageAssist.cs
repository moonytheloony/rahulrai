﻿// ***********************************************************************
// Assembly         : RahulRai.Websites.Utilities.AzureStorage
// Author           : rahulrai
// Created          : 04-15-2015
//
// Last Modified By : rahulrai
// Last Modified On : 06-24-2015
// ***********************************************************************
// <copyright file="AzureTableStorageAssist.cs" company="Rahul Rai">
//     Copyright (c) Rahul Rai. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace RahulRai.Websites.Utilities.AzureStorage.TableStorage
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;
    using Common.Exceptions;
    using Common.Helpers;
    using Common.RegularTypes;
    using Microsoft.WindowsAzure.Storage.Table;

    #endregion

    #region

    #endregion

    /// <summary>
    ///     The azure table storage assist.
    /// </summary>
    public static class AzureTableStorageAssist
    {
        #region Public Methods and Operators

        /// <summary>
        ///     The convert dynamic entity to entity.
        /// </summary>
        /// <typeparam name="TEntity">Type of target object.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns>The <see cref="TEntity" />.</returns>
        /// <exception cref="InputValidationFailedException">Input is not valid.</exception>
        /// <exception cref="RahulRai.Websites.Utilities.Common.Exceptions.InputValidationFailedException">
        ///     Count of properties with id postfix is not one
        ///     or
        ///     Count of properties with key postfix is not one
        ///     or
        ///     Count of properties with entity tag name is not one
        /// </exception>
        public static TEntity ConvertDynamicEntityToEntity<TEntity>(this DynamicTableEntity entity)
        {
            var targetObject = (TEntity)Activator.CreateInstance(typeof(TEntity));
            var targetObjectType = targetObject.GetType();
            var objectProperties = targetObjectType.GetProperties().ToList();

            //// There should not be more than one property with ID suffix
            if (objectProperties.Count(element => element.Name.EndsWith("rowkey", StringComparison.OrdinalIgnoreCase)) != 1)
            {
                throw new InputValidationFailedException("Count of properties with id postfix is not one");
            }

            //// There should not be more than one property with key suffix
            if (objectProperties.Count(element => element.Name.EndsWith("partitionkey", StringComparison.OrdinalIgnoreCase)) != 1)
            {
                throw new InputValidationFailedException("Count of properties with key postfix is not one");
            }

            //// There should be one property with name entity tag
            if (objectProperties.Count(element => element.Name.CompareCaseInvariant("entitytag")) != 1)
            {
                throw new InputValidationFailedException("Count of properties with entity tag name is not one");
            }

            //// Populate key fields.
            foreach (var propertyInfo in objectProperties)
            {
                var propertyName = propertyInfo.Name;

                //// Set Id property as row key
                if (propertyName.EndsWith("rowkey", StringComparison.OrdinalIgnoreCase))
                {
                    propertyInfo.SetValue(targetObject, entity.RowKey);
                }
                else if (propertyName.EndsWith("partitionkey", StringComparison.OrdinalIgnoreCase))
                {
                    propertyInfo.SetValue(targetObject, entity.PartitionKey);
                }
                else if (propertyName.CompareCaseInvariant("entitytag"))
                {
                    propertyInfo.SetValue(targetObject, entity.ETag);
                }
            }

            var listElementName = string.Empty;
            SortedList<int, string> sortedList = null;
            foreach (var property in entity.Properties)
            {
                //// Handle special case of auto generated string list. Known Issue: Can handle classes with one list only.
                if (property.Key.StartsWith(ApplicationConstants.IndexSubscript, StringComparison.OrdinalIgnoreCase))
                {
                    var splitString = property.Key.Split(
                        new[] { KnownTypes.Underscore },
                        StringSplitOptions.RemoveEmptyEntries);
                    if (null == sortedList)
                    {
                        sortedList = new SortedList<int, string>();
                        listElementName = splitString[2];
                    }

                    sortedList.Add(
                        Convert.ToInt32(splitString[1], CultureInfo.InvariantCulture),
                        property.Value.StringValue);
                }
                else
                {
                    var propertyInfo =
                        objectProperties.First(
                            elementProperty => elementProperty.Name.CompareCaseInvariant(property.Key));
                    PopulateEntityPropertyWithDynamicEntityValue(entity, propertyInfo, targetObject, property.Key);
                }
            }

            if (null != sortedList && !string.IsNullOrWhiteSpace(listElementName))
            {
                //// Get sorted values
                var elementList = sortedList.Values;

                //// Apply these values to element
                var propertyInfo =
                    objectProperties.First(
                        elementProperty => elementProperty.Name.CompareCaseInvariant(listElementName));
                propertyInfo.SetValue(targetObject, elementList);
            }

            return targetObject;
        }

        /// <summary>
        ///     The convert entity to dynamic table entity.
        /// </summary>
        /// <typeparam name="TEntity">Type of entity to convert.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns>The <see cref="DynamicTableEntity" />.</returns>
        /// <exception cref="InputValidationFailedException">Input could not be validated</exception>
        /// <exception cref="RahulRai.Websites.Utilities.Common.Exceptions.InputValidationFailedException">
        ///     Count of properties with id postfix is not one
        ///     or
        ///     Count of properties with key postfix is not one
        ///     or
        ///     Count of properties with entity tag name is not one
        ///     or
        ///     dynamic entity
        /// </exception>
        public static DynamicTableEntity ConvertEntityToDynamicTableEntity<TEntity>(this TEntity entity)
        {
            var dynamicEntity = new DynamicTableEntity();
            var sourceEntity = entity.GetType();
            dynamicEntity.Properties = new Dictionary<string, EntityProperty>();
            var objectProperties = sourceEntity.GetProperties().ToList();

            //// There should not be more than one property with ID suffix
            if (objectProperties.Count(element => element.Name.EndsWith("rowkey", StringComparison.OrdinalIgnoreCase)) != 1)
            {
                throw new InputValidationFailedException("Count of properties with id postfix is not one");
            }

            //// There should not be more than one property with key suffix
            if (objectProperties.Count(element => element.Name.EndsWith("partitionkey", StringComparison.OrdinalIgnoreCase)) != 1)
            {
                throw new InputValidationFailedException("Count of properties with key postfix is not one");
            }

            //// There should be one property with name entity tag
            if (objectProperties.Count(element => element.Name.CompareCaseInvariant("entitytag")) != 1)
            {
                throw new InputValidationFailedException("Count of properties with entity tag name is not one");
            }

            FillDynamicEntityPropertyBag(entity, ref dynamicEntity, objectProperties);

            if (dynamicEntity.ETag == null || dynamicEntity.RowKey == null || dynamicEntity.PartitionKey == null)
            {
                throw new InputValidationFailedException("dynamic entity");
            }

            return dynamicEntity;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     The fill dynamic entity property bag.
        /// </summary>
        /// <typeparam name="TEntity">T Entity</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="dynamicEntity">The dynamic entity.</param>
        /// <param name="objectProperties">The object properties.</param>
        private static void FillDynamicEntityPropertyBag<TEntity>(
            this TEntity entity,
            ref DynamicTableEntity dynamicEntity,
            IEnumerable<PropertyInfo> objectProperties)
        {
            foreach (var propertyInfo in objectProperties)
            {
                if (propertyInfo.GetCustomAttribute<IgnoreDataMemberAttribute>() != null)
                {
                    continue;
                }

                var propertyName = propertyInfo.Name;
                var propertyType = propertyInfo.PropertyType;
                dynamic propertyValue = propertyInfo.GetValue(entity);
                if (null == propertyValue && propertyType.Name.CompareCaseInvariant("String"))
                {
                    propertyValue = string.Empty;
                }

                if (null == propertyValue
                    || (propertyType.Name.CompareCaseInvariant("DateTime") && propertyValue == DateTime.MinValue))
                {
                    continue;
                }

                if (propertyName.EndsWith("rowkey", StringComparison.OrdinalIgnoreCase))
                {
                    dynamicEntity.RowKey = propertyValue;
                }
                else if (propertyName.EndsWith("partitionkey", StringComparison.OrdinalIgnoreCase))
                {
                    dynamicEntity.PartitionKey = propertyValue;
                }
                else if (propertyName.CompareCaseInvariant("entitytag"))
                {
                    dynamicEntity.ETag = string.IsNullOrWhiteSpace(propertyValue)
                        ? KnownTypes.MatchAny.ToString(CultureInfo.InvariantCulture)
                        : propertyValue;
                }
                else if (propertyType == typeof(IList<string>))
                {
                    //// Special Case: If input is list of string then spread this list to multiple properties.
                    var dynamicEntities = propertyValue as List<string>;
                    if (null == dynamicEntities)
                    {
                        continue;
                    }

                    for (var index = 0; index < dynamicEntities.Count; index++)
                    {
                        var formattedProperty = Routines.FormatStringInvariantCulture(
                            "{0}_{1}_{2}",
                            ApplicationConstants.IndexSubscript,
                            index,
                            propertyName);
                        dynamicEntity.Properties.Add(formattedProperty, new EntityProperty(dynamicEntities[index]));
                    }
                }
                else
                {
                    dynamicEntity.Properties.Add(
                        propertyName,
                        new EntityProperty(Convert.ChangeType(propertyValue, propertyType)));
                }
            }
        }

        /// <summary>
        ///     The populate entity property with dynamic entity value.
        /// </summary>
        /// <typeparam name="TEntity">Entity type of element.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="propertyInfo">The property info.</param>
        /// <param name="targetObject">The target object.</param>
        /// <param name="propertyName">The property name.</param>
        private static void PopulateEntityPropertyWithDynamicEntityValue<TEntity>(
            DynamicTableEntity entity,
            PropertyInfo propertyInfo,
            TEntity targetObject,
            string propertyName)
        {
            var typeSwitch =
                new TypeSwitch().Case<int>(
                    () => propertyInfo.SetValue(targetObject, entity.Properties[propertyName].Int32Value))
                    .Case<long>(() => propertyInfo.SetValue(targetObject, entity.Properties[propertyName].Int64Value))
                    .Case<string>(
                        () => propertyInfo.SetValue(targetObject, entity.Properties[propertyName].StringValue))
                    .Case<byte[]>(
                        () => propertyInfo.SetValue(targetObject, entity.Properties[propertyName].BinaryValue))
                    .Case<bool>(() => propertyInfo.SetValue(targetObject, entity.Properties[propertyName].BooleanValue))
                    .Case<DateTime>(() => propertyInfo.SetValue(targetObject, entity.Properties[propertyName].DateTime))
                    .Case<Guid>(() => propertyInfo.SetValue(targetObject, entity.Properties[propertyName].GuidValue))
                    .Case<object>(
                        () => propertyInfo.SetValue(targetObject, entity.Properties[propertyName].PropertyAsObject));

            typeSwitch.Switch(propertyInfo.PropertyType);
        }

        #endregion
    }
}