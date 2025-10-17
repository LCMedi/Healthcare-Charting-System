using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Library.ChartingSystem.Models
{   
    public enum RACE
    {
        Asian,
        AfricanAmerican,
        White,
        Hispanic,
        Other
    }

    public enum GENDER
    {
        Male,
        Female,
        Other
    }
    public class Patient
    {
        private static int idCounter = 1;

        public int Id { get; private set; }
        public string? Name { get; private set; } = string.Empty;
        public string? Address { get; private set; } = string.Empty;
        public DateTime? Birthdate { get; private set; }
        public RACE? Race { get; private set; }
        public GENDER? Gender { get; private set; }
        public List<MedicalNote> MedicalHistory { get; private set; } = new();

        public Patient()
        {
        }

        public Patient(string name, DateTime date, RACE race, GENDER gender, string? address)
        {
            SetName(name);
            SetBirthdate(date);
            SetRace(race);
            SetGender(gender);
            SetAddress(address ?? string.Empty);

            this.Id = idCounter++;
            this.MedicalHistory = new List<MedicalNote>();
        }

        public void SetName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Patient name cannot be empty");
            Name = name;
        }

        public void SetAddress(string address)
        {
            Address = address;
        }

        public void SetBirthdate(DateTime date)
        {
            if (date > DateTime.Now)
                throw new ArgumentException("Birthdate cannot be in the future.");

            Birthdate = date;
        }

        public void SetRace(RACE race)
        {
            if (!Enum.IsDefined(typeof(RACE), race))
                throw new ArgumentException("Specified race is not valid.");

            Race = race;
        }

        public void SetGender(GENDER gender)
        {
            if (!Enum.IsDefined(typeof(GENDER), gender))
                throw new ArgumentException("Specified gender is not valid.");

            Gender = gender;
        }

        public void AddMedicalNote(DateTime time, string diagnosis, string prescription, Physician physician)
        {
            if (time > DateTime.Now)
                throw new ArgumentException("Time cannot be in the future.");

            if (string.IsNullOrEmpty(diagnosis))
                throw new ArgumentException("Diagnosis cannot be empty");

            if (string.IsNullOrWhiteSpace(prescription))
                throw new ArgumentException("Prescription cannot be empty.");

            if (physician == null)
                throw new ArgumentException("Physician cannot be null.");

            MedicalHistory.Add(new MedicalNote(time, diagnosis, prescription, physician));
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
            sb.AppendLine($"[{Id}]\t{Name ?? "N/A"}\t{Gender?.ToString() ?? "N/A"}\t\t{Birthdate:MM/dd/yyyy}\t{Race?.ToString() ?? "N/A"}\t\t{Address ?? "N/A"}");

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
