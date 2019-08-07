namespace ExportExcelDemo
{
    using System.Collections.Generic;
    using System.ComponentModel;

    public class Student
    {
        public int ID { get; set; }

        [DisplayName("姓名")]
        public string Name { get; set; }

        public string Gender { get; set; }

        [DisplayName("The Age")]
        public int Age { get; set; }

        public string Email { get; set; }
    }

    public class StaticDataOfStudent
    {
        public static List<Student> ListStudent
        {
            get
            {
                return new List<Student>()
                {
                    new Student() { ID=1, Name="Catcher", Gender="男", Email="example@example.com", Age=18},
                    new Student() { ID=2, Name="Kobe", Gender="男", Email="example@example.com", Age=24},
                    new Student() { ID=3, Name="James", Gender="男", Email="example@example.com", Age=26},
                    new Student() { ID=4, Name="Lisa", Gender="女", Email="example@example.com", Age=24},
                };
            }
        }
    }
}
