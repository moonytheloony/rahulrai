namespace RahulRai.Websites.Utilities.Web
{
    #region

    using CookComputing.XmlRpc;

    #endregion

    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public struct MediaObject
    {
        public byte[] Bits;
        public string Name;
        public string Type;
    }
}