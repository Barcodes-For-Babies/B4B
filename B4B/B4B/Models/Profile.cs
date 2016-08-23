using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace B4B.Models
{
    public enum Colors
    {
        Blue, Red, Yellow, Pink
    }
    public class Profile
    {
        public int ProfileID { get; set; }
        [Display(Name = "First Name")]
        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
        public string LastName { get; set; }
        [Display(Name = "Street Address")]
        [StringLength(50)]
        public string StreetAdress { get; set; }
        [StringLength(50)]
        public string City { get; set; }
        [StringLength(50)]
        public string State { get; set; }
        public int? ZipCode { get; set; }

        public Colors forgroundColor { get; set; }
        public Colors backgroundColor { get; set; }

        [Display(Name = "Name")]
        public string FullName {
            get{ return LastName + ", " + FirstName; }
        }

        [Display(Name = "Photo Name")]
        public string PhotoName { get; set; }
        public string PhotoType { get; set; }
        public byte[] PhotoBytes { get; set; }
        public FileType FileType { get; set; }

        [StringLength(100)]
        public string EcontactName { get; set; }
              
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Emergency Phone Number")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Entered phone format is not valid.")]
        public string EmergencyPhone { get; set; }
        
        //navigation properties
        // 1 to Many
        public virtual ICollection<MedicalInfo> MedicalInfos { get; set; }
        public virtual ApplicationUser Admin { get; set; }
    }
}