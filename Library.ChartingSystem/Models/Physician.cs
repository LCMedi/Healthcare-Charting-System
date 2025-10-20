using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.ChartingSystem.Models
{
    public class Physician
    {
        private static int idCounter = 1;
        public int Id { get; private set; }
        public string? Name { get; private set; } = string.Empty;
        public string? LicenseNumber { get; private set; } = string.Empty;
        public DateTime? graduationDate { get; private set; }
        public string? Specializations { get; set; } = string.Empty;

        public Physician()
        {
        }

        // Constructor
        public Physician(string name, string licenseNumber, DateTime date, string specializations)
        {
            SetName(name);
            SetLicenseNumber(licenseNumber);
            SetGraduationDate(date);
            Specializations = specializations;
            this.Id = idCounter++;
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
            if (!string.IsNullOrEmpty(specialization))
            { 
                if (string.IsNullOrEmpty(Specializations))
                    Specializations = specialization;
                else
                    Specializations += ", " + specialization;
            }
        }

        // Delete Specialization
        public void RemoveSpecialization(string specialization)
        {
            if (string.IsNullOrEmpty(specialization))
                throw new ArgumentException("Specialization cannot be empty.");

            if (string.IsNullOrEmpty(Specializations) || !Specializations.Contains(specialization))
                throw new ArgumentException("Specialization not found.");

            // Remove the specialization from the comma-separated list
            var specs = Specializations.Split(new[] { ", " }, StringSplitOptions.None)
                .Where(s => !string.Equals(s, specialization, StringComparison.OrdinalIgnoreCase))
                .ToArray();
            Specializations = string.Join(", ", specs);
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
            string specs = (Specializations != null && Specializations.Length > 0) ? string.Join(", ", Specializations) : "None";
            return $"[{Id}]\t{Name ?? "N/A"}\t\t{LicenseNumber ?? "N/A"}\t\t\t{specs}";
        }
    }
}
