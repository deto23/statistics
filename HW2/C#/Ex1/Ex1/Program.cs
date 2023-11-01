using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;

class Program
{
    static void Main()
    {
        string fileName = "dataset.tsv";
        var data = new List<Dictionary<string, string>>();

        string[] lines = File.ReadAllLines(fileName);

        if (lines.Length > 0)
        {
            string[] keys = lines[0].Split('\t');

            for (int i = 1; i < lines.Length; i++)
            {
                string[] values = lines[i].Split('\t');
                var row = new Dictionary<string, string>();

                if (keys.Length == values.Length)
                {
                    for (int j = 0; j < keys.Length; j++)
                    {
                        string key = keys[j];
                        string value = values[j].ToLower();

                        row[key] = value;
                    }

                    data.Add(row);
                }
            }
        }

        const String qualitativeVariable = "Main hobbies";
        const String quantitativeDiscreteVariable = "Hard Worker (0-5)";
        const String quantitativeContinuousVariable = "weight";

        int totalFrequencyQualitativeVariable = 0;
        int totalFrequencyQuantitativeDiscreteVariable = 0;
        int totalFrequencyQuantitativeContinuousVariable = 0;

        var valuesCountsQualitativeVariable = new Dictionary<string, int>();

        var valuesCountQuantitativeDiscreteVariable = new Dictionary<string, int>();

        var valuesCountQuantitativeContinuousVariable = new Dictionary<double, int>();

        int numIntervals = 3;
        double max = 0;
        double min = 0;
        double intervalWidth = 0;

        foreach (var row in data)
        {
            foreach (var kvp in row)
            {
                if (kvp.Key == qualitativeVariable)
                {
                    if (valuesCountsQualitativeVariable.ContainsKey(kvp.Value))
                    {
                        valuesCountsQualitativeVariable[kvp.Value] += 1;
                    }
                    else
                    {
                        valuesCountsQualitativeVariable[kvp.Value] = 1;
                    }
                    totalFrequencyQualitativeVariable += 1;
                }
                if (kvp.Key == quantitativeDiscreteVariable)
                {

                    if (valuesCountQuantitativeDiscreteVariable.ContainsKey(kvp.Value))
                    {
                        valuesCountQuantitativeDiscreteVariable[kvp.Value] += 1;
                    }
                    else
                    {
                        valuesCountQuantitativeDiscreteVariable[kvp.Value] = 1;
                    }
                    totalFrequencyQuantitativeDiscreteVariable += 1;
                }
                if (kvp.Key == quantitativeContinuousVariable)
                {
                    string valueString = kvp.Value;
                    double value;
                    if (double.TryParse(valueString, out value))
                    {
                        Console.WriteLine("Conversione riuscita");
                    }
                    else
                    {
                        Console.WriteLine("Conversione fallita. Utilizzo del valore predefinito: " + value);
                    }

                    if (value > max)
                    {
                        max = value;
                    }
                    if (value <= min)
                    {
                        min = value;
                    }

                }
            }
        }
        foreach (var row in data)
        {
            foreach (var kvp in row)
            {
                if (kvp.Key == quantitativeContinuousVariable)
                {
                    intervalWidth = (max - min) / numIntervals;

                    for (int i = 0; i < numIntervals; i++)
                    {
                        double intervalStart = min + i * intervalWidth;
                        double intervalEnd = intervalStart + intervalWidth;

                        string valueString = kvp.Value;
                        double value;
                        if (double.TryParse(valueString, out value))
                        {
                            Console.WriteLine("Conversione riuscita");
                        }
                        else
                        {
                            Console.WriteLine("Conversione fallita. Utilizzo del valore predefinito: " + value);
                        }

                        if (value >= intervalStart && value < intervalEnd)

                        {
                            if (valuesCountQuantitativeContinuousVariable.ContainsKey(intervalStart))
                            {
                                valuesCountQuantitativeContinuousVariable[intervalStart] += 1;
                            }
                            else
                            {
                                valuesCountQuantitativeContinuousVariable[intervalStart] = 1;
                            }
                        }
                    }
                    totalFrequencyQuantitativeContinuousVariable += 1;

                }

            }
        }

        Console.WriteLine("Frequency Distribution of Qualitative Variable");

        var sortedValuesCountsQualitativeVariable = valuesCountsQualitativeVariable.OrderBy(kvp => kvp.Key).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

        foreach (var kvp in sortedValuesCountsQualitativeVariable)
        {
            Console.WriteLine($"Value: {kvp.Key}, Frequency: {kvp.Value}");
        }

        Console.WriteLine("Press ENTER to view the next...");
        Console.ReadLine();

        Console.WriteLine("Frequency Distribution of Quantitative Discrete Variable");

        foreach (var kvp in valuesCountQuantitativeDiscreteVariable)
        {
            Console.WriteLine($"Value: {kvp.Key}, Frequency: {kvp.Value}");
        }

        Console.WriteLine("Press ENTER to view the next...");
        Console.ReadLine();

        Console.WriteLine("Frequency Distribution of Quantitative Continuous Variable");

        var sortedValuesCountQuantitativeContinuousVariable = valuesCountQuantitativeContinuousVariable.OrderBy(kvp => kvp.Key).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

        foreach (var kvp in sortedValuesCountQuantitativeContinuousVariable)
        {

            Console.WriteLine($"Range: {kvp.Key}, Frequency: {kvp.Value}");
        }

        Console.WriteLine("Press ENTER to view the next...");
        Console.ReadLine();


        var jointDistributionMatrix = CalculateJointDistributionMatrix("Hard Worker (0-5)", "Age", data);
        Console.WriteLine("Joint Distribution Matrix:");
        Console.Write("        ");
        foreach (var column2 in jointDistributionMatrix.First().Value)
        {
            Console.Write($"{column2.Key,-10}");
        }
        Console.WriteLine();

        foreach (var kvp in jointDistributionMatrix)
        {
            Console.Write($"{kvp.Key,-10}");
            foreach (var count in kvp.Value)
            {
                Console.Write($"{count.Value,-10}");
            }
            Console.WriteLine();
        }

        Console.WriteLine("Press ENTER to exit...");
        Console.ReadLine(); // Attendere l'input dell'utente prima di chiudersi
    }

    static Dictionary<string, Dictionary<string, int>> CalculateJointDistributionMatrix(string columnName1, string columnName2, List<Dictionary<string, string>> dataSet)
    {
        // Estrai gli indici distinti per columnName1 e columnName2
        var index1 = dataSet.Select(item => item[columnName1]).Distinct().ToList();
        var index2 = dataSet.Select(item => item[columnName2]).Distinct().ToList();

        // Inizializza la matrice con zeri
        var matrix = new Dictionary<string, Dictionary<string, int>>();

        foreach (var value1 in index1)
        {
            var counts = new Dictionary<string, int>();
            foreach (var value2 in index2)
            {
                counts[value2] = 0;
            }
            matrix[value1] = counts;
        }

        // Popola la matrice conteggiando le occorrenze
        foreach (var data in dataSet)
        {
            string value1 = data[columnName1];
            string value2 = data[columnName2];

            matrix[value1][value2]++;
        }

        return matrix;
    }
}




