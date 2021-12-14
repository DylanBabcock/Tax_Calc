using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Part2;
namespace Part1
{
    static class TaxCalculator
    {
        static public bool VerboseBool;
        static public bool checkEmpRecordBool;// This boolean is used to disable the employee list while processing the user input

        // Create a static dictionary field that holds a List of TaxRecords and is keyed by a string
        static public Dictionary<string, List<TaxRecord>> taxDictionary = new Dictionary<string, List<TaxRecord>>();

        // create a static constructor that:
        static TaxCalculator()
        {
            int lineNumber = 0;
            // declare a streamreader to read a file
            StreamReader sr = new StreamReader(@"D:\Woz\ADN102.Starter.10.FinalProject\Part1\taxtable.csv");
            try
            {
                // initialize the static dictionary to a newly create empty one
                taxDictionary = new Dictionary<string, List<TaxRecord>>();

                using (sr)
                {
                    string currentLine;

                    // currentLine will be null when the StreamReader reaches the end of file
                    while ((currentLine = sr.ReadLine()) != null)
                    {
                        try
                        {
                            TaxRecord tx = new TaxRecord(currentLine, lineNumber);
                            if (taxDictionary.ContainsKey(tx.StateCode))//check if state code is in dictionary
                            {
                                taxDictionary[tx.StateCode].Add(tx);
                            }
                            else
                            {
                                taxDictionary.Add(tx.StateCode, new List<TaxRecord>());
                                taxDictionary[tx.StateCode].Add(tx);
                            }
                            lineNumber++;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error: " + ex + "\n\n");
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex + "\n\n");
            }


        }

        public static double ComputeTaxFor(string sttCode, double income, bool silent, bool employeeBool)
        {
            double computedTax = 0;//total tax variable
            List<TaxRecord> stateRecords = new List<TaxRecord>();// stateRecords holds the list of tax records associated with the users state code

            if (taxDictionary.ContainsKey(sttCode) == false)//if the code is not in our collection
            {
                throw new Exception($"State not in our dictionary {sttCode}");
            }
            else
            {
                stateRecords = taxDictionary[sttCode];

                for (int i = 0; i < stateRecords.Count; i++)
                {
                    if (income > stateRecords[i].Ceiling)//if income is NOT within tax bracket
                    {
                        computedTax += (stateRecords[i].Ceiling - stateRecords[i].Floor) * stateRecords[i].Rate;//main calculation
                        if (silent == false && employeeBool == true)//If silent mode is off
                        {
                            Console.WriteLine("--------------------------------------\nCalculating: ...\n");
                            Console.WriteLine($" Total so far - {computedTax.ToString("0.00")} - after taking the Ceiling - {stateRecords[i].Ceiling.ToString("0.00")} - and subtracting it by" +
                            $" the floor - {stateRecords[i].Floor.ToString("0.00")} - then finally multiplying that by the rate - {stateRecords[i].Rate.ToString("0.00")}\n");
                        }

                    }
                    else
                    {
                        computedTax += (income - stateRecords[i].Floor) * stateRecords[i].Rate;
                        if (silent == false && employeeBool == true)//If silent mode is off
                        {
                            Console.WriteLine($" Total - {computedTax.ToString("0.00")} - after taking the user income - {income.ToString("0.00")} - and subtracting it by" +
                                $" the floor - {stateRecords[i].Floor.ToString("0.00")} - then finally multiplying that by the rate - {stateRecords[i].Rate.ToString("0.00")}\n");
                            Console.WriteLine($"Tax Bracket: \n{stateRecords[i]}\n");

                        }
                        return computedTax;
                    }
                }

            }



            return 0;
        }


    }  // this is the end of the Tax Calculator


    class TaxRecord
    {

        //  StateCode   (used as the key to the dictionary)
        public string StateCode;
        //  State       (Full state name)
        public string StateName;
        //  Floor       (lowest income for this tax bracket)
        public double Floor;
        //  Ceiling     (highest income for this tax bracket )
        public double Ceiling;
        //  Rate        (Rate at which income is taxed for this tax bracket)
        public double Rate;
        //
        //  Create a ctor taking a single string (a csv) and use it to load the record

        public TaxRecord(string csv, int lineNumber)
        {


            string[] lineArr = csv.Split(',');
            if (lineArr.Length == 5)
            {
                if (lineArr[0].Length > 2)//if the state code is larger than 2 then throw an error
                {
                    throw new Exception($"State code too large in line {lineNumber + 1}");
                }
                if ((lineArr[1] is string) == false)// if statename is a number
                {
                    throw new Exception($"Error: not a statename in line {lineNumber + 1}");
                }
                if (!double.TryParse(lineArr[2], out Floor))//These next statements check to see if the file contains numbers in the correct collumns
                {
                    throw new Exception($"Error: not a number in line {lineNumber + 1}");
                }
                if (!double.TryParse(lineArr[3], out Ceiling))
                {
                    throw new Exception($"Error: expected number in line {lineNumber + 1}");
                }
                if (!double.TryParse(lineArr[4], out Rate))
                {
                    throw new Exception($"Error: expected number in line {lineNumber + 1}");
                }

                StateCode = lineArr[0];
                StateName = lineArr[1];
                Floor = double.Parse(lineArr[2]);
                Ceiling = double.Parse(lineArr[3]);
                Rate = double.Parse(lineArr[4]);


            }



            else
            {
                throw new Exception($"line number {lineNumber + 1} Not the correct length");
            }


        }
        public override string ToString()//decimalVar.ToString("#.##");
        {
            return $"\n Floor: {Floor.ToString("0.00")} | Ceiling: {Ceiling.ToString("0.00")} | Rate: {Rate.ToString("0.00")}";
        }


    }  // this is the end of the TaxRecord

    class Program
    {
        public static void Main()
        {


            do
            {
                try
                {
                    bool silent = false;//silent mode or default boolean
                   
                    Console.Write(@"                   





 ______   ______     __  __        ______     ______     __         ______     __  __     __         ______     ______   ______     ______    
/\__  _\ /\  __ \   /\_\_\_\      /\  ___\   /\  __ \   /\ \       /\  ___\   /\ \/\ \   /\ \       /\  __ \   /\__  _\ /\  __ \   /\  == \   
\/_/\ \/ \ \  __ \  \/_/\_\/_     \ \ \____  \ \  __ \  \ \ \____  \ \ \____  \ \ \_\ \  \ \ \____  \ \  __ \  \/_/\ \/ \ \ \/\ \  \ \  __<   
   \ \_\  \ \_\ \_\   /\_\/\_\     \ \_____\  \ \_\ \_\  \ \_____\  \ \_____\  \ \_____\  \ \_____\  \ \_\ \_\    \ \_\  \ \_____\  \ \_\ \_\ 
    \/_/   \/_/\/_/   \/_/\/_/      \/_____/   \/_/\/_/   \/_____/   \/_____/   \/_____/   \/_____/   \/_/\/_/     \/_/   \/_____/   \/_/ /_/ 
                                                                                                                                              




        
Enter your income below to start!
" + "\n>");
                    double income = int.Parse(Console.ReadLine());
                    Console.WriteLine("\nEnter your state code\n>");
                    string state = "";
                    while (true)
                    {
                        state = Console.ReadLine();
                        if (int.TryParse(state, out int j) || state.Length != 2)//if the state entered by user is a number
                        {
                            Console.WriteLine("Error: Please enter the name of the state code in which you need to calculate\n");
                            continue;
                        }
                        break;
                    }
                    Console.WriteLine("Would you like the detailed message or just the calculated tax for your state entered? Verbose or Silent\n>");

                    string silentVerboseString = "";
                    while (true)//checking if user entered Verbose or Silent
                    {
                        silentVerboseString = Console.ReadLine();
                        Console.Clear();
                        Console.WriteLine(@"
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * 
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * 
");
                        if (silentVerboseString.Equals("Verbose"))
                        {
                            silent = false;
                            TaxCalculator.VerboseBool = false;
                            break;
                        }
                        else if (silentVerboseString.Equals("Silent"))
                        {
                            silent = true;
                            TaxCalculator.VerboseBool = true;
                            break;
                        }
                        else
                        {
                            Console.WriteLine("\nPlease enter either Silent or Verbose\n");
                        }
                    }


                    Console.WriteLine("\n\n" +
                        "--------------------------------------------------" +
                        "\nTotal Tax due: $" + TaxCalculator.ComputeTaxFor(state, income, silent, true) +
                        "\n-------------------------------------------------- \n\n");
                    Part2.Program.Main();

                    Console.WriteLine("\n Press Enter to go again >");
                    Console.ReadLine();
                    Console.Clear();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            } while (true);


        }
    }
}


