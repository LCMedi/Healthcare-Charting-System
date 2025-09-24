using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC_ConsoleApp.Models
{
    public class Physician
    {
        public static int idCounter = 1;
        public int Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string LicenseNumber { get; private set; } = string.Empty;
        public DateTime graduationDate { get; private set; }
        public List<string> Specializations { get; private set; }

        // Constructor
        public Physician(string name, string licenseNumber, DateTime date, List<string> specializations)
        {
            SetName(name);
            SetLicenseNumber(licenseNumber);
            SetGraduationDate(date);
            Specializations = specializations ?? new List<string>();
            Id = idCounter++;
        }

        // Name Setter
        public void SetName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Physician name cannot be empty.");

            this.Name = name;
        }

        // License Number Setter
        public void SetLicenseNumber(string licenseNumber)
        {
            if (string.IsNullOrWhiteSpace(licenseNumber))
                throw new ArgumentException("License number cannot be empty.");

            this.LicenseNumber = licenseNumber;
        }

        // Graduation Date Setter
        public void SetGraduationDate(DateTime date)
        {
            if (date > DateTime.Now)
                throw new ArgumentException("Graduation date cannot be in the future.");

            this.graduationDate = date;
        }

        // Create Specialization
        public void AddSpecialization(string specialization)
        {
            if (!string.IsNullOrEmpty(specialization) && !Specializations.Contains(specialization))
            {
                Specializations.Add(specialization);
            }
        }

        // Delete Specialization
        public void RemoveSpecialization(string specialization)
        {
            if (string.IsNullOrEmpty(specialization))
                throw new ArgumentException("Specialization cannot be empty.");

            if (!Specializations.Contains(specialization))
                throw new ArgumentException("Specialization not found.");

            Specializations.Remove(specialization);
        }

        public override string ToString()
        {
            string specs = Specializations.Count > 0 ? string.Join(", ", Specializations) : "None";
            return $"[{Id}]\t{Name}\t{LicenseNumber}\t\t\t{specs}";
        }
    }
}
