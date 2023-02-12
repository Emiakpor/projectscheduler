using CriticalPathApp.Services;
using CriticalPathApp.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CriticalPathApp
{
    public partial class App : Application
    {

        public App()
        {
            Xamarin.Forms.DataGrid.DataGridComponent.Init();
            InitializeComponent();

            DependencyService.Register<ActivityDataStore>();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
