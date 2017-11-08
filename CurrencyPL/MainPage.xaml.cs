using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
using Microsoft.EntityFrameworkCore.Internal;
using Windows.UI.Popups;
using CurrencyPL.ViewModels;
using CurrencyBL;
using CurrencyPL.Annotations;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CurrencyPL
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        private string titleOfFrame;

        public String TitleOfFrame
        {
            get => titleOfFrame;
            set
            {
                titleOfFrame = value;
                OnPropertyChanged(nameof(TitleOfFrame));
            }
        }

        public MainPage()
        {
            this.InitializeComponent();
            DataContext = this;
            ContentFrame.Navigate(typeof(HomePage));
            TitleOfFrame = "Hello";
        }

        private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            TitleOfFrame = args.InvokedItem as string;
            switch (args.InvokedItem)
            {
                case "Home":
                    ContentFrame.Navigate(typeof(HomePage));
                    break;
                case "Conversion":
                    ContentFrame.Navigate(typeof(ConvertionPage));
                    break;
                case "Live Rates":
                    ContentFrame.Navigate(typeof(LiveRatesPage));
                    break;
                case "History":
                    ContentFrame.Navigate(typeof(HistoryPage));
                    break;
                case "Settings":
                    ContentFrame.Navigate(typeof(SettingsPage));
                    break;
                default:
                    ContentFrame.Navigate(typeof(ErrorPage));
                    TitleOfFrame = "Error";
                    break;
            }
        }

        private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
