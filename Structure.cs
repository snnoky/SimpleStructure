using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zadanie1_2MIW
{
    public class Structure
    {
        public static List<Edge> Edges = new List<Edge>();
        public static Dictionary<int, List<Node>> structure = new Dictionary<int, List<Node>>();
        public static bool FromFile = false;
        public static int edgesInStructure = 0;
        public static List<string> userStructure = new List<string>();

        public static void Menu()
        {
            //Console.WriteLine("Podaj ilość kolumn: ");
            //var cols = Convert.ToInt32(Console.ReadLine());
            //List<int> rows = new List<int>();
            //for (int i = 0; i < cols; i++)
            //{
            //    Console.WriteLine($"Podaj ilość wierszy w kolumnie {i}:");
            //    rows.Add(Convert.ToInt32(Console.ReadLine()));
            //}
            
            Console.WriteLine("Podaj strukturę (x-y-z)");
            var input = Console.ReadLine();
            userStructure.Add(input);
            var structureItems = new List<int>();
            var strings = new List<string>();
            List<int> rows = new List<int>();
            var cols = 0;
            foreach (var items in userStructure)
            {
                strings = items.Split("-").ToList();
            }

            for (int i = 0; i < strings.Count; i++)
            {
                structureItems.Add(Convert.ToInt32(strings[i]));
            }

            cols = structureItems.Count;
            for (int i = 0; i < cols; i++)
            {
                rows.Add(structureItems[i]);
            }
            Console.WriteLine("Czy wczytać z pliku? T/t-Tak inne-Nie");
            var czyZPliku = Console.ReadLine();
            if (czyZPliku == "t" || czyZPliku == "T")
            {
                if (File.Exists("../../../ZapisanaStruktura.txt"))
                {
                    var listFromFile = ReadStructure();
                    if (userStructure[0] == listFromFile[0])
                    {
                        var edgesFromFile = ReadWeights(cols, rows);
                        var structureCheck = CreateStructure(cols, rows);
                        var checkEdges = 0;
                        for (int i = 0; i < structureCheck.Count - 1; i++)
                        {
                            checkEdges += (rows[i] * rows[i + 1]) + rows[i + 1];
                        }
                        if (checkEdges == edgesFromFile.Count-1)
                        {
                            //ReadEdges(edgesFromFile);
                            //foreach (var edge in edgesFromFile)
                            //{
                            //    Console.WriteLine(edge);
                            //}

                            for (int i = 1; i < edgesFromFile.Count; i++)
                            {
                                Console.WriteLine(edgesFromFile[i]);
                            }
                        }
                        else
                        {
                            Edges = new List<Edge>();
                            CreateStructure(cols, rows);
                            CreateEdges(rows);
                            SaveStructure(rows);
                            foreach (var edge in Edges)
                            {
                                Console.WriteLine(edge.Weight);
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("W pliku była inna struktura tworzenie nowej podanej na początku.");
                        Edges = new List<Edge>();
                        CreateStructure(cols, rows);
                        CreateEdges(rows);
                        SaveStructure(rows);
                        foreach (var edge in Edges)
                        {
                            Console.WriteLine(edge.Weight);
                        }
                    }

                }
                else
                {
                    Console.WriteLine("Plik nie istnieje, stworzono nowy o podanej strukturze.");
                    CreateStructure(cols, rows);
                    CreateEdges(rows);
                    SaveStructure(rows);
                }
            }
            else
            {
                CreateStructure(cols, rows);
                CreateEdges(rows);
                SaveStructure(rows);
            }
        }

        public static Dictionary<int, List<Node>> CreateStructure(int cols, List<int> rows)
        {

            for (int i = 0; i < cols; i++)
            {
                structure[i] = new List<Node>();

                for (int j = 0; j < rows[i]; j++)
                {
                    var node = new Node(null);
                    structure[i].Add(node);
                }
            }

            return structure;
        }

        public static void CreateEdges(List<int> rows)
        {
            //krawędzie bez biasów
            for (int i = 0; i < structure.Count - 1; i++)
            {
                for (int j = 0; j < structure[i].Count; j++)
                {
                    for (int l = 0; l < structure[i + 1].Count; l++)
                    {
                        var randomWeight = new Random();
                        var edge = new Edge(structure[i][j], structure[i + 1][l], randomWeight.NextDouble());
                        Edges.Add(edge);
                    }
                }
            }
            //Bias
            for (int i = 1; i <= structure.Count - 1; i++)
            {
                for (int j = 0; j < structure[i].Count; j++)
                {
                    var randomWeight = new Random();
                    var edge = new Edge(structure[i][j], null, randomWeight.NextDouble());
                    Edges.Add(edge);
                }
            }
        }

        //public static void ReadEdges(List<string> fileValues)
        //{
        //    int counter = 0;
        //    for (int i = 0; i < structure.Count - 1; i++)
        //    {
        //        for (int j = 0; j < structure[i].Count; j++)
        //        {
        //            for (int l = 0; l < structure[i + 1].Count; l++)
        //            {
        //                var edge = new Edge(structure[i][j], structure[i + 1][l], Convert.ToDouble(fileValues[l]));
        //                Edges.Add(edge);
        //                counter++;
        //            }
        //        }
        //    }

        //    for (int i = 1; i <= structure.Count - 1; i++)
        //    {
        //        for (int j = 0; j < structure[i].Count; j++)
        //        {
        //            var randomWeight = new Random();
        //            var edge = new Edge(structure[i][j], null, Convert.ToDouble(fileValues[counter]));
        //            Edges.Add(edge);
        //        }
        //    }
        //}

        public static void SaveStructure(List<int> rows)
        {
            List<double> result = new List<double>();
            foreach (var edge in Edges)
            {
                result.Add(edge.Weight);
            }

            TextWriter tw = new StreamWriter("../../../ZapisanaStruktura.txt");
            //tw.WriteLine($"{rows[0]}-{rows[1]}-{rows[2]}");
            tw.WriteLine(string.Format("{0}", string.Join("-", rows)));
            foreach (var item in result)
            {
                tw.WriteLine(item);
            }
            tw.Close();
        }

        public static List<string> ReadWeights(int cols, List<int> rows)
        {
            FromFile = true;
            var path = "../../../ZapisanaStruktura.txt";

            var values = File.ReadAllLines(path).ToList();

            for (var i = 0; i < rows.Count - 1; i++)
            {
                edgesInStructure += (rows[i] * rows[i + 1]) + rows[i + 1];
            }

            if (edgesInStructure != values.Count-1)
            {
                Console.WriteLine("Niezgodna ilość krawędzi!");
                FromFile = false;
                Console.WriteLine("Tworzenie nowej struktury o podanych parametrach.");
            }

            return values;
        }
        public static List<string> ReadStructure()
        {
            FromFile = true;
            var newList = new List<string>();
            var path = "../../../ZapisanaStruktura.txt";

            var structureFromFile = File.ReadLines(path).First();
            newList.Add(structureFromFile);
            return newList;
        }
    }

}

