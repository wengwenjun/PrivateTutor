using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PrivateTutorCenter.Models.View_Model
{
    public class Registration
    {
        public int id { get; set; }
        public int studentId { get; set; }
        public int courseId { get; set; }
        
        public Student student { get; set; }
        public Course course { get; set; }

        [Timestamp]
        public byte[] timeStamp { get; set; }

    }
}