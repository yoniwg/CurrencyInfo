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
            TitleOfFrame = "Hello";
        }

        public void NavView_Loaded(object sender, RoutedEventArgs e)
        {
            // set the initial SelectedItem 
            foreach (NavigationViewItemBase item in NavView.MenuItems)
            {
                if (item is NavigationViewItem && item.Tag.ToString() == "home")
                {
                    NavView.SelectedItem = item;
                    NavigateTo(item.Tag.ToString());
                    break;
                }
            }
        }

        private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                ContentFrame.Navigate(typeof(SettingsPage));
                TitleOfFrame = "Settings";
                return;
            }

            TitleOfFrame = args.InvokedItem as string;
            var name = args.InvokedItem.ToString().ToLower();
            NavigateTo(name);
        }

        private void NavigateTo(string name)
        {
            switch (name)
            {
                case "home":
                    ContentFrame.Navigate(typeof(HomePage));
                    break;
                case "conversion":
                    ContentFrame.Navigate(typeof(ConvertionPage));
                    break;
                case "live rates":
                    ContentFrame.Navigate(typeof(LiveRatesPage));
                    break;
                case "history":
                    ContentFrame.Navigate(typeof(HistoryPage));
                    break;
                case "settings":
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
