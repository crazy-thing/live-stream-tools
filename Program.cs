using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using LiveStreamTools.Commands;
using LiveStreamTools.Core;
using LiveStreamTools.Models;
using LiveStreamTools.Util;

class Program
{
    public static Dictionary<string, TaskInfo> tasks = new Dictionary<string, TaskInfo>();
    public static Dictionary<string, string> nameToIds = new Dictionary<string, string>();
    public static string helpCmdInfo = @"
    Start command used to start a specific task. Countdown option requires a date and time
    start 
        countdown (yyyy-mm-dd) (hh:mm:ss) **OPTIONAL** name (""enter-name-here"") file-path (""C:\Enter\File\path\here.txt"")
        bible-verses  **OPTIONAL** name (""enter-name-here"") file-path (""C:\Enter\File\path\here.txt"")
        countdown-verses (yyyy-mm-dd) (hh:mm:ss)

    Stop command used to stop a specific task. Requires the name of the task to stop
    stop (""task-name"")
          (""all"")

    Show command used to show all running tasks. If no tasks are running there will be no output.
    show

    Set command used to configure a setting. Each setting option requires arguments
    set
        Sets the text to be displayed with the time
        countdown-text (enter text here)

        Sets the text to be displayed when the time is over
        countdown-over-text (enter text here)

        Sets the format for the time to be displayed in hh:mm:ss mm:ss
        countdown-format (enter format here) 
            dd:hh:mm:ss
            dd:hh:mm
            dd:hh
            dd
            hh:mm:ss
            hh:mm
            hh:ss
            hh
            mm:ss
            mm
            ss

        Sets the file path for where to write the countdown time to
        file-path (C:\Enter\File Path\here\file.txt)

        Sets day and time for the countdown to automatically count to
        auto-start-time (full day of week, e.g: monday, tuesday, etc) (hh:mm:ss)

        Sets how often for new bible verses to be displayed
        bible-verses-interval (number in seconds e.g: 10 )

        Sets the file path for where to write bible verses to
        bible-verses-file-path (C:\Enter\Bilbe Verses\File\path\here.txt)

        Sets the translation for the bible verses
        bible-verses-translation
            ASV
            BBE
            DARBY
            KJV
            WEB
            YLT
            ESV
            NIV
            NLT

        Sets the genre to get bible verses from
        bible-verses-genre
            All
            Law
            History
            Wisdom
            Prophets
            Gospels
            Acts
            Epistles
            Apocalyptic

    Enable-auto-start command used to configure a setting to auto start a task on program startup. ""countdown"" for countdown timer and ""bible-verses"" for bible verses.
    enable-auto-start
        countdown
        bible-verses

    Disable-auto-start command used to configure a setting to not start a task on program startup. ""countdown"" for countdown timer and ""bible-verses"" for bible verses.
    disable-auto-start
        countdown
        bible-verses
    Edit-bible-verses command used to open a txt file to configure settings for the bible verses display. Changes can be made to the font, font-size, and color.
    More changes can be made directly by going to the verses-template.html file.
    edit-bible-verses

    Exit command used to close close program
    exit

    ";
    public static string useHelp = "Use ('help') to see all commands";
    public static object lockObject = new object();
    
    static void Main(string[] args)
    {
        Settings.LoadSettings();

        StartAutoTasks();

        Console.WriteLine($"Enter a command. {useHelp} ");
        
        while (true)
        {
            string userInput = GetUserInput();

            string[] inputParts = ParseUserInput(userInput);

            ExecuteCommand(inputParts);
        }
    }
    
    static void StartAutoTasks()
    {
        if (Settings.settings.AutoStartCountdown)
        {
            Console.WriteLine("Auto Start Countdown is enabled");
            DateTime startTime = TimeCalculator.CalculateAutoStartDateTime(Settings.settings.AutoCountdownDay, Settings.settings.AutoCountdownTime);
            Countdown.StartCountdown(startTime);
        }
        
        if (Settings.settings.AutoStartBibleVersesLoop)
        {
            Console.WriteLine("Auto Start Bible Verses is enabled");
            BibleVersesWriter.StartBibleVerses();
        }
    }
    
    static string GetUserInput()
    {
        Console.Write("> ");
        return Console.ReadLine();
    }
    
    static string[] ParseUserInput(string userInput)
    {
        return Regex.Matches(userInput, @"[\""].+?[\""]|[^ ]+")
                        .Cast<Match>()
                        .Select(m => m.Value)
                        .ToArray();
    }
    
    static void ExecuteCommand(string[] inputParts )
    {
        string command = inputParts.Length > 0 ? inputParts[0].ToLower() : "";
        Console.WriteLine(command);

        DateTime selectedDateTime = DateTime.Now;

        switch (command)
        {
            case "help":
                Console.WriteLine(helpCmdInfo);
                break;
            case "start":
                Start.StartCommand(inputParts);
                break;
            case "stop":
                Stop.StopCommand(inputParts);
                break;
            case "set":
                Set.SetCommand(inputParts);
                break;
            case "show":
                Show.ShowCommand();
                break;
            case "edit-bible-verses":
                EditVerses.EditBibleVersesCommand();
                break;
            case "enable-auto-start":
                AutoStart.EnableAutoStartCommand(inputParts);
                break;
            case "disable-auto-start":
                AutoStart.DisableAutoStartCommand(inputParts);
                break;
            case "exit":
                return;
            default:
                Console.WriteLine($"Invalid command. {useHelp}");
                break;
        }
    }
}