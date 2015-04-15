// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Routines.cs" company="Microsoft">
//   Copyright (c) Glasgow City Council. All Rights Reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace RahulRai.Websites.Utilities.Common.Helpers
{
    #region

    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization;
    using System.Security;
    using System.Security.Claims;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Web.Script.Serialization;
    using System.Xml.Linq;
    using Exceptions;
    using RegularTypes;

    #endregion

    /// <summary>
    ///     Repository of regular utility functions.
    /// </summary>
    public static class Routines
    {
       #region Public Methods and Operators

        /// <summary>
        /// The add to CSV.
        /// </summary>
        /// <param name="value">
        /// The CSV string.
        /// </param>
        /// <param name="collection">
        /// The collection.
        /// </param>
        /// <returns>
        /// The System.String.
        /// </returns>
        public static string AddToCsv(this string value, IEnumerable<string> collection)
        {
            var cleanCsv = string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim();
            return FormatStringInvariantCulture(
                string.IsNullOrEmpty(cleanCsv) ? "{2}" : "{0}{1}{2}",
                cleanCsv,
                KnownTypes.CsvSeparator,
                collection.ToCsv());
        }

        /// <summary>
        /// The add to CSV.
        /// </summary>
        /// <param name="baseCollection">
        /// The base collection.
        /// </param>
        /// <param name="collectionToAdd">
        /// The collection to add.
        /// </param>
        /// <returns>
        /// The System.String.
        /// </returns>
        public static string AddToCsv(this IEnumerable<string> baseCollection, IEnumerable<string> collectionToAdd)
        {
            return FormatStringInvariantCulture(
                "{0}{1}{2}",
                baseCollection.ToCsv(),
                KnownTypes.CsvSeparator,
                collectionToAdd.ToCsv());
        }

        /// <summary>
        /// The compare case invariant.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="newValue">
        /// The string to compare to.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool CompareCaseInvariant(this string value, string newValue)
        {
            Contract.Requires<InputValidationFailedException>(!string.IsNullOrEmpty(value), "value");
            Contract.Requires<InputValidationFailedException>(!string.IsNullOrEmpty(newValue), "newValue");
            return value.Equals(newValue, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// The create generic.
        /// </summary>
        /// <param name="generic">
        /// The generic.
        /// </param>
        /// <param name="innerType">
        /// The inner type.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        /// <returns>
        /// The System.Object.
        /// </returns>
        public static object CreateGeneric(Type generic, Type innerType, params object[] args)
        {
            if (generic == null)
            {
                return null;
            }

            var specificType = generic.MakeGenericType(new[] { innerType });
            return Activator.CreateInstance(specificType, args);
        }

        /// <summary>
        /// The format string current culture.
        /// </summary>
        /// <param name="value">
        /// The invoking string.
        /// </param>
        /// <param name="arguments">
        /// The arguments.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string FormatStringCurrentCulture(string value, params object[] arguments)
        {
            return string.Format(CultureInfo.CurrentCulture, value, arguments);
        }

        /// <summary>
        /// Formats string to current culture
        /// </summary>
        /// <param name="value">string value to be converted</param>
        /// <returns>string value</returns>
        public static string FormatStringCurrentCulture(string value)
        {
            return string.Format(CultureInfo.CurrentCulture, value);
        }

        /// <summary>
        /// The format string invariant culture.
        /// </summary>
        /// <param name="value">
        /// The invoking string.
        /// </param>
        /// <param name="arguments">
        /// The arguments.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string FormatStringInvariantCulture(string value, params object[] arguments)
        {
            return string.Format(CultureInfo.InvariantCulture, value, arguments);
        }

        /// <summary>
        ///     The get active user name.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public static string GetActiveUserName()
        {
            if (!Thread.CurrentPrincipal.Identity.IsAuthenticated)
            {
                return "Anonymous User";
            }

            //// If this is an AD claim
            if (!(Thread.CurrentPrincipal is ClaimsPrincipal))
            {
                return !string.IsNullOrWhiteSpace(Thread.CurrentPrincipal.Identity.Name)
                           ? Thread.CurrentPrincipal.Identity.Name
                           : "Unknown User";
            }

            var currentPrincipal = Thread.CurrentPrincipal as ClaimsPrincipal;
            var usernameClaim = currentPrincipal.Claims.FirstOrDefault(
                claim => claim.Type.CompareCaseInvariant(KnownTypes.UserNameClaimIdentifier));
            if (usernameClaim != null && !string.IsNullOrWhiteSpace(usernameClaim.Value))
            {
                return usernameClaim.Value;
            }

            return "Unsupported User";
        }

        /// <summary>
        /// Gets the bytes.
        /// </summary>
        /// <typeparam name="T">
        /// object convert
        /// </typeparam>
        /// <param name="objectToConvert">
        /// The object to convert.
        /// </param>
        /// <returns>
        /// Gets the bytes of the passed in object
        /// </returns>
        public static byte[] GetBytes<T>(T objectToConvert)
        {
            var serializer = new DataContractSerializer(typeof(T));
            byte[] returnBytes;

            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, objectToConvert);
                returnBytes = stream.ToArray();
            }

            return returnBytes;
        }

        /// <summary>
        /// Gets the combine URL.
        /// </summary>
        /// <param name="urls">
        /// The URLS.
        /// </param>
        /// <returns>
        /// returns URI
        /// </returns>
        public static Uri GetCombineUrl(params string[] urls)
        {
            if (urls == null || urls.Length <= 0)
            {
                return null;
            }

            var combinedUrl = new StringBuilder();
            const char Separator = '/';
            foreach (var url in urls)
            {
                combinedUrl.Append(url);
                if (!url.EndsWith(Separator.ToString(CultureInfo.InvariantCulture), StringComparison.OrdinalIgnoreCase))
                {
                    combinedUrl.Append(Separator);
                }
            }

            return new Uri(combinedUrl.ToString().TrimEnd(Separator));
        }

        /// <summary>
        /// Converts file size from byte to formatted string.
        /// </summary>
        /// <param name="fileSize">
        /// Size of file in no. of bytes.
        /// </param>
        /// <returns>
        /// Formatted size in string.
        /// </returns>
        public static string GetFormattedFileSize(long fileSize)
        {
            decimal size;
            if (fileSize < 1024)
            {
                size = fileSize;
                return size.ToString("0.##", CultureInfo.InvariantCulture) + " Bytes";
            }

            if (fileSize >= 0 && fileSize < KnownTypes.MB)
            {
                size = (decimal)fileSize / KnownTypes.KB;
                return size.ToString("0.##", CultureInfo.InvariantCulture) + " KB";
            }

            if (fileSize > KnownTypes.KB && fileSize < KnownTypes.GB)
            {
                size = (decimal)fileSize / KnownTypes.MB;
                return size.ToString("0.##", CultureInfo.InvariantCulture) + " MB";
            }

            size = (decimal)fileSize / KnownTypes.GB;
            return size.ToString("0.##", CultureInfo.InvariantCulture) + " GB";
        }

        /// <summary>
        /// Determines whether the specified input matches the pattern.
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        /// <param name="pattern">
        /// The pattern.
        /// </param>
        /// <returns>
        /// True, if pattern matches, else false
        /// </returns>
        public static bool IsPatternMatch(this string input, string pattern)
        {
            return Regex.IsMatch(input, pattern);
        }

        /// <summary>
        /// Validates the date time.
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        /// <param name="dateTimePattern">
        /// The date time pattern.
        /// </param>
        /// <returns>
        /// True, if pattern matches, else false
        /// </returns>
        public static bool IsValidDateTime(this string input, string dateTimePattern)
        {
            DateTime parsedDateTime;
            return DateTime.TryParseExact(
                input,
                dateTimePattern,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out parsedDateTime);
        }

        /// <summary>
        /// The partition object list.
        /// </summary>
        /// <param name="collectionToSplit">
        /// The collection to split.
        /// </param>
        /// <param name="chunkSize">
        /// The chunk size.
        /// </param>
        /// <typeparam name="T">
        /// Type of list
        /// </typeparam>
        /// <returns>
        /// The
        ///     <see>
        ///         <cref>List</cref>
        ///     </see>
        ///     .
        /// </returns>
        public static IList<List<T>> PartitionObjectList<T>(IList<T> collectionToSplit, int chunkSize)
        {
            return
                collectionToSplit.Select((element, index) => new { Index = index, Value = element })
                    .GroupBy(element => element.Index / chunkSize)
                    .Select(element => element.Select(indexedElement => indexedElement.Value).ToList())
                    .ToList();
        }

        /// <summary>
        /// The strip ns.
        /// </summary>
        /// <param name="root">
        /// The root.
        /// </param>
        /// <returns>
        /// The <see cref="XElement"/>.
        /// </returns>
        public static XElement RemoveNamespaceFromXml(XElement root)
        {
            return root != null
                       ? new XElement(
                             root.Name.LocalName,
                             root.HasElements ? root.Elements().Select(RemoveNamespaceFromXml) : (object)root.Value)
                       : null;
        }

        /// <summary>
        /// The to collection.
        /// </summary>
        /// <param name="value">
        /// The CSV string.
        /// </param>
        /// <returns>
        /// The System.Collections.Generic.IEnumerable.System.String
        /// </returns>
        public static IEnumerable<string> ToCollection(this string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                return value.Split(KnownTypes.CsvSeparator).ToList();
            }

            return null;
        }

        /// <summary>
        /// The to CSV.
        /// </summary>
        /// <param name="collection">
        /// The collection.
        /// </param>
        /// <returns>
        /// The System.String.
        /// </returns>
        public static string ToCsv(this IEnumerable<string> collection)
        {
            return string.Join(KnownTypes.CsvSeparator.ToString(CultureInfo.InvariantCulture), collection);
        }

        /// <summary>
        /// The to invariant culture string.
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string ToInvariantCultureString(this int input)
        {
            return input.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// The to invariant culture string.
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string ToInvariantCultureString(this double input)
        {
            return input.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// The to JSON.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The System.String.
        /// </returns>
        public static string ToJson(object entity)
        {
            var serializer = new JavaScriptSerializer();
            return serializer.Serialize(entity);
        }

        /// <summary>
        /// The secure string to string.
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        /// <returns>
        /// The System.String.
        /// </returns>
        public static string ToValueString(this SecureString input)
        {
            var ptr = Marshal.SecureStringToBSTR(input);
            var insecureString = Marshal.PtrToStringBSTR(ptr);
            Marshal.ZeroFreeBSTR(ptr);
            return insecureString;
        }

        /// <summary>
        /// The split by length.
        /// </summary>
        /// <param name="value">
        /// The source string.
        /// </param>
        /// <param name="maxLength">
        /// The max length.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        public static IEnumerable<string> SplitByLength(this string value, int maxLength)
        {
            for (var index = 0; index < value.Length; index += maxLength)
            {
                yield return value.Substring(index, Math.Min(maxLength, value.Length - index));
            }
        }

        /// <summary>
        /// The combine.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string Combine(this IList<string> value)
        {
            return string.Join(string.Empty, value);
        }

        #endregion
    }
}