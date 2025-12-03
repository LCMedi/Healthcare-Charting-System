using Library.ChartingSystem.Data;
using Library.ChartingSystem.DTO;
using Library.ChartingSystem.Models;
using Library.eCommerce.Utilities;
using Newtonsoft.Json;

namespace Library.ChartingSystem.Services
{
    public class PatientServiceProxy
    {
        private List<PatientDTO> _patients;
        public List<Patient> Patients { get; } = new();
        private const string baseUrl = "/patient";

        private static PatientServiceProxy? instance;
        private static object instanceLock = new object();

        public static PatientServiceProxy Current
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new PatientServiceProxy();
                    }
                }
                return instance;
            }
        }

        private PatientServiceProxy()
        {
            _patients = new List<PatientDTO>();

            var response = new WebRequestHandler().Get(baseUrl).Result;

            if (!string.IsNullOrWhiteSpace(response))
            {
                _patients = JsonConvert.DeserializeObject<List<PatientDTO>>(response) ?? new List<PatientDTO>();
            }
        }

        // Get Patient by ID
        public async Task<Patient> GetById(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID cannot be negative or zero.");

            var dto = await GetByIdAsync(id);

            return new Patient(dto);
        }

        // Get All Patients
        public async Task<List<Patient>> GetAll()
        {
            var dtos = await GetAllAsync();

            Patients.Clear();

            foreach (var dto in dtos)
            {
                Patients.Add(new Patient(dto));
            }

            return Patients;
        }

        // Delete Patient
        public async Task<Patient> Delete(Patient patient)
        {
            if (patient == null)
                throw new ArgumentException("Patient cannot be empty.");

            var existing = Patients.FirstOrDefault(p => p?.Id == patient.Id);
            if (existing == null)
                throw new ArgumentException("Patient not found in the system.");

            // Remove all appointments associated with this patient
            /*var appointments = Appointments.Where(x => x.Patient == patient).ToList();

            foreach (var appointment in appointments)
                Appointments.Remove(appointment);*/

            var deletedDto = await DeleteAsync(patient.Id);

            Patients.Remove(existing);

            // Remove from DTO cache too
            var cachedDto = _patients.FirstOrDefault(p => p?.Id == patient.Id);
            if (cachedDto != null)
            {
                _patients.Remove(cachedDto);
            }

            return new Patient(deletedDto ?? new PatientDTO(patient));
        }

        // Create Patient
        public async Task<Patient> Add(Patient patient)
        {
            if (patient == null)
                throw new ArgumentException("Patient cannot be empty.");

            var newDto = await AddAsync(new PatientDTO(patient));

            if (newDto == null)
                throw new Exception("Failure creating patient on server. Please try again later.");

            // Add to DTO cache
            _patients.Add(newDto);

            // Add to local cache
            var newPatient = new Patient(newDto);
            Patients.Add(newPatient);

            return newPatient;
        }

        // Update Patient
        public async Task<Patient> Update(Patient patient)
        {
            if (patient == null)
                throw new ArgumentException("Patient cannot be empty.");

            var existing = Patients.FirstOrDefault(p => p?.Id == patient.Id);

            if (existing == null)
                throw new ArgumentException("Patient not found in the system.");

            var updatedDto = await UpdateAsync(new PatientDTO(patient));

            if (updatedDto == null)
                throw new Exception("Failure updating patient on server. Please try again later.");

            // Update local cache
            var index = Patients.IndexOf(existing);
            Patients[index] = new Patient(updatedDto);

            // Update DTO cache too
            var cachedDto = _patients.FirstOrDefault(p => p?.Id == patient.Id);
            if (cachedDto != null)
            {
                var dtoIndex = _patients.IndexOf(cachedDto);
                _patients[dtoIndex] = updatedDto;
            }

            return Patients[index];
        }

        // Search Patients
        public async Task<List<Patient>> Search(string query)
        {
            var dtos = await SearchAsync(new QueryRequest(query));

            _patients = dtos;

            Patients.Clear();

            foreach (var dto in _patients)
            {
                Patients.Add(new Patient(dto));
            }

            return Patients;
        }

        // Get All Patients Async
        private async Task<List<PatientDTO>> GetAllAsync()
        {
            var response = await new WebRequestHandler().Get($"{baseUrl}");

            if (string.IsNullOrWhiteSpace(response))
                throw new Exception("Error retrieving patients: response was empty");

            var patients = JsonConvert.DeserializeObject<List<PatientDTO>>(response) ?? new List<PatientDTO>();

            _patients = patients;

            return patients;
        }

        // Get Patient by ID Async
        private async Task<PatientDTO> GetByIdAsync(int id)
        {
            var json = await new WebRequestHandler().Get($"{baseUrl}/{id}");

            if (string.IsNullOrWhiteSpace(json))
                return _patients.FirstOrDefault(p => p?.Id == id);

            var dto = JsonConvert.DeserializeObject<PatientDTO>(json);

            if (dto == null)
                return _patients.FirstOrDefault(p => p?.Id == id);

            var existing = _patients.FirstOrDefault(p => p?.Id == id);

            if (existing != null)
            {
                var index = _patients.IndexOf(existing);
                _patients[index] = dto;
            }
            else
            {
                _patients.Add(dto);
            }

            return dto;
        }

        // Delete Patient Async
        private async Task<PatientDTO?> DeleteAsync(int id)
        {
            var response = await new WebRequestHandler().Delete($"{baseUrl}/{id}");

            if (string.IsNullOrWhiteSpace(response) || response == "ERROR")
            {
                return _patients.FirstOrDefault(p => p?.Id == id);
            }

            var dto = JsonConvert.DeserializeObject<PatientDTO>(response);
            return dto ?? _patients.FirstOrDefault(p => p?.Id == id);
        }

        // Add Patient Async
        private async Task<PatientDTO?> AddAsync(PatientDTO dto)
        {
            if (dto == null) return null;

            var response = await new WebRequestHandler().Post($"{baseUrl}", dto);

            if (string.IsNullOrWhiteSpace(response) || response == "ERROR")
                return null;

            var dtoFromServer = JsonConvert.DeserializeObject<PatientDTO>(response);

            return dtoFromServer;
        }

        // Update Patient Async
        private async Task<PatientDTO?> UpdateAsync(PatientDTO dto)
        {
            if (dto == null) return null;

            var response = await new WebRequestHandler().Put($"{baseUrl}/{dto.Id}", dto);

            if (string.IsNullOrWhiteSpace(response) || response == "ERROR")
                return null;

            var dtoFromServer = JsonConvert.DeserializeObject<PatientDTO>(response);

            return dtoFromServer;
        }

        // Search Patients Async
        private async Task<List<PatientDTO>> SearchAsync(QueryRequest query)
        {
            var response = await new WebRequestHandler().Post($"{baseUrl}/search", query);
            
            var dtoFromServer = JsonConvert.DeserializeObject<List<PatientDTO>>(response) ?? new List<PatientDTO>();
            
            return dtoFromServer;
        }
    }
}
