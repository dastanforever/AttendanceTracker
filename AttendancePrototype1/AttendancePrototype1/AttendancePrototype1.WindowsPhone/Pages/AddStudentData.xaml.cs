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
    public sealed partial class AddStudentData : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public AddStudentData()
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
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
        }

        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateData())
            {
                var dbPath = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "StudentInformation.db");
                var dbPath2 = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "SubjectRelatedInformation.db");
                SQLiteConnection conn = new SQLiteConnection(dbPath);
                SQLiteConnection conn2 = new SQLiteConnection(dbPath2);
                		/// a. The student related data - StudentInformation.db
		                /// b. The Subject related data - SubjectRelatedInformation.db
                Model.Student student = new Model.Student();
                student.Age = int.Parse(TextAge.Text);
                student.Name = TextName.Text;
                student.NumSubjects = int.Parse(TextSubject.Text);
                student.School = TextSchool.Text;
                student.yearInSchool = int.Parse(TextYear.Text);

                conn.CreateTable<Model.Student>();
                conn2.CreateTable<Model.Subject>();
                conn.Insert(student);

                var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                localSettings.Values["FirstTime"] = "Done";
                localSettings.Values["TheUser"] = student.Name;
                localSettings.Values["StudentRegistered"] = "Yes";

                this.Frame.Navigate(typeof(Langing));

            }
        }

        private bool ValidateData()
        {
            
            if (TextName.Text.Length < 5 || TextName.Text.ToString().Length > 30)
            {
                Exception exe = new Exception("Either the Name provided is shorter than 5 chars, or longer than 30!!");
                TextName.Text = "";
                ShowDialogAsync(exe);
                return false;
            }
            if (TextSchool.Text.Length < 5 || TextSchool.Text.ToString().Length > 200)
            {
                Exception exe = new Exception("Either the School/College Name provided is shorter than 5 chars, or longer than 200!!");
                TextSchool.Text = "";
                ShowDialogAsync(exe);
                return false;
            }
            try
            {
                int Age = int.Parse(TextAge.Text.ToString());
                if (Age > 30 || Age < 8)
                {
                    Exception exe = new Exception("The age should be between 8 to 30 years!!");
                    TextAge.Text = "";
                    ShowDialogAsync(exe);
                    return false;
                }
            }
            catch (Exception ex)
            {
                TextAge.Text = "";
                ShowDialogAsync(ex);
                return false;
            }
            try
            {
                int Year = int.Parse(TextYear.Text.ToString());
                if (Year > 7 || Year < 1)
                {
                    Exception exe = new Exception("The year should be between 1 to 7!!");
                    TextYear.Text = "";
                    ShowDialogAsync(exe);
                    return false;
                }
            }
            catch (Exception ex)
            {
                TextYear.Text = "";
                ShowDialogAsync(ex);
                return false;
            }
            try
            {
                int Numsub = int.Parse(TextSubject.Text.ToString());
                if (Numsub > 8 || Numsub < 1)
                {
                    Exception exe = new Exception("Negative number of subjects, or Greater that 8 subjects are not yet supported.");
                    TextSubject.Text = "";
                    ShowDialogAsync(exe);
                    return false;
                }
            }
            catch (Exception ex)
            {
                TextSubject.Text = "";
                ShowDialogAsync(ex);
                return false;
            }
            return true;
        }

            public async void ShowDialogAsync(Exception ex)
            {
                var messageDialog = new MessageDialog(ex.Message, "Error!!!!");
                messageDialog.Commands.Add(new UICommand("Retry", new UICommandInvokedHandler(this.CommandInvokedHandler)));
                await messageDialog.ShowAsync();
            }

        

            private void CommandInvokedHandler(IUICommand command)
            {
                if (command.Label == "Retry")
                {
                    
                }
            }
        }
}

