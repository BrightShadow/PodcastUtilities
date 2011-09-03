using PodcastUtilities.Common;
using PodcastUtilities.Common.Configuration;
using PodcastUtilities.Common.Tests;
using PodcastUtilities.Presentation.Services;
using PodcastUtilities.Presentation.ViewModels;

namespace PodcastUtilities.Presentation.Tests.ViewModels.ConfigurePodcastsViewModelTests
{
	public abstract class WhenTestingConfigurePodcastsViewModel
		: WhenTestingBehaviour
	{
		protected ConfigurePodcastsViewModel ViewModel { get; set; }

		protected IApplicationService ApplicationService { get; set; }

		protected IBrowseForFileService BrowseForFileService { get; set; }

		protected IDialogService DialogService { get; set; }

		protected IControlFileFactory ControlFileFactory { get; set; }

		protected IPodcastFactory PodcastFactory { get; set; }

		protected IClipboardService ClipboardService { get; set; }

		protected override void GivenThat()
		{
			base.GivenThat();

			ApplicationService = GenerateMock<IApplicationService>();
			BrowseForFileService = GenerateMock<IBrowseForFileService>();
			DialogService = GenerateMock<IDialogService>();
			ControlFileFactory = GenerateMock<IControlFileFactory>();
			PodcastFactory = GenerateMock<IPodcastFactory>();
            ClipboardService = GenerateMock<IClipboardService>();

			ViewModel = new ConfigurePodcastsViewModel(
				ApplicationService,
				BrowseForFileService,
				DialogService,
				ControlFileFactory,
                PodcastFactory,
                ClipboardService);
		}
	}
}