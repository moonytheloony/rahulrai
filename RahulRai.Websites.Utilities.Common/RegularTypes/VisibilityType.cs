// ***********************************************************************
// Assembly         : RahulRai.Websites.Utilities.Common
// Author           : rahulrai
// Created          : 04-16-2015
//
// Last Modified By : rahulrai
// Last Modified On : 06-24-2015
// ***********************************************************************
// <copyright file="VisibilityType.cs" company="Rahul Rai">
//     Copyright (c) Rahul Rai. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace RahulRai.Websites.Utilities.Common.RegularTypes
{
    /// <summary>
    ///     The visibility type.
    /// </summary>
    public enum VisibilityType
    {
        /// <summary>
        ///     The files visible to all.
        /// </summary>
        FilesVisibleToAll,

        /// <summary>
        ///     The folder visible to all.
        /// </summary>
        FolderVisibleToAll,

        /// <summary>
        ///     The visible to owner only.
        /// </summary>
        VisibleToOwnerOnly
    }
}