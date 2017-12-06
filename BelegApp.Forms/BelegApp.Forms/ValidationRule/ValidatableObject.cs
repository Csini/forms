using BelegApp.Forms.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace BelegApp.Forms.ValidationRule
{
    public class ValidatableObject<T> : BaseViewModel
    {
        private List<string> errors = new List<string>();
        private T innerValue;
        private bool isValid = true;

        public ValidatableObject(bool autoValidation)
        {
            AutoValidation = autoValidation;
        }

        public List<IValidationRule<T>> Validations { get; } = new List<IValidationRule<T>>();

        public List<string> Errors
        {
            get => errors;
            set
            {
                if (Equals(errors, value)) return;
                errors = value;
                OnPropertyChanged(nameof(FirstError));
            }
        }

        public string FirstError
        {
            get
            {
                return Errors.FirstOrDefault();
            }
        }

        public bool AutoValidation { get; set; }

        public T Value
        {
            get => innerValue;
            set
            {
                //if (value.Equals(innerValue))
                if (Equals(innerValue, value))
                {
                    var i = errors.Count;
                }
                //if (Set(ref innerValue, value) && AutoValidation) Validate();
                if (Equals(innerValue, value) && AutoValidation) return;
                innerValue = value;
                Validate();

            }
        }

        public bool IsValid
        {
            get => isValid;
            set
            {
                if (Equals(isValid, value)) return;
                isValid = value;
            }
        }
        public bool Validate()
        {
            Errors.Clear();
            Errors = Validations.Where(v => !v.Check(Value))
                    .Select(v => v.ValidationMessage).ToList();
            IsValid = !Errors.Any();

            return IsValid;
        }
    }
}
