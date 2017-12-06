using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using BelegApp.Forms.Models;
using BelegApp.Forms.ViewModels;
using BelegApp.Forms.Utils;
using BelegApp.Forms.Services;

namespace BelegApp.Forms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            // ViewModel initialisieren
            getDatabaseBelegList().Wait();
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            //Hacky stuff ;)
            if (e.Item == null)
                return;

            var beleg = e.Item as BelegDetailsViewModel;

            try
            {
                await Navigation.PushAsync(new DetailPage(beleg.Belegnummer, Navigation));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Fehler", ex.Message, "Continue");
            }
            //Deselect Item
            ((ListView) sender).SelectedItem = null;
        }

        async void Handle_Refreshing(Object sender, EventArgs e)
        {
            if (belegeListView != null)
            {
                // Refreshmodus aktivieren
                //belegeListView.BeginRefresh();

                try
                {
                    // ViewModel initialisieren
                    refreshBelegList().Wait();
                }
                finally
                {
                    // Refreshmodus beenden
                    //belegeListView.EndRefresh();
                }
            }
        }

        private async Task getDatabaseBelegList()
        {
            // Belegliste aus der Datenbank holen
            Beleg[] belegList = new Storage().GetBelege().Result;
            BelegMasterViewModel viewModel = new BelegMasterViewModel(this.Navigation, belegList);
            this.BindingContext = viewModel;
        }

        private async Task refreshBelegList()
        {
            // Belegliste in Datenbank mit Online-Belegen aktualisieren
            new BelegServiceHelper().RefreshStatus().Wait();

            // Belegliste aus der Datenbank erneut lesen und anzeigen
            getDatabaseBelegList().Wait();
        }
    }
}
