using InformationSystemHZS.Models.Entities;
using InformationSystemHZS.Services;
namespace InformationSystemHZS.Services;

// IMPORTANT NOTE: For this part of code use only LINQ.
// NOTE: You can change the signature of functions to take a single parameter, i.e. something like List<Station>.

public static class StatisticsService
{
    /// <summary>
    /// Returns the total number of all 'FIRE_ENGINE' across all stations.
    /// </summary>
    public static int GetTotalFireEnginesCount(InformationSystem system) => 
       (from station in system._stations.GetAllEntities()
        from unit in station.unitsMap.GetAllEntities() //this is cool, i didn't know you can do this
        where unit.VehicleDto.Type == "FIRE_ENGINE"
        select unit).Count();

    /// <summary>
    /// Returns the name of the station closest to the hospital at coordinates (45, 60).
    /// </summary>
    public static string GetClosestToHospital(InformationSystem system) =>
       (from station in system._stations.GetAllEntities()
        let distance = DistanceService.CalculateDistance(station.PositionDto.X, station.PositionDto.Y, 45, 60)
        orderby distance
        select station).First().Name;
    
    /// <summary>
    /// Returns the callsign of the unit with the fastest vehicle. If no decision can be made, an error is printed.
    /// </summary>
    public static string GetFastestVehicleUnit(InformationSystem system) => 
       (from station in system._stations.GetAllEntities()
        from unit in station.unitsMap.GetAllEntities()
        orderby unit.VehicleDto.Speed descending
        select unit).FirstOrDefault()?.Callsign ?? throw new Exception("No unit found");

    /// <summary>
    /// Returns the callsign of the station that has the most firefighters under it.
    /// </summary>
    public static string GetStationWithMostPersonel(InformationSystem system)=>
        (from station in system._stations.GetAllEntities()
         orderby station.unitsMap.GetAllEntities().Sum(unit => unit.membersMap.GetEntitiesCount()) descending
         select station).First().Callsign;

    /// <summary>
    /// Returns a list of all vehicle names sorted by fuel consumption (first with the lowest, last with the highest).
    /// Duplicate names must not appear in the list.
    /// </summary>
    public static List<string> GetVehiclesByFuelConsumption(InformationSystem system) =>
        (from station in system._stations.GetAllEntities()
         from unit in station.unitsMap.GetAllEntities()
         orderby unit.VehicleDto.FuelConsumption
         select unit.VehicleDto.Name).Distinct().ToList();

    /// <summary>
    /// Returns the callsign of the unit that has historically resolved the highest number of events.
    /// </summary>
    public static string GetMostBusyUnit(InformationSystem system) =>
        (from report in system.incidentsHistory
            group report by report.AssignedUnit into g
            orderby g.Count() descending
            select g).FirstOrDefault()?.Key ?? "NO REPORTS ISSUED";

    /// <summary>
    /// Returns the callsign of the unit that has consumed the most fuel with its vehicle in the sum of all its historical events.
    /// </summary>
    public static string MostFuelConsumedUnit(InformationSystem system) => 
       (from incident in system.incidentsHistory
        group incident by incident.AssignedUnit into unitIncidents
        orderby unitIncidents.ToList().Sum(e => {
            var station = system._stations.GetEntity(e.AssignedStation);
            var vehicle = station.unitsMap.GetEntity(e.AssignedUnit).VehicleDto;
            return DistanceService.CalculateDistance(
                        station.PositionDto.X,
                        station.PositionDto.Y,
                        e.Location.X,
                        e.Location.Y) * vehicle.FuelConsumption;
        }) descending
        select unitIncidents.Key).First();

}