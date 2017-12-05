using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace BelegApp.Forms.ViewModels
{
    public class BelegMasterViewModel : BaseViewModel
    {
        private ObservableCollection<BelegDetailsViewModel> _belege;
        public BelegMasterViewModel() {
            Belege = new ObservableCollection<BelegDetailsViewModel>(); // Ladeoperation von Service
            Belege.Add(new BelegDetailsViewModel(new IO.Swagger.Model.Beleg(1, "asdf", DateTime.Now, "Geiler Typ", IO.Swagger.Model.Beleg.StatusEnum.ERFASST, null, 12)));
            Belege.Add(new BelegDetailsViewModel(new IO.Swagger.Model.Beleg(2, "jklö", DateTime.Now, "Typ", IO.Swagger.Model.Beleg.StatusEnum.EXPORTIERT, null, 121)));
            Belege.Add(new BelegDetailsViewModel(new IO.Swagger.Model.Beleg(2, "jklö", DateTime.Now, "Typ", IO.Swagger.Model.Beleg.StatusEnum.EXPORTIERT, null, 121)));
            Belege.Add(new BelegDetailsViewModel(new IO.Swagger.Model.Beleg(2, "jklö", DateTime.Now, "Typ", IO.Swagger.Model.Beleg.StatusEnum.EXPORTIERT, null, 121)));
            Belege.Add(new BelegDetailsViewModel(new IO.Swagger.Model.Beleg(2, "jklö", DateTime.Now, "Typ", IO.Swagger.Model.Beleg.StatusEnum.EXPORTIERT, null, 121)));
            Belege.Add(new BelegDetailsViewModel(new IO.Swagger.Model.Beleg(2, "jklö", DateTime.Now, "Typ", IO.Swagger.Model.Beleg.StatusEnum.EXPORTIERT, null, 121)));
            Belege.Add(new BelegDetailsViewModel(new IO.Swagger.Model.Beleg(2, "jklö", DateTime.Now, "Typ", IO.Swagger.Model.Beleg.StatusEnum.EXPORTIERT, null, 121)));
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
