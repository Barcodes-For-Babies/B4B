using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace B4B.Models
{
    public class Photo
    {
        public int PhotoID { get; set; }
        [MaxLength(50)]
        public string PhotoName { get; set; }
        public byte [] PhotoBytes { get; set; }
        public string Extension { get; set; }

        //navigation properties
        public virtual ICollection<Profile> Profiles { get; set; }
    }
}