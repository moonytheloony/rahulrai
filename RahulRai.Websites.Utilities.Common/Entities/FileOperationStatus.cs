// ***********************************************************************
// Assembly         : RahulRai.Websites.Utilities.Common
// Author           : rahulrai
// Created          : 04-15-2015
//
// Last Modified By : rahulrai
// Last Modified On : 06-24-2015
// ***********************************************************************
// <copyright file="FileOperationStatus.cs" company="Rahul Rai">
//     Copyright (c) Rahul Rai. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace RahulRai.Websites.Utilities.Common.Entities
{
    /// <summary>
    ///     The file operation status.
    /// </summary>
    public enum FileOperationStatus
    {
        /// <summary>
        ///     The folder created.
        /// </summary>
        FolderCreated,

        /// <summary>
        ///     The error.
        /// </summary>
        Error,

        /// <summary>
        ///     The folder deleted.
        /// </summary>
        FolderDeleted,

        /// <summary>
        ///     The file created or updated.
        /// </summary>
        FileCreatedOrUpdated,

        /// <summary>
        ///     The file updated with new version.
        /// </summary>
        FileUpdatedWithNewVersion,

        /// <summary>
        ///     The file deleted.
        /// </summary>
        FileDeleted,

        /// <summary>
        ///     The initiated copy operation.
        /// </summary>
        InitiatedCopyOperation,

        /// <summary>
        ///     The copy completed.
        /// </summary>
        CopyCompleted,

        /// <summary>
        ///     The time out.
        /// </summary>
        Timeout
    }
}