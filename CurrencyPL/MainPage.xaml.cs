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
using Microsoft.EntityFrameworkCore.Internal;
using Windows.UI.Popups;
using CurrencyPL.ViewModels;
using CurrencyBL;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CurrencyPL
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {


        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            switch(args.InvokedItem)
            {
                case "Home":
                    ContentFrame.Navigate(typeof(ConvertionPage));
                    break;
                default:
                    ContentFrame.Navigate(typeof(ErrorPage));
                    break;
            }
        }

        private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {

        }

        private void NavView_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private async void More_Click(object sender, RoutedEventArgs e)
        {
            await new MessageDialog("This amaizong application havae been done by HGWY Technologies...").ShowAsync();
        }

    }
}
