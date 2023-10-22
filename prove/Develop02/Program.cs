using System;
using System.IO;
using System.Collections.Generic;

class JournalEntry
{
    public string Text { get; set; }
    public DateTime Date { get; set; }
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
                    if (parts.Length == 2)
                    {
                        if (DateTime.TryParse(parts[0], out DateTime date))
                        {
                            Entries.Add(new JournalEntry { Date = date, Text = parts[1] });
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
                    writer.WriteLine($"{entry.Date:s},{entry.Text}");
                }
            }

            Console.WriteLine("Journal entries saved successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while saving journal entries: {ex.Message}");
        }
    }

    public void AddEntry(DateTime date, string entryText)
    {
        Entries.Add(new JournalEntry { Date = date, Text = entryText });
        Console.WriteLine("Journal entry added successfully.");
    }

    public void DisplayEntries()
    {
        Console.WriteLine("Journal Entries:");
        foreach (var entry in Entries)
        {
            Console.WriteLine($"{entry.Date.ToString("yyyy-MM-dd")}: {entry.Text}");
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
            string prompt = PromptGenerator.GetRandomPrompt();
            Console.WriteLine(prompt);
            Console.WriteLine("Enter your journal entry:");
            string entryText = Console.ReadLine();
            journal.AddEntry(date, entryText);
        }
        else
        {
            Console.WriteLine("Invalid date format. Please use YYYY-MM-DD.");
        }
    }
}
