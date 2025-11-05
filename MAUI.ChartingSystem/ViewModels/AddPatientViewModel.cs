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
    private Patient? _patient = null;

    private string _name = string.Empty;
    private string _address = string.Empty;
    private DateTime _birthdate = DateTime.Today;
    private RACE _selectedRace;
    private GENDER _selectedGender;
    private string _newDiagnosis = string.Empty;
    private string _newPrescription = string.Empty;
    private DateTime _newNoteDate = DateTime.Today;
    private Physician? _selectedPhysician = null;

    public event PropertyChangedEventHandler? PropertyChanged;

    public ObservableCollection<Physician> Physicians
    {
        get
        {
            return new ObservableCollection<Physician>(ChartServiceProxy.Current.GetAllPhysicians());
        }
    }

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

    public Physician? SelectedPhysician
    {
        get => _selectedPhysician;
        set { _selectedPhysician = value; OnPropertyChanged(); }
    }

    public ObservableCollection<MedicalNote> MedicalNotes { get; } = new();

    public ICommand? AddMedicalNoteCommand { get; set; }
    public ICommand? SavePatientCommand { get; set; }

    // Constructor for Creates
    public AddPatientViewModel()
    {
        _selectedRace = Races.FirstOrDefault();
        _selectedGender = Genders.FirstOrDefault();

        SetUpCommands();
    }

    // Constructor for Updates
    public AddPatientViewModel(int id) : this()
    {
        _patient = ChartServiceProxy.Current.GetPatient(id);
        if (_patient is null)
            return;

        Name = _patient.Name ?? string.Empty;
        Address = _patient.Address ?? string.Empty;
        Birthdate = _patient.Birthdate ?? DateTime.Today;
        SelectedRace = _patient.Race ?? Races.FirstOrDefault();
        SelectedGender = _patient.Gender ?? Genders.FirstOrDefault();

        MedicalNotes.Clear();
        foreach (var note in _patient.MedicalHistory)
            MedicalNotes.Add(note);

        NewNoteDate = DateTime.Today;
        NewDiagnosis = string.Empty;
        NewPrescription = string.Empty;
        SelectedPhysician = null;

        SetUpCommands();
    }

    private void SetUpCommands()
    {
        AddMedicalNoteCommand = new Command(AddMedicalNote);
        SavePatientCommand = new Command(async _ => await DoSave());
    }

    private async Task DoSave()
    {
        try
        {
            // If updating
            if (_patient != null)
            {
                _patient.SetName(Name);
                _patient.SetAddress(Address);
                _patient.SetBirthdate(Birthdate);
                _patient.SetRace(SelectedRace);
                _patient.SetGender(SelectedGender);

                _patient.MedicalHistory.Clear();
                foreach (var note in MedicalNotes)
                    _patient.MedicalHistory.Add(note);
                await Shell.Current.DisplayAlert("Success", "Patient updated successfully!", "OK");
                await Shell.Current.GoToAsync("//Patients");
            }
            // If creating
            else
            {
                // Try adding patient
                _patient = new Patient(Name, Birthdate, SelectedRace, SelectedGender, Address);

                foreach (var note in MedicalNotes)
                    _patient.MedicalHistory.Add(note);

                ChartServiceProxy.Current.AddPatient(_patient);
                await Shell.Current.DisplayAlert("Success", "Patient added successfully!", "OK");
                await Shell.Current.GoToAsync("//Patients");
            }
        }
        catch (ArgumentException ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"Something went wrong: {ex.Message}", "OK");
        }
    }

    public void AddMedicalNote()
    {
        if (!string.IsNullOrWhiteSpace(NewDiagnosis) && !string.IsNullOrWhiteSpace(NewPrescription) && SelectedPhysician != null)
        {
            MedicalNotes.Add(new MedicalNote(NewNoteDate, NewDiagnosis, NewPrescription, SelectedPhysician));
            NewNoteDate = DateTime.Today;
            NewDiagnosis = string.Empty;
            NewPrescription = string.Empty;
            SelectedPhysician = null;
        }
    }

    public void Reset()
    {
        _patient = null;
        Name = string.Empty;
        Address = string.Empty;
        Birthdate = DateTime.Today;
        SelectedRace = Races.FirstOrDefault();
        SelectedGender = Genders.FirstOrDefault();
        MedicalNotes.Clear();
        NewNoteDate = DateTime.Today;
        NewDiagnosis = string.Empty;
        NewPrescription = string.Empty;
        SelectedPhysician = null;
    }

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}