using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PrivateTutorCenter.Models
{
    public class Teacher
    {
        public Teacher() {
            this.courses = new HashSet<Course>();
        }

        public int id { get; set; }

        [Required]
        [DisplayName("*First Name")]
        [StringLength(25)]
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
        [Range(18, 100)]
        public Nullable<int> age { get; set; }

        [Required]
        [DisplayName("*Work Experience")]
        [StringLength(100)]
        public string workExperience { get; set; }

        [Required]
        [DisplayName("*Education History")]
        [StringLength(100)]
        public string educationHistory { get; set; }

        [Required]
        [DisplayName("*Expertise")]
        [StringLength(100)]
        public string expertise { get; set; }

        [Timestamp]
        public byte[] timeStamp { get; set; }

        public ICollection<Course> courses{get; set;}

    }
}