﻿using System;
using PodcastUtilities.Common;
using PodcastUtilities.Common.Configuration;
using PodcastUtilities.Common.Feeds;
using PodcastUtilities.Common.Files;
using PodcastUtilities.Common.Perfmon;
using PodcastUtilities.Common.Platform;
using PodcastUtilities.Common.Playlists;

namespace PodcastUtilities.Ioc
{
	public static class IocRegistration
	{
        [CLSCompliant(false)]
        public static void RegisterFileServices(IIocContainer container)
		{
			container.Register<IDriveInfoProvider, SystemDriveInfoProvider>();
			container.Register<IDirectoryInfoProvider, SystemDirectoryInfoProvider>();
			container.Register<IFileUtilities, FileUtilities>();
			container.Register<ICopier, Copier>();
			container.Register<IFinder, Finder>();
			container.Register<ISorter, Sorter>();
            container.Register<IUnwantedFileRemover, UnwantedFileRemover>();
            container.Register<IEpisodePurger, EpisodePurger>();
            container.Register<IControlFileFactory, ControlFileFactory>();
        }

        [CLSCompliant(false)]
        public static void RegisterSystemServices(IIocContainer container)
        {
            container.Register<ITimeProvider, SystemDateTimeProvider>();
            container.Register<IPerfmonCounterUtilities, SystemPerfmonCounterUtilities>();
            container.Register<IPerfmonCounterCreationDataProvider, SystemPerfmonCounterCreationDataProvider>();
            container.Register<ICategoryInstaller, CategoryInstaller>();
            container.Register<ICounterFactory, CounterFactory>();
        }

        [CLSCompliant(false)]
        public static void RegisterPlaylistServices(IIocContainer container)
        {
            container.Register<IPlaylistFactory, PlaylistFactory>();
        }

        [CLSCompliant(false)]
        public static void RegisterPodcastServices(IIocContainer container)
        {
            container.Register<IPodcastFactory, PodcastFactory>();
        }

        [CLSCompliant(false)]
        public static void RegisterFeedServices(IIocContainer container)
        {
            container.Register<IPodcastFeedFactory, PodcastFeedFactory>();
            container.Register<IWebClientFactory, WebClientFactory>();
            container.Register<IEpisodeFinder, EpisodeFinder>();
            container.Register<ISyncItemToEpisodeDownloaderTaskConverter, SyncItemToEpisodeDownloaderTaskConverter>();
            container.Register<IEpisodeDownloaderFactory, EpisodeDownloaderFactory>();
            container.Register<ITaskPool, TaskPool>();
            container.Register<IStateProvider, StateProvider>();
        }
    }
}
