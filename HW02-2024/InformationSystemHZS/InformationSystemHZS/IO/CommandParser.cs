using InformationSystemHZS.IO.Helpers.Interfaces;
using InformationSystemHZS.IO.Helpers;
using InformationSystemHZS.Services;
using System.Diagnostics;
using InformationSystemHZS.Models.Entities;
namespace InformationSystemHZS.IO;
using System.Text.RegularExpressions;

public class CommandParser
{
    private IConsoleManager _consoleManager;
    public event EventHandler<string> CommandEntered;
    OutputWriter writer;

    public CommandParser(IConsoleManager consoleManager)
    {
        _consoleManager = consoleManager;
        writer = new OutputWriter(consoleManager);
    }

    public bool ParseCommand(InformationSystem informationSystem, string command)
    {
        CommandEntered?.Invoke(this, command);
        var matches = Regex.Matches(command, @"[^\s""']+|""([^""]*)""|'([^']*)'");

        List<string> commandParts = new List<string>();
        foreach (Match match in matches)
        {
            commandParts.Add(match.Value.Trim('"'));
        }

        string res;
        try
        {
            switch (new {Command = commandParts[0], Len = commandParts.Count})
            {
                case {Command: "exit", Len: 1}:
                    return false;
                case { Command: "help", Len: 1}:
                    writer.PrintHelp();
                    break;
                case { Command: "list-stations", Len: 1}:
                    writer.ListStations(informationSystem);
                    break;
                case { Command: "list-units", Len: 1 }:
                    writer.ListUnits(informationSystem);
                    break;
                case { Command: "list-incidents", Len: 1 }:
                    writer.ListIncidents(informationSystem);
                    break;
                case { Command: "add-member", Len: 4 }:
                    res = informationSystem.AddMember(commandParts[1], commandParts[2], commandParts[3]);
                    writer.Print(res);
                    break;
                case { Command: "remove-member", Len: 4 }:
                    res = informationSystem.RemoveMember(commandParts[1], commandParts[2], commandParts[3]);
                    writer.Print(res);
                    break;
                case { Command: "reassign-member", Len: 6 }:
                    res = informationSystem.ReassignMember(commandParts[1], commandParts[2], commandParts[3], commandParts[4], commandParts[5]);
                    writer.Print(res);
                    break;
                case { Command: "reassign-unit", Len: 4 }:
                    res = informationSystem.ReassignUnit(commandParts[1], commandParts[2], commandParts[3]);
                    writer.Print(res);
                    break;
                case { Command: "report", Len: 5 }:
                    Reporter.Report(informationSystem, int.Parse(commandParts[1]), int.Parse(commandParts[2]), commandParts[3], commandParts[4]);
                    break;
                case { Command: "statistics", Len: 1 }:
                    writer.PrintStatistics(informationSystem);
                    break;
                default:
                    _consoleManager.WriteLine("[Syntax error]: Invalid command.");
                    break;
            }
        } catch (ArgumentException e)
        {
            _consoleManager.WriteLine("[Argument]| " + e.Message);
        }
        return true;
    }
    //public static void ListIncidents(Runner runner) =>
    // TODO: Implement
}