using System.Collections.Generic;

namespace B4B.Models
{
    // This class acts as a Model of Models
    // This makes it possible to be able to work with multiple models in a view
    // Our wizard form is an example of this since we access multiple models throughout the process
    // Those models are set at properties
    // Further implementation of this can be found in the ProfilesController

    public class WizardViewModel
    {
        public Profile _profile { get; set; }                       // Sets the Profile model as a property
        public MedicalInfo _medicalInfo { get; set; }               // Sets the MedicalInfo model as a property
        //public List<MedicalInfo> _medicalInfo { get; set; }
    }
}