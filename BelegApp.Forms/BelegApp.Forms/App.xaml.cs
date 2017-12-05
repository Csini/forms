using BelegApp.Forms.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace BelegApp.Forms
{
	public partial class App : Application
	{
        private BelegMasterViewModel _belegMasterViewModel;
		public App ()
		{
			InitializeComponent();

            //MainPage = new BelegApp.Forms.MainPage();
            MainPage = new BelegApp.Forms.Views.MainPage();
            BelegMasterViewModel = new BelegMasterViewModel();
            MainPage.BindingContext = BelegMasterViewModel;
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}

        public BelegMasterViewModel BelegMasterViewModel
        {
            get
            {
                return _belegMasterViewModel;
            }
            private set
            {
                if (Equals(_belegMasterViewModel, value)) return;
                _belegMasterViewModel = value;
            }
        }
	}
}
