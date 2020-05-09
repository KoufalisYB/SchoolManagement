namespace IndividualProjectPartB.DBModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Linq;
    public partial class Student
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Student()
        {
            Courses = new HashSet<Course>();
        }

        public int studentId { get; set; }

        [Required]
        [StringLength(30)]
        public string firstName { get; set; }

        [Required]
        [StringLength(30)]
        public string lastName { get; set; }

        [Column(TypeName = "date")]
        public DateTime dateOfBirth { get; set; }

        public decimal tuitionFees { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Course> Courses { get; set; }

        public void ListOfStudents(IndividualProjectDBModel ipModel)
        {
            var studentList = ipModel.Students.ToList();
            Console.WriteLine("Firstname / Lastname / DateOfBirth / TuitionFees");
            int counter = 1;
            foreach (var std in studentList)
            {
                Console.WriteLine($"#{counter} {std.firstName} / {std.lastName} / {std.dateOfBirth.ToShortDateString()} / {std.tuitionFees}");
                counter++;
            }
        }
        public void StudentsPerCourse(IndividualProjectDBModel iPModel)
        {
            var studentsPerCourse = iPModel.Students.Include(s => s.Courses).ToList();
            foreach (var s in studentsPerCourse)
            {
                foreach (var c in s.Courses)
                {
                    Console.WriteLine($"Student {s.firstName} {s.lastName} has {c.title} {c.stream} {c.type}");
                }
            }
        }
        public void StudentsWithMoreCourses(IndividualProjectDBModel iPModel)
        {
            var studentsWithMoreCourses = iPModel.Students
                .Include(s => s.Courses)
                .Where(s => s.Courses.Count > 1)
                .Select(s => new { s.firstName, s.lastName })
                .ToList();
            foreach (var s in studentsWithMoreCourses)
            {
                Console.WriteLine($"{s.firstName} {s.lastName}");
            }
        }
        public void AddStudents(IndividualProjectDBModel iPModel, int listcount)
        {

            Console.WriteLine("Type number of students to add");
            int studentNumber = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Type student's properties in the following order:\n" +
                              "First Name / Last Name / Date Of Birth / Tuition Fees");
            for (int i = 0; i < studentNumber; i++)
            {
                var student = new Student
                {
                    firstName = Console.ReadLine(),
                    lastName = Console.ReadLine(),
                    dateOfBirth = Convert.ToDateTime(Console.ReadLine()),
                    tuitionFees = Convert.ToDecimal(Console.ReadLine())
                };
                iPModel.Students.Add(student);
                iPModel.SaveChanges();
                if (i < studentNumber - 1)
                {
                    Console.WriteLine("Type properties of new student");
                }
            }
            var studentList = iPModel.Students.ToList();
            var addedStudentsList = studentList.Skip(listcount).ToList();
            int counter = 1;
            Console.WriteLine("List of added students:");
            foreach (var std in addedStudentsList)
            {
                Console.WriteLine($"#{counter} {std.firstName} / {std.lastName} / {std.dateOfBirth.ToShortDateString()} / {std.tuitionFees}");
                counter++;
            }
        }

        public void AddStudentsPerCourse(IndividualProjectDBModel iPModel)
        {
            var studentList = iPModel.Students.ToList();
            Console.WriteLine("Type number(#) of course");
            int crsCounter = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine($"How many students do you want to add to {crsCounter} course?");
            int studentNumber = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Type number(#) of student to add");
            for (int i = 0; i < studentNumber; i++)
            {
                int stdCounter = Convert.ToInt32(Console.ReadLine());
                var id = 0;
                for (int j = 0; j < studentList.Count; j++)
                {
                    if (j+1 == stdCounter)
                    {
                        id = studentList[j].studentId;
                        break;
                    }   
                }
                
                try
                {
                    var addStudentInCourse = iPModel.Students
                        .Include("Courses").FirstOrDefault(s => s.studentId == id);
                    addStudentInCourse.Courses.Add(addStudentInCourse.Courses.FirstOrDefault(c => c.courseId == crsCounter));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                iPModel.SaveChanges();
                if (i < studentNumber - 1)
                {
                    Console.WriteLine("Type number(#) of new student to add");
                }
            }
        }
    }
}
