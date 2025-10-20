using Library.ChartingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.ChartingSystem.Services
{
    public class ChartServiceProxy
    {
        private List<Appointment> Appointments;
        private List<Patient> Patients;
        private List<Physician> Physicians;

        private static ChartServiceProxy? instance;
        private static object instanceLock = new object();
        public static ChartServiceProxy Current
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new ChartServiceProxy();
                    }
                }
                return instance;
            }
        }

        private ChartServiceProxy()
        {
            Appointments = new List<Appointment>();
            Patients = new List<Patient>();
            Physicians = new List<Physician>();
            // Add Patients, and Physicians for testing purposes
            AddPatient(new Patient("John Smith", new DateTime(1980, 10, 20), RACE.White, GENDER.Male, "123 Main St"));
            AddPatient(new Patient("Mateo Rivas", new DateTime(1987, 7, 22), RACE.Hispanic, GENDER.Male, "1765 Cypress Ln"));
            AddPatient(new Patient("Jasmine Holloway", new DateTime(1992, 3, 14), RACE.Black, GENDER.Female, "4823 Maplewood Dr"));
            AddPhysician(new Physician("Dr Allison", "HGY-8463", new DateTime(1990, 12, 10), "General"));
            AddPhysician(new Physician("Dr Elena Marquez", "TXB-61592", new DateTime(2015, 5, 20), "Family Medicine, Geriatrics"));
            AddPhysician(new Physician("Dr Marcus Chen", "FLR-92741", new DateTime(2016, 5, 18), "Cardiology, Preventive Medicine"));
        }

        // Create Patient
        public Patient AddPatient(Patient patient)
        {
            if (patient == null)
                throw new ArgumentException("Patient cannot be empty.");

            Patients.Add(patient);
            return patient;
        }

        // Create Physician
        public Physician AddPhysician(Physician physician)
        {
            if (physician == null)
                throw new ArgumentException("Physician cannot be empty.");

            Physicians.Add(physician);
            return physician;
        }

        // Create Appointment
        public Appointment ScheduleAppointment(Appointment appointment)
        {
            if (appointment == null)
                throw new ArgumentException("Appointment cannot be empty.");

            if (!Patients.Contains(appointment.Patient))
                throw new ArgumentException("Patient not found in the system.");

            if (!Physicians.Contains(appointment.Physician))
                throw new ArgumentException("Physician not found in the system.");

            if (Appointments.Contains(appointment))
                throw new ArgumentException("Appointment already in the system.");

            // Check for overlapping appointments for the same physician
            if (!IsTimeAvailable(appointment.Physician!, appointment.AppointmentDate!.Value, appointment.EndTime!.Value))
                throw new ArgumentException("There already exists an appointment with the given time.");

            Appointments.Add(appointment);
            return appointment;
        }

        // Update Appointment
        public Appointment UpdateAppointment(Appointment appointment, Patient patient, Physician physician, DateTime newStart)
        {
            if (appointment == null)
                throw new ArgumentException("Appointment cannot be empty.");

            var existing = Appointments.FirstOrDefault(x => x.Id == appointment.Id);

            if (existing == null)
                throw new ArgumentException("Appointment not found in the system.");

            if (patient == null)
                throw new ArgumentNullException(nameof(patient));

            if (!Patients.Contains(patient))
                throw new ArgumentException("Patient not found in the system.");

            if (physician == null)
                throw new ArgumentNullException(nameof(physician));

            if (!Physicians.Contains(physician))
                throw new ArgumentException("Physician not found in the system.");

            if (!Appointment.IsValidTime(newStart))
                throw new ArgumentException("Appointments can only be scheduled between 8 AM and 5 PM, Monday to Friday.");

            if (newStart < DateTime.Now)
                throw new ArgumentException("Appointment date has to be in the future.");

            var end = newStart.AddMinutes(30);

            if (!IsTimeAvailable(physician, newStart, end, existing))
                throw new ArgumentException("There already exists an appointment with the given time.");

            existing.SetPatient(patient);
            existing.SetPhysician(physician);
            existing.SetAppointmentDate(newStart);

            return existing;
        }

        // Reschedule Appointment
        public Appointment RescheduleAppointment(Appointment appointment, DateTime newTime)
        {
            if (appointment == null)
                throw new ArgumentException("Appointment cannot be empty.");

            if (!Appointments.Contains(appointment))
                throw new ArgumentException("Appointment not found in the system.");

            appointment.SetAppointmentDate(newTime);

            CancelAppointment(appointment);

            return ScheduleAppointment(appointment);    // Error checking is done inside ScheduleAppointment()
        }

        // Update Physician (Add Specializations only)
        public void AddPhysicianSpecialization(Physician physician, string specialization)
        {
            if (physician == null)
                throw new ArgumentNullException("Physician cannot be empty.");

            if (!Physicians.Contains(physician))
                throw new ArgumentException("Physician not found in the system.");

            physician.AddSpecialization(specialization);
        }

        // Update Physician (Remove Specializations only)
        public void RemovePhysicianSpecialization(Physician physician, string specialization)
        {
            if (physician == null)
                throw new ArgumentNullException("Physician cannot be empty.");

            if (!Physicians.Contains(physician))
                throw new ArgumentException("Physician not found in the system.");

            physician.RemoveSpecialization(specialization);
        }

        // Update Patient (Add Medical Note only)
        public void AddPatientMedicalNote(Patient patient, DateTime time, string diagnosis, string prescription, Physician physician)
        {
            if (patient == null)
                throw new ArgumentNullException("Patient cannot be empty.");

            if (!Patients.Contains(patient))
                throw new ArgumentException("Patient not found in the system.");

            if (physician == null)
                throw new ArgumentNullException("Physician cannot be empty.");

            if (!Physicians.Contains(physician))
                throw new ArgumentException("Physician not found in the system.");

            patient.AddMedicalNote(time, diagnosis, prescription, physician);
        }

        // Delete Appointment
        public bool CancelAppointment(Appointment appointment)
        {
            if (appointment == null)
                throw new ArgumentException("Appointment cannot be empty.");

            if (!Appointments.Contains(appointment))
                throw new ArgumentException("Appointment not found in the system.");

            return Appointments.Remove(appointment);
        }

        // Delete Physician
        public bool RemovePhysician(Physician physician)
        {
            if (physician == null)
                throw new ArgumentException("Physician cannot be empty.");

            if (!Physicians.Contains(physician))
                throw new ArgumentException("Physician not found in the system.");

            // Remove all appointments associated with this physician
            var appointments = Appointments.Where(x => x.Physician == physician).ToList();

            foreach (var appointment in appointments)
                Appointments.Remove(appointment);

            return Physicians.Remove(physician);
        }

        // Delete Patient
        public bool RemovePatient(Patient patient)
        {
            if (patient == null)
                throw new ArgumentException("Patient cannot be empty.");

            if (!Patients.Contains(patient))
                throw new ArgumentException("Patient not found in the system.");

            // Remove all appointments associated with this patient
            var appointments = Appointments.Where(x => x.Patient == patient).ToList();

            foreach (var appointment in appointments)
                Appointments.Remove(appointment);

            return Patients.Remove(patient);
        }

        // Get Appointment by ID
        public Appointment? GetAppointment(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID cannot be negative or zero.");

            return Appointments.FirstOrDefault(x => x.Id == id);
        }

        // Get All Appointments by Patient
        public List<Appointment> GetAllAppointments(Patient patient)
        {
            if (patient == null)
                throw new ArgumentException("Patient cannot be empty.");

            if (!Patients.Contains(patient))
                return new List<Appointment>();

            return Appointments.Where(x => x.Patient == patient).ToList();
        }

        // Get All Appointments by Physician
        public List<Appointment> GetAllAppointments(Physician physician)
        {
            if (physician == null)
                throw new ArgumentException("Physician cannot be empty.");

            if (!Physicians.Contains(physician))
                return new List<Appointment>();

            return Appointments.Where(x => x.Physician == physician).ToList();
        }

        // Get All Appointments (sorted by date)
        public List<Appointment> GetAllAppointments()
        {
            return Appointments.OrderBy(x => x.AppointmentDate).ToList();
        }

        // Get Physician (by ID)
        public Physician? GetPhysician(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID cannot be negative or zero.");

            return Physicians.FirstOrDefault(x => x.Id == id);
        }

        // Get Physician (by Name)
        public Physician? GetPhysician(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Name cannot be empty.");

            return Physicians.FirstOrDefault(x => string.Equals(x.Name, name, StringComparison.OrdinalIgnoreCase));
        }

        // Get All Physicians
        public List<Physician> GetAllPhysicians()
        {
            return Physicians;
        }

        // Get Patient (by ID)
        public Patient? GetPatient(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID cannot be negative or zero.");

            return Patients.FirstOrDefault(x => x.Id == id);
        }

        // Get Patient (by Name)
        public Patient? GetPatient(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Name cannot be empty.");

            return Patients.FirstOrDefault(x => string.Equals(x.Name, name, StringComparison.OrdinalIgnoreCase));
        }

        // Get All Patients
        public List<Patient> GetAllPatients()
        {
            return Patients;
        }

        // HELPER FUNCTIONS

        // Returns true when the provided appointment overlaps an existing appointment.
        private bool CheckOverlapping(Appointment appointment)
        {
            if (appointment == null)
                return false;

            var physician = appointment.Physician;

            if (physician == null || !appointment.AppointmentDate.HasValue || !appointment.EndTime.HasValue)
                return false;

            var s = appointment.AppointmentDate.Value;
            var e = appointment.EndTime.Value;

            return Appointments.Any(x =>
                x.Physician == physician
                && x != appointment
                && x.AppointmentDate.HasValue
                && x.EndTime.HasValue
                && x.AppointmentDate.Value < e
                && s < x.EndTime.Value);
        }

        public bool IsTimeAvailable(Physician physician, DateTime start, DateTime end, Appointment? exclude = null)
        {
            if (physician == null)
                throw new ArgumentNullException(nameof(physician));

            if (end <= start)
                throw new ArgumentException("End must be after start.", nameof(end));

            // Overlap condition: existing.Start < new.End && new.Start < existing.End
            var overlapExists = Appointments.Any(x =>
                x.Physician == physician
                && x != exclude
                && x.AppointmentDate.HasValue
                && x.EndTime.HasValue
                && x.AppointmentDate.Value < end
                && start < x.EndTime.Value);

            return !overlapExists;
        }
        
        // Int Overload for duration. Returns true when available.
        public bool IsTimeAvailable(Physician physician, DateTime start, int durationMinutes = 30, Appointment? exclude = null)
        {
            if (durationMinutes <= 0)
                throw new ArgumentException("Duration must be positive.", nameof(durationMinutes));

            var end = start.AddMinutes(durationMinutes);
            return IsTimeAvailable(physician, start, end, exclude);
        }

        // Convenience overload that accepts patient
        public bool IsTimeAvailable(Patient patient, Physician physician, DateTime start, int durationMinutes = 30, Appointment? exclude = null)
        {
            if (patient == null)
                throw new ArgumentNullException(nameof(patient));

            return IsTimeAvailable(physician, start, durationMinutes, exclude);
        }

    }
}
