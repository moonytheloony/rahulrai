namespace RahulRai.Websites.Utilities.Common.Entities
{
    #region

    using System;

    #endregion

    /// <summary>
    ///     The data file.
    /// </summary>
    public class DataFile
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the file content.
        /// </summary>
        public byte[] FileContent { get; set; }

        /// <summary>
        ///     Gets or sets the file encoding type.
        /// </summary>
        public string FileEncodingType { get; set; }

        /// <summary>
        ///     Gets or sets the file name.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        ///     Gets or sets the type of the file.
        /// </summary>
        /// <value>
        ///     The type of the file.
        /// </value>
        public FileType FileType { get; set; }

        /// <summary>
        ///     Gets or sets the file external URL.
        /// </summary>
        /// <value>
        ///     The file external URL.
        /// </value>
        public Uri FileExternalUrl { get; set; }

        #endregion
    }
}