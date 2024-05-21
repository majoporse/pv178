using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InformationSystemHZS.Services;
using InformationSystemHZS.Models.HelperModels;
using InformationSystemHZS.Models.Entities;

namespace InformationSystemHZS.Services
{
    public static class Reporter
    {
        public static void Report(InformationSystem system, int x, int y, string type, string desc)
        {
            var stationsByDistance = 
                from station in system._stations.GetAllEntities()
                from unit in station.unitsMap.GetAllEntities()
                where unit.State == "AVAILABLE" && InformationSystem.incidentByVehicle[unit.VehicleDto.Type].Contains(type)
                let Station = system._stations.GetEntity(unit.StationCallsign)
                orderby DistanceService.CalculateDistance(Station.PositionDto.X, Station.PositionDto.Y, x, y), 
                        unit.VehicleDto.Speed,
                        unit.StationCallsign, 
                        unit.Callsign
                select unit;

            if (x > 100 || y > 100 || x < 0 || y < 0)
                throw new ArgumentException("Invalid coordinates");

            if (stationsByDistance.Count() == 0)
                throw new ArgumentException("No available units found");

            Unit First = stationsByDistance.First();
            First.State = "EN_ROUTE";

            var incident = new RecordedIncident
            {
                Type = type,
                Location = new Position( x, y ),
                Description = desc,
                IncidentStartTIme = DateTime.Now.ToString(),
                AssignedStation = First.StationCallsign,
                AssignedUnit = First.Callsign
            };

            system.ongoingIncidents.Add((First, incident));
            system.incidentsHistory.Add(incident);
        }
    }
}
