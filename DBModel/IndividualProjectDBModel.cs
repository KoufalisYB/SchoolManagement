namespace IndividualProjectPartB.DBModel
{
    using System.Data.Entity;

    public partial class IndividualProjectDBModel : DbContext
    {
        public IndividualProjectDBModel()
            : base("name=IndividualProjectDBModel")
        {
        }

        public virtual DbSet<Assignment> Assignments { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Trainer> Trainers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Assignment>()
                .Property(e => e.title)
                .IsUnicode(false);

            modelBuilder.Entity<Assignment>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<Assignment>()
                .Property(e => e.oralMark)
                .HasPrecision(5, 2);

            modelBuilder.Entity<Assignment>()
                .Property(e => e.totalMark)
                .HasPrecision(5, 2);

            modelBuilder.Entity<Assignment>()
                .HasMany(e => e.Courses)
                .WithMany(e => e.Assignments)
                .Map(m => m.ToTable("AssignmentsPerCourse").MapLeftKey("assignmentId").MapRightKey("courseId"));

            modelBuilder.Entity<Course>()
                .Property(e => e.title)
                .IsUnicode(false);

            modelBuilder.Entity<Course>()
                .Property(e => e.stream)
                .IsUnicode(false);

            modelBuilder.Entity<Course>()
                .Property(e => e.type)
                .IsUnicode(false);

            modelBuilder.Entity<Course>()
                .HasMany(e => e.Students)
                .WithMany(e => e.Courses)
                .Map(m => m.ToTable("StudentsPerCourse").MapLeftKey("courseId").MapRightKey("studentId"));

            modelBuilder.Entity<Course>()
                .HasMany(e => e.Trainers)
                .WithMany(e => e.Courses)
                .Map(m => m.ToTable("TrainersPerCourse").MapLeftKey("courseId").MapRightKey("trainerId"));

            modelBuilder.Entity<Student>()
                .Property(e => e.firstName)
                .IsUnicode(false);

            modelBuilder.Entity<Student>()
                .Property(e => e.lastName)
                .IsUnicode(false);

            modelBuilder.Entity<Student>()
                .Property(e => e.tuitionFees)
                .HasPrecision(8, 2);

            modelBuilder.Entity<Trainer>()
                .Property(e => e.firstName)
                .IsUnicode(false);

            modelBuilder.Entity<Trainer>()
                .Property(e => e.lastName)
                .IsUnicode(false);

            modelBuilder.Entity<Trainer>()
                .Property(e => e.subject)
                .IsUnicode(false);
        }
    }
}
