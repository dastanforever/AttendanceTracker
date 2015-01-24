using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AttendancePrototype1.Methods.ModifyData
{
    public static class ClearAppData
    {
        public static void ClearApplicationData()
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values.Remove("NumberOfSubjects");
            localSettings.Values.Remove("StudentRegistered");
            localSettings.Values.Remove("TheUser");
            localSettings.Values.Remove("FirstTime");
            localSettings.Values.Remove("StudentRegistered");

            var dbPath = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "SubjectRelatedInformation.db");
            SQLiteConnection conn = new SQLiteConnection(dbPath);

            conn.DropTable<Model.Subject>();

            dbPath = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "StudentInformation.db");
            conn = new SQLiteConnection(dbPath);

            conn.DropTable<Model.Student>();

        }
    }
}
