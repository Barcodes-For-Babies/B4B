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
        public EmergencyContact()
        {
            this.Profiles = new HashSet<Profile>();
        }
        public int EmergencyContactID { get; set; }
        [MaxLength(50)]
        public string EmergencyName { get; set; }
        [MaxLength(50)]
        public string EmergencyPhone { get; set; }
        public int ProfileID { get; set; }

        //navigation properties
        public virtual ICollection<Profile> Profiles { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}