using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;


namespace AttendancePrototype1.Model
{
    class Student
    {
        [PrimaryKey,Column("Name")]
        public string Name { get; set; }
        [Column("Age")]
        public int Age { get; set; }
        [Column("School")]
        public string School { get; set; }
        [Column("YearInSchool")]
        public int yearInSchool { get; set; }
        [Column("NumberOfSubjects")]
        public int NumSubjects { get; set; }


        public void setData(string name,int age,int year,string school,int numSubjects)
        {
            Name = name;
            Age = age;
            School = school;
            yearInSchool = year;
            NumSubjects = numSubjects;
        }
        public string getName()
        {
            return Name;
        }
        public int getAge()
        {
            return Age;
        }
        public string getSchool()
        {
            return School;
        }
        public int getYear()
        {
            return yearInSchool;
        }
    }
}
