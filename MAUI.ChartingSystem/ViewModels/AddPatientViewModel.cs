using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Library.ChartingSystem.Models;
using Library.ChartingSystem.Services;

namespace MAUI.ChartingSystem.ViewModels;

public class AddPatientViewModel : INotifyPropertyChanged
{
    private string _name = string.Empty;
    private string _address = string.Empty;
    private DateTime _birthdate = DateTime.Today;
    private RACE _selectedRace;
    private GENDER _selectedGender;
    private string _newDiagnosis = string.Empty;
    private string _newPrescription = string.Empty;
    private DateTime _newNoteDate = DateTime.Today;
    private Physician _newPhysician = CreateDefaultPhysician();

    public event PropertyChangedEventHandler? PropertyChanged;

    public string Name
    {
        get => _name;
        set { _name = value; OnPropertyChanged(); }
    }

    public string Address
    {
        get => _address;
        set { _address = value; OnPropertyChanged(); }
    }

    public DateTime Birthdate
    {
        get => _birthdate;
        set { _birthdate = value; OnPropertyChanged(); }
    }

    public List<RACE> Races { get; } = new(Enum.GetValues<RACE>());

    public RACE SelectedRace
    {
        get => _selectedRace;
        set { _selectedRace = value; OnPropertyChanged(); }
    }

    public List<GENDER> Genders { get; } = new(Enum.GetValues<GENDER>());

    public GENDER SelectedGender
    {
        get => _selectedGender;
        set { _selectedGender = value; OnPropertyChanged(); }
    }

    public DateTime NewNoteDate
    {
        get => _newNoteDate;
        set { _newNoteDate = value; OnPropertyChanged(); }
    }

    public string NewDiagnosis
    {
        get => _newDiagnosis;
        set { _newDiagnosis = value; OnPropertyChanged(); }
    }

    public string NewPrescription
    {
        get => _newPrescription;
        set { _newPrescription = value; OnPropertyChanged(); }
    }

    public Physician NewPhysician
    {
        get => _newPhysician;
        set { _newPhysician = value; OnPropertyChanged(); }
    }

    public ObservableCollection<MedicalNote> MedicalNotes { get; } = new();

    public ICommand AddMedicalNoteCommand { get; }

    public AddPatientViewModel()
    {
        // initialize defaults if needed
        _selectedRace = Races.FirstOrDefault();
        _selectedGender = Genders.FirstOrDefault();

        AddMedicalNoteCommand = new Command(AddMedicalNote);
    }

    public AddPatientViewModel(int id) : this()
    {
        var patient = ChartServiceProxy.Current.GetPatient(id);
        if (patient is null)
            return;

        Name = patient.Name ?? string.Empty;
        Address = patient.Address ?? string.Empty;
        Birthdate = patient.Birthdate ?? DateTime.Today;
        SelectedRace = patient.Race ?? Races.FirstOrDefault();
        SelectedGender = patient.Gender ?? Genders.FirstOrDefault();

        MedicalNotes.Clear();
        foreach (var note in patient.MedicalHistory)
            MedicalNotes.Add(note);

        NewNoteDate = DateTime.Today;
        NewDiagnosis = string.Empty;
        NewPrescription = string.Empty;
        NewPhysician = CreateDefaultPhysician();
    }

    public void AddMedicalNote()
    {
        if (!string.IsNullOrWhiteSpace(NewDiagnosis) && !string.IsNullOrWhiteSpace(NewPrescription) && NewPhysician != null)
        {
            MedicalNotes.Add(new MedicalNote(NewNoteDate, NewDiagnosis, NewPrescription, NewPhysician));
            NewNoteDate = DateTime.Today;
            NewDiagnosis = string.Empty;
            NewPrescription = string.Empty;
            NewPhysician = CreateDefaultPhysician();
        }
    }

    public Patient ToPatient()
    {
        var patient = new Patient();
        patient.SetName(Name);
        patient.SetAddress(Address);
        patient.SetBirthdate(Birthdate);
        patient.SetRace(SelectedRace);
        patient.SetGender(SelectedGender);

        foreach (var note in MedicalNotes)
            patient.MedicalHistory.Add(note);

        return patient;
    }

    // Reset all fields to defaults
    public void Reset()
    {
        Name = string.Empty;
        Address = string.Empty;
        Birthdate = DateTime.Today;
        SelectedRace = Races.FirstOrDefault();
        SelectedGender = Genders.FirstOrDefault();
        MedicalNotes.Clear();
        NewNoteDate = DateTime.Today;
        NewDiagnosis = string.Empty;
        NewPrescription = string.Empty;
        NewPhysician = CreateDefaultPhysician();
    }

    private static Physician CreateDefaultPhysician()
    {
        return new Physician("Dr. Smith", "12345", DateTime.Today, new List<string> { "General" });
    }

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}