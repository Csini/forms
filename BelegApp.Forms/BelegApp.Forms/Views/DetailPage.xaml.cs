using BelegApp.Forms.Models;
using BelegApp.Forms.Utils;
using BelegApp.Forms.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BelegApp.Forms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailPage : ContentPage
    {
        private int? _belegnummer;
        public DetailPage(int? belegnummer, INavigation navigation)
        {
            InitializeComponent();
            _belegnummer = belegnummer;
            Beleg beleg = null;
            Action callback = new Action(() =>
            {
                navigation.PopToRootAsync();
            });
            if (_belegnummer.HasValue)
                beleg = Storage.Database.GetBeleg(_belegnummer.Value).Result;
            if (beleg == null)
                BindingContext = new BelegDetailsViewModel();
            else
                BindingContext = new BelegDetailsViewModel(beleg);

            //Callback setzen
            (BindingContext as BelegDetailsViewModel).Callback = callback;
        }

        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            //var ctl = sender as Entry;
            //if(ctl != null)
            //    ctl.Text = "300";

        }
    }
}