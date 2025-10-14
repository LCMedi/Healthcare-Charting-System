using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.ChartingSystem.Models
{
    public class ChartManager
    {
        public List<Appointment> Appointments { get; private set; } = new List<Appointment>();
        public List<Patient> Patients { get; private set; } = new List<Patient>();
        public List<Physician> Physicians { get; private set; } = new List<Physician>();

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
        public Appointment GetAppointment(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID cannot be negative or zero.");

            var appointment = Appointments.FirstOrDefault(x => x.Id == id);

            if (appointment == null)
                throw new ArgumentException($"No appointment found with ID {id}.");

            return appointment;
        }

        // Get All Appointments by Patient
        public List<Appointment> GetAllAppointments(Patient patient)
        {
            if (patient == null)
                throw new ArgumentException("Patient cannot be empty.");

            if (!Patients.Contains(patient))
                throw new ArgumentException("Patient not found in the system.");

            return Appointments.Where(x => x.Patient == patient).ToList();
        }

        // Get All Appointments by Physician
        public List<Appointment> GetAllAppointments(Physician physician)
        {
            if (physician == null)
                throw new ArgumentException("Physician cannot be empty.");

            if (!Physicians.Contains(physician))
                throw new ArgumentException("Physician not found in the system.");

            return Appointments.Where(x => x.Physician == physician).ToList();
        }

        // Get All Appointments (sorted by date)
        public List<Appointment> GetAllAppointments()
        {
            if (Appointments.Count == 0)
                return new List<Appointment>();

            return Appointments.OrderBy(x => x.AppointmentDate).ToList();
        }

        // Get Physician (by ID)
        public Physician GetPhysician(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID cannot be negative or zero.");

            var physician = Physicians.FirstOrDefault(x => x.Id == id);

            if (physician == null)
                throw new ArgumentException($"No physician found with ID {id}.");

            return physician;
        }

        // Get Physician (by Name)
        public Physician GetPhysician(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Name cannot be empty.");

            var physician = Physicians.FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (physician == null)
                throw new ArgumentException($"No physician found with name {name}.");

            return physician;
        }

        // Get All Physicians
        public List<Physician> GetAllPhysicians()
        {
            if (Physicians.Count == 0)
                return new List<Physician>();

            return Physicians;
        }

        // Get Patient (by ID)
        public Patient GetPatient(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID cannot be negative or zero.");

            var patient = Patients.FirstOrDefault(x => x.Id == id);

            if (patient == null)
                throw new ArgumentException($"No patient found with ID {id}.");

            return patient;
        }

        // Get Patient (by Name)
        public Patient GetPatient(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Name cannot be empty.");

            var patient = Patients.FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (patient == null)
                throw new ArgumentException($"No patient found with name {name}.");

            return patient;
        }

        // Get All Patients
        public List<Patient> GetAllPatients()
        {
            if (Patients.Count == 0)
                return new List<Patient>();

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
