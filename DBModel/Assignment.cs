namespace IndividualProjectPartB.DBModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Linq;
    using System.Text.RegularExpressions;

    public partial class Assignment
    {
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

        public virtual ICollection<Course> Courses { get; set; }

        public void ListOfAssignments(IndividualProjectDBModel iPModel)
        {
            var assignmentList = iPModel.Assignments.ToList();
            Console.WriteLine("Title --- Description --- SubDateTime --- OralMark --- TotalMark");
            int counter = 1;
            foreach (var asn in assignmentList)
            {
                Console.WriteLine($"#{counter} {asn.title} --- {asn.description} --- {asn.subDateTime:dd / MM / yyyy} --- " +
                                  $"{asn.oralMark} --- {asn.totalMark}");
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
                    Console.WriteLine($"Assignment {a.title} {a.description} has {c.title} {c.stream} {c.type} course");
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
                        Console.WriteLine($"Assignment {a.title} {a.description} " +
                                          $"has {c.title} {c.stream} {c.type} course and {s.firstName} {s.lastName} student");
                    }
                }
            }
        }
        public void AddAssignments(IndividualProjectDBModel iPModel, int assignmentListCount)
        {
            try
            {
                Console.WriteLine("Type number of assignments to add");
                string isInt = Console.ReadLine();
                int assignmentNumber;
                while (!int.TryParse(isInt, out assignmentNumber) || assignmentNumber <= 0)
                {
                    Console.WriteLine("Invalid input, please retry");
                    isInt = Console.ReadLine();
                }
                Console.WriteLine("Type assignment's properties in the following order:\n" +
                                  "Title --- Description --- SubDateTime (DD/MM/YYYY) --- OralMark (Decimal) --- TotalMark (Decimal)");
                for (int i = 0; i < assignmentNumber; i++)
                {
                    Assignment assignment = new Assignment();
                    string tl = Console.ReadLine();
                    while (!Regex.IsMatch(tl, "^[A-Za-z ][A-Za-z0-9!@#$%^&* ]*$", RegexOptions.IgnoreCase))
                    {
                        Console.WriteLine("Invalid input, please enter valid characters for Title");
                        tl = Console.ReadLine();
                    }
                    assignment.title = tl;
                    string dcr = Console.ReadLine();
                    while (!Regex.IsMatch(dcr, "^[A-Za-z ][A-Za-z0-9!@#$%^&* ]*$", RegexOptions.IgnoreCase))
                    {
                        Console.WriteLine("Invalid input, please enter valid characters for Description");
                        dcr = Console.ReadLine();
                    }
                    assignment.description = dcr;
                    string isSubDateTime = Console.ReadLine();
                    DateTime subDtTime;
                    while (!DateTime.TryParseExact(isSubDateTime, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out subDtTime))
                    {
                        Console.WriteLine("Invalid input, please enter a valid Date(e.g., 05/06/2020)");
                        isSubDateTime = Console.ReadLine();
                    }
                    assignment.subDateTime = subDtTime;
                    string isOralMark = Console.ReadLine();
                    decimal oralMrk;
                    while (!decimal.TryParse(isOralMark, out oralMrk) || oralMrk <= 0)
                    {
                        Console.WriteLine("Invalid input, please retry");
                        isOralMark = Console.ReadLine();
                    }
                    assignment.oralMark = oralMrk;
                    string isTotalMark = Console.ReadLine();
                    decimal totalMrk;
                    while (!decimal.TryParse(isTotalMark, out totalMrk) || totalMrk <= 0)
                    {
                        Console.WriteLine("Invalid input, please retry");
                        isTotalMark = Console.ReadLine();
                    }
                    assignment.totalMark = totalMrk;
                    iPModel.Assignments.Add(assignment);
                    iPModel.SaveChanges();
                    if (i < assignmentNumber - 1)
                    {
                        Console.WriteLine("Type properties of new assignment");
                    }
                }
                List<Assignment> assignmentList = iPModel.Assignments.ToList();
                var addedAssignmentsList = assignmentList.Skip(assignmentListCount).ToList();
                Console.WriteLine("List of added assignments:");
                foreach (var asn in addedAssignmentsList)
                {
                    Console.WriteLine($"{asn.title} --- {asn.description} --- {asn.subDateTime:dd / MM / yyyy} --- {asn.oralMark} --- {asn.totalMark}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public void AddAssignmentsPerCourse(IndividualProjectDBModel iPModel)
        {
            try
            {
                var assignmentList = iPModel.Assignments.ToList();
                var courseList = iPModel.Courses.ToList();
                Console.WriteLine("Type number(#) of course");
                string isCrsInt = Console.ReadLine();
                int crsCounter;
                while (!int.TryParse(isCrsInt, out crsCounter) || crsCounter <= 0 || crsCounter > courseList.Count)
                {
                    Console.WriteLine("That's not a valid course, please retry");
                    isCrsInt = Console.ReadLine();
                }
                Console.WriteLine($"How many assignments do you want to add to course #{crsCounter}?");
                string isAsnInt = Console.ReadLine();
                int assignmentNumber;
                while (!int.TryParse(isAsnInt, out assignmentNumber) || assignmentNumber <= 0 || assignmentNumber > assignmentList.Count)
                {
                    Console.WriteLine("Invalid input, please retry");
                    isAsnInt = Console.ReadLine();
                }
                Console.WriteLine("Type number(#) of assignment to add");
                var cid = 0;
                for (int i = 0; i < assignmentNumber; i++)
                {
                    string isInt = Console.ReadLine();
                    int asnCounter;
                    while (!int.TryParse(isInt, out asnCounter) || asnCounter <= 0 || asnCounter > assignmentList.Count)
                    {
                        Console.WriteLine("That's not a valid assignment, please retry");
                        isInt = Console.ReadLine();
                    }
                    var aid = 0;
                    for (int j = 0; j < assignmentList.Count; j++)
                    {
                        if (j == asnCounter - 1)
                        {
                            aid = assignmentList[j].assignmentId;
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
                    var addAssignmentInCourse = iPModel.Assignments
                        .Include(a => a.Courses).FirstOrDefault(a => a.assignmentId == aid);
                    var courseToAddTrainer = iPModel.Courses.Find(cid);
                    addAssignmentInCourse?.Courses.Add(courseToAddTrainer);
                    int rowsAffected = iPModel.SaveChanges();
                    if (rowsAffected == 0)
                    {
                        Console.WriteLine($"Data couldn't be inserted because assignment #{asnCounter} already has course #{crsCounter}");
                    }
                    if (i < assignmentNumber - 1)
                    {
                        Console.WriteLine("Type number(#) of new assignment to add");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public void ShowAssignmentsBasedOnCourseNumber(IndividualProjectDBModel iPModel)
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
                        var assignmentsBasedOnCourse = iPModel.Courses.Include(c => c.Assignments)
                            .Where(c => c.courseId == cid).ToList();
                        foreach (var c in assignmentsBasedOnCourse)
                        {
                            foreach (var a in c.Assignments)
                            {
                                Console.WriteLine($"Course {c.title} {c.stream} {c.type} has {a.title} {a.description} assignment");
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
        public void ShowStudentsAssignmentsBasedOnCourseNumber(IndividualProjectDBModel iPModel)
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
                        var studentsAssignmentsBasedOnCourse = iPModel.Courses.Include(c => c.Students).Include(c => c.Assignments)
                            .Where(c => c.courseId == cid).ToList();
                        foreach (var c in studentsAssignmentsBasedOnCourse)
                        {
                            foreach (var s in c.Students)
                            {
                                foreach (var a in c.Assignments)
                                {
                                    Console.WriteLine($"Course {c.title} {c.stream} {c.type} has {s.firstName} {s.lastName} student " +
                                                      $"and {a.title} {a.description} assignment");
                                }
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
