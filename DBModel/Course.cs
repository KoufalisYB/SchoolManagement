namespace IndividualProjectPartB.DBModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    [Table("Courses")]
    public partial class Course
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Assignment> Assignments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Student> Students { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Trainer> Trainers { get; set; }

        public void ListOfCourses(List<Course> courseList)
        {
            Console.WriteLine("Title / Stream / Type / StartDate / EndDate");
            int counter = 1;
            foreach (var crs in courseList)
            {
                Console.WriteLine($"{counter} {crs.title} / {crs.stream} / {crs.type} " +
                                  $"{crs.start_date.ToShortDateString()} / {crs.end_date.ToShortDateString()}");
                counter++;
            }
        }
        public void AddCourses(IndividualProjectDBModel iPModel, Course course, List<Course> courseList)
        {
            Console.WriteLine("Type number of courses to add");
            int courseNumber = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Type course's properties in the following order:\n" +
                              "Title / Stream / Type / Start_Date / End_Date");
            for (int i = 0; i < courseNumber; i++)
            {
                course = new Course
                {
                    title = Console.ReadLine(),
                    stream = Console.ReadLine(),
                    type = Console.ReadLine(),
                    start_date = Convert.ToDateTime(Console.ReadLine()),
                    end_date = Convert.ToDateTime(Console.ReadLine())
                };
                iPModel.Courses.Add(course);
                iPModel.SaveChanges();
                if (i < courseNumber - 1)
                {
                    Console.WriteLine("Type properties of new course");
                }
            }
            var addedCoursesList = courseList.Skip(4).ToList();
            int counter = 1;
            Console.WriteLine("List of added courses:");
            foreach (var crs in addedCoursesList)
            {
                Console.WriteLine($"#{counter} {crs.title} / {crs.stream} / {crs.type} / {crs.start_date} / {crs.end_date}");
                counter++;
            }
        }
    }
}
