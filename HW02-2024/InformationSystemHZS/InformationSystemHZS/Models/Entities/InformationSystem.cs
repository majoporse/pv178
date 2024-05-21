using InformationSystemHZS.Collections;
using InformationSystemHZS.IO;
using InformationSystemHZS.Models.HelperModels;
using InformationSystemHZS.Models.Interfaces;
using InformationSystemHZS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace InformationSystemHZS.Models.Entities
{
    public class InformationSystem
    {
        public CallsignEntityMap<Station> _stations;

        public List<(Unit, RecordedIncident)> ongoingIncidents = [];
        public List<RecordedIncident> incidentsHistory;

        public static readonly Dictionary<string, int> incidentTimes = new()
        {
            {"FIRE", 10 },
            {"ACCIDENT", 6},
            {"DISASTER", 8},
            {"HAZARD", 10},
            {"TECHNICAL", 4},
            {"RESCUE", 6},
        };

        public static readonly Dictionary<string, List<string>> incidentByVehicle = new()
        {
            { "FIRE_ENGINE",        new (){ "FIRE", "ACCIDENT" }},
            { "TECHNICAL_VEHICLE",  new (){ "DISASTER", "TECHNICAL"}},
            { "ANTI_GAS_VEHICLE",   new (){ "HAZARD"}},
            { "RESCUE_VEHICLE",     new (){ "ACCIDENT", "RESCUE"}},
            { "CRANE_TRUCK",        new (){ "TECHNICAL", "RESCUE"}},
        };

        public InformationSystem(ScenarioObjectDto data)
        {
            var stations = data.Stations.Select(s => new Station(s)).ToList();
            _stations = new CallsignEntityMap<Station>(stations);
            incidentsHistory = data.IncidentsHistory?.Select(i => new RecordedIncident(i)).ToList() ?? [];
        }

        public InformationSystem() { }
        public string AddMember(string stationCallsign, string unitCallsign, string name)
        {
            var station = _stations.GetEntity(stationCallsign) ??
                throw new ArgumentException($"[Invalid]: Station {stationCallsign} not found");

            var unit = station.unitsMap.GetEntity(unitCallsign) ??
                throw new ArgumentException($"[Invalid]: Unit {unitCallsign} not found");

            if (unit.membersMap.GetEntitiesCount() == unit.VehicleDto.Capacity)
                throw new ArgumentException($"[Capacity]: Unit {unitCallsign} is full ");

            var member = new Member()
            {
                Callsign = "will be set in the map",
                UnitCallsign = unit.Callsign,
                Name = name,
            };

            unit.membersMap.SafelyAddEntity(member);
            return $"{member.Callsign} - {member.Name} added to {unit.Callsign} - {station.Callsign}";
        }

        public string RemoveMember(string stationCallsign, string unitCallsign, string memberCallsign)
        {
            var station = _stations.GetEntity(stationCallsign) ??
                throw new ArgumentException($"[Invalid]: Station {stationCallsign} not found");

            var unit = station.unitsMap.GetEntity(unitCallsign) ??
                throw new ArgumentException($"[Invalid]: Unit {unitCallsign} not found");

            var member = unit.membersMap.GetEntity(memberCallsign) ??
                throw new ArgumentException($"[Invalid]: Member {memberCallsign} not found");

            if (unit.membersMap.GetEntitiesCount() == 1)
                throw new ArgumentException("[Capacity]: Unit must have at least one member");

            unit.membersMap.SafelyRemoveEntity(memberCallsign);
            return $"{member.Callsign} - {member.Name} removed from {unit.Callsign} - {station.Callsign}";
        }

        public string ReassignMember(string stationCallsign, string unitCallsign, string memberCallsign,
                            string newStationsign, string newUnitCallsign)
        {
            // finding original member
            var station = _stations.GetEntity(stationCallsign) ??
                throw new ArgumentException($"[Invalid]: Station {stationCallsign} not found");

            var unit = station.unitsMap.GetEntity(unitCallsign) ??
                throw new ArgumentException($"[Invalid]: Unit {unitCallsign} - {stationCallsign} not found");

            var member = unit.membersMap.GetEntity(memberCallsign) ??
                throw new ArgumentException($"[Invalid]: Member {memberCallsign} - {unitCallsign} - {stationCallsign} not found");

            //finding new unit
            var newStation = _stations.GetEntity(newStationsign) ??
                throw new ArgumentException($"[Invalid]: New station {newStationsign} not found");

            var newUnit = newStation.unitsMap.GetEntity(newUnitCallsign) ??
                throw new ArgumentException($"[Invalid]: New unit {newUnitCallsign} - {stationCallsign} not found");

            if (unit.Callsign == newUnit.Callsign)
                throw new ArgumentException($"[Invalid]: Unit is the same as new unit");

            if (newUnit.membersMap.GetEntitiesCount() == newUnit.VehicleDto.Capacity)
                throw new ArgumentException($"[Capacity]: unit {newUnit.Callsign} - {newUnit.StationCallsign} is full");

            if (unit.membersMap.GetEntitiesCount() == 1)
                throw new ArgumentException($"[Capacity]: Unit {unit.Callsign} - {unit.StationCallsign} must have at least one member");

            member.UnitCallsign = newUnit.Callsign;
            unit.membersMap.SafelyRemoveEntity(memberCallsign);
            newUnit.membersMap.SafelyAddEntity(member); //this sets new callsign safely
            return $"{member.Callsign} - {member.Name} reassigned from {unit.Callsign} - {unit.StationCallsign} to {newUnit.Callsign} - {newStation.Callsign}";
        }

        public string ReassignUnit(string stationCallsign, string unitCallsign, string newStationCallsign)
        {
            var station = _stations.GetEntity(stationCallsign) ??
                throw new ArgumentException($"[Invalid]: Station {stationCallsign} not found");

            var unit = station.unitsMap.GetEntity(unitCallsign) ??
                throw new ArgumentException($"[Invalid]: Unit {unitCallsign} - {stationCallsign} not found");

            var newStation = _stations.GetEntity(newStationCallsign) ??
                throw new ArgumentException($"[Invalid]: Station {newStationCallsign} not found");

            if (station.unitsMap.GetEntitiesCount() == 1)
                throw new ArgumentException($"[Capacity]: Station {stationCallsign} must have at least one unit");

            unit.StationCallsign = newStation.Callsign;

            station.unitsMap.SafelyRemoveEntity(unitCallsign);
            newStation.unitsMap.SafelyAddEntity(unit); //this sets new callsign safely
            return $"{unit.Callsign} - {unit.VehicleDto.Name} reassigned from {station.Callsign} to {newStation.Callsign}";
        }
    }
}
