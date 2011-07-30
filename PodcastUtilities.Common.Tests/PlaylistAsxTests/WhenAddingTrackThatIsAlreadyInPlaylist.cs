using NUnit.Framework;

namespace PodcastUtilities.Common.Tests.PlaylistAsxTests
{
	public class WhenAddingTrackThatIsAlreadyInPlaylist
		: WhenTestingBehaviour
	{
		protected PlaylistAsx Playlist { get; set; }

		protected bool Added { get; set; }

		protected override void GivenThat()
		{
			base.GivenThat();

			Playlist = new PlaylistAsx("MyPodcastPlaylist.asx", true);
			Playlist.AddTrack(@"c:\podcasts\1.mp3");
		}

		protected override void When()
		{
			Added = Playlist.AddTrack(@"c:\podcasts\1.mp3");
		}

		[Test]
		public void ItShouldReturnFalseFromAddTrack()
		{
			Assert.IsFalse(Added);
		}

		[Test]
		public void ItShouldNotInsertTheXmlTwice()
		{
			var podcastNodes = Playlist.SelectNodes(@"ASX/ENTRY/REF[@HREF = 'c:\podcasts\1.mp3']");

			Assert.IsNotNull(podcastNodes);
			Assert.AreEqual(1, podcastNodes.Count);
		}
	}
}