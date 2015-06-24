// ***********************************************************************
// Assembly         : RahulRai.Websites.Utilities.Common
// Author           : rahulrai
// Created          : 04-15-2015
//
// Last Modified By : rahulrai
// Last Modified On : 06-24-2015
// ***********************************************************************
// <copyright file="Routines.cs" company="Rahul Rai">
//     Copyright (c) Rahul Rai. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace RahulRai.Websites.Utilities.Common.Helpers
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;
    using RegularTypes;

    #endregion

    /// <summary>
    /// Repository of regular utility functions.
    /// </summary>
    public static class Routines
    {
        #region Public Methods and Operators

        /// <summary>
        /// The compare case invariant.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="newValue">The string to compare to.</param>
        /// <returns>The <see cref="bool" />.</returns>
        public static bool CompareCaseInvariant(this string value, string newValue)
        {
            return value.Equals(newValue, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// The format string invariant culture.
        /// </summary>
        /// <param name="value">The invoking string.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns>The <see cref="string" />.</returns>
        public static string FormatStringInvariantCulture(string value, params object[] arguments)
        {
            return string.Format(CultureInfo.InvariantCulture, value, arguments);
        }

        /// <summary>
        /// Strips the HTML formatting.
        /// </summary>
        /// <param name="htmlString">The HTML string.</param>
        /// <returns>System.String.</returns>
        public static string StripHtmlFormatting(string htmlString)
        {
            var noHtml = Regex.Replace(htmlString, @"<[^>]+>|&nbsp;", string.Empty).Trim();
            return Regex.Replace(noHtml, @"\s{2,}", " ");
        }

        /// <summary>
        /// The to collection.
        /// </summary>
        /// <param name="value">The CSV string.</param>
        /// <returns>The System.Collections.Generic.IEnumerable.System.String</returns>
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
        /// <param name="collection">The collection.</param>
        /// <returns>The System.String.</returns>
        public static string ToCsv(this IEnumerable<string> collection)
        {
            return string.Join(KnownTypes.CsvSeparator.ToString(CultureInfo.InvariantCulture), collection);
        }

        /// <summary>
        /// The to invariant culture string.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>The <see cref="string" />.</returns>
        public static string ToInvariantCultureString(this int input)
        {
            return input.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// To the invariant culture string.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>System.String.</returns>
        public static string ToInvariantCultureString(this char input)
        {
            return input.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// The split by length.
        /// </summary>
        /// <param name="value">The source string.</param>
        /// <param name="maxLength">The max length.</param>
        /// <returns>The <see cref="IEnumerable" />.</returns>
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
        /// <param name="value">The value.</param>
        /// <returns>The <see cref="string" />.</returns>
        public static string Combine(this IList<string> value)
        {
            return string.Join(string.Empty, value);
        }

        #endregion

        /// <summary>
        /// Formats the title.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <returns>System.String.</returns>
        public static string FormatTitle(string title)
        {
            var cleanString = Regex.Replace(title, @"[^\w\.@-]", KnownTypes.HyphenSeparator, RegexOptions.None);
            return cleanString;
        }
    }
}