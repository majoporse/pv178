using InformationSystemHZS.Models.HelperModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Reflection.Metadata.Ecma335;
using InformationSystemHZS.Models.Interfaces;
using InformationSystemHZS.Models.Entities;

namespace InformationSystemHZS.Services
{
    class MyValidationException : Exception
    {
        public MyValidationException(string message) : base(message) { }
    }

    public static class ValidationService
    {
        static HashSet<string> _ValidVehicleTypes = new HashSet<string>{
            "FIRE_ENGINE",
            "TECHNICAL_VEHICLE",
            "ANTI_GAS_VEHICLE",
            "RESCUE_VEHICLE",
            "CRANE_TRUCK",
        };
        static bool IsValidVehicleType(VehicleDto v) => _ValidVehicleTypes.Contains(v.Type);

        public static bool HasValidCallsign(IBaseModel obj)
        {
            if (obj is StationDto || obj is Station)
                return IsValidCallsign(obj.Callsign, 'S');
            if (obj is UnitDto || obj is Unit)
                return IsValidCallsign(obj.Callsign, 'J');
            if (obj is MemberDto || obj is Member)
                return IsValidCallsign(obj.Callsign, 'H');

            throw new ArgumentException();
        }
        static bool IsValidCallsign(string s, char c) =>
            s.Length == 3 && s[0] == c && char.IsDigit(s[1]) && char.IsDigit(s[2]);

        public static bool ValidateScenario(ScenarioObjectDto? scenario)
        {
            if (scenario == null ||
                scenario.Stations == null || scenario.Stations.Count == 0 ||
                scenario.IncidentsHistory == null || scenario.IncidentsHistory.Count == 0)
                throw new MyValidationException("Empty data!");

            if (HasInvalidIDs(scenario))
                throw new MyValidationException("Invalid Callsign");

            if (HasInvalidVehicleType(scenario))
                throw new MyValidationException("Invalid Vehicle Type");

            if (HasInvalidNumberOfFiremen(scenario))
                throw new MyValidationException("Invalid Number of Firemen");

            return true;
        }

        static bool HasInvalidNumberOfFiremen(ScenarioObjectDto scenario) =>
            scenario.Stations.Find(HasInvalidNumberOfFiremen) != null;
        
        static bool HasInvalidNumberOfFiremen(StationDto station) => 
            station.Units.Find((unit) => unit.Members.Count > unit.VehicleDto.Capacity) != null;


        static bool HasInvalidVehicleType(ScenarioObjectDto scenario) => 
            scenario.Stations.Find(HasInvalidVehicleType) != null;
        static bool HasInvalidVehicleType(StationDto station) =>
                station.Units.Find((unit) => !IsValidVehicleType(unit.VehicleDto)) != null;

        static bool HasInvalidIDs(ScenarioObjectDto scenario)
        {
            var StationIDs = new HashSet<string>();
            foreach (var station in scenario.Stations)
            {
                if (!HasValidCallsign(station) ||
                    StationIDs.Contains(station.Callsign))
                {
                    return true;
                }
                StationIDs.Add(station.Callsign);

                if (HasInvalidIDs(station))
                    return true;
            }
            return false;
        }

        static bool HasInvalidIDs(StationDto station)
        {
            var UnitIDs = new HashSet<string>();

            foreach(var unit in station.Units)
            {
                if (!HasValidCallsign(unit) ||
                    UnitIDs.Contains(unit.Callsign))
                {
                    return true;
                }
                UnitIDs.Add(unit.Callsign);
                if (HasInvalidIDs(unit))
                    return true;
            }
            return false;
        }

        static bool HasInvalidIDs(UnitDto u)
        {
            var Callsigns = new HashSet<string>();
            foreach(var member in u.Members)
            {
                if (!HasValidCallsign(member) || 
                    Callsigns.Contains(member.Callsign))
                {
                    return true;
                }
            }
            return false;
        }

    }
}
