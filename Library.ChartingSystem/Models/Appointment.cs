using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.ChartingSystem.Models
{
    public class Appointment
    {
        private static int idCounter = 1;
        public int Id { get; private set; }
        public Patient? Patient { get; private set; }
        public Physician? Physician { get; private set; }

        public DateTime? AppointmentDate { get; private set; } = DateTime.Now;
        public DateTime? EndTime => AppointmentDate.HasValue ? AppointmentDate.Value.AddMinutes(30) : (DateTime?)default;

        public Appointment() { }

        public Appointment(Patient patient, Physician physician, DateTime date)
        {
            if (patient == null)
                throw new ArgumentException("Appointment must have a patient.");

            if (physician == null)
                throw new ArgumentException("Appointment must have a physician.");

            SetAppointmentDate(date);

            this.Id = idCounter++;
            this.Patient = patient;
            this.Physician = physician;
        }

        public void SetAppointmentDate(DateTime date)
        {
            if (!IsValidTime(date))
                throw new ArgumentException("Appointments can only be scheduled between 8 AM and 5 PM, Monday to Friday.");
            if (date < DateTime.Now)
                throw new ArgumentException("Appointment date has to be in the future.");


            AppointmentDate = date;
        }

        public static bool IsValidTime(DateTime date)
        {
            if (date.DayOfWeek < DayOfWeek.Monday || date.DayOfWeek > DayOfWeek.Friday)
                return false;

            if (date.Hour < 8 || date.Hour >= 17)
                return false;

            return true;
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
            // Use null-conditional and null-coalescing operators to avoid null dereference warnings
            var physicianName = Physician?.Name ?? "N/A";
            var patientName = Patient?.Name ?? "N/A";
            return $"{Id}\t{physicianName}\t\t{patientName}\t\tFrom: {AppointmentDate:g} To: {EndTime:g}";
        }
    }
}
