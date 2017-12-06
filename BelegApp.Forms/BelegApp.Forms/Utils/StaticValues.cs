using BelegApp.Forms.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace BelegApp.Forms.Utils
{
    public static class StaticValues
    {
        public static void UpdateStaticValues()
        { 
            BelagTypes = BelegService.GetTypeList().Result.ToList();
        }

        public static List<string> BelagTypes { get; set; }
    }
}
