﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using BelegApp.Forms.Models;
using BelegApp.Forms.ViewModels;

namespace BelegApp.Forms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<string> Items { get; set; }

        public MainPage()
        {
            InitializeComponent();

            Items = new ObservableCollection<string>
            {
                "Item 1",
                "Item 2",
                "Item 3",
                "Item 4",
                "Item 5"
            };
			
			//MyListView.ItemsSource = Items;
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            //Hacky stuff ;)
            if (e.Item == null)
                return;
            var beleg = e.Item as BelegDetailsViewModel;

            try
            {
                await Navigation.PushAsync(new DetailPage(beleg.Belegnummer));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Fehler", ex.Message, "Continue");
            }
            //Deselect Item
            ((ListView) sender).SelectedItem = null;
        }

        //private void MenuItem_Clicked(object sender, EventArgs e)
        //{

        //}
    }
}