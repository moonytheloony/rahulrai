#region



#endregion

namespace RahulRai.Websites.Utilities.AzureStorage.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.IO;
    using Common.Entities;

    #region

    

    #endregion

    /// <summary>
    ///     The FileRepository interface.
    /// </summary>
    [ContractClass(typeof (FileRepositoryContract))]
    public interface IFileRepository
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
        FileOperationStatus AddFileToFolder(string folderName, Stream fileStream, string fileName);

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
        FileOperationStatus AddFileToSubdirectory(
            string folderName,
            string directoryName,
            Stream fileStream,
            string fileName);

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
        FileOperationStatus CopyFileAcrossServer(
            string sourceFolder,
            string fileName,
            IContext targetContext,
            string targetFolder,
            string targetFileName);

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
        FileOperationStatus CopyFileAcrossServerSynchronous(
            string sourceFolder,
            string fileName,
            IContext targetContext,
            string targetFolder,
            string targetFileName);

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
        FileOperationStatus CopyFileWithinServer(
            string sourceFolder,
            string fileName,
            string targetFolder,
            string targetFileName);

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
        FileOperationStatus CopyFileWithinServerSynchronous(
            string sourceFolder,
            string fileName,
            string targetFolder,
            string targetFileName);

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
        FileOperationStatus CreateFolder(string folderName, VisibilityType visibilityType);

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
        FileOperationStatus CreateFolder(
            string folderName,
            VisibilityType visibilityType,
            IDictionary<string, string> folderMetadata);

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
        FileOperationStatus DeleteDirectory(string folderName, string directoryName);

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
        FileOperationStatus DeleteFile(string folderName, string fileName);

        /// <summary>
        ///     The delete file.
        /// </summary>
        /// <param name="fullyQualifiedFileName">
        ///     The fully qualified file name.
        /// </param>
        /// <returns>
        ///     The <see cref="FileOperationStatus" />.
        /// </returns>
        FileOperationStatus DeleteFile(Uri fullyQualifiedFileName);

        /// <summary>
        ///     The delete folder.
        /// </summary>
        /// <param name="folderName">
        ///     The folder name.
        /// </param>
        /// <returns>
        ///     The <see cref="FileOperationStatus" />.
        /// </returns>
        FileOperationStatus DeleteFolder(string folderName);

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
        ///     The <see cref="Stream" />.
        /// </returns>
        DataFile DownloadFile(string folderName, string fileName);

        /// <summary>
        ///     The download file.
        /// </summary>
        /// <param name="fullyQualifiedFileName">
        ///     The fully qualified file name.
        /// </param>
        /// <returns>
        ///     The <see cref="Stream" />.
        /// </returns>
        DataFile DownloadFile(Uri fullyQualifiedFileName);

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
        ///     The target File Path.
        /// </param>
        /// <returns>
        ///     The <see cref="FileOperationStatus" />.
        /// </returns>
        FileOperationStatus DownloadFile(string folderName, string fileName, string targetFilePath);

        /// <summary>
        ///     The enlist documents.
        /// </summary>
        /// <param name="folderName">
        ///     The folder name.
        /// </param>
        /// <returns>
        ///     The <see cref="IEnumerable" />.
        /// </returns>
        IEnumerable<Document> EnlistDocuments(string folderName);

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
        IEnumerable<Document> EnlistFilesInSubdirectory(string folderName, string directoryName);

        /// <summary>
        ///     The enlist folders.
        /// </summary>
        /// <returns>
        ///     The <see cref="IEnumerable" />.
        /// </returns>
        IEnumerable<Folder> EnlistFolders();

        /// <summary>
        ///     The enlist sub directories.
        /// </summary>
        /// <param name="folderName">
        ///     The folder name.
        /// </param>
        /// <returns>
        ///     The <see cref="IEnumerable" />.
        /// </returns>
        IEnumerable<string> EnlistSubdirectories(string folderName);

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
        Uri GetTimeBoundFileReadAccess(string folderName, string fileName, TimeSpan fileReadAccessPeriod);

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
        Uri GetTimeBoundFileReadAccess(Uri fullyQualifiedFileName, TimeSpan fileReadAccessPeriod);

        #endregion
    }
}