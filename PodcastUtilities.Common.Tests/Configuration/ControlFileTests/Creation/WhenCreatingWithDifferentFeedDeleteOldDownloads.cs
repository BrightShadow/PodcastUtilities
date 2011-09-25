﻿using System;
using System.Linq;
using System.Xml;
using NUnit.Framework;
using PodcastUtilities.Common.Configuration;

namespace PodcastUtilities.Common.Tests.Configuration.ControlFileTests.Creation
{
    public abstract class WhenCreatingAControlfileWithDifferentFeedDeleteOldDownloads : WhenTestingAControlFile
    {
        protected string FeedDeleteDaysOldText { get; set; }
        protected int MaxDaysOld1 { get; set; }
        protected int MaxDaysOld2 { get; set; }
        protected Exception ThrownException { get; set; }

        protected override void GivenThat()
        {
            base.GivenThat();

            XmlNode n = ControlFileXmlDocument.SelectSingleNode("podcasts/global/feed/deleteDownloadsDaysOld");
            n.InnerText = FeedDeleteDaysOldText;
        }

        protected override void When()
        {
            ThrownException = null;
            try
            {
                ControlFile = new ReadOnlyControlFile(ControlFileXmlDocument);

                // this will be the defaulted value from the global section
                MaxDaysOld1 = ControlFile.GetPodcasts().ElementAt(1).Feed.DeleteDownloadsDaysOld.Value;
                // this is set explicitly by the feed
                MaxDaysOld2 = ControlFile.GetPodcasts().ElementAt(2).Feed.DeleteDownloadsDaysOld.Value;
            }
            catch (Exception exception)
            {
                ThrownException = exception;
            }
        }
    }


    public class WhenCreatingAControlFileWithNoDefaultDeleteDaysOld : WhenCreatingAControlfileWithDifferentFeedDeleteOldDownloads
    {
        protected override void GivenThat()
        {
            FeedDeleteDaysOldText = "";
            base.GivenThat();
        }

        [Test]
        public void ItShouldNotThorw()
        {
            Assert.That(ThrownException, Is.Null);
        }

        [Test]
        public void ItShouldReadTheValue()
        {
            Assert.That(MaxDaysOld1, Is.EqualTo(int.MaxValue));
            Assert.That(MaxDaysOld2, Is.EqualTo(14));
        }
    }

    public class WhenCreatingAControlFileWithDefaultDeleteDaysOld : WhenCreatingAControlfileWithDifferentFeedDeleteOldDownloads
    {
        protected override void GivenThat()
        {
            FeedDeleteDaysOldText = "88";
            base.GivenThat();
        }

        [Test]
        public void ItShouldNotThorw()
        {
            Assert.That(ThrownException, Is.Null);
        }

        [Test]
        public void ItShouldReadTheValue()
        {
            Assert.That(MaxDaysOld1, Is.EqualTo(88));
            Assert.That(MaxDaysOld2, Is.EqualTo(14));
        }
    }

    public class WhenCreatingAControlFileWithDefaultDeleteDaysOldZero : WhenCreatingAControlfileWithDifferentFeedDeleteOldDownloads
    {
        protected override void GivenThat()
        {
            FeedDeleteDaysOldText = "0";
            base.GivenThat();
        }

        [Test]
        public void ItShouldNotThorw()
        {
            Assert.That(ThrownException, Is.Null);
        }

        [Test]
        public void ItShouldReadTheValue()
        {
            Assert.That(MaxDaysOld1, Is.EqualTo(0));
            Assert.That(MaxDaysOld2, Is.EqualTo(14));
        }
    }


}