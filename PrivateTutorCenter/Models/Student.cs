using PrivateTutorCenter.Models.View_Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PrivateTutorCenter.Models
{
    public class Student
    {
        public Student()
        {
            this.registrations = new HashSet<Registration>();
        }

        public int id { get; set; }

        [Required]
        [DisplayName ("*First Name")]
        [StringLength (25)]
        public string firstName { get; set; }

        [Required]
        [DisplayName("*Last Name")]
        [StringLength(25)]
        public string lastName { get; set; }

        [Required]
        [DisplayName("*Gender")]
        [StringLength(10)]
        public String Gender { get; set; }

        [Required]
        [DisplayName("*Address")]
        [StringLength(25)]
        public string address { get; set; }

        [Required]
        [DisplayName("*City")]
        [StringLength(25)]
        public string city { get; set; }

        [Required]
        [DisplayName("*State")]
        [StringLength(10)]
        public string state { get; set; }

        [Required]
        [DisplayName("*Zipcode")]
        [StringLength(5)]
        public string zipcode { get; set; }

        [Required]
        [DisplayName("*Age")]
        [Range(18,100)]
        public Nullable<int> age { get; set; }

        [DisplayName("Highest Education Level")]
        [StringLength(25)]
        public string highestEducationLevel { get; set; }

        [Timestamp]
        public byte[] timeStamp { get; set; }

        public ICollection<Registration> registrations { get; set; }
    }
}