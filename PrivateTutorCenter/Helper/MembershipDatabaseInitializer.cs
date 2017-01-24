using Microsoft.AspNet.Identity.EntityFramework;
using PrivateTutorCenter.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PrivateTutorCenter.Helper
{
    public class MembershipDatabaseInitializer: DropCreateDatabaseAlways<PrivateTutorContext>
    {
        protected override void Seed(PrivateTutorContext context)
        {
            GetRoles().ForEach(c => context.Roles.Add(c));
            context.SaveChanges();
            PrivateTutorHasher hasher = new PrivateTutorHasher();
            var user = new ApplicationUser { UserName = "admin", Email = "admin@admin.com", PasswordHash = hasher.HashPassword("password") };
            var role = context.Roles.Where(r => r.Name == "Admin").First();
            user.Roles.Add(new IdentityUserRole { RoleId = role.Id, UserId = user.Id });
            context.Users.Add(user);

            context.courses.Add(new Course
            {
                title = "CS 101",
                subject = "CS",
                description = "This is a fundamental class of Computer Science",
                courseFee = "$20.00",
                teacher = new Teacher
                {
                    firstName = "Rebecca",
                    lastName = "Weng",
                    address = "1 E Jackson Blvd",
                    city = "Chicago",
                    state = "IL",
                    zipcode = "60616",
                    educationHistory = "MS in Computer Science",
                    Gender = "female",
                    workExperience = "Professional ASP.NET developer for 2 years",
                    age = 25,
                    expertise = "Computer Programming"                   
                }
            });

            context.courses.Add(new Course
            {
                title = "CS 102",
                subject = "CS",
                description = "Introduction to C# and ASP.NET 5.0",
                courseFee = "$25.00",
                teacher = new Teacher
                {
                    firstName = "James",
                    lastName = "Bond",
                    address = "1 E Jackson Blvd",
                    city = "Chicago",
                    state = "IL",
                    zipcode = "60616",
                    educationHistory = "Prof in Computer Science",
                    Gender = "Male",
                    workExperience = "Professional ASP.NET developer for 20 years",
                    age = 45,
                    expertise = "Computer Programming"
                }
            });

            context.courses.Add(new Course
            {
                title = "Math 101",
                subject = "Math",
                description = "Introduction to Algebra",
                courseFee = "$10.00",
                teacher = new Teacher
                {
                    firstName = "Mary",
                    lastName = "Chris",
                    address = "1 E Jackson Blvd",
                    city = "Chicago",
                    state = "IL",
                    zipcode = "60616",
                    educationHistory = "Prof in Math ",
                    Gender = "Female",
                    workExperience = "Professional Math teacher for 20 years",
                    age = 50,
                    expertise = "Discrete Math"
                }
            });

            context.courses.Add(new Course
            {
                title = "Eng 101",
                subject = "Eng",
                description = "Introduction to writing and grammar",
                courseFee = "$15.00",
                teacher = new Teacher
                {
                    firstName = "Rosie",
                    lastName = "Waston",
                    address = "1 E Jackson Blvd",
                    city = "Chicago",
                    state = "IL",
                    zipcode = "60616",
                    educationHistory = "Prof in English",
                    Gender = "Male",
                    workExperience = "Professional English teacher for 20 years",
                    age = 46,
                    expertise = "English education"
                }
            });

            context.SaveChanges();
            base.Seed(context);
        }

            private static List<ApplicationRole> GetRoles()
            {
                var roles = new List<ApplicationRole> {
                new ApplicationRole {Name="Admin", Description="Admin"},
                new ApplicationRole {Name="Teacher", Description="Member"},
                new ApplicationRole {Name="Student", Description="Member" }
            };

                return roles;
            }
        }

    }
