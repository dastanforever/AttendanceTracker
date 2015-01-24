using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Windows.UI.Xaml.Media;
using Windows.UI;

namespace AttendancePrototype1.Model
{
    public class Subject
    {
        [PrimaryKey,Column("SubjectCode")]
        public string Code { get; set; }
        [Column("Name")]
        public string Name { get; set; }
        [Column("InstructorName")]
        public string InstructorName { get; set; }
        [Column("NumClassesHeld")]
        public int NumClassesHeld{ get; set; }
        [Column("NumClassesAttended")]
        public int NumClassesAttended { get; set; }
        [Column("PercentAttend")]
        public float PercentAttendance { get; set; }
        [Column("LastUpdate")]
        public String LastUpdated { get; set; }
        [Column("Color")]
        public string color { get; set; }


        public void setName(string name)
        {
            Name = name;
        }

        public void setCode(string code)
        {
            Code = code;
        }

        public void setInstructorName(string name)
        {
            Name = name;
        }

        public string getName()
        {
            return Name;
        }

        public string getCode()
        {
            return Code;
        }

        public string getInstructorName()
        {
            return InstructorName;
        }

        public void UpdateNumHeld(int num)
        {
            NumClassesHeld += num;
            UpdatePercentage();
        }

        public void UpdateNumAttended(int num)
        {
            NumClassesAttended += num;
            UpdatePercentage();
        }

        public void UpdatePercentage()
        {
            PercentAttendance = ((NumClassesAttended) / NumClassesHeld) * 100;
        }
        public float ReturnPercentage()
        {
            return PercentAttendance;
        }
        public void UpdateColor()
        {
            if (PercentAttendance > 85)
            {
                color = (new SolidColorBrush(Colors.LightGreen)).ToString();
            }
            else if(75 < PercentAttendance && PercentAttendance < 85)
            {
                color = (new SolidColorBrush(Colors.GreenYellow)).ToString();
            }
            else
            {
                color = (new SolidColorBrush(Colors.Red)).ToString();
            }
        }
    }
}
