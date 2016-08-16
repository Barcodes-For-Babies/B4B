using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace B4B.Models
{
    public class EmergencyContact
    {
        public int EmergencyContactID { get; set; }
        [MaxLength(50)]
        public string EmergencyName { get; set; }
        [MaxLength(50)]
        public string EmergencyPhone { get; set; }


        //navigation properties
        // Many to Many
        public virtual ICollection<Profile> Profiles { get; set; }
        // 1 to Many
        public virtual ApplicationUser User { get; set; }
    }
}