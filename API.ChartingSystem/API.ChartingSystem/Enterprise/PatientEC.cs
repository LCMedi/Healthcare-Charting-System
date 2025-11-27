using API.ChartingSystem.Database;
using Library.ChartingSystem.DTO;
using Library.ChartingSystem.Models;
using Library.ChartingSystem.Utilities;
using Microsoft.EntityFrameworkCore;

namespace API.ChartingSystem.Enterprise
{
    public class PatientEC
    {
        private readonly HealthCareDbContext _db;

        public PatientEC(HealthCareDbContext db)
        {
            _db = db;
        }

        // GET: All
        public List<PatientDTO> Get()
        {
            var patients = _db.Patients
                .Include(p => p.MedicalHistory)
                .ThenInclude(m => m.CreatedBy)
                .ToList();

            return patients.Select(x => new PatientDTO(x)).ToList();
        }

        // GET: By Id
        public PatientDTO? GetById(int id)
        {
            var patient = _db.Patients
                .Include(x => x.MedicalHistory)
                .ThenInclude(m => m.CreatedBy)
                .FirstOrDefault(x => x.Id == id);

            if (patient == null) return null;

            return new PatientDTO(patient);
        }

        // POST: Create
        public PatientDTO Add(PatientDTO patientDTO)
        {
            if (patientDTO == null) return null;

            var patient = new Patient(patientDTO);

            _db.Patients.Add(patient);
            _db.SaveChanges();

            return new PatientDTO(patient);
        }

        // PUT: Full Update
        public PatientDTO Update(PatientDTO patientDTO)
        {
            if (patientDTO == null) return null;

            var patient = _db.Patients
                .Include(x => x.MedicalHistory)
                .FirstOrDefault(x => x.Id == patientDTO.Id);

            if (patient == null) return null;

            try
            {
                patient.SetName(patientDTO.Name);
                patient.SetAddress(patientDTO.Address);
                patient.SetBirthdate(patientDTO.Birthdate);
                patient.SetRace(RaceConverter.ConvertRace(patientDTO.Race));
                patient.SetGender(GenderConverter.ConvertGender(patientDTO.Gender));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }

            patient.MedicalHistory.Clear();

            foreach (var noteDTO in patientDTO.MedicalHistory)
            {
                patient.MedicalHistory.Add(new MedicalNote(noteDTO));
            }

            _db.SaveChanges();

            return new PatientDTO(patient);
        }

        // PATCH: Partial Update
        public PatientDTO? PartialUpdate(int id, PatientUpdateDTO patientDTO)
        {
            if (patientDTO == null) return null;

            var patient = _db.Patients
                .Include(x => x.MedicalHistory)
                .FirstOrDefault(x => x.Id == id);

            if (patient == null) return null;

            if (patientDTO.Name != null) patient.SetName(patientDTO.Name);
            if (patientDTO.Address != null) patient.SetAddress(patientDTO.Address);
            if (patientDTO.Birthdate.HasValue) patient.SetBirthdate(patientDTO.Birthdate.Value);
            if (patientDTO.Race != null) patient.SetRace(RaceConverter.ConvertRace(patientDTO.Race));
            if (patientDTO.Gender != null) patient.SetGender(GenderConverter.ConvertGender(patientDTO.Gender));

            if (patientDTO.MedicalHistory != null)
            {
                if (patient.MedicalHistory != null)
                {
                    patient.MedicalHistory.Clear();
                    foreach (var noteDto in patientDTO.MedicalHistory)
                    {
                        patient.MedicalHistory.Add(new MedicalNote(noteDto));
                    }
                }
            }

            _db.SaveChanges();

            return new PatientDTO(patient);
        }

        // DELETE: By Id
        public bool Delete(int id)
        {
            var patient = _db.Patients.Find(id);

            if (patient == null) return false;

            _db.Patients.Remove(patient);
            _db.SaveChanges();

            return true;
        }
    }
}
