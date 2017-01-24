using Microsoft.AspNet.Identity.EntityFramework;
using PrivateTutorCenter.Helper;
using PrivateTutorCenter.Models.View_Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PrivateTutorCenter.Models
{
    public class PrivateTutorContext:IdentityDbContext<ApplicationUser>
    {

        public PrivateTutorContext():base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer<PrivateTutorContext>(new MembershipDatabaseInitializer());
        }

        public virtual DbSet <Registration> registrations { get; set; }
        public virtual DbSet <Student> students { get; set; }
        public virtual DbSet <Teacher> teachers { get; set; }
        public virtual DbSet <Course> courses { get; set; }

        public static PrivateTutorContext create()
        {
            return new PrivateTutorContext();
        }        

    }
}