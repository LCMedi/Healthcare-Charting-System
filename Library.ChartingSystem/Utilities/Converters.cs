using Library.ChartingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.ChartingSystem.Utilities
{
    public static class RaceConverter
    {
        public static RACE ConvertRace(string race)
        {
            if (string.IsNullOrWhiteSpace(race))
                throw new ArgumentException("Race string cannot be empty.", nameof(race));

            var normalized = race.Trim().ToLowerInvariant();

            return normalized switch
            {
                "asian" => RACE.Asian,
                "black" or "african american" or "african-american" => RACE.Black,
                "white" or "caucasian" => RACE.White,
                "hispanic" or "latino" or "latinx" => RACE.Hispanic,
                "other" => RACE.Other,
                _ => RACE.Other
            };
        }
    }

    public static class GenderConverter
    {
        public static GENDER ConvertGender(string gender)
        {
            if (string.IsNullOrWhiteSpace(gender))
                throw new ArgumentException("Gender string cannot be empty.", nameof(gender));

            var normalized = gender.Trim().ToLowerInvariant();

            return normalized switch
            {
                "male" or "m" => GENDER.Male,
                "female" or "f" => GENDER.Female,
                "other" or "nonbinary" or "non-binary" or "nb" => GENDER.Other,
                _ => GENDER.Other
            };
        }
    }
}
