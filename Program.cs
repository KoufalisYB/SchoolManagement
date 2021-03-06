﻿using System;
using System.Collections.Generic;
using System.Linq;
using IndividualProjectPartB.DBModel;

namespace IndividualProjectPartB
{
    public class Program
    {
        static void Main(string[] args)
        {
            IndividualProjectDBModel iPModel = new IndividualProjectDBModel();
            List<Student> studentList = iPModel.Students.ToList();
            Student student = new Student();
            List<Trainer> trainerList = iPModel.Trainers.ToList();
            Trainer trainer = new Trainer();
            List<Assignment> assignmentList = iPModel.Assignments.ToList();
            Assignment assignment = new Assignment();
            List<Course> courseList = iPModel.Courses.ToList();
            Course course = new Course();
            bool menuFlag = true;
            bool selectDataFlag = true;
            bool addDataFlag = true;
            do
            {
                Console.WriteLine("---Main Menu---");
                Console.WriteLine("Type");
                Console.WriteLine("1: To select data from database");
                Console.WriteLine("2: To add data to the database");
                Console.WriteLine("3: To exit");
                int.TryParse(Console.ReadLine(), out int menuChoice);
                switch (menuChoice)
                {
                    case 1:
                        SelectData();
                        break;
                    case 2:
                        AddData();
                        break;
                    case 3:
                        menuFlag = false;
                        break;
                    default:
                        Console.WriteLine("Invalid input, please try again\n");
                        break;
                }
            } while (menuFlag);

            void SelectData()
            {
                do
                {
                    Console.WriteLine("\nType");
                    Console.WriteLine("1: To show all the students");
                    Console.WriteLine("2: To show all the trainers");
                    Console.WriteLine("3: To show all the assignments");
                    Console.WriteLine("4: To show all the courses");
                    Console.WriteLine("5: To show all the students per course");
                    Console.WriteLine("6: To show all the trainers per course");
                    Console.WriteLine("7: To show all the assignments per course");
                    Console.WriteLine("8: To show all the assignments per course per student");
                    Console.WriteLine("9: To show a list of students who belong to more than one courses");
                    Console.WriteLine("10: To show all students course has based on given course number(#)");
                    Console.WriteLine("11: To show all trainers course has based on given course number(#)");
                    Console.WriteLine("12: To show all assignments course has based on given course number(#)");
                    Console.WriteLine("13: To show all students and assignments course has based on given course number(#)");
                    Console.WriteLine("14: To go back to the main menu");
                    int.TryParse(Console.ReadLine(), out int menuChoice);
                    switch (menuChoice)
                    {
                        case 1:
                            student.ListOfStudents(iPModel);
                            break;
                        case 2:
                            trainer.ListOfTrainers(iPModel);
                            break;
                        case 3:
                            assignment.ListOfAssignments(iPModel);
                            break;
                        case 4:
                            course.ListOfCourses(iPModel);
                            break;
                        case 5:
                            student.StudentsPerCourse(iPModel);
                            break;
                        case 6:
                            trainer.TrainersPerCourse(iPModel);
                            break;
                        case 7:
                            assignment.AssignmentsPerCourse(iPModel);
                            break;
                        case 8:
                            assignment.AssignmentsPerCoursePerStudent(iPModel);
                            break;
                        case 9:
                            student.StudentsWithMoreCourses(iPModel);
                            break;
                        case 10:
                            student.ShowStudentsBasedOnCourseNumber(iPModel);
                            break;
                        case 11:
                            trainer.ShowTrainersBasedOnCourseNumber(iPModel);
                            break;
                        case 12:
                            assignment.ShowAssignmentsBasedOnCourseNumber(iPModel);
                            break;
                        case 13:
                            assignment.ShowStudentsAssignmentsBasedOnCourseNumber(iPModel);
                            break;
                        case 14:
                            selectDataFlag = false;
                            break;
                        default:
                            Console.WriteLine("Invalid input, please try again\n");
                            break;
                    }
                } while (selectDataFlag);
                selectDataFlag = true;
            }

            void AddData()
            {
                do
                {
                    Console.WriteLine("\nType");
                    Console.WriteLine("1: To add students");
                    Console.WriteLine("2: To add trainers");
                    Console.WriteLine("3: To add assignments");
                    Console.WriteLine("4: To add courses");
                    Console.WriteLine("5: To select which student to connect with course");
                    Console.WriteLine("6: To select which trainer to connect with course");
                    Console.WriteLine("7: To select which assignment to connect with course");
                    Console.WriteLine("8: To go back to the main menu");
                    int.TryParse(Console.ReadLine(), out int menuChoice);
                    switch (menuChoice)
                    {
                        case 1:
                            student.AddStudents(iPModel, studentList.Count);
                            break;
                        case 2:
                            trainer.AddTrainers(iPModel, trainerList.Count);
                            break;
                        case 3:
                            assignment.AddAssignments(iPModel, assignmentList.Count);
                            break;
                        case 4:
                            course.AddCourses(iPModel, courseList.Count);
                            break;
                        case 5:
                            student.AddStudentsPerCourse(iPModel);
                            break;
                        case 6:
                            trainer.AddTrainersPerCourse(iPModel);
                            break;
                        case 7:
                            assignment.AddAssignmentsPerCourse(iPModel);
                            break;
                        case 8:
                            addDataFlag = false;
                            break;
                        default:
                            Console.WriteLine("Invalid input, please try again\n");
                            break;
                    }
                } while (addDataFlag);
                addDataFlag = true;
            }
        }
    }
}