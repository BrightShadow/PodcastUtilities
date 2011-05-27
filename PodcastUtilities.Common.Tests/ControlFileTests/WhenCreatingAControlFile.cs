﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace PodcastUtilities.Common.Tests.ControlFileTests
{
    public class WhenCreatingAControlFile : WhenTestingAControlFile
    {
        protected override void When()
        {
            ControlFile = new ControlFile(ControlFileXmlDocument);
        }

        [Test]
        public void ItShouldCreateAnObject()
        {
            Assert.That(ControlFile, Is.Not.Null);
        }

        [Test]
        public void ItShouldReadThePodcasts()
        {
            Assert.That(ControlFile.Podcasts.Count, Is.EqualTo(3));

            Assert.That(ControlFile.Podcasts[0].Feed, Is.Null);
            Assert.That(ControlFile.Podcasts[0].Folder, Is.EqualTo("Test Match Special"));
            Assert.That(ControlFile.Podcasts[0].MaximumNumberOfFiles, Is.EqualTo(-1));
            Assert.That(ControlFile.Podcasts[0].Pattern, Is.EqualTo("*.mp3"));
            Assert.That(ControlFile.Podcasts[0].SortField, Is.EqualTo("name"));
            Assert.That(ControlFile.Podcasts[0].AscendingSort, Is.True);

            Assert.That(ControlFile.Podcasts[1].Feed.Address.ToString(), Is.EqualTo("http://www.hanselminutes.com/hanselminutes_MP3Direct.xml"));
            Assert.That(ControlFile.Podcasts[1].Feed.Format, Is.EqualTo(PodcastFeedFormat.RSS));
            Assert.That(ControlFile.Podcasts[1].Feed.MaximumDaysOld, Is.EqualTo(11));
            Assert.That(ControlFile.Podcasts[1].Folder, Is.EqualTo("Hanselminutes"));
            Assert.That(ControlFile.Podcasts[1].MaximumNumberOfFiles, Is.EqualTo(34));
            Assert.That(ControlFile.Podcasts[1].Pattern, Is.EqualTo("*.mp3"));
            Assert.That(ControlFile.Podcasts[1].SortField, Is.EqualTo("fred"));
            Assert.That(ControlFile.Podcasts[1].AscendingSort, Is.False);

            Assert.That(ControlFile.Podcasts[2].Feed.MaximumDaysOld, Is.EqualTo(int.MaxValue));
        }

        [Test]
        public void ItShouldReadTheGlobalInformation()
        {
            Assert.That(ControlFile.SourceRoot,Is.EqualTo(".\\profile\\iPodder\\downloads"));
            Assert.That(ControlFile.DestinationRoot,Is.EqualTo("W:\\Podcasts"));
            Assert.That(ControlFile.PlaylistFilename,Is.EqualTo("podcasts.wpl"));
            Assert.That(ControlFile.FreeSpaceToLeaveOnDestination,Is.EqualTo(2000));
            Assert.That(ControlFile.PlaylistFormat,Is.EqualTo(PlaylistFormat.WPL));
        }
    }
}
