﻿using AttendancePrototype1.Common;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace AttendancePrototype1.Pages
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class EditCourseDetails : Page
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


        public EditCourseDetails()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
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

        private Model.Subject SubjectToUpdate { get; set; }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SubjectToUpdate = e.Parameter as Model.Subject;

            var dbPath = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "SubjectRelatedInformation.db");
            SQLiteConnection conn = new SQLiteConnection(dbPath);

            var query = conn.Table<Model.Subject>().Where(x => x.Code == SubjectToUpdate.Code);
            SubjectToUpdate = query.FirstOrDefault();

            UpdateTextBoxes(SubjectToUpdate);


        }

        private void UpdateTextBoxes(Model.Subject subjectToUpdate)
        {
            SubjectCode.Text = subjectToUpdate.Code.ToString();
            SubName.Text = subjectToUpdate.Name.ToString();
            InstructorCode.Text = subjectToUpdate.InstructorName.ToString();
            ClassesHeld.Text = subjectToUpdate.NumClassesHeld.ToString();
            ClassesAttended.Text = subjectToUpdate.NumClassesAttended.ToString();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
        }

        #endregion

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            UpdateTextBoxes(SubjectToUpdate);
        }

        private bool ValidateInput()
        {
            if (SubjectCode.Text.Length < 2 || SubjectCode.Text.Length > 30)
            {
                ShowDialogAsync("Length of Subject Code should be less than 30 characters, and a minimum of 2 characters!!");
                SubjectCode.Text = "";
                return false;
            }
            if (InstructorCode.Text.Length < 2 || InstructorCode.Text.Length > 30)
            {
                ShowDialogAsync("The Instructor Name cannot be larger than 30 chars or lower than 2 chars!!");
                InstructorCode.Text = "";
                return false;
            }
            if (SubName.Text.Length < 2 || SubName.Text.Length > 30)
            {
                ShowDialogAsync("The subject Name should have a minimum of 2 chars, and a max of 30 chars!!");
                SubName.Text = "";
                return false;
            }
            try
            {
                var a = int.Parse(ClassesAttended.Text);
                var b = int.Parse(ClassesHeld.Text);
            }
            catch (Exception ex)
            {
                ShowDialogAsync(ex.Message);
                ClassesAttended.Text = "";
                ClassesHeld.Text = "";
            }
            if ((int.Parse(ClassesAttended.Text) > int.Parse(ClassesHeld.Text)) && int.Parse(ClassesAttended.Text) >= 0 && int.Parse(ClassesHeld.Text) >= 0)
            {
                ShowDialogAsync("The attended classes cannot be more that the classes held, and both of them should be greater than zero!");
                ClassesAttended.Text = "";
                ClassesHeld.Text = "";
                return false;
            }
            return true;
        }

        private async void ShowDialogAsync(String ex)
        {
            MessageDialog messageDialog = new MessageDialog(ex, "Error!!!!");

            messageDialog.Commands.Add(new UICommand(
        "Try again",
                new UICommandInvokedHandler(this.CommandInvokedHandler)));

            await messageDialog.ShowAsync();

        }

        private void CommandInvokedHandler(IUICommand command)
        {

        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateInput())
            {
                
                

                var dbPath = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "SubjectRelatedInformation.db");
                SQLiteConnection conn = new SQLiteConnection(dbPath);
                conn.CreateTable<AttendancePrototype1.Model.Subject>();

                conn.Delete<Model.Subject>(SubjectToUpdate.Code);

                AttendancePrototype1.Model.Subject subject = new Model.Subject();
                subject.Code = SubjectCode.Text;
                subject.InstructorName = InstructorCode.Text;
                subject.Name = SubName.Text.ToString();
                subject.NumClassesAttended = int.Parse(ClassesAttended.Text);
                subject.NumClassesHeld = int.Parse(ClassesHeld.Text);
                subject.PercentAttendance = float.Parse(((float)subject.NumClassesAttended / (float)subject.NumClassesHeld * 100).ToString("0.00"));
                subject.LastUpdated = DateTime.Now.Date.ToString("dd/MM/yy");
                subject.UpdateColor();
                conn.Insert(subject);

                


                Redirect();

            }
        }

        private void Redirect()
        {
            this.Frame.Navigate(typeof(Langing2));
        }

        private void pageRoot_SizeChanged(object sender, SizeChangedEventArgs e)
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