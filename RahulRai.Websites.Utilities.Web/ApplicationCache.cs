// ***********************************************************************
// Assembly         : RahulRai.Websites.Utilities.Web
// Author           : rahulrai
// Created          : 07-15-2015
//
// Last Modified By : rahulrai
// Last Modified On : 07-15-2015
// ***********************************************************************
// <copyright file="ApplicationCache.cs" company="Rahul Rai">
//     Copyright (c) Rahul Rai. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace RahulRai.Websites.Utilities.Web
{
    #region

    using System;
    using System.Configuration;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Web.Configuration;
    using Common.RegularTypes;
    using StackExchange.Redis;

    #endregion

    /// <summary>
    /// Applicationcache class.
    /// </summary>
    public static class ApplicationCache
    {
        /// <summary>
        /// The cache
        /// </summary>
        private static readonly IDatabase Cache;

        /// <summary>
        /// The eviction time
        /// </summary>
        private static readonly TimeSpan EvictionTime;

        /// <summary>
        /// Initializes static members of the <see cref="ApplicationCache"/> class.
        /// </summary>
        static ApplicationCache()
        {
            var connection =
                ConnectionMultiplexer.Connect(
                string.Format(
                "{0},ssl=true,password={1}",
                WebConfigurationManager.AppSettings[ApplicationConstants.SessionStateHost],
                WebConfigurationManager.AppSettings[ApplicationConstants.SessionStateKey]));
            Cache = connection.GetDatabase();
            EvictionTime = TimeSpan.FromMinutes(ApplicationConstants.CacheEvictionMinutes);
        }

        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">The key.</param>
        /// <returns>Cache object</returns>
        public static T Get<T>(string key)
        {
            return Deserialize<T>(Cache.StringGet(key));
        }

        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Desirialized object</returns>
        public static object Get(string key)
        {
            return Deserialize<object>(Cache.StringGet(key));
        }

        /// <summary>
        /// Sets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public static void Set(string key, object value)
        {
            Cache.StringSet(key, Serialize(value), EvictionTime);
        }

        /// <summary>
        /// Removes a key from cache
        /// </summary>
        /// <param name="key">Key to remove.</param>
        public static void Remove(string key)
        {
            Cache.KeyDelete(key);
        }

        /// <summary>
        /// Serializes the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>Serialized object</returns>
        private static byte[] Serialize(object obj)
        {
            if (obj == null)
            {
                return null;
            }

            var binaryFormatter = new BinaryFormatter();
            using (var memoryStream = new MemoryStream())
            {
                binaryFormatter.Serialize(memoryStream, obj);
                var objectDataAsStream = memoryStream.ToArray();
                return objectDataAsStream;
            }
        }

        /// <summary>
        /// Deserializes the specified stream.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="stream">The stream.</param>
        /// <returns>Deserialized object</returns>
        private static T Deserialize<T>(byte[] stream)
        {
            if (stream == null)
            {
                return default(T);
            }

            var binaryFormatter = new BinaryFormatter();
            using (var memoryStream = new MemoryStream(stream))
            {
                var result = (T)binaryFormatter.Deserialize(memoryStream);
                return result;
            }
        }
    }
}