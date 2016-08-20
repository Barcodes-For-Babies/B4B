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
        public int? ZipCode { get; set; }

        [Display(Name = "Name")]
        public string FullName {
            get{ return LastName + ", " + FirstName; }
        }

        public string PhotoName { get; set; }
        public string PhotoType { get; set; }
        public byte[] PhotoBytes { get; set; }
        public FileType FileType { get; set; }

        [StringLength(50)]
        public string EcontactFirstName { get; set; }
        [StringLength(50)]
        public string EcontactLasttName { get; set; }
        [StringLength(50)]
        public string EmergencyPhone { get; set; }
        [Display(Name = "Name")]
        public string EcontactName
        {
            get { return EcontactLasttName + ", " + EcontactFirstName; }
        }

        //navigation properties
        // 1 to Many
        public virtual ICollection<MedicalInfo> MedicalInfos { get; set; }
        public virtual ApplicationUser Admin { get; set; }
    }
}