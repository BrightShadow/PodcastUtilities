﻿using System;
using System.IO;
using PodcastUtilities.Common.Platform.Mtp;
using PodcastUtilities.PortableDevices;

namespace PodcastUtilities.Common.Platform
{
    /// <summary>
    /// Utility methods to manipulate files in the physical file system.
    /// This class abstracts away the file system from the main body of code - it also 
    /// copes with MTP file systems.
    /// </summary>
    public class FileSystemAwareFileUtilities : IFileUtilities
    {
        private readonly IFileUtilities _fileUtilities;
        private readonly IDeviceManager _deviceManager;
        private readonly IStreamHelper _streamHelper;

        ///<summary>
        /// Construct the object
        ///</summary>
        public FileSystemAwareFileUtilities(IDeviceManager deviceManager, IStreamHelper streamHelper)
            : this(new FileUtilities(), deviceManager, streamHelper)
        {
            
        }

        internal FileSystemAwareFileUtilities(
            IFileUtilities fileUtilities,
            IDeviceManager deviceManager,
            IStreamHelper streamHelper)
        {
            _fileUtilities = fileUtilities;
            _deviceManager = deviceManager;
            _streamHelper = streamHelper;
        }

        /// <summary>
        /// check if a file exists
        /// </summary>
        /// <param name="path">pathname to check</param>
        /// <returns>true if the file exists</returns>
        public bool FileExists(string path)
        {
            var pathInfo = MtpPath.GetPathInfo(path);

            if (!pathInfo.IsMtpPath)
            {
                return _fileUtilities.FileExists(path);
            }

            var device = _deviceManager.GetDevice(pathInfo.DeviceName);
            if (device == null)
            {
                return false;
            }

            return (device.GetObjectFromPath(pathInfo.RelativePathOnDevice) != null);
        }

        /// <summary>
        /// rename / move a file
        /// </summary>
        /// <param name="sourceFileName">source pathname</param>
        /// <param name="destinationFileName">destination pathname</param>
        public void FileRename(string sourceFileName, string destinationFileName)
        {
            FileRename(sourceFileName, destinationFileName, false);
        }

        /// <summary>
        /// rename / move a file
        /// </summary>
        /// <param name="sourceFileName">source pathname</param>
        /// <param name="destinationFileName">destination pathname</param>
        /// <param name="allowOverwrite">set to true to overwrite an existing destination file</param>
        public void FileRename(string sourceFileName, string destinationFileName, bool allowOverwrite)
        {
            if (MtpPath.IsMtpPath(sourceFileName) || MtpPath.IsMtpPath(destinationFileName))
            {
                throw new NotImplementedException();
            }

            _fileUtilities.FileRename(sourceFileName, destinationFileName, allowOverwrite);
        }

        /// <summary>
        /// copy a file - will not overwrite an existing file
        /// the containing folder will be created if it does not exist
        /// </summary>
        /// <param name="sourceFileName">source pathname</param>
        /// <param name="destinationFileName">destination pathname</param>
        public void FileCopy(string sourceFileName, string destinationFileName)
        {
            FileCopy(sourceFileName, destinationFileName, false);
        }

        /// <summary>
        /// copy a file - the containing folder will be created if it does not exist
        /// </summary>
        /// <param name="sourceFileName">source pathname</param>
        /// <param name="destinationFileName">destination pathname</param>
        /// <param name="allowOverwrite">set to true to overwrite an existing file</param>
        public void FileCopy(string sourceFileName, string destinationFileName, bool allowOverwrite)
        {
            if (!MtpPath.IsMtpPath(sourceFileName) && !MtpPath.IsMtpPath(destinationFileName))
            {
                _fileUtilities.FileCopy(sourceFileName, destinationFileName, allowOverwrite);
            }
            else
            {
                using (var sourceStream = OpenReadStream(sourceFileName))
                {
                    using (var destinationStream = OpenWriteStream(destinationFileName, allowOverwrite))
                    {
                        _streamHelper.Copy(sourceStream, destinationStream);
                    }
                }
                
            }
        }

        /// <summary>
        /// delete a file
        /// </summary>
        /// <param name="path">pathname of the file to delete</param>
        public void FileDelete(string path)
        {
            var pathInfo = MtpPath.GetPathInfo(path);

            if (!pathInfo.IsMtpPath)
            {
                _fileUtilities.FileDelete(path);
                return;
            }

            var device = GetDevice(pathInfo);

            device.Delete(pathInfo.RelativePathOnDevice);
        }

        private IDevice GetDevice(MtpPathInfo pathInfo)
        {
            var device = _deviceManager.GetDevice(pathInfo.DeviceName);
            if (device == null)
            {
                throw new DirectoryNotFoundException(String.Format("Device [{0}] not found", pathInfo.DeviceName));
            }
            return device;
        }

        private Stream OpenReadStream(string filename)
        {
            var pathInfo = MtpPath.GetPathInfo(filename);

            if (pathInfo.IsMtpPath)
            {
                return GetDevice(pathInfo).OpenRead(pathInfo.RelativePathOnDevice);
            }

            return _streamHelper.OpenRead(filename);
        }

        private Stream OpenWriteStream(string filename, bool allowOverwrite)
        {
            var pathInfo = MtpPath.GetPathInfo(filename);

            if (pathInfo.IsMtpPath)
            {
                return GetDevice(pathInfo).OpenWrite(pathInfo.RelativePathOnDevice, allowOverwrite);
            }

            return _streamHelper.OpenWrite(filename, allowOverwrite);
        }
    }
}