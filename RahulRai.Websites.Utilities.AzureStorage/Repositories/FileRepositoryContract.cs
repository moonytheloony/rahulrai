#region



#endregion

namespace RahulRai.Websites.Utilities.AzureStorage.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.IO;

    #region

    

    #endregion

    /// <summary>
    ///     The file repository contract.
    /// </summary>
    [ContractClassFor(typeof (IFileRepository))]
    public abstract class FileRepositoryContract : IFileRepository
    {
        #region Public Methods and Operators

        /// <summary>
        ///     The add file to folder.
        /// </summary>
        /// <param name="folderName">
        ///     The folder name.
        /// </param>
        /// <param name="fileStream">
        ///     The file stream.
        /// </param>
        /// <param name="fileName">
        ///     The file name.
        /// </param>
        /// <returns>
        ///     The <see cref="FileOperationStatus" />.
        /// </returns>
        /// <exception cref="NotImplementedException">
        ///     throws exception
        /// </exception>
        public FileOperationStatus AddFileToFolder(string folderName, Stream fileStream, string fileName)
        {
            Contract.Requires<InputValidationFailedException>(!string.IsNullOrEmpty(folderName), "folderName");
            Contract.Requires<InputValidationFailedException>(null != fileStream, "fileStream");
            Contract.Requires<InputValidationFailedException>(!string.IsNullOrEmpty(fileName), "fileName");
            throw new NotImplementedException();
        }

        /// <summary>
        ///     The add file to sub directory.
        /// </summary>
        /// <param name="folderName">
        ///     The folder name.
        /// </param>
        /// <param name="directoryName">
        ///     The directory name.
        /// </param>
        /// <param name="fileStream">
        ///     The file stream.
        /// </param>
        /// <param name="fileName">
        ///     The file name.
        /// </param>
        /// <returns>
        ///     The <see cref="FileOperationStatus" />.
        /// </returns>
        /// <exception cref="NotImplementedException">
        ///     throws exception
        /// </exception>
        public FileOperationStatus AddFileToSubdirectory(
            string folderName,
            string directoryName,
            Stream fileStream,
            string fileName)
        {
            Contract.Requires<InputValidationFailedException>(!string.IsNullOrEmpty(folderName), "folderName");
            Contract.Requires<InputValidationFailedException>(!string.IsNullOrEmpty(directoryName), "directoryName");
            Contract.Requires<InputValidationFailedException>(null != fileStream, "fileStream");
            Contract.Requires<InputValidationFailedException>(!string.IsNullOrEmpty(fileName), "fileName");
            throw new NotImplementedException();
        }

        /// <summary>
        ///     The copy file across server.
        /// </summary>
        /// <param name="sourceFolder">
        ///     The source folder.
        /// </param>
        /// <param name="fileName">
        ///     The file name.
        /// </param>
        /// <param name="targetContext">
        ///     The target context.
        /// </param>
        /// <param name="targetFolder">
        ///     The target folder.
        /// </param>
        /// <param name="targetFileName">
        ///     The target File Name.
        /// </param>
        /// <returns>
        ///     The <see cref="FileOperationStatus" />.
        /// </returns>
        /// <exception cref="NotImplementedException">
        ///     throws exception
        /// </exception>
        public FileOperationStatus CopyFileAcrossServer(
            string sourceFolder,
            string fileName,
            IContext targetContext,
            string targetFolder,
            string targetFileName)
        {
            Contract.Requires<InputValidationFailedException>(!string.IsNullOrEmpty(sourceFolder), "sourceFolder");
            Contract.Requires<InputValidationFailedException>(!string.IsNullOrEmpty(fileName), "fileName");
            Contract.Requires<InputValidationFailedException>(!string.IsNullOrEmpty(targetFolder), "targetFolder");
            throw new NotImplementedException();
        }

        /// <summary>
        ///     The copy file across server synchronous.
        /// </summary>
        /// <param name="sourceFolder">
        ///     The source folder.
        /// </param>
        /// <param name="fileName">
        ///     The file name.
        /// </param>
        /// <param name="targetContext">
        ///     The target context.
        /// </param>
        /// <param name="targetFolder">
        ///     The target folder.
        /// </param>
        /// <param name="targetFileName">
        ///     The target File Name.
        /// </param>
        /// <returns>
        ///     The <see cref="FileOperationStatus" />.
        /// </returns>
        /// <exception cref="NotImplementedException">
        ///     Not implemented
        /// </exception>
        public FileOperationStatus CopyFileAcrossServerSynchronous(
            string sourceFolder,
            string fileName,
            IContext targetContext,
            string targetFolder,
            string targetFileName)
        {
            Contract.Requires<InputValidationFailedException>(!string.IsNullOrEmpty(sourceFolder), "sourceFolder");
            Contract.Requires<InputValidationFailedException>(!string.IsNullOrEmpty(fileName), "fileName");
            Contract.Requires<InputValidationFailedException>(!string.IsNullOrEmpty(targetFolder), "targetFolder");
            throw new NotImplementedException();
        }

        /// <summary>
        ///     The copy file within server.
        /// </summary>
        /// <param name="sourceFolder">
        ///     The source folder.
        /// </param>
        /// <param name="fileName">
        ///     The file name.
        /// </param>
        /// <param name="targetFolder">
        ///     The target folder.
        /// </param>
        /// <param name="targetFileName">
        ///     The target File Name.
        /// </param>
        /// <returns>
        ///     The <see cref="FileOperationStatus" />.
        /// </returns>
        /// <exception cref="NotImplementedException">
        ///     throws exception
        /// </exception>
        public FileOperationStatus CopyFileWithinServer(
            string sourceFolder,
            string fileName,
            string targetFolder,
            string targetFileName)
        {
            Contract.Requires<InputValidationFailedException>(!string.IsNullOrEmpty(sourceFolder), "sourceFolder");
            Contract.Requires<InputValidationFailedException>(!string.IsNullOrEmpty(fileName), "fileName");
            Contract.Requires<InputValidationFailedException>(!string.IsNullOrEmpty(targetFolder), "targetFolder");
            throw new NotImplementedException();
        }

        /// <summary>
        ///     The copy file within server synchronous.
        /// </summary>
        /// <param name="sourceFolder">
        ///     The source folder.
        /// </param>
        /// <param name="fileName">
        ///     The file name.
        /// </param>
        /// <param name="targetFolder">
        ///     The target folder.
        /// </param>
        /// <param name="targetFileName">
        ///     The target File Name.
        /// </param>
        /// <returns>
        ///     The <see cref="FileOperationStatus" />.
        /// </returns>
        /// <exception cref="NotImplementedException">
        ///     Not implemented
        /// </exception>
        public FileOperationStatus CopyFileWithinServerSynchronous(
            string sourceFolder,
            string fileName,
            string targetFolder,
            string targetFileName)
        {
            Contract.Requires<InputValidationFailedException>(!string.IsNullOrEmpty(sourceFolder), "sourceFolder");
            Contract.Requires<InputValidationFailedException>(!string.IsNullOrEmpty(fileName), "fileName");
            Contract.Requires<InputValidationFailedException>(!string.IsNullOrEmpty(targetFolder), "targetFolder");
            throw new NotImplementedException();
        }

        /// <summary>
        ///     The create folder.
        /// </summary>
        /// <param name="folderName">
        ///     The folder name.
        /// </param>
        /// <param name="visibilityType">
        ///     The visibility type.
        /// </param>
        /// <returns>
        ///     The <see cref="FileOperationStatus" />.
        /// </returns>
        /// <exception cref="NotImplementedException">
        ///     throws exception
        /// </exception>
        public FileOperationStatus CreateFolder(string folderName, VisibilityType visibilityType)
        {
            Contract.Requires<InputValidationFailedException>(!string.IsNullOrEmpty(folderName), "sourceFolder");
            throw new NotImplementedException();
        }

        /// <summary>
        ///     The create folder.
        /// </summary>
        /// <param name="folderName">
        ///     The folder name.
        /// </param>
        /// <param name="visibilityType">
        ///     The visibility type.
        /// </param>
        /// <param name="folderMetadata">
        ///     The folder metadata.
        /// </param>
        /// <returns>
        ///     The <see cref="FileOperationStatus" />.
        /// </returns>
        /// <exception cref="NotImplementedException">
        ///     throws exception
        /// </exception>
        public FileOperationStatus CreateFolder(
            string folderName,
            VisibilityType visibilityType,
            IDictionary<string, string> folderMetadata)
        {
            Contract.Requires<InputValidationFailedException>(!string.IsNullOrEmpty(folderName), "folderName");
            Contract.Requires<InputValidationFailedException>(null != folderMetadata, "folderMetadata");
            throw new NotImplementedException();
        }

        /// <summary>
        ///     The delete directory.
        /// </summary>
        /// <param name="folderName">
        ///     The folder name.
        /// </param>
        /// <param name="directoryName">
        ///     The directory name.
        /// </param>
        /// <returns>
        ///     The <see cref="FileOperationStatus" />.
        /// </returns>
        /// <exception cref="NotImplementedException">
        ///     Contract does not implement functionality.
        /// </exception>
        public FileOperationStatus DeleteDirectory(string folderName, string directoryName)
        {
            Contract.Requires<InputValidationFailedException>(!string.IsNullOrWhiteSpace(folderName), "folderName");
            Contract.Requires<InputValidationFailedException>(
                !string.IsNullOrWhiteSpace(directoryName),
                "directoryName");
            throw new NotImplementedException();
        }

        /// <summary>
        ///     The delete file.
        /// </summary>
        /// <param name="folderName">
        ///     The folder name.
        /// </param>
        /// <param name="fileName">
        ///     The file name.
        /// </param>
        /// <returns>
        ///     The <see cref="FileOperationStatus" />.
        /// </returns>
        /// <exception cref="NotImplementedException">
        ///     throws exception
        /// </exception>
        public FileOperationStatus DeleteFile(string folderName, string fileName)
        {
            Contract.Requires<InputValidationFailedException>(!string.IsNullOrEmpty(folderName), "folderName");
            Contract.Requires<InputValidationFailedException>(!string.IsNullOrEmpty(fileName), "fileName");
            throw new NotImplementedException();
        }

        /// <summary>
        ///     The delete file.
        /// </summary>
        /// <param name="fullyQualifiedFileName">
        ///     The fully qualified file name.
        /// </param>
        /// <returns>
        ///     The <see cref="FileOperationStatus" />.
        /// </returns>
        /// <exception cref="NotImplementedException">
        ///     throws exception
        /// </exception>
        public FileOperationStatus DeleteFile(Uri fullyQualifiedFileName)
        {
            Contract.Requires<InputValidationFailedException>(null != fullyQualifiedFileName, "fullyQualifiedFileName");
            throw new NotImplementedException();
        }

        /// <summary>
        ///     The delete folder.
        /// </summary>
        /// <param name="folderName">
        ///     The folder name.
        /// </param>
        /// <returns>
        ///     The <see cref="FileOperationStatus" />.
        /// </returns>
        /// <exception cref="NotImplementedException">
        ///     throws exception
        /// </exception>
        public FileOperationStatus DeleteFolder(string folderName)
        {
            Contract.Requires<InputValidationFailedException>(!string.IsNullOrEmpty(folderName), "folderName");
            throw new NotImplementedException();
        }

        /// <summary>
        ///     The download file.
        /// </summary>
        /// <param name="folderName">
        ///     The folder name.
        /// </param>
        /// <param name="fileName">
        ///     The file name.
        /// </param>
        /// <param name="targetFilePath">
        ///     The target file path.
        /// </param>
        /// <returns>
        ///     The <see cref="FileOperationStatus" />.
        /// </returns>
        /// <exception cref="NotImplementedException">
        ///     Not implemented by contract class
        /// </exception>
        public FileOperationStatus DownloadFile(string folderName, string fileName, string targetFilePath)
        {
            Contract.Requires<InputValidationFailedException>(!string.IsNullOrEmpty(folderName), "folderName");
            Contract.Requires<InputValidationFailedException>(!string.IsNullOrEmpty(fileName), "fileName");
            Contract.Requires<InputValidationFailedException>(!string.IsNullOrEmpty(targetFilePath), "targetFilePath");
            throw new NotImplementedException();
        }

        /// <summary>
        ///     The enlist documents.
        /// </summary>
        /// <param name="folderName">
        ///     The folder name.
        /// </param>
        /// <returns>
        ///     The <see cref="IEnumerable" />.
        /// </returns>
        /// <exception cref="NotImplementedException">
        ///     throws exception
        /// </exception>
        public IEnumerable<Document> EnlistDocuments(string folderName)
        {
            Contract.Requires<InputValidationFailedException>(!string.IsNullOrEmpty(folderName), "folderName");
            throw new NotImplementedException();
        }

        /// <summary>
        ///     The enlist files in subdirectory.
        /// </summary>
        /// <param name="folderName">
        ///     The folder name.
        /// </param>
        /// <param name="directoryName">
        ///     The directory name.
        /// </param>
        /// <returns>
        ///     The <see cref="IEnumerable" />.
        /// </returns>
        /// <exception cref="NotImplementedException">
        ///     throws exception
        /// </exception>
        public IEnumerable<Document> EnlistFilesInSubdirectory(string folderName, string directoryName)
        {
            Contract.Requires<InputValidationFailedException>(!string.IsNullOrEmpty(folderName), "folderName");
            Contract.Requires<InputValidationFailedException>(!string.IsNullOrEmpty(directoryName), "directoryName");
            throw new NotImplementedException();
        }

        /// <summary>
        ///     The enlist folders.
        /// </summary>
        /// <returns>
        ///     The <see cref="IEnumerable" />.
        /// </returns>
        /// <exception cref="NotImplementedException">
        ///     throws exception
        /// </exception>
        public IEnumerable<Folder> EnlistFolders()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     The enlist sub directories.
        /// </summary>
        /// <param name="folderName">
        ///     The folder name.
        /// </param>
        /// <returns>
        ///     The <see cref="IEnumerable" />.
        /// </returns>
        /// <exception cref="NotImplementedException">
        ///     throws exception
        /// </exception>
        public IEnumerable<string> EnlistSubdirectories(string folderName)
        {
            Contract.Requires<InputValidationFailedException>(!string.IsNullOrEmpty(folderName), "folderName");
            throw new NotImplementedException();
        }

        /// <summary>
        ///     The get time bound file read access.
        /// </summary>
        /// <param name="folderName">
        ///     The folder name.
        /// </param>
        /// <param name="fileName">
        ///     The file name.
        /// </param>
        /// <param name="fileReadAccessPeriod">
        ///     The file read access period.
        /// </param>
        /// <returns>
        ///     The <see cref="Uri" />.
        /// </returns>
        /// <exception cref="NotImplementedException">
        ///     throws exception
        /// </exception>
        public Uri GetTimeBoundFileReadAccess(string folderName, string fileName, TimeSpan fileReadAccessPeriod)
        {
            Contract.Requires<InputValidationFailedException>(!string.IsNullOrEmpty(folderName), "folderName");
            Contract.Requires<InputValidationFailedException>(!string.IsNullOrEmpty(fileName), "fileName");
            Contract.Requires<InputValidationFailedException>(null != fileReadAccessPeriod, "fileReadAccessPeriod");
            throw new NotImplementedException();
        }

        /// <summary>
        ///     The get time bound file read access.
        /// </summary>
        /// <param name="fullyQualifiedFileName">
        ///     The fully qualified file name.
        /// </param>
        /// <param name="fileReadAccessPeriod">
        ///     The file read access period.
        /// </param>
        /// <returns>
        ///     The <see cref="Uri" />.
        /// </returns>
        /// <exception cref="NotImplementedException">
        ///     throws exception
        /// </exception>
        public Uri GetTimeBoundFileReadAccess(Uri fullyQualifiedFileName, TimeSpan fileReadAccessPeriod)
        {
            Contract.Requires<InputValidationFailedException>(null != fullyQualifiedFileName, "fullyQualifiedFileName");
            Contract.Requires<InputValidationFailedException>(null != fileReadAccessPeriod, "fileReadAccessPeriod");
            throw new NotImplementedException();
        }

        /// <summary>
        ///     The acquire lease on file.
        /// </summary>
        /// <param name="folderName">
        ///     The folder name.
        /// </param>
        /// <param name="fileName">
        ///     The file name.
        /// </param>
        /// <param name="requestedLeaseIdentifier">
        ///     The requested lease identifier.
        /// </param>
        /// <param name="leaseDuration">
        ///     The lease duration.
        /// </param>
        /// <returns>
        ///     The <see cref="FileLeaseStatus" />.
        /// </returns>
        /// <exception cref="NotImplementedException">
        ///     throws exception
        /// </exception>
        public FileLeaseStatus AcquireLeaseOnFile(
            string folderName,
            string fileName,
            string requestedLeaseIdentifier,
            TimeSpan leaseDuration)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(folderName), "folderName");
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(fileName), "fileName");
            Contract.Requires<ArgumentNullException>(
                !string.IsNullOrEmpty(requestedLeaseIdentifier),
                "requestedLeaseIdentifier");
            Contract.Requires<ArgumentNullException>(null != leaseDuration, "leaseDuration");
            throw new NotImplementedException();
        }

        /// <summary>
        ///     The acquire lease on file.
        /// </summary>
        /// <param name="fullyQualifiedFileName">
        ///     The fully qualified file name.
        /// </param>
        /// <param name="requestedLeaseIdentifier">
        ///     The requested lease identifier.
        /// </param>
        /// <param name="leaseDuration">
        ///     The lease duration.
        /// </param>
        /// <returns>
        ///     The <see cref="FileLeaseStatus" />.
        /// </returns>
        /// <exception cref="NotImplementedException">
        ///     throws exception
        /// </exception>
        public FileLeaseStatus AcquireLeaseOnFile(
            Uri fullyQualifiedFileName,
            string requestedLeaseIdentifier,
            TimeSpan leaseDuration)
        {
            Contract.Requires<InputValidationFailedException>(null != fullyQualifiedFileName, "fullyQualifiedFileName");
            Contract.Requires<InputValidationFailedException>(
                !string.IsNullOrEmpty(requestedLeaseIdentifier),
                "requestedLeaseIdentifier");
            Contract.Requires<InputValidationFailedException>(null != leaseDuration, "leaseDuration");
            throw new NotImplementedException();
        }

        /// <summary>
        ///     The break file lease.
        /// </summary>
        /// <param name="folderName">
        ///     The folder name.
        /// </param>
        /// <param name="fileName">
        ///     The file name.
        /// </param>
        /// <returns>
        ///     The <see cref="FileLeaseState" />.
        /// </returns>
        /// <exception cref="NotImplementedException">
        ///     throws exception
        /// </exception>
        public FileLeaseState BreakFileLease(string folderName, string fileName)
        {
            Contract.Requires<InputValidationFailedException>(!string.IsNullOrEmpty(folderName), "folderName");
            Contract.Requires<InputValidationFailedException>(!string.IsNullOrEmpty(fileName), "fileName");
            throw new NotImplementedException();
        }

        /// <summary>
        ///     The break file lease.
        /// </summary>
        /// <param name="fullyQualifiedFileName">
        ///     The fully qualified file name.
        /// </param>
        /// <returns>
        ///     The <see cref="FileLeaseState" />.
        /// </returns>
        /// <exception cref="NotImplementedException">
        ///     throws exception
        /// </exception>
        public FileLeaseState BreakFileLease(Uri fullyQualifiedFileName)
        {
            Contract.Requires<InputValidationFailedException>(null != fullyQualifiedFileName, "fullyQualifiedFileName");
            throw new NotImplementedException();
        }

        /// <summary>
        ///     The release file lease.
        /// </summary>
        /// <param name="folderName">
        ///     The folder name.
        /// </param>
        /// <param name="fileName">
        ///     The file name.
        /// </param>
        /// <param name="existingLeaseIdentifier">
        ///     The existing lease identifier.
        /// </param>
        /// <returns>
        ///     The <see cref="FileLeaseState" />.
        /// </returns>
        /// <exception cref="NotImplementedException">
        ///     throws exception
        /// </exception>
        public FileLeaseState ReleaseFileLease(string folderName, string fileName, string existingLeaseIdentifier)
        {
            Contract.Requires<InputValidationFailedException>(!string.IsNullOrEmpty(folderName), "folderName");
            Contract.Requires<InputValidationFailedException>(!string.IsNullOrEmpty(fileName), "fileName");
            Contract.Requires<InputValidationFailedException>(
                !string.IsNullOrEmpty(existingLeaseIdentifier),
                "existingLeaseIdentifier");
            throw new NotImplementedException();
        }

        /// <summary>
        ///     The release file lease.
        /// </summary>
        /// <param name="fullyQualifiedFileName">
        ///     The fully qualified file name.
        /// </param>
        /// <param name="existingLeaseIdentifier">
        ///     The existing lease identifier.
        /// </param>
        /// <returns>
        ///     The <see cref="FileLeaseState" />.
        /// </returns>
        /// <exception cref="NotImplementedException">
        ///     throws exception
        /// </exception>
        public FileLeaseState ReleaseFileLease(Uri fullyQualifiedFileName, string existingLeaseIdentifier)
        {
            Contract.Requires<InputValidationFailedException>(null != fullyQualifiedFileName, "fullyQualifiedFileName");
            Contract.Requires<InputValidationFailedException>(
                !string.IsNullOrEmpty(existingLeaseIdentifier),
                "existingLeaseIdentifier");
            throw new NotImplementedException();
        }

        /// <summary>
        ///     The renew lease on file.
        /// </summary>
        /// <param name="folderName">
        ///     The folder name.
        /// </param>
        /// <param name="fileName">
        ///     The file name.
        /// </param>
        /// <param name="existingLeaseIdentifier">
        ///     The existing lease identifier.
        /// </param>
        /// <returns>
        ///     The <see cref="FileLeaseState" />.
        /// </returns>
        /// <exception cref="NotImplementedException">
        ///     throws exception
        /// </exception>
        public FileLeaseState RenewLeaseOnFile(string folderName, string fileName, string existingLeaseIdentifier)
        {
            Contract.Requires<InputValidationFailedException>(!string.IsNullOrEmpty(folderName), "folderName");
            Contract.Requires<InputValidationFailedException>(!string.IsNullOrEmpty(fileName), "fileName");
            Contract.Requires<InputValidationFailedException>(
                !string.IsNullOrEmpty(existingLeaseIdentifier),
                "existingLeaseIdentifier");
            throw new NotImplementedException();
        }

        /// <summary>
        ///     The renew lease on file.
        /// </summary>
        /// <param name="fullyQualifiedFileName">
        ///     The fully qualified file name.
        /// </param>
        /// <param name="existingLeaseIdentifier">
        ///     The existing lease identifier.
        /// </param>
        /// <returns>
        ///     The <see cref="FileLeaseState" />.
        /// </returns>
        /// <exception cref="NotImplementedException">
        ///     throws exception
        /// </exception>
        public FileLeaseState RenewLeaseOnFile(Uri fullyQualifiedFileName, string existingLeaseIdentifier)
        {
            Contract.Requires<InputValidationFailedException>(null != fullyQualifiedFileName, "fullyQualifiedFileName");
            Contract.Requires<InputValidationFailedException>(
                !string.IsNullOrEmpty(existingLeaseIdentifier),
                "existingLeaseIdentifier");

            throw new NotImplementedException();
        }

        #endregion

        #region Explicit Interface Methods

        /// <summary>
        ///     The download file.
        /// </summary>
        /// <param name="folderName">
        ///     The folder name.
        /// </param>
        /// <param name="fileName">
        ///     The file name.
        /// </param>
        /// <returns>
        ///     The <see cref="DataFile" />.
        /// </returns>
        /// <exception cref="NotImplementedException">
        ///     Not implemented by contract class
        /// </exception>
        DataFile IFileRepository.DownloadFile(string folderName, string fileName)
        {
            Contract.Requires<InputValidationFailedException>(!string.IsNullOrEmpty(folderName), "folderName");
            Contract.Requires<InputValidationFailedException>(!string.IsNullOrEmpty(fileName), "fileName");
            throw new NotImplementedException();
        }

        /// <summary>
        ///     The download file.
        /// </summary>
        /// <param name="fullyQualifiedFileName">
        ///     The fully qualified file name.
        /// </param>
        /// <returns>
        ///     The <see cref="DataFile" />.
        /// </returns>
        /// <exception cref="NotImplementedException">
        ///     Not implemented by contract class
        /// </exception>
        DataFile IFileRepository.DownloadFile(Uri fullyQualifiedFileName)
        {
            Contract.Requires<InputValidationFailedException>(null != fullyQualifiedFileName, "fullyQualifiedFileName");
            throw new NotImplementedException();
        }

        #endregion
    }
}