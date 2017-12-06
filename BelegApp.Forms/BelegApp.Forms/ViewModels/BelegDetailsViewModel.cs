using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using BelegApp.Forms.Models;
using static BelegApp.Forms.Models.Beleg;
using BelegApp.Forms.Utils;
using System.Windows.Input;
using Xamarin.Forms;
using BelegApp.Forms.ValidationRule;
using BelegApp.Forms.Services;
using System.IO;

namespace BelegApp.Forms.ViewModels
{
    public class BelegDetailsViewModel : BaseViewModel
    {
        private int? _belegNummer;
        private StatusEnum? _statusEnum;
        private ValidatableObject<string> _validatableDescription;
        private DateTime? _datum;
        private ValidatableObject<string> _validatableType;
        private byte[] _thumbnail;
        private ImageSource _thumbnailImageSource;
        private long? _belegSize;
        private string _iconName;
        private decimal? _betrag;
        private byte[] _image;
        private ImageSource _imageImageSource;

        public BelegDetailsViewModel()
        {
            Status = StatusEnum.ERFASST;
            Datum = DateTime.Now;
            _validatableDescription = new ValidatableObject<string>(true);
            _validatableType = new ValidatableObject<string>(true);
            Init();
        }

        public BelegDetailsViewModel(Beleg beleg)
        {
            if (beleg == null)
                throw new ArgumentNullException("beleg");

            _validatableDescription = new ValidatableObject<string>(true);
            _validatableType = new ValidatableObject<string>(true);

            Belegnummer = beleg.Belegnummer;
            _statusEnum = beleg.Status;
            
            Description = beleg.Description;
            _datum = beleg.Date;
            
            Type = beleg.Type;
            _thumbnail = beleg.Thumbnail;
            _belegSize = beleg.BelegSize;
            _iconName = beleg.Status + ".png";
            _image = beleg.Image;

            Init();
        }

        private void Init()
        {
            PropertyChanged += (s, e) =>
            {
                // Update IsEditable
                if (e.PropertyName == nameof(Status))
                    OnPropertyChanged(nameof(IsEditable));
                if (e.PropertyName == nameof(Description) || e.PropertyName == nameof(Type))
                    ((Command)SaveBelegCommand).ChangeCanExecute();
                //OnPropertyChanged(nameof(CanSave));
            };

            SaveBelegCommand = new Command(() =>
            {
                Beleg beleg = this.GetBusinessObject();
                if (beleg.Thumbnail == null && beleg.Image != null)
                {
                    beleg.Thumbnail = BelegService.CreateThumbnail(beleg.Image).Result;
                }
                var result = Storage.Database.StoreBeleg(beleg).Result;
                if (result > 0)
                    Callback();
            }, () => CanSave);
            AddValidations();
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
                return ValidatableDescription.Value;
            }
            set
            {
                if (Equals(ValidatableDescription.Value, value)) return;
                ValidatableDescription.Value = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public ValidatableObject<string> ValidatableDescription
        {
            get
            {
                return _validatableDescription;
            }
            set
            {
                //if (Equals(_description, value)) return;
                _validatableDescription = value;
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
                return ValidatableType.Value;
            }
            set {
                if (Equals(ValidatableType.Value, value)) return;
                ValidatableType.Value = value;
                OnPropertyChanged(nameof(Type));
            }
        }

        public ValidatableObject<string> ValidatableType
        {
            get
            {
                return _validatableType;
            }
            set
            {
                if (Equals(_validatableType, value)) return;
                _validatableType = value;
                OnPropertyChanged(nameof(ValidatableType));
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
                _thumbnailImageSource = null;
                OnPropertyChanged(nameof(Thumbnail));
                OnPropertyChanged(nameof(ThumbnailImageSource));
            }
        }

        public ImageSource ThumbnailImageSource
        {
            get
            {
                if ((_thumbnailImageSource == null) && (_thumbnail != null))
                {
                    _thumbnailImageSource = ImageSource.FromStream(() => new MemoryStream(_thumbnail));
                }
                return _thumbnailImageSource;
            }
        }

        public byte[] Image
        {
            get
            {
                return _image;
            }
            set
            {
                if (Equals(_image, value)) return;
                _image = value;
                _imageImageSource = null;
                OnPropertyChanged(nameof(Image));
                OnPropertyChanged(nameof(ImageImageSource));
            }
        }

        public ImageSource ImageImageSource
        {
            get
            {
                if ((_imageImageSource == null) && (_image != null))
                {
                    _imageImageSource = ImageSource.FromStream(() => new MemoryStream(_image));
                }
                return _imageImageSource;
            }
        }
        public bool IsEditable
        {
            get
            {
                return (Status == StatusEnum.ERFASST);
            }
        }

        public bool CanSave
        {
            get
            {
                ValidatableDescription.Validate();
                ValidatableType.Validate();
                return
                    ValidatableDescription.IsValid && ValidatableType.IsValid;
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

        private void AddValidations()
        {
            _validatableDescription.Validations.Add(new IsNotNullOrEmptyRule<string>
            {
                ValidationMessage = "Beschreibung eingeben"
            });
            _validatableType.Validations.Add(new IsNotNullOrEmptyRule<string>
            {
                ValidationMessage = "Art auswählen"
            });
        }
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
