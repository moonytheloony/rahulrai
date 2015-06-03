namespace RahulRai.Websites.Utilities.Web
{
    #region

    using CookComputing.XmlRpc;

    #endregion

    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public struct MediaObjectUrl
    {
        public string Url;
    }
}