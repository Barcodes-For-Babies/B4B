using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace B4B.Models
{
    public class MedicalInfo
    {
        public int MedicalInfoID { get; set; }
        [StringLength(50)]
        public string MedicalCondition { get; set; }
        [StringLength(150)]
        public string Symptoms { get; set; }
        [StringLength(150)]
        [Display(Name = "Related Information")]
        public string RelatedInformation { get; set; }

        public int ProfileID { get; set; }

        //navigatoin properties
        // 1 to manny
        public virtual Profile Profile { get; set; }

    }
}