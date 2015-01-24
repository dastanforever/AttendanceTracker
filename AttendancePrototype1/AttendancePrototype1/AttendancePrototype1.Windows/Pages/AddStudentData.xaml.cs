using AttendancePrototype1.Common;
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
using SQLite;
using AttendancePrototype1.Model;
using Windows.UI.Popups;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace AttendancePrototype1
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class AddStudentData : Page
    {

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }


        public AddStudentData()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
        }

        /// <summary>
        /// Populates the page with content passed during navigation. Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session. The state will be null the first time a page is visited.</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
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
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

        
        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Validate Data.
            // If error Insertion, UI popup.
            if (AddStudentDetails())
            {
                this.Frame.Navigate(typeof(Pages.Langing2));
            }
        }

        public bool AddStudentDetails()
        {
                    var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                    var set = localSettings.Values["StudentRegistered"];
                    if (set != null)
                        NavigationHelper.GoBack();
                    if (ValidateData())
                    {
                        var dbPath = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "StudentInformation.db");
                        var dbpath2 = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, " SubjectRelatedInformation.db");
                        SQLiteConnection conn = new SQLiteConnection(dbPath);
                        SQLiteConnection conn2 = new SQLiteConnection(dbpath2);
                        //Table Creation
                        conn.CreateTable<Student>();
                        conn2.CreateTable<Subject>();
                        //Adding Data
                        Student student = new Student { Name = name.Text, Age = int.Parse(age.Text), NumSubjects = int.Parse(numsub.Text), School = school.Text, yearInSchool = int.Parse(year.Text) };
                        // Validate Data!
                        conn.Insert(student);
                        
                        localSettings.Values["StudentRegistered"] = "Done";
                        localSettings.Values["NumberOfSubjects"] = student.NumSubjects;
                        localSettings.Values["TheUser"] = student.Name;
                        localSettings.Values["FirstTime"] = "Done";

                        return true;
                    }
                    return false;
        }

        public bool ValidateData()
        {
            
            if (name.Text.Length < 5 || name.Text.ToString().Length > 30)
            {
                Exception exe = new Exception("Either the Name provided is shorter than 5 chars, or longer than 30!!");
                name.Text = "";
                ShowDialogAsync(exe);
                return false;
            }
            if (school.Text.Length < 5 || school.Text.ToString().Length > 200)
            {
                Exception exe = new Exception("Either the School/College Name provided is shorter than 5 chars, or longer than 200!!");
                school.Text = "";
                ShowDialogAsync(exe);
                return false;
            }
           
            try
            {
                int Age = int.Parse(age.Text.ToString());
                
            }
            catch (Exception ex)
            {
                age.Text = "";
                ShowDialogAsync(ex);
                return false;
            }
            if(int.Parse(age.Text.ToString()) > 30 || int.Parse(age.Text.ToString()) < 8)
            {
                    Exception exe = new Exception("The age should be between 8 to 30 years!!");
                    age.Text = "";
                    ShowDialogAsync(exe);
                    return false;
            }
            try
            {
                int Year = int.Parse(year.Text.ToString());
            }
            catch (Exception ex)
            {
                year.Text = "";
                ShowDialogAsync(ex);
                return false;
            }
            try
            {
                int Numsub = int.Parse(numsub.Text.ToString());
            }
            catch (Exception ex)
            {
                numsub.Text = "";
                ShowDialogAsync(ex);
                return false;
            }
            return true;
        }

            public async void ShowDialogAsync(Exception ex)
            {
                var messageDialog = new MessageDialog(ex.Message, "Error!!!!");
                messageDialog.Commands.Add(new UICommand("Retry", new UICommandInvokedHandler(this.CommandInvokedHandler)));
                messageDialog.Commands.Add(new UICommand("Back", new UICommandInvokedHandler(this.CommandInvokedHandler)));
                await messageDialog.ShowAsync();
            }

        

            private void CommandInvokedHandler(IUICommand command)
            {
                if (command.Label == "Retry")
                {
                    
                }
                if (command.Label == "Back")
                {
                    NavigationHelper.GoBack();
                }
            }

            private void name_TextChanged(object sender, TextChangedEventArgs e)
            {

            }

            private void ParentGrid_SizeChanged(object sender, SizeChangedEventArgs e)
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
