using Library.ChartingSystem.Models;
using Library.ChartingSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.ChartingSystem.DTO
{
    public class PatientDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public DateTime Birthdate { get; set; }
        public string? Race { get; set; }
        public string? Gender { get; set; }

        public List<MedicalNoteDTO> MedicalHistory { get; set; } = new();

        public PatientDTO() { }

        public PatientDTO(Patient patient)
        {
            Id = patient.Id;
            Name = patient.Name;
            Address = patient.Address;
            Birthdate = patient.Birthdate;
            Race = patient.Race?.ToString();
            Gender = patient.Gender?.ToString();
            MedicalHistory = patient.MedicalHistory
                            .Select(m => new MedicalNoteDTO(m))
                            .ToList();
        }

        public PatientDTO(int id)
        {
            var patientCopyTask = PatientServiceProxy.Current.GetById(id);
            var patientCopy = patientCopyTask is Task<Patient?> task ? task.GetAwaiter().GetResult() : null;

            if (patientCopy != null)
            {
                Id = patientCopy.Id;
                Name = patientCopy.Name;
                Address = patientCopy.Address;
                Birthdate = patientCopy.Birthdate;
                Race = patientCopy.Race?.ToString();
                Gender = patientCopy.Gender?.ToString();
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
            var sb = new StringBuilder();
            sb.AppendLine($"[{Id}]\t{Name ?? "N/A"}\t\t{Gender?.ToString() ?? "N/A"}\t\t{Birthdate:MM/dd/yyyy}\t{Race?.ToString() ?? "N/A"}\t\t\t{Address ?? "N/A"}");

            if (MedicalHistory != null && MedicalHistory.Count > 0)
            {
                sb.AppendLine("Medical History:");
                foreach (var note in MedicalHistory)
                {
                    sb.Append('\t');
                    sb.AppendLine(note?.ToString() ?? "N/A");
                }
            }
            else
            {
                sb.AppendLine("Medical History: None");
            }

            return sb.ToString().TrimEnd();
        }
    }
}
