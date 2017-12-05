using BelegApp.Forms.Models;
using BelegApp.Forms.Utils;
using BelegApp.Forms.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using static BelegApp.Forms.Models.Beleg;

namespace BelegApp.Forms.ViewModels
{
    public class BelegMasterViewModel : BaseViewModel
    {
        private ObservableCollection<BelegDetailsViewModel> _belege;
        public BelegMasterViewModel(INavigation navigation) : this(navigation, null)
        {
        }

        public BelegMasterViewModel(INavigation navigation, Beleg[] belegList)
        {
            AddNewBelegCommand = new Command(() =>
            {
                navigation.PushAsync(new DetailPage(null));
            });

            Belege = new ObservableCollection<BelegDetailsViewModel>(); // Ladeoperation von Service

            if (belegList != null)
            {
                foreach (Beleg beleg in belegList)
                {
                    Belege.Add(new BelegDetailsViewModel(beleg));
                }
            }
        }

        public ObservableCollection<BelegDetailsViewModel> Belege
        {
            get
            {
                return _belege;
            }
            set
            {
                if (Equals(_belege, value)) return;
                _belege = value;
                OnPropertyChanged(nameof(Belege));
            }
        }

        public ICommand AddNewBelegCommand { get; private set; }
    }
}
