namespace IndividualProjectPartB.DBModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity;
    using System.Linq;
    using System.Text.RegularExpressions;

    public partial class Trainer
    {
        public Trainer()
        {
            Courses = new HashSet<Course>();
        }

        public int trainerId { get; set; }

        [Required]
        [StringLength(30)]
        public string firstName { get; set; }

        [Required]
        [StringLength(30)]
        public string lastName { get; set; }

        [Required]
        [StringLength(30)]
        public string subject { get; set; }

        public virtual ICollection<Course> Courses { get; set; }

        public void ListOfTrainers(IndividualProjectDBModel iPModel)
        {
            var trainerList = iPModel.Trainers.ToList();
            Console.WriteLine("Firstname --- Lastname --- Subject");
            int counter = 1;
            foreach (var trn in trainerList)
            {
                Console.WriteLine($"#{counter} {trn.firstName} --- {trn.lastName} --- {trn.subject}");
                counter++;
            }
        }
        public void TrainersPerCourse(IndividualProjectDBModel ipModel)
        {
            var trainersPerCourse = ipModel.Trainers.Include(t => t.Courses).ToList();
            foreach (var t in trainersPerCourse)
            {
                foreach (var c in t.Courses)
                {
                    Console.WriteLine($"Trainer {t.firstName} {t.lastName} has {c.title} {c.stream} {c.type} course");
                }
            }
        }
        public void AddTrainers(IndividualProjectDBModel iPModel, int trainerListCount)
        {
            try
            {
                Console.WriteLine("Type number of trainers to add");
                string isInt = Console.ReadLine();
                int trainerNumber;
                while (!int.TryParse(isInt, out trainerNumber) || trainerNumber <= 0)
                {
                    Console.WriteLine("Invalid input, please retry");
                    isInt = Console.ReadLine();
                }
                Console.WriteLine("Type trainer's properties in the following order:\n" +
                                  "First Name --- Last Name --- Subject");
                for (int i = 0; i < trainerNumber; i++)
                {
                    Trainer trainer = new Trainer();
                    string firstN = Console.ReadLine();
                    while (!Regex.IsMatch(firstN, "^[A-Za-z ][A-Za-z0-9!@#$%^&* ]*$", RegexOptions.IgnoreCase))
                    {
                        Console.WriteLine("Invalid input, please enter valid characters for First Name");
                        firstN = Console.ReadLine();
                    }
                    trainer.firstName = firstN;
                    string lastN = Console.ReadLine();
                    while (!Regex.IsMatch(lastN, "^[A-Za-z ][A-Za-z0-9!@#$%^&* ]*$", RegexOptions.IgnoreCase))
                    {
                        Console.WriteLine("Invalid input, please enter valid characters for Last Name");
                        lastN = Console.ReadLine();
                    }
                    trainer.lastName = lastN;
                    string sbj = Console.ReadLine();
                    while (!Regex.IsMatch(sbj, "^[A-Za-z ][A-Za-z0-9!@#$%^&* ]*$", RegexOptions.IgnoreCase))
                    {
                        Console.WriteLine("Invalid input, please enter valid characters for Subject");
                        sbj = Console.ReadLine();
                    }
                    trainer.subject = sbj;
                    iPModel.Trainers.Add(trainer);
                    iPModel.SaveChanges();
                    if (i < trainerNumber - 1)
                    {
                        Console.WriteLine("Type properties of new trainer");
                    }
                }
                List<Trainer> trainerList = iPModel.Trainers.ToList();
                var addedTrainersList = trainerList.Skip(trainerListCount).ToList();
                Console.WriteLine("List of added trainers:");
                foreach (var trn in addedTrainersList)
                {
                    Console.WriteLine($"{trn.firstName} --- {trn.lastName} --- {trn.subject}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public void AddTrainersPerCourse(IndividualProjectDBModel iPModel)
        {
            try
            {
                var trainerList = iPModel.Trainers.ToList();
                var courseList = iPModel.Courses.ToList();
                Console.WriteLine("Type number(#) of course");
                string isCrsInt = Console.ReadLine();
                int crsCounter;
                while (!int.TryParse(isCrsInt, out crsCounter) || crsCounter <= 0 || crsCounter > courseList.Count)
                {
                    Console.WriteLine("That's not a valid course, please retry");
                    isCrsInt = Console.ReadLine();
                }
                Console.WriteLine($"How many trainers do you want to add to course #{crsCounter}?");
                string isTrnInt = Console.ReadLine();
                int trainerNumber;
                while (!int.TryParse(isTrnInt, out trainerNumber) || trainerNumber <= 0 || trainerNumber > trainerList.Count)
                {
                    Console.WriteLine("Invalid input, please retry");
                    isTrnInt = Console.ReadLine();
                }
                Console.WriteLine("Type number(#) of trainer to add");
                var cid = 0;
                for (int i = 0; i < trainerNumber; i++)
                {
                    string isInt = Console.ReadLine();
                    int trnCounter;
                    while (!int.TryParse(isInt, out trnCounter) || trnCounter <= 0 || trnCounter > trainerList.Count)
                    {
                        Console.WriteLine("That's not a valid trainer, please retry");
                        isInt = Console.ReadLine();
                    }
                    var tid = 0;
                    for (int j = 0; j < trainerList.Count; j++)
                    {
                        if (j == trnCounter - 1)
                        {
                            tid = trainerList[j].trainerId;
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
                    var addTrainerInCourse = iPModel.Trainers
                        .Include(t => t.Courses).FirstOrDefault(t => t.trainerId == tid);
                    var courseToAddTrainer = iPModel.Courses.Find(cid);
                    addTrainerInCourse?.Courses.Add(courseToAddTrainer);
                    int rowsAffected = iPModel.SaveChanges();
                    if (rowsAffected == 0)
                    {
                        Console.WriteLine($"Data couldn't be inserted because trainer #{trnCounter} already has course #{crsCounter}");
                    }
                    if (i < trainerNumber - 1)
                    {
                        Console.WriteLine("Type number(#) of new trainer to add");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public void ShowTrainersBasedOnCourseNumber(IndividualProjectDBModel iPModel)
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
                        var trainersBasedOnCourse = iPModel.Courses.Include(c => c.Trainers)
                            .Where(c => c.courseId == cid).ToList();
                        foreach (var c in trainersBasedOnCourse)
                        {
                            foreach (var t in c.Trainers)
                            {
                                Console.WriteLine($"Course {c.title} {c.stream} {c.type} has {t.firstName} {t.lastName} trainer");
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
