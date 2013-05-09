﻿using System;
using System.Globalization;
using System.Linq;
using System.IO;
using PodcastUtilities.Common.Configuration;
using PodcastUtilities.Common.Files;
using PodcastUtilities.Common.Platform;

namespace PodcastUtilities.Common.Playlists
{
    /// <summary>
    /// generate a playlist
    /// </summary>
    public class Generator
    {
    	/// <summary>
    	/// create a playlist generator
    	/// </summary>
    	/// <param name="fileFinder">abstract access to the file system to find the files for the playlist</param>
        /// <param name="fileUtilities">abstract file utilities</param>
    	/// <param name="playlistFactory">factpry to generate the correct playlist object depending upon the selected format</param>
        public Generator(
			IFinder fileFinder,
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

    	private IFinder FileFinder { get; set; }
    	private IFileUtilities FileUtilities { get; set; }
    	private IPlaylistFactory PlaylistFactory { get; set; }

    	private void OnStatusUpdate(string message)
        {
            OnStatusUpdate(new StatusUpdateEventArgs(StatusUpdateLevel.Status, message));
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
        public void GeneratePlaylist(IReadOnlyControlFile control, bool copyToDestination)
        {
			var allDestFiles = control.GetPodcasts().SelectMany(
        		podcast => FileFinder.GetFiles(Path.Combine(control.GetDestinationRoot(), podcast.Folder), podcast.Pattern.Value));

			IPlaylist p = PlaylistFactory.CreatePlaylist(control.GetPlaylistFormat(), control.GetPlaylistFileName());

            foreach (IFileInfo thisFile in allDestFiles)
            {
                string thisRelativeFile = thisFile.FullName;
                string absRoot = Path.GetFullPath(control.GetDestinationRoot());
                if (thisRelativeFile.StartsWith(absRoot,StringComparison.Ordinal))
                {
                    thisRelativeFile = thisRelativeFile.Substring(absRoot.Length);
                }
                p.AddTrack("." + thisRelativeFile);
            }

            if (copyToDestination)
            {
                string destPlaylist = Path.Combine(control.GetDestinationRoot(), control.GetPlaylistFileName());
                OnStatusUpdate(string.Format(CultureInfo.InvariantCulture,"Copying Playlist with {0} items to {1}", p.NumberOfTracks, destPlaylist));
                p.SaveFile(destPlaylist);
            }
            else
            {
                p.SaveFile();
                OnStatusUpdate(string.Format(CultureInfo.InvariantCulture, "Playlist with {0} items generated: {1}", p.NumberOfTracks, control.GetPlaylistFileName()));
            }
        }
    }
}