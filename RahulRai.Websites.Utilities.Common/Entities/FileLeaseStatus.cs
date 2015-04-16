namespace RahulRai.Websites.Utilities.Common.Entities
{
    /// <summary>
    ///     The file lease status.
    /// </summary>
    public class FileLeaseStatus
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the lease id.
        /// </summary>
        public string LeaseId { get; set; }

        /// <summary>
        ///     Gets or sets the lease state.
        /// </summary>
        public FileLeaseState LeaseState { get; set; }

        #endregion
    }
}