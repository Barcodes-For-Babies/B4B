using System.Collections.Generic;

namespace B4B.Models
{
    public class MedicalInfo
    {
        public int MedicalInfoID { get; set; }
        public string MedicalInformation { get; set; }

        //navigatoin properties
        public virtual Profile Profiles { get; set; }

    }
}