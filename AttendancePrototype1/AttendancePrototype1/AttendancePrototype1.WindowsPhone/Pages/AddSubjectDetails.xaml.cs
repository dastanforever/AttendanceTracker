using AttendancePrototype1.Common;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace AttendancePrototype1.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddSubjectDetails : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public AddSubjectDetails()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Gets the view model for this <see cref="Page"/>.
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateDetails())
            {
                AddSubjectToDatabase();
                this.Frame.Navigate(typeof(Langing));
            }            
        }

        private void AddSubjectToDatabase()
        {
            var dbPath = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "SubjectRelatedInformation.db");
            SQLiteConnection conn = new SQLiteConnection(dbPath);

            conn.CreateTable<Model.Subject>();
            Model.Subject subject = new Model.Subject();

            subject.Name = SubjectText.Text;
            subject.Code = SubjectCode.Text;
            subject.InstructorName = InstructorName.Text;
            subject.NumClassesAttended = int.Parse(ClassesAttend.Text);
            subject.NumClassesHeld = int.Parse(ClassesHeld.Text);
            subject.LastUpdated = DateTime.Now.Date.ToString("dd/MM/yy");
            subject.PercentAttendance = float.Parse((((float)subject.NumClassesAttended / (float)subject.NumClassesHeld) * 100).ToString("0.00"));
            conn.Insert(subject);

            conn.Close();

        }

        private bool ValidateDetails()
        {
            if (SubjectCode.Text.Length < 3 || SubjectCode.Text.Length > 30)
            {
                ShowMessageDialog("The length of subject name should be between 2 to 30");
                return false;
            }
            if (SubjectText.Text.Length < 2 || SubjectText.Text.Length > 30)
            {
                ShowMessageDialog("The length of subject name should be between 2 to 30");
                return false;
            }

            if (InstructorName.Text.Length < 3 || InstructorName.Text.Length > 30)
            {
                ShowMessageDialog("The length of subject name should be between 3 to 30");
                return false;
            }
            int r,s;
            int.TryParse(ClassesHeld.Text,out r);
            int.TryParse(ClassesAttend.Text,out s);
            if ((int)r == 0 || (int)s == 0)
            {
                ShowMessageDialog("Enter proper classes Data!!!");
                return false;
            }
            if (int.Parse(ClassesAttend.Text) > int.Parse(ClassesHeld.Text))
            {
                ShowMessageDialog("The attended classes should be less than held classes");
                return false;
            }
            return true;
        }

        private async void ShowMessageDialog(string Error)
        {
            MessageDialog messageDialog = new MessageDialog(Error, "Error!!");
            messageDialog.Commands.Add(new UICommand(
        "Try again",
        new UICommandInvokedHandler(this.CommandInvokedHandler)));

            await messageDialog.ShowAsync();
        }

        private void CommandInvokedHandler(IUICommand command)
        {
            
        }

    }
}
