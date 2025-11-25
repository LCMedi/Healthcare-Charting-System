using Library.ChartingSystem.DTO;
using Library.ChartingSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.ChartingSystem.Models
{
    public class MedicalNote
    {
        public int Id { get; private set; }
        public DateTime DateCreated { get; private set; }
        public string Diagnosis { get; set; } = String.Empty;
        public string Prescription { get; set; } = String.Empty;
        public int? PhysicianId { get; private set; }
        public Physician? CreatedBy { get; private set; } = null!;

        public MedicalNote() { }

        //[Obsolete("Use AddMedicalNote instead. Not for EF.", true)]
        public MedicalNote(DateTime date,  string diagnosis, string prescription, Physician? physician)
        {
            if (date > DateTime.Now)
                throw new ArgumentException("Creation date cannot be in the future.");

            this.DateCreated = date;
            this.Diagnosis = diagnosis;
            this.Prescription = prescription;
            if (physician != null)
            {
                CreatedBy = physician;
                PhysicianId = physician.Id;
            }
        }

        public MedicalNote(MedicalNoteDTO dto)
        {
            Id = dto.Id;
            DateCreated = dto.DateCreated;
            Diagnosis = dto.Diagnosis;
            Prescription = dto.Prescription;

            if (dto.PhysicianId != null)
            {
                PhysicianId = dto.PhysicianId;
                CreatedBy = ChartServiceProxy.Current.GetPhysician((int)PhysicianId);
            }
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
