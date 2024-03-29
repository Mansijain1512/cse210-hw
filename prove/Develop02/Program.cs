﻿using System;
using System.IO;
using System.Collections.Generic;

class JournalEntry
{
    public string Text { get; set; }
    public DateTime Date { get; set; }
    public string Location { get; set; }
    public string Weather { get; set; }
    public string Mood { get; set; }
    public List<string> Tags { get; set; }
}

class Journal
{
    public List<JournalEntry> Entries { get; set; }

    public Journal()
    {
        Entries = new List<JournalEntry>();
    }

    public void LoadEntries(string filePath)
    {
        try
        {
            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);

                foreach (string line in lines)
                {
                    string[] parts = line.Split(',');
                    if (parts.Length >= 2)
                    {
                        if (DateTime.TryParse(parts[0], out DateTime date))
                        {
                            string entryText = parts[1];

                            JournalEntry entry = new JournalEntry
                            {
                                Date = date,
                                Text = entryText
                            };

                            if (parts.Length > 2)
                            {
                                entry.Location = parts[2];
                            }
                            if (parts.Length > 3)
                            {
                                entry.Weather = parts[3];
                            }
                            if (parts.Length > 4)
                            {
                                entry.Mood = parts[4];
                            }
                            if (parts.Length > 5)
                            {
                                entry.Tags = new List<string>(parts[5].Split(';'));
                            }

                            Entries.Add(entry);
                        }
                    }
                }

                Console.WriteLine("Journal entries loaded successfully.");
            }
            else
            {
                Console.WriteLine("File not found. Journal entries were not loaded.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while loading journal entries: {ex.Message}");
        }
    }

    public void SaveEntries(string filePath)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (JournalEntry entry in Entries)
                {
                    string tags = entry.Tags != null ? string.Join(";", entry.Tags) : "";
                    writer.WriteLine($"{entry.Date:s},{entry.Text},{entry.Location ?? ""},{entry.Weather ?? ""},{entry.Mood ?? ""},{tags}");
                }
            }

            Console.WriteLine("Journal entries saved successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while saving journal entries: {ex.Message}");
        }
    }

    public void AddEntry(DateTime date, string entryText, string location, string weather, string mood, List<string> tags)
    {
        JournalEntry entry = new JournalEntry
        {
            Date = date,
            Text = entryText,
            Location = location,
            Weather = weather,
            Mood = mood,
            Tags = tags
        };

        Entries.Add(entry);
        Console.WriteLine("Journal entry added successfully.");
    }

    public void DisplayEntries()
    {
        Console.WriteLine("Journal Entries:");
        foreach (var entry in Entries)
        {
            Console.WriteLine($"{entry.Date.ToString("yyyy-MM-dd")}: {entry.Text}");
            if (!string.IsNullOrEmpty(entry.Location))
            {
                Console.WriteLine($"Location: {entry.Location}");
            }
            if (!string.IsNullOrEmpty(entry.Weather))
            {
                Console.WriteLine($"Weather: {entry.Weather}");
            }
            if (!string.IsNullOrEmpty(entry.Mood))
            {
                Console.WriteLine($"Mood: {entry.Mood}");
            }
            if (entry.Tags != null && entry.Tags.Count > 0)
            {
                Console.WriteLine($"Tags: {string.Join(", ", entry.Tags)}");
            }
        }
    }
}

public class PromptGenerator
{
    private static readonly string[] Prompts = new string[]
    {
        "Who was the most interesting person I interacted with today?",
        "What was the best part of my day?",
        "How did I see the hand of the Lord in my life today?",
        "What was the strongest emotion I felt today?",
        "If I had one thing I could do over today, what would it be?",
        "What was the most unexpected thing that happened today?",
        "Describe a goal or dream you have and what you did today to move closer to it.",
        "Write about a challenge you faced today and how you overcame it."
    };

    public static string GetRandomPrompt()
    {
        Random rand = new Random();
        int index = rand.Next(Prompts.Length);
        return Prompts[index];
    }
}

class Program
{
    static void Main()
    {
        Journal journal = new Journal();

        Console.WriteLine("Welcome to the Journal Program!");

        while (true)
        {
            DisplayMenu();

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    string prompt = PromptGenerator.GetRandomPrompt();
                    Console.WriteLine(prompt);
                    WriteJournalEntry(journal);
                    break;
                case "2":
                    journal.DisplayEntries();
                    break;
                case "3":
                    LoadJournalEntries(journal);
                    break;
                case "4":
                    SaveJournalEntries(journal);
                    break;
                case "5":
                    Console.WriteLine("Exiting the program. Goodbye!");
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    static void DisplayMenu()
    {
        Console.WriteLine("Please select one of the following choices:");
        Console.WriteLine("1. Write");
        Console.WriteLine("2. Display");
        Console.WriteLine("3. Load");
        Console.WriteLine("4. Save");
        Console.WriteLine("5. Exit");
        Console.WriteLine("What would you like to do?");
    }

    static void LoadJournalEntries(Journal journal)
    {
        Console.WriteLine("Enter the file path to load journal entries from:");
        string filePath = Console.ReadLine();
        journal.LoadEntries(filePath);
    }

    static void SaveJournalEntries(Journal journal)
    {
        Console.WriteLine("Enter the file path to save journal entries:");
        string filePath = Console.ReadLine();
        journal.SaveEntries(filePath);
    }

    static void WriteJournalEntry(Journal journal)
    {
        Console.WriteLine("Enter the date (e.g., 2023-10-21):");
        string dateInput = Console.ReadLine();

        if (DateTime.TryParse(dateInput, out DateTime date))
        {
            Console.WriteLine("Enter your journal entry:");
            string entryText = Console.ReadLine();

            Console.WriteLine("Enter location:");
            string location = Console.ReadLine();

            Console.WriteLine("Enter weather conditions:");
            string weather = Console.ReadLine();

            Console.WriteLine("Enter your mood:");
            string mood = Console.ReadLine();

            Console.WriteLine("Enter tags (comma-separated):");
            string tagsInput = Console.ReadLine();
            List<string> tags = new List<string>(tagsInput.Split(','));

            journal.AddEntry(date, entryText, location, weather, mood, tags);
        }
        else
        {
            Console.WriteLine("Invalid date format. Please use YYYY-MM-DD.");
        }

        DisplayMenu();
    }
}
