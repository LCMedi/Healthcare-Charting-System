using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.ChartingSystem.DTO
{
    public class PatientUpdateDTO
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public DateTime? Birthdate { get; set; }
        public string? Race { get; set; }
        public string? Gender { get; set; }

        public List<MedicalNoteDTO>? MedicalHistory { get; set; }
    }

}
