namespace IndividualProjectPartB.DBModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Text.RegularExpressions;

    public partial class Course
    {
        public Course()
        {
            Assignments = new HashSet<Assignment>();
            Students = new HashSet<Student>();
            Trainers = new HashSet<Trainer>();
        }

        [Key]
        public int courseId { get; set; }

        [Required]
        [StringLength(30)]
        public string title { get; set; }

        [Required]
        [StringLength(30)]
        public string stream { get; set; }

        [Required]
        [StringLength(30)]
        public string type { get; set; }

        [Column(TypeName = "date")]
        public DateTime start_date { get; set; }

        [Column(TypeName = "date")]
        public DateTime end_date { get; set; }

        public virtual ICollection<Assignment> Assignments { get; set; }

        public virtual ICollection<Student> Students { get; set; }

        public virtual ICollection<Trainer> Trainers { get; set; }

        public void ListOfCourses(IndividualProjectDBModel iPModel)
        {
            var courseList = iPModel.Courses.ToList();
            Console.WriteLine("Title --- Stream --- Type --- StartDate --- EndDate");
            int counter = 1;
            foreach (var crs in courseList)
            {
                Console.WriteLine($"#{counter} {crs.title} --- {crs.stream} --- {crs.type} --- " +
                                  $"{crs.start_date:dd / MM / yyyy} --- {crs.end_date:dd / MM / yyyy}");
                counter++;
            }
        }
        public void AddCourses(IndividualProjectDBModel iPModel, int courseListCount)
        {
            try
            {
                Console.WriteLine("Type number of courses to add");
                string isInt = Console.ReadLine();
                int courseNumber;
                while (!int.TryParse(isInt, out courseNumber) || courseNumber <= 0)
                {
                    Console.WriteLine("Invalid input, please retry");
                    isInt = Console.ReadLine();
                }
                Console.WriteLine("Type course's properties in the following order:\n" +
                                  "Title --- Stream --- Type --- Start_Date (DD/MM/YYYY) --- End_Date (DD/MM/YYYY)");
                for (int i = 0; i < courseNumber; i++)
                {
                    Course course = new Course();
                    string tl = Console.ReadLine();
                    while (!Regex.IsMatch(tl, "^[A-Za-z ][A-Za-z0-9!@#$%^&* ]*$", RegexOptions.IgnoreCase))
                    {
                        Console.WriteLine("Invalid input, please enter valid characters for Title");
                        tl = Console.ReadLine();
                    }
                    course.title = tl;
                    string stm = Console.ReadLine();
                    while (!Regex.IsMatch(stm, "^[A-Za-z ][A-Za-z0-9!@#$%^&* ]*$", RegexOptions.IgnoreCase))
                    {
                        Console.WriteLine("Invalid input, please enter valid characters for Stream");
                        stm = Console.ReadLine();
                    }
                    course.stream = stm;
                    string tp = Console.ReadLine();
                    while (!Regex.IsMatch(tp, "^[A-Za-z ][A-Za-z0-9!@#$%^&* ]*$", RegexOptions.IgnoreCase))
                    {
                        Console.WriteLine("Invalid input, please enter valid characters for Type");
                        tp = Console.ReadLine();
                    }
                    course.type = tp;
                    string isStartDate = Console.ReadLine();
                    DateTime startDate;
                    while (!DateTime.TryParseExact(isStartDate, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out startDate))
                    {
                        Console.WriteLine("Invalid input, please enter a valid Date(e.g., 05/06/2020)");
                        isStartDate = Console.ReadLine();
                    }
                    course.start_date = startDate;
                    string isEndDate = Console.ReadLine();
                    DateTime endDate;
                    while (!DateTime.TryParseExact(isEndDate, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out endDate))
                    {
                        Console.WriteLine("Invalid input, please enter a valid Date(e.g., 05/06/2020)");
                        isEndDate = Console.ReadLine();
                    }
                    course.end_date = endDate;
                    iPModel.Courses.Add(course);
                    iPModel.SaveChanges();
                    if (i < courseNumber - 1)
                    {
                        Console.WriteLine("Type properties of new course");
                    }
                }
                List<Course> courseList = iPModel.Courses.ToList();
                var addedCoursesList = courseList.Skip(courseListCount).ToList();
                Console.WriteLine("List of added courses:");
                foreach (var crs in addedCoursesList)
                {
                    Console.WriteLine($"{crs.title} --- {crs.stream} --- {crs.type} --- {crs.start_date:dd / MM / yyyy} --- {crs.end_date:dd / MM / yyyy}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
