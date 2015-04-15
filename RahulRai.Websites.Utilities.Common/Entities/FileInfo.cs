// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileInfo.cs" company="Microsoft">
//   Copyright (c) Glasgow City Council. All Rights Reserved.
// </copyright>
// <summary>
//   The data file info.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace RahulRai.Websites.Utilities.Common.Entities
{
    #region

    using System;
    using System.Collections.Generic;

    #endregion

    /// <summary>
    ///     The file info.
    /// </summary>
    public class FileInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileInfo"/> class.
        /// </summary>
        public FileInfo()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileInfo" /> class.
        /// </summary>
        /// <param name="fileMetadata">The file metadata.</param>
        public FileInfo(Dictionary<string, string> fileMetadata)
        {
            this.FileMetadata = fileMetadata;
        }
        
        #region Public Properties

        /// <summary>
        ///     Gets or sets the data set id.
        /// </summary>
        public Guid DataSetId { get; set; }

        /// <summary>
        /// Gets or sets the file set metadata.
        /// </summary>
        public Dictionary<string, string> FileMetadata { get; set; }

        /// <summary>
        ///     Gets or sets the status.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [is active].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is active]; otherwise, <c>false</c>.
        /// </value>
        public bool IsActive { get; set; }

        /// <summary>
        ///     Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the organization identifier.
        /// </summary>
        /// <value>
        /// The organization identifier.
        /// </value>
        public Guid OrganisationId { get; set; }

        /// <summary>
        /// Gets or sets the file identifier.
        /// </summary>
        /// <value>
        /// The file identifier.
        /// </value>
        public Guid FileId { get; set; }

        /// <summary>
        /// Gets or sets the created time.
        /// </summary>
        /// <value>
        /// The created time.
        /// </value>
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// Gets or sets the modified time.
        /// </summary>
        /// <value>
        /// The modified time.
        /// </value>
        public DateTime ModifiedTime { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public Guid Version { get; set; }

        /// <summary>
        /// Gets or sets the type of the file.
        /// </summary>
        /// <value>
        /// The type of the file.
        /// </value>
        public FileType FileType { get; set; }

        #endregion
    }
}