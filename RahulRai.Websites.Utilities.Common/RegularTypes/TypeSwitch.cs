// ***********************************************************************
// Assembly         : RahulRai.Websites.Utilities.Common
// Author           : rahulrai
// Created          : 04-16-2015
//
// Last Modified By : rahulrai
// Last Modified On : 06-24-2015
// ***********************************************************************
// <copyright file="TypeSwitch.cs" company="Rahul Rai">
//     Copyright (c) Rahul Rai. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace RahulRai.Websites.Utilities.Common.RegularTypes
{
    #region

    using System;
    using System.Collections.Generic;

    #endregion

    /// <summary>
    /// The type switch.
    /// </summary>
    public class TypeSwitch
    {
        #region Fields

        /// <summary>
        /// The matches.
        /// </summary>
        private readonly Dictionary<Type, Action> matches = new Dictionary<Type, Action>();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The case.
        /// </summary>
        /// <typeparam name="T">Type of input</typeparam>
        /// <param name="action">The action.</param>
        /// <returns>The <see cref="TypeSwitch" />.</returns>
        public TypeSwitch Case<T>(Action action)
        {
            this.matches.Add(typeof(T), action);
            return this;
        }

        /// <summary>
        /// The switch.
        /// </summary>
        /// <param name="value">The target.</param>
        public void Switch(Type value)
        {
            this.matches[value].Invoke();
        }

        #endregion
    }
}