using InformationSystemHZS.IO.Helpers.Interfaces;
using InformationSystemHZS.Models.Entities;
using InformationSystemHZS.Models.HelperModels;
using InformationSystemHZS.Services;
namespace InformationSystemHZS.IO;

public class OutputWriter
{
    private  IConsoleManager _consoleManager;

    public OutputWriter(IConsoleManager consoleManager)
    {
        _consoleManager = consoleManager;
    }

    public void ListStations(InformationSystem system) =>
    system._stations.GetAllEntities().ForEach(PrintStation);

    void PrintStation(Station s) =>
        _consoleManager.WriteLine($"{s.Callsign} | {s.Name, -30} | {s.unitsMap.GetEntitiesCount()} | ({s.PositionDto.X}, {s.PositionDto.Y})");

    public void ListUnits(InformationSystem system) =>
        system._stations.GetAllEntities().ForEach(s => s.unitsMap.GetAllEntities().ForEach(e => PrintUnit(e, system)));

    RecordedIncident? getOngoingIncident(Unit u, InformationSystem system) => 
        system.ongoingIncidents.Find(e => e.Item2.AssignedUnit == u.Callsign).Item2;
    
    void PrintUnit(Unit u, InformationSystem system)
    {
        var s = u.State != "AVAILABLE" ?
           $" | {DateTime.Now - DateTime.Parse(getOngoingIncident(u, system).IncidentStartTIme):mm\\:ss}" : 
            "";
        _consoleManager.WriteLine($"{u.StationCallsign} | {u.Callsign} | {u.VehicleDto.Name, -25} | {
                                     u.membersMap.GetEntitiesCount()}/{u.VehicleDto.Capacity} | {u.State, -15}{s}");
    }

    public void ListIncidents(InformationSystem system) =>
        system.ongoingIncidents.ForEach(a => PrintIncident(a.Item2));

    void PrintIncident(RecordedIncident i) =>
        _consoleManager.WriteLine($"{i.Type} | {i.Location.X} {i.Location.Y} | {i.IncidentStartTIme} | {i.Description, -30} | {i.AssignedStation} | {i.AssignedUnit}");

    public void PrintHelp() =>
        _consoleManager.WriteLine(@"Commands:
    exit: Exits the program.
    Help: Displays this help message.
    list-stations: Lists all stations.
    list-units: Lists all units.
    list-incidents: Lists all incidents.
    add-member <station> <unit> <member>: Adds a member to a unit in a station.
    remove-member <station> <unit> <member>: Removes a member from a unit in a station.
    reassign-member <station1> <unit1> <member> <station2> <unit2>: Reassigns a member from one unit to another.
    reassign-unit <station1> <unit> <station2>: Reassigns a unit from one station to another.
");

    public void Print(string message) =>
        _consoleManager.WriteLine("[Processed]: " + message);

    public void PrintStatistics(InformationSystem system)
    {
        _consoleManager.WriteLine($"Total Ammount of FIRE_ENGINE wehicles: {StatisticsService.GetTotalFireEnginesCount(system)}");
        _consoleManager.WriteLine("--------------------");
        _consoleManager.WriteLine($"Station closest to hospital: {StatisticsService.GetClosestToHospital(system)}");
        _consoleManager.WriteLine("--------------------");
        _consoleManager.WriteLine($"Fastest vehicle unit: {StatisticsService.GetFastestVehicleUnit(system)}");
        _consoleManager.WriteLine("--------------------");
        _consoleManager.WriteLine($"Station with most personel: {StatisticsService.GetStationWithMostPersonel(system)}");
        _consoleManager.WriteLine("--------------------");
        _consoleManager.WriteLine("Vehicles by fuel consumption: ");
        StatisticsService.GetVehiclesByFuelConsumption(system).ForEach(s => _consoleManager.WriteLine("|  " + s));
        _consoleManager.WriteLine("--------------------");
        _consoleManager.WriteLine($"Most busy unit: {StatisticsService.GetMostBusyUnit(system)}");
        _consoleManager.WriteLine("--------------------");
        _consoleManager.WriteLine($"Most fuel consumed: {StatisticsService.MostFuelConsumedUnit(system)}");
        _consoleManager.WriteLine("--------------------");
    }
    // TODO: Implement
}