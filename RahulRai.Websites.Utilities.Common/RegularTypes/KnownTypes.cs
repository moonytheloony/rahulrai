// ***********************************************************************
// Assembly         : RahulRai.Websites.Utilities.Common
// Author           : rahulrai
// Created          : 04-15-2015
//
// Last Modified By : rahulrai
// Last Modified On : 06-24-2015
// ***********************************************************************
// <copyright file="KnownTypes.cs" company="Rahul Rai">
//     Copyright (c) Rahul Rai. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace RahulRai.Websites.Utilities.Common.RegularTypes
{
    #region

    using System;

    #endregion

    /// <summary>
    ///     Known Types.
    /// </summary>
    public static class KnownTypes
    {
        #region Constants

        /// <summary>
        ///     Valid account name pattern.
        /// </summary>
        public const string AccountValidationPattern =
            "^(([a-z0-9]|[a-z0-9][a-z0-9\\-]*[a-z0-9])\\.)*([a-z]|[a-z][a-z0-9\\-]*[a-z0-9])$";

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
        ///     The entity id.
        /// </summary>
        public const string EntityId = "EntityId";

        /// <summary>
        ///     The entity version id.
        /// </summary>
        public const string EntityVersionId = "EntityVersionId";

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
        ///     The file URL inside the BLOB storage.
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
        public const string HyphenSeparator = "-";

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
        ///     The token validator
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
        ///     Colon and space
        /// </summary>
        public const string Colon = ":";

        /// <summary>
        /// The semi colon
        /// </summary>
        public const string SemiColon = ";";

        /// <summary>
        /// The username claim identifier.
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
        ///     The maximum Azure table string property length (in characters).
        /// </summary>
        public const int MaxTableStringPropertyLength = 16384; // 16 KB

        #endregion

        #region Static Fields

        /// <summary>
        ///     The min date time.
        /// </summary>
        public static readonly DateTimeOffset MinDateTime = new DateTimeOffset(1800, 1, 1, 0, 0, 0, TimeSpan.Zero);

        #endregion
    }
}