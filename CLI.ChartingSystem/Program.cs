using System;
using System.Linq.Expressions;
using Library.ChartingSystem.Models;
using Library.ChartingSystem.Services;

namespace CLI.ChartingSystem
{
    internal class Program
    {
        // Display menu options to the user
        public static void DisplayMenuOptions()
        {
            Console.WriteLine("MAIN MENU\n" +
                "Please choose an option:\n" +
                "\tA: Add patients / physicians / appointments\n" +
                "\tS: Show patients / physicians / appointments\n" +
                "\tU: Update patients / physicians / appointments\n" +
                "\tD: Delete patients / physicians / appointments\n" +
                "\tM: display Menu options\n" +
                "\tX: eXit program\n");
        }

        // Get user input and process commands
        public static string GetMenuInput(string input)
        {
            Console.Write("> ");
            var readInput = Console.ReadLine();
            input = readInput != null ? readInput.ToUpper() : "X";
            return input;
        }

        // Display add options submenu
        public static void DisplayAddOptions(ChartServiceProxy manager)
        {
            string? input = "";
            Console.WriteLine("What would you like to add?\n" +
                "\tP: Add Patient\n" +
                "\tH: Add Physician\n" +
                "\tA: Add Appointment\n" +
                "\tR: Return to main menu\n");
            input = GetMenuInput(input);
            switch (input)
            {
                case "P":
                    AddPatient(manager);
                    break;
                case "H":
                    AddPhysician(manager);
                    break;
                case "A":
                    AddAppointment(manager);
                    break;
                case "M":
                    DisplayMenuOptions();
                    break;
                case "R":
                    return;
                default:
                    break;
            }
        }

        // Display show options submenu
        public static void DisplayShowOptions(ChartServiceProxy manager)
        {
            string? input = "";
            Console.WriteLine("What would you like to show?\n" +
                "\tP: Show Patients\n" +
                "\tH: Show Physicians\n" +
                "\tA: Show Appointments\n" +
                "\tR: Return to main menu\n");
            input = GetMenuInput(input);
            switch (input)
            {
                case "P":
                    Console.WriteLine("ID\tName\t\tGender\t\tBirthdate\tRace\t\tAddress");
                    foreach (var pat in manager.GetAllPatients())
                        Console.WriteLine(pat);
                    break;
                case "H":
                    Console.WriteLine("ID\tName\t\tLicense Number\tSpecializations");
                    foreach (var phy in manager.GetAllPhysicians())
                        Console.WriteLine(phy);
                    break;
                case "A":
                    Console.WriteLine("ID\tPhysican\tPatient\t\tFrom\t\t    To");
                    foreach (var app in manager.GetAllAppointments())
                        Console.WriteLine(app);
                    break;
                case "M":
                    DisplayMenuOptions();
                    break;
                case "R":
                    return;
                default:
                    break;
            }
        }

        // Display update options submenu
        public static void DisplayUpdateOptions(ChartServiceProxy manager)
        {
            string? input = "";
            Console.WriteLine("What would you like to update?\n" +
                "\tP: Update Patient\n" +
                "\tH: Update Physician\n" +
                "\tA: Reschedule Appointment\n" +
                "\tR: Return to main menu\n");
            input = GetMenuInput(input);
            switch (input)
            {
                case "P":
                    UpdatePatient(manager);
                    break;
                case "H":
                    UpdatePhysician(manager);
                    break;
                case "A":
                    RescheduleAppointment(manager);
                    break;
                case "M":
                    DisplayMenuOptions();
                    break;
                case "R":
                    return;
                default:
                    break;
            }
        }

        // Display delete options submenu
        public static void DisplayDeleteOptions(ChartServiceProxy manager)
        {
            string? input = "";
            Console.WriteLine("What would you like to delete?\n" +
                "\tP: Delete Patient\n" +
                "\tH: Delete Physician\n" +
                "\tA: Delete Appointment\n" +
                "\tR: Return to main menu\n");
            input = GetMenuInput(input);
            switch (input)
            {
                case "P":
                    DeletePatient(manager);
                    break;
                case "H":
                    DeletePhysician(manager);
                    break;
                case "A":
                    DeleteAppointment(manager);
                    break;
                case "M":
                    DisplayMenuOptions();
                    break;
                case "R":
                    return;
                default:
                    break;
            }
        }

