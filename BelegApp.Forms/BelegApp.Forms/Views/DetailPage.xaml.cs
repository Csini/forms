using BelegApp.Forms.Models;
using BelegApp.Forms.Utils;
using BelegApp.Forms.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BelegApp.Forms.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DetailPage : ContentPage
	{
        private long? _belegnummer;
		public DetailPage (long? belegnummer)
		{
			InitializeComponent ();
            _belegnummer = belegnummer;
            //Beleg beleg = 
            //BindingContext = new BelegDetailsViewModel()
		}
	}
}