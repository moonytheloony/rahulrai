namespace RahulRai.Websites.Utilities.Common.Entities
{
    #region

    using System.Collections.Generic;

    #endregion

    /// <summary>
    ///     The folder.
    /// </summary>
    public class Folder
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the metadata.
        /// </summary>
        public IDictionary<string, string> Metadata { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        #endregion
    }
}