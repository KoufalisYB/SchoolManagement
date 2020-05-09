namespace IndividualProjectPartB.DBModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Linq;
    public partial class Assignment
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Assignment()
        {
            Courses = new HashSet<Course>();
        }

        public int assignmentId { get; set; }

        [Required]
        [StringLength(30)]
        public string title { get; set; }

        [Required]
        [StringLength(50)]
        public string description { get; set; }

        [Column(TypeName = "date")]
        public DateTime subDateTime { get; set; }

        public decimal oralMark { get; set; }

        public decimal totalMark { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Course> Courses { get; set; }

        public void ListOfAssignments(List<Assignment> assignmentList)
        {
            Console.WriteLine("Title / Description / SubDateTime / OralMark / TotalMark");
            int counter = 1;
            foreach (var asn in assignmentList)
            {
                Console.WriteLine($"{counter} {asn.title} / {asn.description} / {asn.subDateTime.ToShortDateString()} " +
                                  $"/ {asn.oralMark} / {asn.totalMark}");
                counter++;
            }
        }
        public void AssignmentsPerCourse(IndividualProjectDBModel iPModel)
        {
            var assignmentsPerCourse = iPModel.Assignments.Include(a => a.Courses).ToList();
            foreach (var a in assignmentsPerCourse)
            {
                foreach (var c in a.Courses)
                {
                    Console.WriteLine($"Assignment {a.title} {a.description} has {c.title} {c.stream} {c.type}");
                }
            }
        }
        public void AssignmentsPerCoursePerStudent(IndividualProjectDBModel iPModel)
        {
            var assignmentsPerCoursePerStudent =
                iPModel.Assignments.Include(a => a.Courses.Select(c => c.Students)).ToList();
            foreach (var a in assignmentsPerCoursePerStudent)
            {
                foreach (var c in a.Courses)
                {
                    foreach (var s in c.Students)
                    {
                        Console.WriteLine($"Assignment {a.title} {a.description}" +
                                          $" has {c.title} {c.stream} {c.type} course and {s.firstName} {s.lastName}");
                    }
                }
            }
        }
        public void AddAssignments(IndividualProjectDBModel iPModel, Assignment assignment, List<Assignment> assignmentList)
        {
            Console.WriteLine("Type number of assignments to add");
            int assignmentNumber = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Type assignment's properties in the following order:\n" +
                              "Title / Description / SubDateTime / OralMark / TotalMark");
            for (int i = 0; i < assignmentNumber; i++)
            {
                assignment = new Assignment
                {
                    title = Console.ReadLine(),
                    description = Console.ReadLine(),
                    subDateTime = Convert.ToDateTime(Console.ReadLine()),
                    oralMark = Convert.ToDecimal(Console.ReadLine()),
                    totalMark = Convert.ToDecimal(Console.ReadLine())
                };
                iPModel.Assignments.Add(assignment);
                iPModel.SaveChanges();
                if (i < assignmentNumber - 1)
                {
                    Console.WriteLine("Type properties of new assignment");
                }
            }
            var addedAssignmentsList = assignmentList.Skip(2).ToList();
            int counter = 1;
            Console.WriteLine("List of added assignments:");
            foreach (var asn in addedAssignmentsList)
            {
                Console.WriteLine($"#{counter} {asn.title} / {asn.description} / {asn.subDateTime} / {asn.oralMark} / {asn.totalMark}");
                counter++;
            }
        }
    }
}
