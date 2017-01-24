using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PrivateTutorCenter.Models.View_Model
{
    public class EditableCourse
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string subject { get; set; }
        public String teacherId { get; set; }
        public Boolean isEditable { get; set; }
        public Boolean canEnroll { get; set; }
        public string courseFee { get; set; }

        public byte[] Verson { get; set; }

        public SelectList subjects { get; set; }
    }
}