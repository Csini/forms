using BelegApp.Forms.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace BelegApp.Forms.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();


            BelegApp.Forms.Services.BelegService service = new BelegApp.Forms.Services.BelegService();
            string[] types = service.GetTypeList().Result;
            Beleg.StatusEnum[] statuses = service.GetStatusList().Result;

            LoadApplication(new BelegApp.Forms.App());
        }
    }
}
