using BelegApp.Forms.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using static BelegApp.Forms.Models.Beleg;

namespace BelegApp.Forms.ViewModels
{
    public class BelegMasterViewModel : BaseViewModel
    {
        private ObservableCollection<BelegDetailsViewModel> _belege;
        public BelegMasterViewModel()
        {
            Belege = new ObservableCollection<BelegDetailsViewModel>(); // Ladeoperation von Service
            Belege.Add(new BelegDetailsViewModel(new Beleg(1, "asdf", DateTime.Now, "Geiler Typ", 123, StatusEnum.GEBUCHT, null, 12)));
            Belege.Add(new BelegDetailsViewModel(new Beleg(2, "jklö", DateTime.Now, "Typ", 999, StatusEnum.EXPORTIERT, null, 121)));
            Belege.Add(new BelegDetailsViewModel(new Beleg(2, "jklö", DateTime.Now, "Typ", 999, StatusEnum.EXPORTIERT, null, 121)));
            Belege.Add(new BelegDetailsViewModel(new Beleg(2, "jklö", DateTime.Now, "Typ", 999, StatusEnum.EXPORTIERT, null, 121)));
            Belege.Add(new BelegDetailsViewModel(new Beleg(2, "jklö", DateTime.Now, "Typ", 999, StatusEnum.EXPORTIERT, null, 121)));
            Belege.Add(new BelegDetailsViewModel(new Beleg(2, "jklö", DateTime.Now, "Typ", 999, StatusEnum.EXPORTIERT, null, 121)));
            Belege.Add(new BelegDetailsViewModel(new Beleg(2, "jklö", DateTime.Now, "Typ", 999, StatusEnum.EXPORTIERT, null, 121)));
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
    }
}
