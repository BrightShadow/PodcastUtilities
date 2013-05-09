﻿using System;
using System.IO;
using NUnit.Framework;
using PodcastUtilities.Common.Feeds;

namespace PodcastUtilities.Common.Tests.Feeds.EpisodeFinderTests
{
    public class WhenFindingEpisodesInAFeedWithOutOfDateEpisodes : WhenUsingTheEpisodeFinder
    {
        protected override void SetupData()
        {
            base.SetupData();

            _feedInfo.MaximumDaysOld.Value = 35;

            _podcastFeedItems.Add(new PodcastFeedItem()
                                      {
                                          Address = new Uri("http://test/podcast.mp3"),
                                          EpisodeTitle = "TestEpisode",
                                          Published = _now.AddMonths(-2)
                                      });
            _podcastFeedItems.Add(new PodcastFeedItem()
                                      {
                                          Address = new Uri("http://test/podcast2.mp3"),
                                          EpisodeTitle = "TestEpisode2",
                                          Published = _now.AddMonths(-1)
                                      });
        }

        protected override void When()
        {
            _episodesToSync = _episodeFinder.FindEpisodesToDownload(_rootFolder, _retryWaitTime, _podcastInfo, _retainFeedXml);
        }

        [Test]
        public void ItShouldReturnTheList()
        {
            Assert.That(_episodesToSync.Count, Is.EqualTo(1));
        }

        [Test]
        public void ItShouldReturnTheUrl()
        {
            Assert.That(_episodesToSync[0].EpisodeUrl.ToString(), Is.EqualTo("http://test/podcast2.mp3"));
        }

        [Test]
        public void ItShouldReturnTheDestinationPath()
        {
            Assert.That(_episodesToSync[0].DestinationPath, Is.EqualTo(Path.Combine(Path.Combine(_rootFolder, _podcastInfo.Folder), "podcast2.mp3")));
        }

        [Test]
        public void ItShouldReturnTheRetryWaitTimeInSeconds()
        {
            Assert.That(_episodesToSync[0].RetryWaitTimeInSeconds, Is.EqualTo(_retryWaitTime));
        }

        [Test]
        public void ItShouldSetTheRetryTime()
        {
            foreach (var episode in _episodesToSync)
            {
                Assert.That(episode.RetryWaitTimeInSeconds,Is.EqualTo(_retryWaitTime));
            }
        }
    }
}