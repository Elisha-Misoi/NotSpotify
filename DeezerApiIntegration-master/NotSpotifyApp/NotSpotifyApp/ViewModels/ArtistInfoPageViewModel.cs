﻿using NotSpotifyApp.Models;
using NotSpotifyApp.Services;
using Plugin.Share;
using Plugin.Share.Abstractions;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NotSpotifyApp.ViewModels
{
    public class ArtistInfoPageViewModel : BaseViewModel, IInitialize
    {
        public Artist ArtistInfo { get; set; }
        public DelegateCommand GetArtistInfoCommand { get; set; }
        public DelegateCommand ShareArtistCommand { get; set; }
        public DelegateCommand ReturnToArtistPageCommand { get; set; }
        public string Id { get; set; }

        public ArtistInfoPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IDeezerApiService apiService) : base(navigationService, apiService)
        {
            GetArtistInfoCommand = new DelegateCommand(async () =>
            {
                await GetArtistData();
            });

            ReturnToArtistPageCommand = new DelegateCommand(async () =>
            {
                await navigationService.GoBackAsync();
            });

            ShareArtistCommand = new DelegateCommand(async () =>
            {
                await CrossShare.Current.Share(new ShareMessage
                {
                    Title = $"Hey check this artist out! - {ArtistInfo.Name}",
                    Url = $"{ArtistInfo.Link}"
                });
            });

        }     

        public void Initialize(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("Artist id"))
            {
                Id = parameters["Artist id"].ToString();
                GetArtistInfoCommand.Execute();
            }           

        }

        async Task GetArtistData()
        {
            ArtistInfo = await ApiService.GetArtistInfo(Id);
        }
    }
}
