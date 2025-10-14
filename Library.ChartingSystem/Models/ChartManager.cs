using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.ChartingSystem.Models
{
    public class ChartManager
    {
        public List<Appointment> Appointments { get; private set; }
        public List<Patient> Patients { get; private set; }
        public List<Physician> Physicians { get; private set; }

        public ChartManager()
        {
            Appointments = new List<Appointment>();
            Patients = new List<Patient>();
            Physicians = new List<Physician>();
        }

        // Create Patient
        public void AddPatient(Patient patient)
        {
            if (patient == null)
                throw new ArgumentException("Patient cannot be empty.");
            Patients.Add(patient);
        }

        // Create Physician
        public void AddPhysician(Physician physician)
        {
            if (physician == null)
                throw new ArgumentException("Physician cannot be empty.");
            Physicians.Add(physician);
        }

        // Create Appointment
        public void ScheduleAppointment(Appointment appointment)
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
            if (CheckOverlapping(appointment))
                throw new ArgumentException("There already exists an appointment with the given time.");

            Appointments.Add(appointment);
        }

        // Update Appointment
        public void RescheduleAppointment(Appointment appointment, DateTime newTime)
        {
            if (appointment == null)
                throw new ArgumentException("Appointment cannot be empty.");

            if (!Appointments.Contains(appointment))
                throw new ArgumentException("Appointment not found in the system.");

            var newAppointment = appointment;
            newAppointment.SetAppointmentDate(newTime);

            CancelAppointment(appointment);

            ScheduleAppointment(newAppointment);    // Error checking is done inside ScheduleAppointment()
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
        public void CancelAppointment(Appointment appointment)
        {
            if (appointment == null)
                throw new ArgumentException("Appointment cannot be empty.");

            if (!Appointments.Contains(appointment))
                throw new ArgumentException("Appointment not found in the system.");

            Appointments.Remove(appointment);
        }

        // Delete Physician
        public void RemovePhysician(Physician physician)
        {
            if (physician == null)
                throw new ArgumentException("Physician cannot be empty.");

            if (!Physicians.Contains(physician))
                throw new ArgumentException("Physician not found in the system.");

            // Remove all appointments associated with this physician
            var appointments = Appointments.Where(x => x.Physician == physician).ToList();

            foreach (var appointment in appointments)
                Appointments.Remove(appointment);

            Physicians.Remove(physician);
        }

        // Delete Patient
        public void RemovePatient(Patient patient)
        {
            if (patient == null)
                throw new ArgumentException("Patient cannot be empty.");

            if (!Patients.Contains(patient))
                throw new ArgumentException("Patient not found in the system.");

            // Remove all appointments associated with this patient
            var appointments = Appointments.Where(x => x.Patient == patient).ToList();

            foreach (var appointment in appointments)
                Appointments.Remove(appointment);

            Patients.Remove(patient);
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
        private bool CheckOverlapping(Appointment appointment)
        {
            var physician = appointment.Physician;

            return Appointments.Any(x => x.Physician == physician && x.AppointmentDate < appointment.EndTime && appointment.AppointmentDate < x.EndTime);
            
        }

    }
}
