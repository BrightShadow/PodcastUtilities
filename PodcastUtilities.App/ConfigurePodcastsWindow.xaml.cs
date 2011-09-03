﻿using System.Windows;
using PodcastUtilities.Common;
using PodcastUtilities.Common.Configuration;
using PodcastUtilities.Presentation.Services;
using PodcastUtilities.Presentation.ViewModels;

namespace PodcastUtilities.App
{
    /// <summary>
    /// Interaction logic for ConfigurePodcastsWindow.xaml
    /// </summary>
    public partial class ConfigurePodcastsWindow : Window
    {
        public ConfigurePodcastsWindow()
        {
            InitializeComponent();

        	var viewModel = new ConfigurePodcastsViewModel(
				AppIocContainer.Container.Resolve<IApplicationService>(),
				AppIocContainer.Container.Resolve<IBrowseForFileService>(),
				AppIocContainer.Container.Resolve<IDialogService>(),
				AppIocContainer.Container.Resolve<IControlFileFactory>(),
                AppIocContainer.Container.Resolve<IPodcastFactory>(),
                AppIocContainer.Container.Resolve<IClipboardService>());

            DataContext = viewModel;
        }
    }
}
