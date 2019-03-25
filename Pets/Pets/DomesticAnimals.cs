using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Pets
{
    class DomesticAnimals
    {
        public static Dictionary<string, HashSet<string>> quantityOfPetsOfEverySpecies = new Dictionary<string, HashSet<string>>();
        public static Dictionary<string, Tuple<HashSet<string>, HashSet<string>>> speciesInfo = new Dictionary<string, Tuple<HashSet<string>, HashSet<string>>>();
        public static Dictionary<string, HashSet<string>> petNameInfo = new Dictionary<string, HashSet<string>>();
        public static Dictionary<string, SortedSet<int>> agesInfo = new Dictionary<string, SortedSet<int>>();

        public static void AddToQOPOESDict(string ownerName, string petSpecies)
        {
            if (!quantityOfPetsOfEverySpecies.ContainsKey(ownerName))
                quantityOfPetsOfEverySpecies.Add(ownerName, new HashSet<string>());
            quantityOfPetsOfEverySpecies[ownerName].Add(petSpecies);
        }

        public static void AddToSIDict(string petSpecies, string ownerName, string petName)
        {
            if (!speciesInfo.ContainsKey(petSpecies))
                speciesInfo.Add(petSpecies, Tuple.Create(new HashSet<string>(), new HashSet<string>()));
            speciesInfo[petSpecies].Item1.Add(ownerName);
            speciesInfo[petSpecies].Item2.Add(petName);
        }

        public static void AddToPNIDict(string petName, string petSpecies)
        {
            if (!petNameInfo.ContainsKey(petName))
                petNameInfo.Add(petName, new HashSet<string>());
            petNameInfo[petName].Add(petSpecies);
        }

        public static void AddToAIDict(string petSpecies, int age)
        {
            if (!agesInfo.ContainsKey(petSpecies))
                agesInfo.Add(petSpecies, new SortedSet<int>());
            agesInfo[petSpecies].Add(age);
        }
        public static void PetsQuantity()
        {
            foreach (KeyValuePair<string, HashSet<string>> ownerInfo in quantityOfPetsOfEverySpecies)
            {
                Console.WriteLine("{0} has such pet species:", ownerInfo.Key);
                foreach (string species in ownerInfo.Value)
                    Console.WriteLine(species);
            }
        }

        public static void OwnerAndNamesInfo(string species)
        {
            if (!speciesInfo.ContainsKey(species))
                Console.WriteLine("There is no {0} in the database!", species);
            else
            {
                Console.WriteLine("Owners:");
                foreach (string owner in speciesInfo[species].Item1)
                    Console.WriteLine(owner);
                Console.WriteLine("Names:");
                bool nameless = true;
                foreach (string names in speciesInfo[species].Item2)
                    if (names != "")
                    {
                        Console.WriteLine(names);
                        nameless = false;
                    }
                if (nameless)
                    Console.WriteLine("All pets are nameless");
            }
        }

        public static void HowMuchSpeciesHaveThisName(string name)
        {
            if (!petNameInfo.ContainsKey(name))
                Console.WriteLine("There aren't any pets with this name in the database!");
            else
            {
                Console.WriteLine("Species which present such name:");
                foreach (string species in petNameInfo[name])
                    Console.WriteLine(species);
            }
        }

        public static void PetAgeInfo()
        {
            foreach (KeyValuePair<string, SortedSet<int>> speciesAndAges in agesInfo)
            {
                if (speciesAndAges.Value.Min() == speciesAndAges.Value.Max())
                    Console.WriteLine("For species {0} min and max age are equal to {1} (in years)",
                      speciesAndAges.Key, speciesAndAges.Value.Min());
                else
                    Console.WriteLine("For species {0} min and max age are {1} and {2} accordingly",
                      speciesAndAges.Key, speciesAndAges.Value.Min(), speciesAndAges.Value.Max());
            }
        }

        public static void Menu()
        {
            Console.WriteLine("To get information about every owners pets species press 1.");
            Console.WriteLine("Press 2 and enter the species name to get information about every owners and names.");
            Console.WriteLine("Press 3 and enter the name to get information about pets with such name.");
            Console.WriteLine("Press 4 to get information about min and max age of every species.");
            Console.WriteLine("Press 0 to exit the programm.");

            while (true)
            {
                ConsoleKeyInfo action = Console.ReadKey();
                Console.WriteLine();

                if (action.Key == ConsoleKey.D1)
                    PetsQuantity();
                else if (action.Key == ConsoleKey.D2)
                {
                    Console.WriteLine("Enter species name: ");
                    string species = Console.ReadLine();
                    OwnerAndNamesInfo(species);
                }
                else if (action.Key == ConsoleKey.D3)
                {
                    Console.WriteLine("Enter pet name: ");
                    string name = Console.ReadLine();
                    HowMuchSpeciesHaveThisName(name);
                }
                else if (action.Key == ConsoleKey.D4)
                    PetAgeInfo();
                else if (action.Key == ConsoleKey.D0)
                    break;
                else
                    Console.WriteLine("Incorrect input!");
            }
        }

        static void Main()
        {
            try
            {
                FileStream input = File.OpenRead("input.txt");

                string petsInfo;

                byte[] array = new byte[input.Length];
                input.Read(array, 0, array.Length);
                input.Close();
                petsInfo = Encoding.Default.GetString(array);

                string[] petInfoLines = petsInfo.Split('\n');

                foreach (string line in petInfoLines)
                {
                    if (line != "\n" || line != " ")
                    {
                        string[] infos = line.Split(',');
                        string ownerName = infos[0];
                        string petSpecies = infos[1];
                        string petName = infos[2];
                        int age = Convert.ToInt32(infos[3]);

                        AddToQOPOESDict(ownerName, petSpecies);
                        AddToSIDict(petSpecies, ownerName, petName);
                        AddToPNIDict(petName, petSpecies);
                        AddToAIDict(petSpecies, age);
                    }
                }

                Menu();
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex);
            }

            Console.ReadKey();
        }
    }
}
