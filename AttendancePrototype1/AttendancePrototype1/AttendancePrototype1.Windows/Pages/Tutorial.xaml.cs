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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace AttendancePrototype1.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Tutorial : Page
    {
        public Tutorial()
        {
            this.InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Methods.ModifyData.ClearAppData.ClearApplicationData();

            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            var ifFirst = localSettings.Values["FirstTime"];
            if (ifFirst == null)
            {
                // First Time
                this.Frame.Navigate(typeof(AddStudentData), null);
            }
            else
            {
                // Not First Time
                // Directly Navigate
                this.Frame.Navigate(typeof(Langing2), null);
            }
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var bounds = Window.Current.Bounds;

            double height = bounds.Height;

            double width = bounds.Width;



            if (height < 768 || width < 1280)
            {
                ParentGrid.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
            else
            {
                ParentGrid.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
        }



    }
}
