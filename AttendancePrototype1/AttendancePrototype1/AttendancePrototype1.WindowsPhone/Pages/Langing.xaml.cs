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
    public sealed partial class Langing : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public Langing()
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
            /// Load subjects 
            /// Into the grid view
            /// if
            /// 
            try
            {
                PopulateGridView();
            }
            catch (Exception ex)
            {
                ShowDialog(ex.Message);
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
        }

        #endregion

        private void PopulateGridView()
        {
            var listSubjects = ReturnSubjects();
            if (listSubjects.Count == 0)
            {
                
            }
            else
            {
                foreach (var item in listSubjects)
                {
                    SubjectsGridView.Items.Add(item);
                }
            }
        }

        private List<Model.Subject> ReturnSubjects()
        {
            var listSubjects = new List<Model.Subject>();
            try
            {
                var dbPath = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "SubjectRelatedInformation.db");
                SQLiteConnection conn = new SQLiteConnection(dbPath);

                var query = conn.Table<Model.Subject>();
                listSubjects = query.ToList<Model.Subject>();

                return listSubjects;
            }
            catch (Exception ex)
            {
                ShowDialog(ex.Message);
                return listSubjects;
            }
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AddSubjectDetails));
        }


        private async void ShowDialog(String Error)
        {
            MessageDialog messageDialog = new MessageDialog(Error, "Error!!!!");
            messageDialog.Commands.Add(new UICommand("Retry", new UICommandInvokedHandler(this.CommandInvokedHandler)));

            await messageDialog.ShowAsync();
        }

        private void CommandInvokedHandler(IUICommand command)
        {
            
        }

        private void UpdateAttendance_Click(object sender, RoutedEventArgs e)
        {
            if (int.Parse(ListAttend.SelectedItem.ToString()) > int.Parse(ListHeld.SelectedItem.ToString()))
            {
                ShowDialog("The held classes cannot be less than attended classes.");
            }
            else
            {
                var dbPath = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "StudentInformation.db");
                SQLiteConnection conn = new SQLiteConnection(dbPath);

                var query = conn.Table<Model.Subject>().Where(x => x.Name == ListSubjects.Name);

                Model.Subject updateSubject = query.FirstOrDefault();

                updateSubject.LastUpdated = DateTime.Now.Date.ToString("dd/MM/yy");
                updateSubject.NumClassesAttended += int.Parse(ListAttend.SelectedItem.ToString());
                updateSubject.NumClassesHeld += int.Parse(ListHeld.SelectedIndex.ToString());
                updateSubject.PercentAttendance = float.Parse((((float)updateSubject.NumClassesAttended / (float)updateSubject.NumClassesHeld) * 100).ToString("0.00"));
                conn.Update(updateSubject);
                SubjectsGridView.Items.Clear();
                PopulateGridView();
            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // Populate ComboBoxes.

            ListSubjects.Items.Clear();
            ListHeld.Items.Clear();
            ListAttend.Items.Clear();

            ListAttend.MaxDropDownHeight = 80;
            ListHeld.MaxDropDownHeight = 80;

            List<Model.Subject> subjectList = ReturnSubjects();
            if (subjectList.Count == 0)
            {
                ShowDialog("No subjects to update Attendance!!");
            }
            else
            {
                foreach (var item in subjectList)
                {
                    ListSubjects.Items.Add(item.Name);
                }

                for (int i = 0; i < 11; i++)
                {
                    ListHeld.Items.Add(i);
                    ListAttend.Items.Add(i);
                }
            }
        }
    }
}
