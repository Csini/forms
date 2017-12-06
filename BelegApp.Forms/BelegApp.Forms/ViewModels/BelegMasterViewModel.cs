using BelegApp.Forms.Models;
using BelegApp.Forms.Services;
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
using BelegApp.Forms.Services;

namespace BelegApp.Forms.ViewModels
{
    public class BelegMasterViewModel : BaseViewModel
    {
        private ObservableCollection<BelegDetailsViewModel> _belege;

        public bool selectMode {get; set;}

        public BelegMasterViewModel(INavigation navigation) : this(navigation, null)
        {
        }

        public BelegMasterViewModel(INavigation navigation, Beleg[] belegList)
        {
            selectMode = false;

            SelectBelegeCommand = new Command(() =>
            {
                selectMode = !selectMode;
                if (!selectMode)
                {
                    resetSelectedBelege();
                }
            });

            ExportBelegeCommand = new Command(async () =>
            {
                if (!selectMode) { return; }

                Beleg[] selectedBelege = null; //TODO filter alle selektierten Belege

                await new BelegServiceHelper().ExportBelege(selectedBelege);

                resetSelectedBelege();
                selectMode = false;
            });

            DeleteBelegeCommand = new Command(async () =>
            {
                if (!selectMode) { return; }

                Beleg[] selectedBelege = new Beleg[0]; //TODO filter alle selektierten Belege
                int[] belegNummern = new int[selectedBelege.Length];
                for (int ix = 0; ix < selectedBelege.Length; ++ix)
                {
                    belegNummern[ix] = selectedBelege[ix].Belegnummer.Value;
                }

                await BelegService.DeleteBelege(BelegService.USER, belegNummern);
                await Storage.Database.RemoveBelege(selectedBelege);

                resetSelectedBelege();
                selectMode = false;
            });

            AddNewBelegCommand = new Command(() =>
            {
                navigation.PushAsync(new DetailPage(null, navigation));
            });
            
            Belege = new ObservableCollection<BelegDetailsViewModel>(); // Ladeoperation von Service

            if (belegList != null)
            {
                foreach (Beleg beleg in belegList)
                {
                    _belege.Add(new BelegDetailsViewModel(beleg));
                }
            }
        }

        public void resetSelectedBelege()
        {
            foreach (BelegDetailsViewModel beleg in _belege)
            {
                beleg.IsSelected = false;
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
        public ICommand ExportBelegeCommand { get; private set; }
        public ICommand SelectBelegeCommand { get; private set; }
        public ICommand DeleteBelegeCommand { get; private set; }
    }
}
