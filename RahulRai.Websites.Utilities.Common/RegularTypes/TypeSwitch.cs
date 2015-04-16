namespace RahulRai.Websites.Utilities.Common.RegularTypes
{
    #region

    using System;
    using System.Collections.Generic;

    #endregion

    /// <summary>
    ///     The type switch.
    /// </summary>
    public class TypeSwitch
    {
        #region Fields

        /// <summary>
        ///     The matches.
        /// </summary>
        private readonly Dictionary<Type, Action> matches = new Dictionary<Type, Action>();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     The case.
        /// </summary>
        /// <param name="action">
        ///     The action.
        /// </param>
        /// <typeparam name="T">
        ///     Type of input
        /// </typeparam>
        /// <returns>
        ///     The <see cref="TypeSwitch" />.
        /// </returns>
        public TypeSwitch Case<T>(Action action)
        {
            matches.Add(typeof (T), action);
            return this;
        }

        /// <summary>
        ///     The switch.
        /// </summary>
        /// <param name="value">
        ///     The target.
        /// </param>
        public void Switch(Type value)
        {
            matches[value].Invoke();
        }

        #endregion
    }
}