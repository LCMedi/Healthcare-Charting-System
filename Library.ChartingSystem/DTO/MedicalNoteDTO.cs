using Library.ChartingSystem.Models;
using Library.ChartingSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.ChartingSystem.DTO
{
    public class MedicalNoteDTO
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string Diagnosis { get; set; } = String.Empty;
        public string Prescription { get; set; } = String.Empty;
        public int? PhysicianId { get; set; }
        public string? PhysicianName { get; set; }

        public MedicalNoteDTO() { }

        public MedicalNoteDTO(MedicalNote note)
        {
            Id = note.Id;
            DateCreated = note.DateCreated;
            Diagnosis = note.Diagnosis;
            Prescription = note.Prescription;
            PhysicianId = note.PhysicianId;
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
            return $"{DateCreated:MM/dd/yyyy}: Diagnosis: {Diagnosis}, Prescription: {Prescription}, Physician: {PhysicianName ?? "Unknown"}";
        }
    }
}
