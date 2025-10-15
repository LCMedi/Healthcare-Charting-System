using Library.ChartingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI.ChartingSystem.ViewModels
{
    public class MainViewModel
    {
        public List<Appointment> Appointments
        {
            get
            {
                return new List<Appointment>
                {
                    new Appointment(
                        new Patient("Walter White", DateTime.Now, RACE.White, GENDER.Male, null),
                        new Physician("Dr. Smith", "AX89ZJ", DateTime.Now, new List<string>()),
                        new DateTime(2025, 10, 31, 9, 0, 0)),
                    new Appointment(
                        new Patient("Thomas John", DateTime.Now, RACE.AfricanAmerican, GENDER.Male, null),
                        new Physician("Dr. Jones", "NSJU80", DateTime.Now, new List<string>()),
                        new DateTime(2025, 10, 31, 9, 30, 0)),
                    new Appointment(
                        new Patient("Tim Blake", DateTime.Now, RACE.Asian, GENDER.Male, null),
                        new Physician("Dr. Yue", "MXUT01", DateTime.Now, new List<string>()),
                        new DateTime(2025, 10, 31, 10, 0, 0)),
                };
            }
        }

        public Appointment? SelectedAppointment { get; set; }
    }
}
