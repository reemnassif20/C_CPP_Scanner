using System;
using System.Collections.Generic;
using System.Linq;

class Parser
{
    static string input;
    static int position = 0;
    static Dictionary<string, List<string>> grammar = new Dictionary<string, List<string>>();

    static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("Recursive Descent Parsing For following grammar");
            Console.WriteLine("\t ***************** Grammars ***************** ");

            // Get grammar rules
            grammar.Clear();
            grammar["S"] = new List<string>();
            grammar["B"] = new List<string>();

            // Get rules for each non-terminal
            for (int i = 1; i <= 2; i++)
            {
                Console.Write($"Enter rule number {i} for non-terminal 'S': ");
                grammar["S"].Add(Console.ReadLine());
            }
            for (int i = 1; i <= 2; i++)
            {
                Console.Write($"Enter rule number {i} for non-terminal 'B': ");
                grammar["B"].Add(Console.ReadLine());
            }

            // Check if grammar is simple
            if (!IsGrammarSimple())
            {
                Console.WriteLine("The Grammar isn't Simple.");
                Console.WriteLine("Try again");
                continue;
            }

            // Get and process first input
            GetInput();

            while (true)
            {
                Console.WriteLine("==========================================");
                Console.WriteLine("1-Another Grammar.");
                Console.WriteLine("2-Another String.");
                Console.WriteLine("3-Exit");
                Console.Write("Enter ur choice : ");
                string choice = Console.ReadLine();

                if (choice == "1") break;
                if (choice == "3") return;
                if (choice == "2")
                {
                    GetInput();
                }
            }
        }
    }

    static bool IsGrammarSimple()
    {
        foreach (var nonTerminal in grammar)
        {
            // Check if every rule starts with a terminal
            foreach (string rule in nonTerminal.Value)
            {
                if (rule.Length == 0 || char.IsUpper(rule[0]))
                    return false;
            }

            // Check if rules for same non-terminal begin with different terminals
            var firstChars = nonTerminal.Value.Select(rule => rule[0]).ToList();
            if (firstChars.Distinct().Count() != firstChars.Count)
                return false;
        }
        return true;
    }

    static void GetInput()
    {
        Console.Write("Enter the string want to be checked : ");
        input = Console.ReadLine();
        position = 0;

        Console.WriteLine($"The input String: [{string.Join("', '", input.ToCharArray())}]");

        bool accepted = ParseNonTerminal("S");

        if (accepted && position == input.Length)
        {
            Console.WriteLine("Stack after checking: []");
            Console.WriteLine("The rest of unchecked string: []");
            Console.WriteLine("Your input String is Accepted.");
        }
        else
        {
            string remaining = position < input.Length ? $"['{input[position]}']" : "[]";
            Console.WriteLine($"Stack after checking: {remaining}");
            Console.WriteLine("The rest of unchecked string: []");
            Console.WriteLine("Your input String is Rejected.");
        }
    }

    static bool ParseNonTerminal(string nonTerminal)
    {
        if (position >= input.Length) return false;

        foreach (string rule in grammar[nonTerminal])
        {
            int savedPosition = position;
            if (TryRule(rule))
            {
                return true;
            }
            position = savedPosition;
        }

        return false;
    }

    static bool TryRule(string rule)
    {
        int originalPosition = position;

        foreach (char symbol in rule)
        {
            if (position >= input.Length)
            {
                position = originalPosition;
                return false;
            }

            if (char.IsUpper(symbol))  // Non-terminal
            {
                if (!ParseNonTerminal(symbol.ToString()))
                {
                    position = originalPosition;
                    return false;
                }
            }
            else  // Terminal
            {
                if (input[position] != symbol)
                {
                    position = originalPosition;
                    return false;
                }
                position++;
            }
        }

        return true;
    }
}