// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApplicationConstants.cs" company="Microsoft">
//   Copyright (c) Glasgow City Council. All Rights Reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace RahulRai.Websites.Utilities.Common.RegularTypes
{
    /// <summary>
    ///     The application constants.
    /// </summary>
    public static class ApplicationConstants
    {
        #region Constants

        /// <summary>
        ///     The approved status.
        /// </summary>
        public const string Approved = "Approved";

        /// <summary>
        ///     The client identity postfix.
        /// </summary>
        public const string ClientIdentityPostfix = "Identity/AccessRights/{0}";

        /// <summary>
        ///     The create data set.
        /// </summary>
        public const string CreateDataSet = "CreateDataSet";

        /// <summary>
        ///     The create data set continuation.
        /// </summary>
        public const string CreateDataSetContinuation = "CreateDataSetContinuation";

        /// <summary>
        /// The create data set with approval check.
        /// </summary>
        public const string CreateDataSetWithApprovalCheck = "CreateDataSetWithApprovalCheck";

        /// <summary>
        ///     The create file.
        /// </summary>
        public const string CreateFile = "CreateFile";

        /// <summary>
        ///     The create organization.
        /// </summary>
        public const string CreateOrganization = "CreateOrganization";

        /// <summary>
        ///     The create user.
        /// </summary>
        public const string CreateUser = "CreateUser";

        /// <summary>
        ///     The system error.
        /// </summary>
        public const string CtpecSystemError = "An error has occurred while initializing some service for the method.";

        /// <summary>
        ///     The delete registered user.
        /// </summary>
        public const string DeleteRegisteredUser = "DeleteRegisteredUser";

        /// <summary>
        ///     The error create user.
        /// </summary>
        public const string ErrorCreateUser = "An error has occurred while creating user.";

        /// <summary>
        ///     The error delete registered user.
        /// </summary>
        public const string ErrorDeleteRegisteredUser = "An error occurred while deleting registered user.";

        /// <summary>
        ///     Error while getting the file specified by file Id
        /// </summary>
        public const string ErrorInGetFileById = "Error while getting the file specified by file Id";

        /// <summary>
        ///     Error while getting latest version of file.
        /// </summary>
        public const string ErrorInGetLatestFileVersion = "Error while getting latest version of file.";

        /// <summary>
        ///     Error in getting the latest files of a dataset
        /// </summary>
        public const string ErrorInGetLatestFiles = "Error in getting the latest files of a dataset";

        /// <summary>
        ///     The error update user.
        /// </summary>
        public const string ErrorUpdateUser = "An error has occurred while updating user.";

        /// <summary>
        ///     The source name this application uses to write to Windows Event Logs.
        /// </summary>
        public const string ErrorInRegisteredUserLogOn = "An error has occurred while logging in registered user";

        /// <summary>
        /// The error update user password
        /// </summary>
        public const string ErrorUpdateUserPassword = "An error has occurred while updating user password";

        /// <summary>
        ///     The source name this application uses to write to Windows Event Logs.
        /// </summary>
        public const string EventLogSourceName = "CTPEC";

        /// <summary>
        ///     The file BLOB download service placeholder
        /// </summary>
        public const string FileBlobDownloadServicePlaceholder =
            "{0}Download/Organisation/{1}/Dataset/{2}/File/{3}/Version/{4}";

        /// <summary>
        /// The file download log partition key
        /// </summary>
        public const string FileDownloadLogPartitionKey = "File Download";

        /// <summary>
        ///     The index subscript.
        /// </summary>
        public const string IndexSubscript = "AutoIndexedElement";

        /// <summary>
        ///     The input validation failed error.
        /// </summary>
        public const string InputValidationFailedError = "An error has occurred validating input passed to the method.";

        /// <summary>
        ///     The member role.
        /// </summary>
        public const string MemberRole = "Member";

        /// <summary>
        ///     The organization editor role.
        /// </summary>
        public const string OrganisationEditorRole = "OrganisationEditor";

        /// <summary>
        ///     Update data set.
        /// </summary>
        public const string UpdateDataSet = "UpdateDataSet";

        /// <summary>
        ///     update dataset configuration lookup empty
        /// </summary>
        public const string UpdateDataSetConfigurationLookupEmptyMessage =
            "Database is inconsistent, The DataSet with DataSetId: {0} does not have valid configurations as the configuration lookup table is empty.";

        /// <summary>
        ///     update dataset copy lookup
        /// </summary>
        public const string UpdateDataSetCopyConfigurationLookupMessage =
            "Copying DataSet Configuration Lookup Id : {0} as DataSet Config Lookup is not null";

        /// <summary>
        ///     update dataset duplicate title
        /// </summary>
        public const string UpdateDataSetDuplicateTitleMessage =
            "DataSet with title : {0} already exists for Organisation Id : {1}.";

        /// <summary>
        ///     update dataset metadata failed
        /// </summary>
        public const string UpdateDataSetFailedMessage = "Failed to process request for Update Dataset to Organisation.";

        /// <summary>
        ///     update dataset invalid configuration
        /// </summary>
        public const string UpdateDataSetInvalidConfigurationMessage =
            "Database is inconsistent, The dataSet with dataSet: {0} does not have valid configurations attached.";

        /// <summary>
        ///     update dataset metadata item count
        /// </summary>
        public const string UpdateDataSetMetadataCountMessage = "Found metadata with count : {0}";

        /// <summary>
        ///     update dataset metadata item
        /// </summary>
        public const string UpdateDataSetMetadataItemMessage = "Item found in metadata : {0} - {1}";

        /// <summary>
        ///     update dataset metadata not null message
        /// </summary>
        public const string UpdateDataSetMetadataNotNullMessage =
            "Update Dataset to Organisation Metadata Domain Service is not null.";

        /// <summary>
        ///     update dataset metadata null or without title
        /// </summary>
        public const string UpdateDataSetMetadataNullOrMissingTitleMessage =
            "Metadata supplied is null or without DataSet Title which is mandatory field.";

        /// <summary>
        ///     update dataset metadata validated
        /// </summary>
        public const string UpdateDataSetMetadataValidatedMessage =
            "Valid metadata passed on to metadata domain service.";

        /// <summary>
        ///     update dataset new configuration lookup
        /// </summary>
        public const string UpdateDataSetNewConfigurationLookupMessage =
            "New Dataset Config Lookup copied with default values as Dataset Config Lookup is null.";

        /// <summary>
        ///     update dataset no data
        /// </summary>
        public const string UpdateDataSetNoDataMessage =
            "DataSet with DataSetId : {0} and Organisation Id : {1} does not exists in the DB.";

        /// <summary>
        ///     update dataset request accepted
        /// </summary>
        public const string UpdateDataSetRequestAcceptedMessage =
            "Successfully accepted dataset update request. RequestId={0}.";

        /// <summary>
        ///     update dataset request failed with status code
        /// </summary>
        public const string UpdateDataSetRequestFailedWithStatusCodeMessage =
            "Failed to process dataset creation request. Response status code = {0}.";

        /// <summary>
        ///     update dataset request received from user
        /// </summary>
        public const string UpdateDataSetRequestReceivedFromUserMessage =
            "Received dataset update request. User={0}. Title={1}.";

        /// <summary>
        ///     update dataset request received
        /// </summary>
        public const string UpdateDataSetRequestReceivedMessage =
            "Update dataset to Organisation : {0} with metadata : {1} received in metadata service.";

        /// <summary>
        ///     update dataset to organization
        /// </summary>
        public const string UpdateDataSetToOrganisationMessage = "Updating DataSet with Id {0} in Organisation {1}";

        /// <summary>
        ///     update dataset user not present
        /// </summary>
        public const string UpdateDataSetUserNotPresentMessage = "User \"{0}\" not present on the platfom.";

        /// <summary>
        ///     update dataset metadata validated
        /// </summary>
        public const string UpdateDataSetWithMetadataMessage =
            "Dataset : {0} update to Organisation : {1} with metadata : {2}";

        /// <summary>
        ///     The update file
        /// </summary>
        public const string UpdateFile = "UpdateFile";

        /// <summary>
        ///     The update organization.
        /// </summary>
        public const string UpdateOrganization = "UpdateOrganization";

        /// <summary>
        ///     The update organization failed.
        /// </summary>
        public const string UpdateOrganizationFailed = "Failed to update organization {0}";

        /// <summary>
        ///     The update user.
        /// </summary>
        public const string UpdateUser = "UpdateUser";

        /// <summary>
        ///     The update user role.
        /// </summary>
        public const string UpdateUserRole = "UpdateUserRole";

        /// <summary>
        ///     The workflow configuration element.
        /// </summary>
        public const string WorkflowConfigurationElement = "workflowconfiguration";

        /// <summary>
        /// The maximum length for Key
        /// </summary>
        public const int MaximumKeyLength = 255;

        /// <summary>
        /// The maximum length for Value
        /// </summary>
        public const int MaximumValueLength = 64000;

        /// <summary>
        /// The maximum metadata length (sum of lengths of keys and values).
        /// </summary>
        public const int MaximumMetadataLength = 100000;

        /// <summary>
        /// The delete file version workflow name.
        /// </summary>
        public const string DeleteFileVersion = "DeleteFileVersion";

        /// <summary>
        /// The restricted metadata separator
        /// </summary>
        public const char RestrictedMetadataSeparator = ';';

        #endregion
    }
}