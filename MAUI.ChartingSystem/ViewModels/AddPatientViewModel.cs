using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Library.ChartingSystem.Models;

namespace MAUI.ChartingSystem.ViewModels;

public class AddPatientViewModel : INotifyPropertyChanged
{
    private string _name;
    private string _address;
    private DateTime _birthdate = DateTime.Today;
    private RACE _selectedRace;
    private GENDER _selectedGender;
    private MedicalNote _newMedicalNote;

    // Fields for new medical note
    private DateTime _newNoteDate = DateTime.Today;
    private string _newDiagnosis;
    private string _newPrescription;
    private Physician _newPhysician = new Physician("Dr. Smith", "12345", DateTime.Today, new List<string> { "General" });

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
        AddMedicalNoteCommand = new Command(AddMedicalNote);
    }

    public void AddMedicalNote()
    {
        if (!string.IsNullOrWhiteSpace(NewDiagnosis) && !string.IsNullOrWhiteSpace(NewPrescription) && NewPhysician != null)
        {
            MedicalNotes.Add(new MedicalNote(NewNoteDate, NewDiagnosis, NewPrescription, NewPhysician));
            NewNoteDate = DateTime.Today;
            NewDiagnosis = string.Empty;
            NewPrescription = string.Empty;
            // Optionally reset physician
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

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}