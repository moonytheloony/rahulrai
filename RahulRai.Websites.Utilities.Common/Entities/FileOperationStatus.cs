// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileOperationStatus.cs" company="Microsoft">
//   Copyright (c) Glasgow City Council. All Rights Reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

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
        /// The file updated with new version.
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
        /// The copy completed.
        /// </summary>
        CopyCompleted, 

        /// <summary>
        /// The time out.
        /// </summary>
        Timeout
    }
}