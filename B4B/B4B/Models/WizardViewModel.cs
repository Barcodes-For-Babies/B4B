using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace B4B.Models
{
    public class WizardViewModel
    {
        public Profile _profile { get; set; }
        public MedicalInfo _medicalInfo { get; set; }
        public EmergencyContact _emergencyContact { get; set; }
    }
}