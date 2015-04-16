namespace RahulRai.Websites.Utilities.Common.Entities
{
    #region

    using System.Collections.Generic;
    using System.Linq;
    using Helpers;
    using RegularTypes;

    #endregion

    /// <summary>
    ///     Transient entity
    /// </summary>
    public class TransientEntity
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the entity identifier.
        /// </summary>
        /// <value>
        ///     The entity identifier.
        /// </value>
        public string EntityId { get; set; }

        /// <summary>
        ///     Gets or sets the entity key.
        /// </summary>
        /// <value>
        ///     The entity key.
        /// </value>
        public string EntityKey { get; set; }

        /// <summary>
        ///     Gets or sets the entity tag.
        /// </summary>
        /// <value>
        ///     The entity tag.
        /// </value>
        public string EntityTag { get; set; }

        /// <summary>
        ///     Gets or sets the payload.
        /// </summary>
        /// <value>
        ///     The payload.
        /// </value>
        public string Payload
        {
            get { return PayloadParts == null ? null : PayloadParts.Combine(); }

            set
            {
                PayloadParts = value == null
                    ? null
                    : value.SplitByLength(KnownTypes.MaxTableStringPropertyLength).ToList();
            }
        }

        /// <summary>
        ///     Gets or sets the payload parts.
        /// </summary>
        /// <value>
        ///     The payload parts.
        /// </value>
        public IList<string> PayloadParts { get; set; }

        #endregion
    }
}