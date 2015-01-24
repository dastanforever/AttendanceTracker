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
using Windows.ApplicationModel;
using SQLite;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Media.Imaging;

// The Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234233

namespace AttendancePrototype1.Pages
{
    /// <summary>
    /// A page that displays a collection of item previews.  In the Split Application this page
    /// is used to display and select one of the available groups.
    /// </summary>
    /// Snapped Event Handler
    
    


    public sealed partial class Langing2 : Page
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

        public Langing2()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
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
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            // TODO: Assign a bindable collection of items to this.DefaultViewModel["Items"]
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            /// Open the database and add all the present subjects to the view.
            /// If the addedsubjects = 0
            /// Display a No subjects added box.
            /// Call a function to get the list of all the subjects.

            helpContent.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                
                Date.Text = "Date : " + DateTime.Now.Date.ToString("dd/MM/yy");
                var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                Username.Text = "Hello " + localSettings.Values["TheUser"].ToString() + "!!";

                ShowGridView();
            
            /// Add the add new subject button at the end.!!!!
            /// A stackPanel.
        }

        private void ShowGridView()
        {
            var subjects = GetListOfSubjects();
            SubjectGridView.Items.Clear();
            NumberOfSubjects.Text = subjects.Count.ToString();
            if (subjects.Count == 0)
            {
                SubjectGridView.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                helpContent.Visibility = Windows.UI.Xaml.Visibility.Visible;
                
            }
            else
            {
                SubjectGridView.Visibility = Windows.UI.Xaml.Visibility.Visible;
                helpContent.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                foreach (var item in subjects)
                {
                    SubjectGridView.Items.Add(item);
                }
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
        }

        #endregion

        private List<Model.Subject> GetListOfSubjects()
        {
            List<Model.Subject> subjects = new List<Model.Subject>();
            try
            {    
                var dbPath = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "SubjectRelatedInformation.db");
                SQLiteConnection conn = new SQLiteConnection(dbPath);

                var query = conn.Table<Model.Subject>();

                subjects = query.ToList<Model.Subject>();

                return subjects;
            }
            catch (Exception ex)
            {
                return subjects;
            }
        }


        private void UpdateAttendance_Click(object sender, RoutedEventArgs e)
        {
            Subjects.Items.Clear();
            List<Model.Subject> subjectList = GetListOfSubjects();
            if (subjectList.Count == 0)
            {
                ShowDialogAsync("No Added Subjects to update!! Add a subject to Continue..");
            }
            else
            {
                foreach (var item in subjectList)
                {
                    Subjects.Items.Add(item.Name);
                }
                for (int i = 0; i < 11; i++)
                {
                    Held.Items.Add(i);
                    Attended.Items.Add(i);
                }
                Subjects.SelectedIndex = 0;
                Held.SelectedIndex = 0;
                Attended.SelectedIndex = 0;
            }
        }

        private void MakeAttendanceUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateUpdate())
            {
                var dbPath = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "SubjectRelatedInformation.db");
                SQLiteConnection conn = new SQLiteConnection(dbPath);

                var query = conn.Table<Model.Subject>().Where(x => x.Name == Subjects.SelectedItem);

                Model.Subject updateSubject = query.FirstOrDefault();
                
                updateSubject.LastUpdated = DateTime.Now.Date.ToString("dd/MM/yy");
                updateSubject.NumClassesAttended += int.Parse(Attended.SelectedItem.ToString());
                updateSubject.NumClassesHeld += int.Parse(Held.SelectedIndex.ToString());
                updateSubject.PercentAttendance = float.Parse((((float)updateSubject.NumClassesAttended / (float)updateSubject.NumClassesHeld) * 100).ToString("0.00"));
                conn.Update(updateSubject);
                SubjectGridView.Items.Clear();
                ShowGridView();
            }
        }

        private bool ValidateUpdate()
        {
            if (int.Parse(Held.SelectedItem.ToString()) < int.Parse(Attended.SelectedItem.ToString()))
            {
                ShowDialogAsync("The classes Attended should be less than Held");
                return false;
            }
            return true;
        }
        
        private async void ShowDialogAsync(String Error)
        {
            MessageDialog messageDialog = new MessageDialog(Error, "Error!!!!");
            messageDialog.Commands.Add(new UICommand(
        "Try again",
        new UICommandInvokedHandler(this.CommandInvokedHandler)));
            await messageDialog.ShowAsync();
        }

        private void CommandInvokedHandler(IUICommand command)
        {
            
        }

        private void DeleteSubject_Click(object sender, RoutedEventArgs e)
        {
            var itemSelected = SubjectGridView.SelectedItem;
            if (itemSelected != null)
            {
                Model.Subject selectedSubject = (Model.Subject)itemSelected;
                var dbPath = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "SubjectRelatedInformation.db");
                SQLiteConnection conn = new SQLiteConnection(dbPath);
                conn.Delete<Model.Subject>(selectedSubject.Code);
                //SubjectGridView.Items.Remove(itemSelected);
                SubjectGridView.Items.Clear();
                ShowGridView();
            }
            else
            {
                ShowDialogAsync("No selected subject!!!!!!");
            }
        }

        private void AddSubject_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AddSubjectDetails));
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            ShowGridView();
        }

        private void SubjectGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (SubjectGridView.SelectedItem != null)
            {
                mainCommandBottom.IsOpen = true;
            }
        }

        private void SubjectGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SubjectGridView.SelectedItem != null)
            {
                mainCommandBottom.IsOpen = true;
            }
        }

        private void EditSubject_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = SubjectGridView.SelectedItem;
            if (selectedItem != null)
            {
                this.Frame.Navigate(typeof(EditCourseDetails), selectedItem);
            }
            else
            {
                ShowDialogAsync("Select a subject to edit!!!");
            }
        }









        /// <summary>
        /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// //////////////////////////////////////////////////
        /// ///////////////////////////////////////////////////
        /// //////////////////////////////////////////
        /// ////////////////////////////////////////////
        /// //////////////////////////
        /// 
        /// If the size of the page is changed
        /// Like if the app is snapped,
        /// Then we have to 
        /// Check the resolution.
        /// And make it accordingly available.
        /// 
        /// 
        /// 
        /// 
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pageRoot_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var bounds = Window.Current.Bounds;

            double height = bounds.Height;

            double width = bounds.Width;
            
            

            if (height < 768 || width < 1280 )
            {
                ParentGrid.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
            else
            {
                ParentGrid.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }

        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Tutorial));
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            ResetDialog();
        }

        private async void ResetDialog()
        {
            MessageDialog messageDialog = new MessageDialog("Do you really want to continue? Keep in mind that all your attendance records will be reset, and so will be your information!!", "Warning!!!");
            messageDialog.Commands.Add(new UICommand("Cancel", new UICommandInvokedHandler(this.ResetCommandInvokedHandler)));
            messageDialog.Commands.Add(new UICommand("Yes, Continue", new UICommandInvokedHandler(this.ResetCommandInvokedHandler)));
            await messageDialog.ShowAsync();
        }

        private void ResetCommandInvokedHandler(IUICommand command)
        {
            if (command.Label == "Yes, Continue")
            {
                /// Delete the data
                /// From the own cleardata file.
                /// 

                Methods.ModifyData.ClearAppData.ClearApplicationData();
                this.Frame.Navigate(typeof(MainPage));
            }
        }

        private void donechangename_Click(object sender, RoutedEventArgs e)
        {
            if (newname.Text.Length < 2 || newname.Text.Length > 30)
            {
                ShowDialogAsync("The name should be between 2 to 30 chars.");
            }
            else
            {
                var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                localSettings.Values["TheUser"] = newname.Text;
                this.Frame.Navigate(typeof(Langing2));
            }
        }

    }
}
