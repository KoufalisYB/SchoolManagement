namespace IndividualProjectPartB.DBModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity;
    using System.Linq;

    public partial class Trainer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Course> Courses { get; set; }

        public void ListOfTrainers(List<Trainer> trainerList)
        {
            Console.WriteLine("Firstname / Lastname / Subject");
            int counter = 1;
            foreach (var trn in trainerList)
            {
                Console.WriteLine($"{counter} {trn.firstName} / {trn.lastName} / {trn.subject}");
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
                    Console.WriteLine($"Trainer {t.firstName} {t.lastName} has {c.title} {c.stream} {c.type}");
                }
            }
        }
        public void AddTrainers(IndividualProjectDBModel iPModel, Trainer trainer, List<Trainer> trainerList)
        {
            Console.WriteLine("Type number of trainers to add");
            int trainerNumber = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Type trainer's properties in the following order:\n" +
                              "First Name / Last Name / Subject");
            for (int i = 0; i < trainerNumber; i++)
            {
                trainer = new Trainer
                {
                    firstName = Console.ReadLine(),
                    lastName = Console.ReadLine(),
                    subject = Console.ReadLine()
                };
                iPModel.Trainers.Add(trainer);
                iPModel.SaveChanges();
                if (i < trainerNumber - 1)
                {
                    Console.WriteLine("Type properties of new trainer");
                }
            }
            var addedTrainersList = trainerList.Skip(4).ToList();
            int counter = 1;
            Console.WriteLine("List of added trainers:");
            foreach (var trn in addedTrainersList)
            {
                Console.WriteLine($"#{counter} {trn.firstName} / {trn.lastName} / {trn.subject}");
                counter++;
            }
        }
    }
}
