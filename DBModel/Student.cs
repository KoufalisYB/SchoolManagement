namespace IndividualProjectPartB.DBModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Linq;
    using System.Text.RegularExpressions;

    public partial class Student
    {
        public Student()
        {
            Courses = new HashSet<Course>();
        }

        public int studentId { get; set; }

        [Required] [StringLength(30)] public string firstName { get; set; }

        [Required] [StringLength(30)] public string lastName { get; set; }

        [Column(TypeName = "date")] public DateTime dateOfBirth { get; set; }

        public decimal tuitionFees { get; set; }

        public virtual ICollection<Course> Courses { get; set; }

        public void ListOfStudents(IndividualProjectDBModel iPModel)
        {
            var studentList = iPModel.Students.ToList();
            Console.WriteLine("Firstname --- Lastname --- DateOfBirth --- TuitionFees");
            int counter = 1;
            foreach (var std in studentList)
            {
                Console.WriteLine(
                    $"#{counter} {std.firstName} --- {std.lastName} --- {std.dateOfBirth:dd / MM / yyyy} --- {std.tuitionFees}");
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
                    Console.WriteLine($"Student {s.firstName} {s.lastName} has {c.title} {c.stream} {c.type} course");
                }
            }
        }

        public void StudentsWithMoreCourses(IndividualProjectDBModel iPModel)
        {
            var studentsWithMoreCourses = iPModel.Students
                .Include(s => s.Courses)
                .Where(s => s.Courses.Count > 1)
                .Select(s => new { s.firstName, s.lastName, s.Courses.Count })
                .ToList();
            foreach (var s in studentsWithMoreCourses)
            {
                Console.WriteLine($"{s.firstName} {s.lastName} has {s.Count} courses");
            }
        }

        public void AddStudents(IndividualProjectDBModel iPModel, int studentListCount)
        {
            try
            {
                Console.WriteLine("Type number of students to add");
                string isInt = Console.ReadLine();
                int studentNumber;
                while (!int.TryParse(isInt, out studentNumber) || studentNumber <= 0)
                {
                    Console.WriteLine("Invalid input, please retry");
                    isInt = Console.ReadLine();
                }

                Console.WriteLine("Type student's properties in the following order:\n" +
                                  "First Name --- Last Name --- Date Of Birth (DD/MM/YYYY) --- Tuition Fees (Decimal)");
                for (int i = 0; i < studentNumber; i++)
                {
                    Student student = new Student();
                    string firstN = Console.ReadLine();
                    while (!Regex.IsMatch(firstN, "^[A-Za-z ][A-Za-z0-9!@#$%^&* ]*$", RegexOptions.IgnoreCase))
                    {
                        Console.WriteLine("Invalid input, please enter valid characters for First Name");
                        firstN = Console.ReadLine();
                    }

                    student.firstName = firstN;
                    string lastN = Console.ReadLine();
                    while (!Regex.IsMatch(lastN, "^[A-Za-z ][A-Za-z0-9!@#$%^&* ]*$"))
                    {
                        Console.WriteLine("Invalid input, please enter valid characters for Last Name");
                        lastN = Console.ReadLine();
                    }

                    student.lastName = lastN;
                    string isDateOfBirth = Console.ReadLine();
                    DateTime dtOfBirth;
                    while (!DateTime.TryParseExact(isDateOfBirth, "dd/MM/yyyy", null,
                        System.Globalization.DateTimeStyles.None, out dtOfBirth))
                    {
                        Console.WriteLine("Invalid input, please enter a valid Date(e.g., 05/06/2020)");
                        isDateOfBirth = Console.ReadLine();
                    }

                    student.dateOfBirth = dtOfBirth;
                    string isTuitionFees = Console.ReadLine();
                    decimal tuitionF;
                    while (!decimal.TryParse(isTuitionFees, out tuitionF) || tuitionF <= 0)
                    {
                        Console.WriteLine("Invalid input, please retry");
                        isTuitionFees = Console.ReadLine();
                    }

                    student.tuitionFees = tuitionF;
                    iPModel.Students.Add(student);
                    iPModel.SaveChanges();
                    if (i < studentNumber - 1)
                    {
                        Console.WriteLine("Type properties of new student");
                    }
                }

                List<Student> studentList = iPModel.Students.ToList();
                var addedStudentsList = studentList.Skip(studentListCount).ToList();
                Console.WriteLine("List of added students:");
                foreach (var std in addedStudentsList)
                {
                    Console.WriteLine(
                        $"{std.firstName} --- {std.lastName} --- {std.dateOfBirth:dd / MM / yyyy} --- {std.tuitionFees}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void AddStudentsPerCourse(IndividualProjectDBModel iPModel)
        {
            try
            {
                var studentList = iPModel.Students.ToList();
                var courseList = iPModel.Courses.ToList();
                Console.WriteLine("Type number(#) of course");
                string isCrsInt = Console.ReadLine();
                int crsCounter;
                while (!int.TryParse(isCrsInt, out crsCounter) || crsCounter <= 0 || crsCounter > courseList.Count)
                {
                    Console.WriteLine("That's not a valid course, please retry");
                    isCrsInt = Console.ReadLine();
                }

                Console.WriteLine($"How many students do you want to add to course #{crsCounter}?");
                string isStdInt = Console.ReadLine();
                int studentNumber;
                while (!int.TryParse(isStdInt, out studentNumber) || studentNumber <= 0 ||
                       studentNumber > studentList.Count)
                {
                    Console.WriteLine("Invalid input, please retry");
                    isStdInt = Console.ReadLine();
                }

                Console.WriteLine("Type number(#) of student to add");
                var cid = 0;
                for (int i = 0; i < studentNumber; i++)
                {
                    string isInt = Console.ReadLine();
                    int stdCounter;
                    while (!int.TryParse(isInt, out stdCounter) || stdCounter <= 0 || stdCounter > studentList.Count)
                    {
                        Console.WriteLine("That's not a valid student, please retry");
                        isInt = Console.ReadLine();
                    }

                    var sid = 0;
                    for (int j = 0; j < studentList.Count; j++)
                    {
                        if (j == stdCounter - 1)
                        {
                            sid = studentList[j].studentId;
                            break;
                        }
                    }

                    for (int k = 0; k < courseList.Count; k++)
                    {
                        if (k == crsCounter - 1)
                        {
                            cid = courseList[k].courseId;
                            break;
                        }
                    }

                    var addStudentInCourse = iPModel.Students
                        .Include(s => s.Courses).FirstOrDefault(s => s.studentId == sid);
                    var courseToAddStudent = iPModel.Courses.Find(cid);
                    addStudentInCourse?.Courses.Add(courseToAddStudent);
                    int rowsAffected = iPModel.SaveChanges();
                    if (rowsAffected == 0)
                    {
                        Console.WriteLine(
                            $"Data couldn't be inserted because student #{stdCounter} already has course #{crsCounter}");
                    }

                    if (i < studentNumber - 1)
                    {
                        Console.WriteLine("Type number(#) of new student to add");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void ShowStudentsBasedOnCourseNumber(IndividualProjectDBModel iPModel)
        {
            try
            {
                var courseList = iPModel.Courses.ToList();
                Console.WriteLine("Type number(#) of course");
                string isCrsInt = Console.ReadLine();
                int crsCounter;
                while (!int.TryParse(isCrsInt, out crsCounter) || crsCounter <= 0 || crsCounter > courseList.Count)
                {
                    Console.WriteLine("That's not a valid course, please retry");
                    isCrsInt = Console.ReadLine();
                }
                for (int i = 0; i < courseList.Count; i++)
                {
                    if (i == crsCounter - 1)
                    {
                        var cid = courseList[i].courseId;
                        var studentsBasedOnCourse = iPModel.Courses.Include(c => c.Students)
                            .Where(c => c.courseId == cid).ToList();
                        foreach (var c in studentsBasedOnCourse)
                        {
                            foreach (var s in c.Students)
                            {
                                Console.WriteLine($"Course {c.title} {c.stream} {c.type} has {s.firstName} {s.lastName} student");
                            }
                        }
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
