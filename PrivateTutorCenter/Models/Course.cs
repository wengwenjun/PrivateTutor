using PrivateTutorCenter.Models.View_Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PrivateTutorCenter.Models
{
    public class Course
    {
        public Course()
        {
            this.registrations = new HashSet<Registration>();
        }

        public int id { get; set; }
        public string title { get; set;}
        public string subject { get; set; }
        public string description { get; set; }
        public string courseFee { get; set; }
        public string teacherId { get; set; }

        public Teacher teacher { get; set; }

        public ICollection<Registration> registrations { get; set; }

        [Timestamp]
        public byte[] timeStamp { get; set; }

    }
}