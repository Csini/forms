using System;
using System.Collections.Generic;
using System.Text;

namespace BelegApp.Forms.ValidationRule
{
    public interface IValidationRule<T>
    {
        string ValidationMessage { get; set; }
        bool Check(T value);
    }
}
