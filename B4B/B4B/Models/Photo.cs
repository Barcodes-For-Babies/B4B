using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace B4B.Models
{
    public class Photo
    {
        [ForeignKey("Profile")]
        public int PhotoID { get; set; }
        [MaxLength(50)]
        public string PhotoName { get; set; }
        public byte [] PhotoBytes { get; set; }
        public string Extension { get; set; }

        //navigation properties
        // 1 to 1
        public virtual Profile Profile { get; set; }
    }
}