        // Add Patient
        public static void AddPatient(ChartServiceProxy manager)
        {
            Console.WriteLine("Enter patient name:");
            string? name = Console.ReadLine();
            while (string.IsNullOrEmpty(name))
            {
                Console.WriteLine("Name cannot be empty. Please enter again:");
                name = Console.ReadLine();
            }

            Console.WriteLine("Enter patient birthdate (MM/DD/YYYY):");
            DateTime birthdate;
            while (!DateTime.TryParse(Console.ReadLine(), out birthdate))
            {
                Console.WriteLine("Invalid date format. Please enter again (MM/DD/YYYY):");
            }

            Console.WriteLine("Enter patient race:\n" +
                "\t A: Asian\n" +
                "\t B: African American\n" +
                "\t C: White\n" +
                "\t D: Hispanic\n" +
                "\t E: Other\n");
            string? raceInput = Console.ReadLine()?.ToUpper();
            // Replace the incomplete switch expression for RACE with an exhaustive one
            RACE race = raceInput switch
            {
                "A" => RACE.Asian,
                "B" => RACE.Black,
                "C" => RACE.White,
                "D" => RACE.Hispanic,
                "E" => RACE.Other,
                _ => RACE.Other // Handles any unexpected input
            };

            Console.WriteLine("Enter patient gender:\n" +
                "\t M: Male\n" +
                "\t F: Female\n" +
                "\t O: Other\n");
            string? genderInput = Console.ReadLine()?.ToUpper();
            GENDER gender = genderInput switch
            {
                "M" => GENDER.Male,
                "F" => GENDER.Female,
                "O" => GENDER.Other,
                _ => GENDER.Other
            };
            Console.WriteLine("Enter patient address (optional):");
            string? address = Console.ReadLine();
            try
            {
                manager.AddPatient(new Patient(name, birthdate, race, gender, address));
                Console.WriteLine("Patient added successfully.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        // Add Physician
        public static void AddPhysician(ChartServiceProxy manager)
        {
            Console.WriteLine("Enter physician name:");
            string? name = Console.ReadLine();
            while (string.IsNullOrEmpty(name))
            {
                Console.WriteLine("Name cannot be empty. Please enter again:");
                name = Console.ReadLine();
            }

            Console.WriteLine("Enter physician license number:");
            string? licenseNumber = Console.ReadLine();
            while (string.IsNullOrEmpty(licenseNumber))
            {
                Console.WriteLine("License number cannot be empty. Please enter again:");
                licenseNumber = Console.ReadLine();
            }

            Console.WriteLine("Enter physician graduation date (MM/DD/YYYY):");
            DateTime graduationDate;
            while (!DateTime.TryParse(Console.ReadLine(), out graduationDate) || graduationDate > DateTime.Now)
            {
                Console.WriteLine("Invalid date format or date is in the future. Please enter again (MM/DD/YYYY):");
            }

            List<string> specializations = new List<string>();
            string? specInput = "";
            do
            {
                Console.WriteLine("Enter a specialization (or type 'done' to finish):");
                specInput = Console.ReadLine();
                if (!string.IsNullOrEmpty(specInput) && specInput.ToLower() != "done")
                {
                    specializations.Add(specInput);
                }
            } while (specInput?.ToLower() != "done");

            try
            {
                manager.AddPhysician(new Physician(name, licenseNumber, graduationDate, ""));
                Console.WriteLine("Physician added successfully.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        // Add Appointment
        public static void AddAppointment(ChartServiceProxy manager)
        {
            if (manager.GetAllPhysicians().Count == 0)
            {
                Console.WriteLine("No physicians available. Please add a physician first.");
                return;
            }
            Console.WriteLine("Choose Physician:");
            foreach (var phy in manager.GetAllPhysicians())
                Console.WriteLine(phy);
            Console.WriteLine("Enter Physician ID:");

            int phyId;
            while (!int.TryParse(Console.ReadLine(), out phyId))
            {
                Console.WriteLine("Invalid Physician ID. Please enter again:");
            }

            try
            {
                manager.GetPhysician(phyId);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return;
            }

            var physician = manager.GetPhysician(phyId);

            if (manager.GetAllPatients().Count == 0)
            {
                Console.WriteLine("No patients available. Please add a patient first.");
                return;
            }
            Console.WriteLine("Choose Patient:");
            foreach (var pat in manager.GetAllPatients())
                Console.WriteLine(pat);
            Console.WriteLine("Enter Patient ID:");

            int patId;
            while (!int.TryParse(Console.ReadLine(), out patId))
            {
                Console.WriteLine("Invalid Patient ID. Please enter again:");
            }

            try
            {
                manager.GetPatient(patId);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return;
            }

            var patient = manager.GetPatient(patId);

            Console.WriteLine("Enter appointment date and time (MM/DD/YYYY HH:MM):");

            DateTime appDate;
            while (!DateTime.TryParse(Console.ReadLine(), out appDate) || !Appointment.IsValidTime(appDate) || appDate <= DateTime.Now)
            {
                Console.WriteLine("Invalid date/time format or time is outside business hours. Please enter again (MM/DD/YYYY HH:MM):");
            }

            try
            {
                manager.ScheduleAppointment(new Appointment(patient, physician, appDate));
                Console.WriteLine("Appointment scheduled successfully.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        // Delete Patient
        public static void DeletePatient(ChartServiceProxy manager)
        {
            if (manager.GetAllPatients().Count == 0)
            {
                Console.WriteLine("No patients available to delete.");
                return;
            }

            Console.WriteLine("Choose Patient to delete:");
            foreach (var pat in manager.GetAllPatients())
                Console.WriteLine(pat);

            Console.WriteLine("Enter Patient ID:");
            int patId;
            while (!int.TryParse(Console.ReadLine(), out patId))
            {
                Console.WriteLine("Invalid Patient ID. Please enter again:");
            }

            try
            {
                manager.GetPatient(patId);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return;
            }

            var patient = manager.GetPatient(patId);

            try
            {
                manager.RemovePatient(patient);
                Console.WriteLine("Patient deleted successfully.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        // Delete Physician
        public static void DeletePhysician(ChartServiceProxy manager)
        {
            if (manager.GetAllPhysicians().Count == 0)
            {
                Console.WriteLine("No physicians available to delete.");
                return;
            }

            Console.WriteLine("Choose Physician to delete:");
            foreach (var phy in manager.GetAllPhysicians())
                Console.WriteLine(phy);

            Console.WriteLine("Enter Physician ID:");
            int phyId;
            while (!int.TryParse(Console.ReadLine(), out phyId))
            {
                Console.WriteLine("Invalid Physician ID. Please enter again:");
            }

            try
            {
                manager.GetPhysician(phyId);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return;
            }

            var physician = manager.GetPhysician(phyId);

            try
            {
                manager.RemovePhysician(physician);
                Console.WriteLine("Physician deleted successfully.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        // Delete Appointment
        public static void DeleteAppointment(ChartServiceProxy manager)
        {
            if (manager.GetAllAppointments().Count == 0)
            {
                Console.WriteLine("No appointments available to delete.");
                return;
            }

            Console.WriteLine("Choose Appointment to delete:");
            foreach (var app in manager.GetAllAppointments())
                Console.WriteLine(app);

            Console.WriteLine("Enter Appointment ID:");
            int appId;

            while (!int.TryParse(Console.ReadLine(), out appId))
            {
                Console.WriteLine("Invalid Appointment ID. Please enter again:");
            }

            try
            {
                manager.GetAppointment(appId);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return;
            }

            var appointment = manager.GetAppointment(appId);

            try
            {
                manager.CancelAppointment(appointment);
                Console.WriteLine("Appointment deleted successfully.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        // Update Patient
        public static void UpdatePatient(ChartServiceProxy manager)
        {
            if (manager.GetAllPatients().Count == 0)
            {
                Console.WriteLine("No patients available to update.");
                return;
            }

            Console.WriteLine("Choose patient:");
            foreach (var pat in manager.GetAllPatients())
                Console.WriteLine(pat);

            Console.WriteLine("Enter Patient ID:");
            int patId;

            while (!int.TryParse(Console.ReadLine(), out patId))
            {
                Console.WriteLine("Invalid Patient ID. Please enter again:");
            }

            try
            {
                manager.GetPatient(patId);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return;
            }

            var patient = manager.GetPatient(patId);

            Console.WriteLine("Enter new name or leave empty for no change:");
            string? name = Console.ReadLine();

            if (!string.IsNullOrEmpty(name))
            {
                try
                {
                    patient.SetName(name);
                    Console.WriteLine("Patient name updated successfully.");
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }

            Console.WriteLine("Enter new address or leave empty for no change:");
            string? address = Console.ReadLine();
            if (!string.IsNullOrEmpty(address))
            {
                patient.SetAddress(address);
                Console.WriteLine("Patient address updated successfully.");
            }

            Console.WriteLine("Enter new birthdate (MM/DD/YYYY) or leave empty for no change:");
            string? birthdateInput = Console.ReadLine();
            if (!string.IsNullOrEmpty(birthdateInput))
            {
                DateTime birthdate;
                while (!DateTime.TryParse(birthdateInput, out birthdate))
                {
                    Console.WriteLine("Invalid date format. Please enter again (MM/DD/YYYY):");
                    birthdateInput = Console.ReadLine();
                }
                try
                {
                    patient.SetBirthdate(birthdate);
                    Console.WriteLine("Patient birthdate updated successfully.");
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }

            Console.WriteLine("Enter new race or leave empty for no change:\n" +
                "\t A: Asian\n" +
                "\t B: African American\n" +
                "\t C: White\n" +
                "\t D: Hispanic\n" +
                "\t E: Other\n");
            string? raceInput = Console.ReadLine()?.ToUpper();
            if (!string.IsNullOrEmpty(raceInput))
            {
                RACE race = raceInput switch
                {
                    "A" => RACE.Asian,
                    "B" => RACE.Black,
                    "C" => RACE.White,
                    "D" => RACE.Hispanic,
                    "E" => RACE.Other,
                    _ => RACE.Other // Handles any unexpected input
                };
                try
                {
                    patient.SetRace(race);
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }

            Console.WriteLine("Enter new gender or leave empty for no change:\n" +
                "\t M: Male\n" +
                "\t F: Female\n" +
                "\t O: Other\n");
            string? genderInput = Console.ReadLine()?.ToUpper();
            if (!string.IsNullOrEmpty(genderInput))
            {
                GENDER gender = genderInput switch
                {
                    "M" => GENDER.Male,
                    "F" => GENDER.Female,
                    "O" => GENDER.Other,
                    _ => GENDER.Other
                };
                try
                {
                    patient.SetGender(gender);
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }

            string? noteInput = "";
            do
            {
                Console.WriteLine("Do you wish to add a medical note? Type 'Y' or 'N':");
                noteInput = Console.ReadLine()?.ToUpper();
                if (noteInput == "Y")
                {
                    Console.WriteLine("Enter creation date in MM/DD/YYYY format: ");
                    DateTime noteDate;
                    while (!DateTime.TryParse(Console.ReadLine(), out noteDate))
                    {
                        Console.WriteLine("Invalid date format. Please enter again (MM/DD/YYYY):");
                    }
                    Console.WriteLine("Enter diagnosis:");
                    string? diagnosis = Console.ReadLine();

                    while (string.IsNullOrEmpty(diagnosis))
                    {
                        Console.WriteLine("Diagnosis cannot be empty. Please enter again:");
                        diagnosis = Console.ReadLine();
                    }

                    Console.WriteLine("Enter prescription:");
                    string? prescription = Console.ReadLine();

                    while (string.IsNullOrEmpty(prescription))
                    {
                        Console.WriteLine("Prescription cannot be empty. Please enter again:");
                        prescription = Console.ReadLine();
                    }
                    if (manager.GetAllPhysicians().Count == 0)
                    {
                        Console.WriteLine("No physicians available. Please add a physician first.");
                        return;
                    }
                    Console.WriteLine("Choose Physician:");
                    foreach (var phy in manager.GetAllPhysicians())
                        Console.WriteLine(phy);
                    Console.WriteLine("Enter Physician ID:");
                    int phyId;
                    while (!int.TryParse(Console.ReadLine(), out phyId))
                    {
                        Console.WriteLine("Invalid Physician ID. Please enter again:");
                    }
                    try
                    {
                        manager.GetPhysician(phyId);
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                        return;
                    }
                    var physician = manager.GetPhysician(phyId);
                    try
                    {
                        patient.AddMedicalNote(noteDate, diagnosis, prescription, physician);
                        Console.WriteLine("Medical note added successfully.");
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
            } while (noteInput?.ToUpper() != "N");
            Console.WriteLine("Patient updated succesfully");
        }

        // Update Physician
        public static void UpdatePhysician(ChartServiceProxy manager)
        {
            if (manager.GetAllPhysicians().Count == 0)
            {
                Console.WriteLine("No physicians available to update.");
                return;
            }

            Console.WriteLine("Choose physician:");
            foreach (var phy in manager.GetAllPhysicians())
                Console.WriteLine(phy);

            Console.WriteLine("Enter Physician ID:");
            int phyId;

            while (!int.TryParse(Console.ReadLine(), out phyId))
            {
                Console.WriteLine("Invalid Physician ID. Please enter again:");
            }

            try
            {
                manager.GetPhysician(phyId);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return;
            }
            var physician = manager.GetPhysician(phyId);

            Console.WriteLine("Enter new name or leave empty for no change:");
            string? name = Console.ReadLine();

            if (!string.IsNullOrEmpty(name))
            {
                try
                {
                    physician.SetName(name);
                    Console.WriteLine("Patient name updated successfully.");
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }

            Console.WriteLine("Enter new license number or leave empty for no change:");
            string? licenseNumber = Console.ReadLine();

            if (!string.IsNullOrEmpty(licenseNumber))
            {
                try
                {
                    physician.SetLicenseNumber(licenseNumber);
                    Console.WriteLine("License number updated successfully.");
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }

            Console.WriteLine("Enter new graduation date (MM/DD/YYYY) or leave empty for no change:");
            string? gradDateInput = Console.ReadLine();
            if (!string.IsNullOrEmpty(gradDateInput))
            {
                DateTime gradDate;
                while (!DateTime.TryParse(gradDateInput, out gradDate) || gradDate > DateTime.Now)
                {
                    Console.WriteLine("Invalid date format or date is in the future. Please enter again (MM/DD/YYYY):");
                    gradDateInput = Console.ReadLine();
                }

                try
                {
                    physician.SetGraduationDate(gradDate);
                    Console.WriteLine("Graduation date updated successfully.");
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }

            string? specInput = "";
            do
            {
                Console.WriteLine("To update specializations:\n" +
                "\t A: Add specialization\n" +
                "\t D: Delete specialization\n" +
                "\t Q: Quit\n");
                specInput = Console.ReadLine()?.ToUpper();
                switch (specInput)
                {
                    case "A":
                        Console.WriteLine("Enter specialization to add:");
                        string? addSpec = Console.ReadLine();
                        if (!string.IsNullOrEmpty(addSpec))
                        {
                            physician.AddSpecialization(addSpec);
                            Console.WriteLine("Specialization added successfully.");
                        }
                        else
                        {
                            Console.WriteLine("Specialization cannot be empty.");
                        }
                        break;
                    case "D":
                        if (physician.Specializations.Count() == 0)
                        {
                            Console.WriteLine("No specializations available to delete.");
                            break;
                        }

                        Console.WriteLine("Enter specialization to delete:");
                        foreach (var spec in physician.Specializations)
                            Console.WriteLine($"\t- {spec}");

                        string? delSpec = Console.ReadLine();
                        if (!string.IsNullOrEmpty(delSpec))
                        {
                            try
                            {
                                physician.RemoveSpecialization(delSpec);
                                Console.WriteLine("Specialization removed successfully.");
                            }
                            catch (ArgumentException ex)
                            {
                                Console.WriteLine($"Error: {ex.Message}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Specialization cannot be empty.");
                        }
                        break;
                    case "Q":
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please enter A, D, or Q.");
                        break;
                }
            } while (specInput != "Q");
            Console.WriteLine("Physician updated successfully.");
        }

        // Reschedule Appointment
        public static void RescheduleAppointment(ChartServiceProxy manager)
        {
            if (manager.GetAllAppointments().Count == 0)
            {
                Console.WriteLine("No appointments available to reschedule.");
                return;
            }

            Console.WriteLine("Choose appointment to reschedule:");
            foreach (var app in manager.GetAllAppointments())
                Console.WriteLine(app);

            Console.WriteLine("Enter Appointment ID:");
            int appId;

            while (!int.TryParse(Console.ReadLine(), out appId))
            {
                Console.WriteLine("Invalid Appointment ID. Please enter again:");
            }

            try
            {
                manager.GetAppointment(appId);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return;
            }

            var appointment = manager.GetAppointment(appId);

            Console.WriteLine("Enter new appointment date and time (MM/DD/YYYY HH:MM):");
            DateTime newDate;
            while (!DateTime.TryParse(Console.ReadLine(), out newDate) || !Appointment.IsValidTime(newDate) || newDate <= DateTime.Now)
            {
                Console.WriteLine("Invalid date/time format or time is outside business hours. Please enter again (MM/DD/YYYY HH:MM):");
            }
            try
            {
                manager.RescheduleAppointment(appointment, newDate);
                Console.WriteLine("Appointment rescheduled successfully.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        static void Main(string[] args)
        {
            var chartManager = ChartServiceProxy.Current;
            string? input = "";

            do
            {
                if (input != "M")
                    DisplayMenuOptions();
                input = GetMenuInput(input);
                switch (input)
                {
                    case "A":
                        DisplayAddOptions(chartManager);
                        break;
                    case "S":
                        DisplayShowOptions(chartManager);
                        break;
                    case "U":
                        DisplayUpdateOptions(chartManager);
                        break;
                    case "D":
                        DisplayDeleteOptions(chartManager);
                        break;
                    case "M":
                        DisplayMenuOptions();
                        break;
                    case "X":
                        Console.WriteLine("Exiting program. Goodbye!");
                        break;
                    default:
                        break;
                }
            } while (input != "X");
        }
    }
}