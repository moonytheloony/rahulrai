// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KnownTypes.cs" company="Microsoft">
//   Copyright (c) Glasgow City Council. All Rights Reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace RahulRai.Websites.Utilities.Common.RegularTypes
{
    #region

    using System;
    using System.Text.RegularExpressions;

    #endregion

    /// <summary>
    /// Known Types.
    /// </summary>
    public static class KnownTypes
    {
        #region Constants

        /// <summary>
        ///     The about.
        /// </summary>
        public const string About = "About";

        /// <summary>
        ///     Valid account name pattern.
        /// </summary>
        public const string AccountValidationPattern =
            "^(([a-z0-9]|[a-z0-9][a-z0-9\\-]*[a-z0-9])\\.)*([a-z]|[a-z][a-z0-9\\-]*[a-z0-9])$";

        /// <summary>
        ///     The ad user display name
        /// </summary>
        public const string AdUserDisplayName = "DisplayName";

        /// <summary>
        ///     The ad user mail nick name
        /// </summary>
        public const string AdUserMailNickname = "MailNickName";

        /// <summary>
        ///     The application Id claim identifier.
        /// </summary>
        public const string AppIdClaimIdentifier = "appid";

        /// <summary>
        ///     Audit Log
        /// </summary>
        public const string AuditLog = "AuditLog";

        /// <summary>
        ///     The compiled expression identifier.
        /// </summary>
        public const string CompiledExpressionIdentifier = @"([{]\w+[}])";

        /// <summary>
        ///     Compression encoding.
        /// </summary>
        public const string CompressionUtility = "7z";

        /// <summary>
        ///     Blob level container access Permission.
        /// </summary>
        public const string ContainerAccessBlobLevel = "Blob - Public read access for blobs";

        /// <summary>
        ///     Private level container access Permission.
        /// </summary>
        public const string ContainerAccessPrivateLevel = "Private - Access to account owner only";

        /// <summary>
        ///     Public level container access Permission.
        /// </summary>
        public const string ContainerAccessPublicLevel =
            "Container - Full public read access for container and blob data";

        /// <summary>
        ///     The CSV separator.
        /// </summary>
        public const char CsvSeparator = ',';

        /// <summary>
        ///     The data set category
        /// </summary>
        public const string DataSetCategory = "Category";

        /// <summary>
        ///     The data set category
        /// </summary>
        public const string DataSetCategoryValidationSettingKey = "DataSetCategoryValidationSettingKey";

        /// <summary>
        ///     The data set created date and time.
        /// </summary>
        public const string DataSetCreated = "Created";

        /// <summary>
        ///     The data set description
        /// </summary>
        public const string DataSetDescription = "Description";

        /// <summary>
        ///     The data set description
        /// </summary>
        public const string DataSetDescriptionValidationSettingKey = "DataSetDescriptionValidationSettingKey";

        /// <summary>
        ///     The data set license
        /// </summary>
        public const string DataSetLicense = "License";

        /// <summary>
        ///     The data set license
        /// </summary>
        public const string DataSetLicenseValidationSettingKey = "DataSetLicenseValidationSettingKey";

        /// <summary>
        ///     The data set maintainer contact
        /// </summary>
        public const string DataSetMaintainerContact = "MaintainerContact";

        /// <summary>
        ///     The data set maintainer contact
        /// </summary>
        public const string DataSetMaintainerContactValidationSettingKey =
            "DataSetMaintainerContactValidationSettingKey";

        /// <summary>
        ///     The data set maintainer name
        /// </summary>
        public const string DataSetMaintainerName = "MaintainerName";

        /// <summary>
        ///     The data set maintainer name
        /// </summary>
        public const string DataSetMaintainerNameValidationSettingKey = "DataSetMaintainerNameValidationSettingKey";

        /// <summary>
        ///     The data set last modification date and time.
        /// </summary>
        public const string DataSetModified = "Modified";

        /// <summary>
        ///     The data set openness rating
        /// </summary>
        public const string DataSetOpennessRating = "OpennessRating";

        /// <summary>
        ///     The data set openness rating
        /// </summary>
        public const string DataSetOpennessRatingValidationSettingKey = "DataSetOpennessRatingValidationSettingKey";

        /// <summary>
        ///     The data set organization identifier.
        /// </summary>
        public const string DataSetOrganizationId = "OrganisationId";

        /// <summary>
        ///     The data set published on behalf of
        /// </summary>
        public const string DataSetPublishedOnBehalfOf = "PublishedOnBehalfOf";

        /// <summary>
        ///     The data set published on behalf of
        /// </summary>
        public const string DataSetPublishedOnBehalfOfValidationSettingKey =
            "DataSetPublishedOnBehalfOfValidationSettingKey";

        /// <summary>
        ///     The data set quality
        /// </summary>
        public const string DataSetQuality = "Quality";

        /// <summary>
        ///     The data set quality
        /// </summary>
        public const string DataSetQualityValidationSettingKey = "DataSetQualityValidationSettingKey";

        /// <summary>
        ///     The data set standard name
        /// </summary>
        public const string DataSetStandardName = "StandardName";

        /// <summary>
        ///     The data set standard name
        /// </summary>
        public const string DataSetStandardNameValidationSettingKey = "DataSetStandardNameValidationSettingKey";

        /// <summary>
        ///     The data set standard rating
        /// </summary>
        public const string DataSetStandardRating = "StandardRating";

        /// <summary>
        ///     The data set standard rating
        /// </summary>
        public const string DataSetStandardRatingValidationSettingKey = "DataSetStandardRatingValidationSettingKey";

        /// <summary>
        ///     The data set standard version
        /// </summary>
        public const string DataSetStandardVersion = "StandardVersion";

        /// <summary>
        ///     The data set standard version
        /// </summary>
        public const string DataSetStandardVersionValidationSettingKey = "DataSetStandardVersionValidationSettingKey";

        /// <summary>
        ///     The data set tags
        /// </summary>
        public const string DataSetTags = "Tags";

        /// <summary>
        ///     The data set tags
        /// </summary>
        public const string DataSetTagsValidationSettingKey = "DataSetTagsValidationSettingKey";

        /// <summary>
        ///     The data set theme
        /// </summary>
        public const string DataSetTheme = "Theme";

        /// <summary>
        ///     The data set theme
        /// </summary>
        public const string DataSetThemeValidationSettingKey = "DataSetThemeValidationSettingKey";

        /// <summary>
        ///     The data set title
        /// </summary>
        public const string DataSetTitle = "Title";

        /// <summary>
        ///     The data set title
        /// </summary>
        public const string DataSetTitleValidationSettingKey = "DataSetTitleValidationSettingKey";

        /// <summary>
        ///     The data set usage guidance
        /// </summary>
        public const string DataSetUsageGuidance = "UsageGuidance";

        /// <summary>
        ///     The data set usage guidance
        /// </summary>
        public const string DataSetUsageGuidanceValidationSettingKey = "DataSetUsageGuidanceValidationSettingKey";

        /// <summary>
        ///     The data set additional custom metadata
        /// </summary>
        public const string DataSetAdditionalAttributes = "DataSetAdditionalAttributes";

        /// <summary>
        ///     The date time format.
        /// </summary>
        public const string DateTimeFormat = "yyyyMMddHHmmss";

        /// <summary>
        ///     The description.
        /// </summary>
        public const string Description = "Description";

        /// <summary>
        ///     The dot.
        /// </summary>
        public const string Dot = ".";

        /// <summary>
        ///     The email.
        /// </summary>
        public const string Email = "Email";

        /// <summary>
        /// The entity id.
        /// </summary>
        public const string EntityId = "EntityId";

        /// <summary>
        /// The entity version id.
        /// </summary>
        public const string EntityVersionId = "EntityVersionId";

        /// <summary>
        ///     Error Log
        /// </summary>
        public const string ErrorLog = "ErrorLog";

        /// <summary>
        ///     The error string.
        /// </summary>
        public const string ErrorString = "Failed to Execute Method - {0} due to this Error - {1}";

        /// <summary>
        ///     The file blob relative URL
        /// </summary>
        public const string FileBlobRelativeUrl = "FileBlobRelativeUrl";

        /// <summary>
        ///     The file creation date
        /// </summary>
        public const string FileCreationDate = "CreationDate";

        /// <summary>
        ///     The file creation date
        /// </summary>
        public const string FileCreationDateValidationSettingKey = "FileCreationDateValidationSettingKey";

        /// <summary>
        ///     The file data set identifier
        /// </summary>
        public const string FileDataSetId = "DataSetId";

        /// <summary>
        ///     The file data set identifier
        /// </summary>
        public const string FileDataSetIdValidationSettingKey = "FileDataSetIdValidationSettingKey";

        /// <summary>
        ///     The file description
        /// </summary>
        public const string FileDescription = "Description";

        /// <summary>
        ///     The file description
        /// </summary>
        public const string FileDescriptionValidationSettingKey = "FileDescriptionValidationSettingKey";

        /// <summary>
        ///     The external file url.
        /// </summary>
        public const string FileExternalFileUrl = "ExternalUrl";

        /// <summary>
        ///     The file external url validation setting key.
        /// </summary>
        public const string FileExternalFileUrlValidationSettingKey = "FileExternalFileUrlValidationSettingKey";

        /// <summary>
        ///     The file blob external URL
        /// </summary>
        public const string FileExternalUrl = "FileExternalUrl";

        /// <summary>
        /// The file URL inside the BLOB storage.
        /// </summary>
        public const string FileUrl = "FileUrl";

        /// <summary>
        ///     The file id.
        /// </summary>
        public const string FileId = "FileId";

        /// <summary>
        ///     The file license
        /// </summary>
        public const string FileLicense = "License";

        /// <summary>
        ///     The file license
        /// </summary>
        public const string FileLicenseValidationSettingKey = "FileLicenseValidationSettingKey";

        /// <summary>
        ///     The file name
        /// </summary>
        public const string FileName = "FileName";

        /// <summary>
        ///     The file name
        /// </summary>
        public const string FileNameValidationSettingKey = "FileNameValidationSettingKey";

        /// <summary>
        ///     The file openness rating
        /// </summary>
        public const string FileOpennessRating = "OpennessRating";

        /// <summary>
        ///     The file openness rating
        /// </summary>
        public const string FileOpennessRatingValidationSettingKey = "FileOpennessRatingValidationSettingKey";

        /// <summary>
        ///     The file quality
        /// </summary>
        public const string FileQuality = "Quality";

        /// <summary>
        ///     The file quality
        /// </summary>
        public const string FileQualityValidationSettingKey = "FileQualityValidationSettingKey";

        /// <summary>
        ///     The file response disposition type
        /// </summary>
        public const string FileResponseDispositionType = "attachment";

        /// <summary>
        ///     The file standard name
        /// </summary>
        public const string FileStandardName = "StandardName";

        /// <summary>
        ///     The file standard name
        /// </summary>
        public const string FileStandardNameValidationSettingKey = "FileStandardNameValidationSettingKey";

        /// <summary>
        ///     The file standard rating
        /// </summary>
        public const string FileStandardRating = "StandardRating";

        /// <summary>
        ///     The file standard rating
        /// </summary>
        public const string FileStandardRatingValidationSettingKey = "FileStandardRatingValidationSettingKey";

        /// <summary>
        ///     The file standard version
        /// </summary>
        public const string FileStandardVersion = "StandardVersion";

        /// <summary>
        ///     The file standard version
        /// </summary>
        public const string FileStandardVersionValidationSettingKey = "FileStandardVersionValidationSettingKey";

        /// <summary>
        ///     The file title
        /// </summary>
        public const string FileTitle = "Title";

        /// <summary>
        ///     The file title
        /// </summary>
        public const string FileTitleValidationSettingKey = "FileTitleValidationSettingKey";

        /// <summary>
        ///     The file type
        /// </summary>
        public const string FileType = "Type";

        /// <summary>
        ///     The file type
        /// </summary>
        public const string FileTypeValidationSettingKey = "FileTypeValidationSettingKey";

        /// <summary>
        ///     The first name.
        /// </summary>
        public const string FirstName = "FirstName";

        /// <summary>
        ///     No. of Bytes for a Giga Byte
        /// </summary>
        public const int GB = KB * MB;

        /// <summary>
        ///     The get request.
        /// </summary>
        public const string GetRequest = "GET";

        /// <summary>
        ///     The hex string format.
        /// </summary>
        public const string HexStringFormat = "X2";

        /// <summary>
        ///     The hyphen separator.
        /// </summary>
        public const char HyphenSeparator = '-';

        /// <summary>
        ///     Information Log
        /// </summary>
        public const string InformationLog = "InformationLog";

        /// <summary>
        ///     The information string.
        /// </summary>
        public const string InformationString = "Executing Method - {0} with data - {1}";

        /// <summary>
        ///     No. of Bytes for a Kilo Byte
        /// </summary>
        public const int KB = 1024;

        /// <summary>
        ///     The last name.
        /// </summary>
        public const string LastName = "LastName";

        /// <summary>
        ///     No. of Bytes for a Mega Byte
        /// </summary>
        public const int MB = KB * KB;

        /// <summary>
        ///     The match any
        /// </summary>
        public const char MatchAny = '*';

        /// <summary>
        ///     The metadata service base URL
        /// </summary>
        public const string MetadataServiceBaseUrl = "MetadataServiceBaseUrl";

        /// <summary>
        ///     Size of metadata
        /// </summary>
        public const int MetadataSize = 1024 * 8;

        /// <summary>
        ///     Represents the milliseconds in a minute
        /// </summary>
        public const int MinToMillisecond = 1000 * 60;

        /// <summary>
        ///     The name.
        /// </summary>
        public const string Name = "Name";

        /// <summary>
        ///     Table Entity Partition Key attribute.
        /// </summary>
        public const string PartitionKey = "PartitionKey";

        /// <summary>
        ///     The password.
        /// </summary>
        public const string Password = "Password";

        /// <summary>
        ///     The pipe separator.
        /// </summary>
        public const char PipeSeparator = '|';

        /// <summary>
        ///     The post request.
        /// </summary>
        public const string PostRequest = "POST";

        /// <summary>
        ///     The production deployment slot.
        /// </summary>
        public const string ProductionDeploymentSlot = "Production";

        /// <summary>
        ///     The query parameter order by.
        /// </summary>
        public const string QueryOptionalParameterOrderBy = "$orderby";

        /// <summary>
        ///     The query parameter order by created time.
        /// </summary>
        public const string QueryOptionalParameterOrderByCreated = "created";

        /// <summary>
        ///     The query parameter order by title.
        /// </summary>
        public const string QueryOptionalParameterOrderByTitle = "title";

        /// <summary>
        ///     The query parameter skip.
        /// </summary>
        public const string QueryOptionalParameterSkip = "$skip";

        /// <summary>
        ///     The query parameter top.
        /// </summary>
        public const string QueryOptionalParameterTop = "$top";

        /// <summary>
        ///     The query parameter registered user
        /// </summary>
        public const string QueryOptionalParameterIsRegisteredUser = "registered";

        /// <summary>
        ///     The query parameter order by user name
        /// </summary>
        public const string QueryOptionalParameterOrderByUserName = "username";

        /// <summary>
        ///     The name of the key that will indicate Authorization Header.
        /// </summary>
        public const string RequestHeaderAuthorization = "Authorization";

        /// <summary>
        ///     The Authorization access token prefix.
        /// </summary>
        public const string RequestHeaderBearer = "Bearer";

        /// <summary>
        ///     The name of the key that will hold the user id in the Web API request header.
        /// </summary>
        public const string RequestHeaderUserId = "UserID";

        /// <summary>
        ///     root container name
        /// </summary>
        public const string RootContainer = "$root";

        /// <summary>
        ///     Table Entity Row Key attribute.
        /// </summary>
        public const string RowKey = "RowKey";

        /// <summary>
        ///     Table Entity Row Key attribute.
        /// </summary>
        public const string Timestamp = "Timestamp";

        /// <summary>
        /// The token validator
        /// </summary>
        public const string TokenValidator = @"authorization\sbearer\s\w+";

        /// <summary>
        ///     The under score.
        /// </summary>
        public const string Underscore = "_";

        /// <summary>
        ///     Uri separator character
        /// </summary>
        public const string UrlSeparator = "/";

        /// <summary>
        ///     The user name.
        /// </summary>
        public const string UserName = "UserName";

        /// <summary>
        ///     Debug Events
        /// </summary>
        public const string DebugEvents = "Debug Events";
        
        /// <summary>
        ///     Colon and space
        /// </summary>
        public const string Colon = ": ";

        /// <summary>
        ///     The username claim identifier.
        /// </summary>
        public const string UserNameClaimIdentifier = @"http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";

        /// <summary>
        ///     Valid container name pattern.
        /// </summary>
        public const string ValidContainerNameRegex = @"^([a-z]|\d){1}([a-z]|-|\d){1,61}([a-z]|\d){1}$";

        /// <summary>
        ///     Valid table name pattern.
        /// </summary>
        public const string ValidEmailIdRegex =
            @"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))"
                + @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$";

        /// <summary>
        ///     Valid GUID pattern.
        /// </summary>
        public const string ValidGuidPattern =
            @"^(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})$";

        /// <summary>
        ///     Valid Queue name pattern.
        /// </summary>
        public const string ValidQueueNameRegex = @"^([a-z]|\d){1}([a-z]|-|\d){1,61}([a-z]|\d){1}$";

        /// <summary>
        ///     Valid table name pattern.
        /// </summary>
        public const string ValidTableNameRegex = @"^([a-z]|[A-Z]){1}([a-z]|[A-Z]|\d){2,62}$";

        /// <summary>
        ///     The version.
        /// </summary>
        public const string Version = "Version";

        /// <summary>
        ///     The version one.
        /// </summary>
        public const string VersionOne = "1";

        /// <summary>
        ///     The whitespace.
        /// </summary>
        public const char WhiteSpace = ' ';

        /// <summary>
        ///     The xml mime type.
        /// </summary>
        public const string XmlMimeType = "application/xml";

        /// <summary>
        ///     The xml schema definition file extension.
        /// </summary>
        public const string XmlSchemaDefinitionFileExtension = "xsd";

        /// <summary>
        ///     The XSL transformation file extension.
        /// </summary>
        public const string XslTransformationFileExtension = "xslt";

        /// <summary>
        /// Needs approval
        /// </summary>
        public const string NeedsApproval = "NeedsApproval";

        /// <summary>
        /// The container type main
        /// </summary>
        public const string ContainerTypeMain = "main";

        /// <summary>
        /// The container type transient
        /// </summary>
        public const string ContainerTypeTransient = "transient";

        /// <summary>
        /// The maximum Azure table string property length (in characters).
        /// </summary>
        public const int MaxTableStringPropertyLength = 16384; // 16 KB

        #endregion

        #region Static Fields

        /// <summary>
        ///     The min date time.
        /// </summary>
        public static readonly DateTimeOffset MinDateTime = new DateTimeOffset(1800, 1, 1, 0, 0, 0, TimeSpan.Zero);

        /// <summary>
        ///     Validation regex for UserName of member user.
        /// </summary>
        public static readonly Regex UserNameOfMemberUserRegex = new Regex(
            @"^[a-zA-Z0-9\-_]{1,16}$",
            RegexOptions.Compiled);

        /// <summary>
        ///     Validation regex for UserName of registered user.
        /// </summary>
        public static readonly Regex UserNameOfRegisteredUserRegex = new Regex(@"^.{1,255}$", RegexOptions.Compiled);

        #endregion
    }
}