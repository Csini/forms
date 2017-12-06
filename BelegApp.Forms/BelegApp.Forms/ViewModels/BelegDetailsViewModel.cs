using System;
using System.Collections.Generic;
using System.Text;
using BelegApp.Forms.Models;
using static BelegApp.Forms.Models.Beleg;
using BelegApp.Forms.Utils;
using System.Windows.Input;
using Xamarin.Forms;

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
        private string _iconName;
        private decimal? _betrag;

        public BelegDetailsViewModel()
        {
            InitCommands();
        }

        private void InitCommands()
        {
            SaveBelegCommand = new Command(() =>
            {
                var result = Storage.Database.StoreBeleg(this.GetBusinessObject()).Result;
                if (result < 0)
                    Callback();
            });
        }

        public BelegDetailsViewModel(Beleg beleg)
        {
            if (beleg == null)
                throw new ArgumentNullException("beleg");

            Belegnummer = beleg.Belegnummer;
            _statusEnum = beleg.Status;
            _description = beleg.Description;
            _datum = beleg.Date;
            _type = beleg.Type;
            _thumbnail = beleg.Thumbnail;
            _belegSize = beleg.BelegSize;
            _iconName = beleg.Status + ".png";

            InitCommands();

            PropertyChanged += (s, e) =>
            {
                // Update IsEditable
                if (e.PropertyName == nameof(Status))
                    OnPropertyChanged(nameof(IsEditable));

            };
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

        public decimal? Betrag
        {
            get
            {
                return _betrag;
            }
            set
            {
                if (Equals(_betrag, value)) return;
                // It's getting hacky ;)
                _betrag = value;
                OnPropertyChanged(nameof(Betrag));
            }
        }

        public byte[] Thumbnail
        {
            get
            {
                return _thumbnail;
            }
            set
            {
                if (Equals(_thumbnail, value)) return;
                _thumbnail = value;
                OnPropertyChanged(nameof(Thumbnail));
            }
        }

        public bool IsEditable
        {
            get
            {
                return (Status != StatusEnum.ERFASST);
            }
        }

        public List<string> Types
        {
            get
            {
                return StaticValues.BelagTypes;
            }
        }

        public Action Callback { get; set; }

        public ICommand SaveBelegCommand { get; private set; }

        private Beleg GetBusinessObject()
        {
            // Konvertierung passieren hier
            // zB Konvertierung des Betrags in Cent
#warning it's too late - hacky
            var dec = decimal.Parse(Betrag.Value.ToString("0.##"));
            long betragInCent = long.Parse((dec * 100).ToString());
            return new Beleg(Belegnummer, Description, Datum, Type, betragInCent, Status, Thumbnail, BelegSize);
        }
    }
}
