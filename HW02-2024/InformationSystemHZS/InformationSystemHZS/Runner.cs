using InformationSystemHZS.Collections;
using InformationSystemHZS.IO;
using InformationSystemHZS.IO.Helpers.Interfaces;
using InformationSystemHZS.Models.Entities;
using InformationSystemHZS.Models.HelperModels;
using InformationSystemHZS.Services;
using Timer = System.Timers.Timer;

namespace InformationSystemHZS;

public class Runner
{
    private static Timer? _updateTimer;

    static InformationSystem informationSystem = new();

    public static Task Main(IConsoleManager consoleManager, string entryFileName = "Brnoslava.json")
    {
        // ---- DO NOT TOUCH ----
        var commandParser = new CommandParser(consoleManager);
        var outputWriter = new OutputWriter(consoleManager);
        StartUpdateFunction();
        // ^^^^^ DO NOT TOUCH ^^^^^

        ScenarioObjectDto data;
        try
        {
            data = ScenarioLoader.GetInitialScenarioData(entryFileName);
            ValidationService.ValidateScenario(data);
        }
        catch (MyValidationException e)
        {
            consoleManager.WriteLine("Error while Validating: " + e.Message);
            return Task.CompletedTask;
        }
        catch (Exception e)
        {
            consoleManager.WriteLine("Error during import: " + e.Message);
            return Task.CompletedTask;
        }
        
        informationSystem = new InformationSystem(data);
        commandParser.CommandEntered += Logger.OnInputGiven;
        string command;
        do
        {
            command = consoleManager.ReadLine();
        } while (commandParser.ParseCommand(informationSystem, command));

        return Task.CompletedTask;
    }

    /// <summary>
    /// Starts the update function to update function on program start.
    /// DO NOT CHANGE THIS METHOD.
    /// </summary>
    private static void StartUpdateFunction()
    {
        // Set up a timer to call the Update function every second
        _updateTimer = new Timer(TimeSpan.FromSeconds(1));
        _updateTimer.Elapsed += (sender, e) => UpdateFunction();
        _updateTimer.Start();
    }

    /// <summary>
    /// Is called every second to update the state of the system and its objects.
    /// </summary>
    private static void UpdateFunction()
    {
        foreach (var (unit, incident) in informationSystem.ongoingIncidents)
        {
            var station = informationSystem._stations.GetEntity(unit.StationCallsign);
            var dist = DistanceService.CalculateDistance(station.PositionDto.X, station.PositionDto.Y,
                                             incident.Location.X, incident.Location.Y);
            var travelTime = DistanceService.CalculateTimeTaken(dist, unit.VehicleDto.Speed);
            var incidentTime = InformationSystem.incidentTimes[incident.Type];

            switch (unit.State)
            {
                case "EN_ROUTE":

                    if (DateTime.Now - DateTime.Parse(incident.IncidentStartTIme)
                         > TimeSpan.FromSeconds(travelTime))
                    {
                        unit.State = "ON_SITE";
                        //Console.WriteLine("Updating...");
                    }
                    break;

                case "ON_SITE":
                    if (DateTime.Now - DateTime.Parse(incident.IncidentStartTIme) >
                        TimeSpan.FromSeconds(travelTime + incidentTime))
                    {
                        unit.State = "RETURNING";
                        //Console.WriteLine("Updating...");
                    }
                    break;

                case "RETURNING":

                    if (DateTime.Now - DateTime.Parse(incident.IncidentStartTIme) >
                        TimeSpan.FromSeconds(travelTime * 2 + incidentTime))
                    {
                        unit.State = "AVAILABLE";
                        //Console.WriteLine("Updating...");
                    }
                    break;
            }
        }
        informationSystem.ongoingIncidents.RemoveAll(x => x.Item1.State == "AVAILABLE");
    }
}