using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace B4B.Models
{
    public class Profile
    {
        public int ProfileID { get; set; }
        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
        public string FirstName { get; set; }
        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
        public string LastName { get; set; }
        [StringLength(50)]
        public string StreetAdress { get; set; }
        [StringLength(50)]
        public string City { get; set; }
        [StringLength(50)]
        public string State { get; set; }
        public int ZipCode { get; set; }

        [Display(Name = "Full Name")]
        public string FullName {
            get{ return LastName + ", " + FirstName; }
        }

        //navigation properties
        // 1 to 1
        public virtual Photo Photo { get; set; }
        // 1 to Many
        public virtual ICollection<MedicalInfo> MedicalInfos { get; set; }
        public virtual ApplicationUser Admin { get; set; }
        // Many to Many
        public virtual ICollection<EmergencyContact> EmergencyContacts { get; set; }
    }
}