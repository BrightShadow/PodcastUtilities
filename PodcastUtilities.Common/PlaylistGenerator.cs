﻿using System;
using System.Linq;
using System.IO;
using PodcastUtilities.Common.Platform;

namespace PodcastUtilities.Common
{
    /// <summary>
    /// generate a playlist
    /// </summary>
    public class PlaylistGenerator
    {
    	/// <summary>
    	/// create a playlist generator
    	/// </summary>
    	/// <param name="fileFinder">abstract access to the file system to find the files for the playlist</param>
        /// <param name="fileUtilities">abstract file utilities</param>
    	/// <param name="playlistFactory">factpry to generate the correct playlist object depending upon the selected format</param>
        public PlaylistGenerator(
			IFileFinder fileFinder,
			IFileUtilities fileUtilities,
			IPlaylistFactory playlistFactory)
    	{
    		FileFinder = fileFinder;
    		FileUtilities = fileUtilities;
    		PlaylistFactory = playlistFactory;
    	}

        /// <summary>
        /// event that is fired when the playlist is generated or copied
        /// </summary>
        public event EventHandler<StatusUpdateEventArgs> StatusUpdate;

    	private IFileFinder FileFinder { get; set; }
    	private IFileUtilities FileUtilities { get; set; }
    	private IPlaylistFactory PlaylistFactory { get; set; }

    	private void OnStatusUpdate(string message)
        {
            OnStatusUpdate(new StatusUpdateEventArgs(StatusUpdateEventArgs.Level.Status, message));
        }

        private void OnStatusUpdate(StatusUpdateEventArgs e)
        {
            if (StatusUpdate != null)
                StatusUpdate(this, e);
        }

        /// <summary>
        /// generate a playlist
        /// </summary>
        /// <param name="control">control file to use to find the destinationRoot, and playlist format</param>
        /// <param name="copyToDestination">true to copy the playlist to the destination, false to write it locally</param>
        public void GeneratePlaylist(IControlFile control, bool copyToDestination)
        {
			var allDestFiles = control.Podcasts.SelectMany(
        		podcast => FileFinder.GetFiles(Path.Combine(control.DestinationRoot, podcast.Folder), podcast.Pattern));

			IPlaylist p = PlaylistFactory.CreatePlaylist(control.PlaylistFormat, control.PlaylistFilename);

            foreach (IFileInfo thisFile in allDestFiles)
            {
                string thisRelativeFile = thisFile.FullName;
                string absRoot = Path.GetFullPath(control.DestinationRoot);
                if (thisRelativeFile.StartsWith(absRoot))
                {
                    thisRelativeFile = thisRelativeFile.Substring(absRoot.Length);
                }
                p.AddTrack("." + thisRelativeFile);
            }

            p.SaveFile();

            if (copyToDestination)
            {
                string destPlaylist = Path.Combine(control.DestinationRoot, control.PlaylistFilename);
                OnStatusUpdate(string.Format("Copying Playlist with {0} items to {1}", p.NumberOfTracks, destPlaylist));
				FileUtilities.FileCopy(control.PlaylistFilename, destPlaylist, true);
            }
            else
            {
                OnStatusUpdate(string.Format("Playlist with {0} items generated: {1}", p.NumberOfTracks, control.PlaylistFilename));
            }
        }
    }
}
