using Library.ChartingSystem.DTO;
using Library.ChartingSystem.Models;
using Library.eCommerce.Utilities;
using Newtonsoft.Json;

namespace Library.ChartingSystem.Services
{
    public class PatientServiceProxy
    {
        private List<PatientDTO> _patients;

        private static PatientServiceProxy? instance;
        private static object instanceLock = new object();
        private const string baseUrl = "/patient";

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

        public List<Patient> Patients { get; } = new();

        public async Task<Patient> GetById(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID cannot be negative or zero.");

            var dto = await GetByIdAsync(id);

            return new Patient(dto);
        }

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

        public async Task<List<PatientDTO>> GetAllAsync()
        {
            var response = await new WebRequestHandler().Get($"{baseUrl}");

            if (string.IsNullOrWhiteSpace(response))
                throw new Exception("Error retrieving patients: response was empty");

            var patients = JsonConvert.DeserializeObject<List<PatientDTO>>(response) ?? new List<PatientDTO>();

            _patients = patients;

            return patients;
        }


        public async Task<PatientDTO> GetByIdAsync(int id)
        {
            var json = await new WebRequestHandler().Get($"{baseUrl}/{id}");

            if (string.IsNullOrWhiteSpace(json))
                return _patients.FirstOrDefault(p => p?.Id == id);

            var dto = JsonConvert.DeserializeObject<PatientDTO>(json);

            if (dto == null)
                return _patients.FirstOrDefault(p => p?.Id == id);

            // Update cache
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
    }
}
