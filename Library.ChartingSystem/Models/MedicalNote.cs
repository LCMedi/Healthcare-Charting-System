using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.ChartingSystem.Models
{
    public class MedicalNote
    {
        public DateTime DateCreated { get; private set; }
        public string Diagnosis {  get; set; }
        public string Prescription { get; set; }
        public Physician CreatedBy { get; private set; }

        public MedicalNote(DateTime date,  string diagnosis, string prescription, Physician physician)
        {
            if (date > DateTime.Now)
                throw new ArgumentException("Creation date cannot be in the future.");

            this.DateCreated = date;
            this.Diagnosis = diagnosis;
            this.Prescription = prescription;
            this.CreatedBy = physician;
        }

        public string Display
        {
            get
            {
                return ToString();
            }
        }

        public override string ToString()
        {
            return $"{DateCreated:MM/dd/yyyy}: Diagnosis: {Diagnosis}, Prescription: {Prescription}, Physician: {CreatedBy?.Name ?? "Unknown"}";
        }
    }
}
