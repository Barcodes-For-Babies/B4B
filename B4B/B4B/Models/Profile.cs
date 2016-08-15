using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace B4B.Models
{
    public class Profile
    {
        public int ProfileID { get; set; }
        [MaxLength(50)]
        public string FirstName { get; set; }
        [MaxLength(50)]
        public string LastName { get; set; }
        public int PhotoID { get; set; }
        [MaxLength(50)]
        public string StreetAdress { get; set; }
        [MaxLength(50)]
        public string City { get; set; }
        [MaxLength(50)]
        public string State { get; set; }
        public int ZipCode { get; set; }
        public int MedicalInfoID { get; set; }
        public int EmergencyContactID { get; set; }

        //navigation properties
        public virtual Photo Photos { get; set; }
        public virtual MedicalInfo MedicalInfos { get; set; }
        public virtual ApplicationUser Admin { get; set; }
        public virtual ICollection<EmergencyContact> EmergencyContacts { get; set; }
    }
}