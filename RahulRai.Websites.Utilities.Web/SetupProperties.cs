// ***********************************************************************
// Assembly         : RahulRai.Websites.Utilities.Web
// Author           : rahulrai
// Created          : 06-24-2015
//
// Last Modified By : rahulrai
// Last Modified On : 06-24-2015
// ***********************************************************************
// <copyright file="SetupProperties.cs" company="Rahul Rai">
//     Copyright (c) Rahul Rai. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace RahulRai.Websites.Utilities.Web
{
    /// <summary>
    ///     Class SetupProperties.
    /// </summary>
    public class SetupProperties : BaseController
    {
        /// <summary>
        ///     Initializes the properties.
        /// </summary>
        /// <returns>dynamic.</returns>
        public dynamic InitializeProperties()
        {
            this.ViewBag.Title = "Rahul on Technology and Things";
            return this.ViewBag;
        }
    }
}