using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using BelegApp.Forms.Models;
using static BelegApp.Forms.Models.Beleg;

namespace BelegApp.Forms.ViewModels
{
    public class BelegDetailsViewModel : BaseViewModel
    {
        private int? _belegNummer;
        private StatusEnum? _statusEnum;
        private string _description;
        private DateTime? _datum;
        private string _type;
        private byte[] _thumbnail;
        private long? _belegSize;
        private long? _betrag;
        private string _iconName;

        public BelegDetailsViewModel() { }

        public BelegDetailsViewModel(Beleg beleg)
        {
            if (beleg == null)
                throw new ArgumentNullException("beleg");

            Belegnummer = beleg.Belegnummer;
            _statusEnum = beleg.Status;
            _description = beleg.Description;
            _datum = beleg.Date;
            _type = beleg.Type;
            /*_thumbnail = */
            _belegSize = beleg.BelegSize;
            _iconName = beleg.Status + ".png";

        }

        public int? Belegnummer
        {
            get
            {
                return _belegNummer;
            }
            private set
            {
                //if (Equals(_belegNummer, value)) return;
                _belegNummer = value;
                //OnPropertyChanged(nameof(Belegnummer));
            }
        }

        public StatusEnum? Status
        {
            get
            {
                return _statusEnum;
            }
            set
            {
                if (Equals(_statusEnum, value)) return;
                _statusEnum = value;
                OnPropertyChanged(nameof(Status));
            }
        }

        public string IconName
        {
            get
            {
                return _iconName;
            }
        }

        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                if (Equals(_description, value)) return;
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public DateTime? Datum
        {
            get
            {
                return _datum;
            }
            set
            {
                if (Equals(_datum, value)) return;
                _datum = value;
                OnPropertyChanged(nameof(Datum));
            }
        }

        public string DatumString
        {
            get
            {
                string result = null;
                if (_datum.HasValue)
                {
                    result = _datum.Value.ToString("dd.MM.yyyy");
                }
                return result;
            }
        }

        public string Type
        {
            get
            {
                return _type;
            }
            set
            {
                if (Equals(_type, value)) return;
                _type = value;
                OnPropertyChanged(nameof(Type));
            }
        }

        public long? BelegSize
        {
            get
            {
                return _belegSize;
            }
            set
            {
                if (Equals(_belegSize, value)) return;
                _belegSize = value;
                OnPropertyChanged(nameof(BelegSize));
            }
        }

        public long? Betrag
        {
            get
            {
                return _betrag;
            }
            set
            {
                if (Equals(_betrag, value)) return;
                _betrag = value;
                OnPropertyChanged(nameof(Betrag));
            }
        }

        public ICommand SaveBelegCommand { get; private set; }

    }
}
