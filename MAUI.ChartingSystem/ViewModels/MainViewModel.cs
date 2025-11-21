using Library.ChartingSystem.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MAUI.ChartingSystem.ViewModels
{
    public class MainViewModel
    {
        private string path = @"C:\temp\data.json";
        public MainViewModel()
        {

        }

        public ICommand? ExportCommand { get; private set; }
        public ICommand? ImportCommand { get; private set; }

        public void Export()
        {
            var data = new
            {
                Patients = ChartServiceProxy.Current.GetAllPatients(),
                Physicians = ChartServiceProxy.Current.GetAllPhysicians(),
                Appointments = ChartServiceProxy.Current.GetAllAppointments(),
            };

            var json = JsonConvert.SerializeObject(data, Formatting.Indented);

            Directory.CreateDirectory(Path.GetDirectoryName(path));

            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.WriteLine(json);
            }
        }

        public void Import()
        {
            if (!File.Exists(path)) return;

            using (StreamReader sr = new StreamReader(path))
            {
                var json = sr.ReadToEnd();

                if (string.IsNullOrEmpty(json)) return;

                
            }
        }
    }
}
