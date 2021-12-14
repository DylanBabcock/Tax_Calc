using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Part1;

namespace Part2
{


    class EmployeeRecord
    {
        // create an employee Record with public properties
        //    ID                        a number to identify an employee
        public int empID;
        //    Name                      the employee name
        public string empName;
        //    StateCode                 the state collecting taxes for this employee
        public string empStateCode;
        //    HoursWorkedInTheYear      the total number of hours worked in the entire year (including fractions of an hour)
        public double empHoursInYear;
        //    HourlyRate                the rate the employee is paid for each hour worked
        public double empHourlyWage;
        //                                  
        public double yearlyPay;

        public double yearlyTax
        {
            get
            {
                try
                {
                    return TaxCalculator.ComputeTaxFor(empStateCode, yearlyPay, TaxCalculator.VerboseBool, TaxCalculator.checkEmpRecordBool);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e.Message);
                    return 0;
                }
            }
        }

        public EmployeeRecord(string csv, int lineNumber)
        {


            string[] lineArr = csv.Split(',');
            if (lineArr.Length == 5)
            {
                if (int.TryParse(lineArr[0], out int j) == false)//if the line is a number
                {
                    throw new Exception($"Error: Not a valid ID in line {lineNumber + 1}");
                }
                if ((lineArr[1] is string) == false)// if name is a number
                {
                    throw new Exception($"Error: not a name in line {lineNumber + 1}");
                }
                if ((lineArr[2] is string) == false)//checking to see if the 3 element in the line is the state code
                {
                    throw new Exception($"Error: not a valid statecode in line {lineNumber + 1}");
                }
                if (lineArr[2].Length > 2)// if state code is more than 2 characters
                {
                    throw new Exception($"Error: not a valid statecode in line {lineNumber + 1}");
                }
                if (!double.TryParse(lineArr[3], out double k))//next two statements check if the last 2 collumns are numbers. 
                {
                    throw new Exception($"Error: expected number in line {lineNumber + 1}");
                }
                if (!double.TryParse(lineArr[4], out k))
                {
                    throw new Exception($"Error: expected number in line {lineNumber + 1}");
                }

                empID = int.Parse(lineArr[0]);
                empName = lineArr[1];
                empStateCode = lineArr[2];
                empHoursInYear = double.Parse(lineArr[3]);
                empHourlyWage = double.Parse(lineArr[4]);
                yearlyPay = empHourlyWage * empHoursInYear;

            }
        }
        public override string ToString()
        {
            return $"\n\nEmployee Name: {empName} | Employee ID: {empID} | Hours in year: {empHoursInYear.ToString("0.00")} | Hourly Wage: {empHourlyWage.ToString("0.00")} | Yearly Pay: {yearlyPay.ToString("0.00")} | Yearly Tax: {yearlyTax.ToString("0.00")}\n--------------------------------------\n\n\n";
        }
    }

    static class EmployeesList
    {

        public static List<EmployeeRecord> empList = new List<EmployeeRecord>();

        static EmployeesList()
        {
            int lineNumber = 0;
            // declare a streamreader to read a file
            StreamReader sr = new StreamReader(@"D:\Woz\ADN102.Starter.10.FinalProject\Part1\employees.csv");
            try
            {
                empList = new List<EmployeeRecord>();
                using (sr)
                {
                    string currentLine;

                    while ((currentLine = sr.ReadLine()) != null)
                    {
                        try
                        {
                            EmployeeRecord emp = new EmployeeRecord(currentLine, lineNumber);

                            empList.Add(emp);

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
    }






    class Program
    {

        public static void printList(List<Part2.EmployeeRecord> myList)
        {
            for (int i = 0; i < EmployeesList.empList.Count; i++)
            {
                Console.WriteLine($"{ myList[i]}");
            }
        }

        public static void Main()
        {

            try
            {

                Console.Write("Press Enter to continue >>> ");
                Console.ReadLine();
                Console.Clear();
                Console.Write(@"


             ______     __    __     ______   __         ______     __  __     ______     ______        ______     ______     ______     ______     ______     _____     ______    
            /\  ___\   /\ '-./  \   /\  == \ /\ \       /\  __ \   /\ \_\ \   /\  ___\   /\  ___\      /\  == \   /\  ___\   /\  ___\   /\  __ \   /\  == \   /\  __-.  /\  ___\   
            \ \  __\   \ \ \-./\ \  \ \  _-/ \ \ \____  \ \ \/\ \  \ \____ \  \ \  __\   \ \  __\      \ \  __<   \ \  __\   \ \ \____  \ \ \/\ \  \ \  __<   \ \ \/\ \ \ \___  \  
             \ \_____\  \ \_\ \ \_\  \ \_\    \ \_____\  \ \_____\  \/\_____\  \ \_____\  \ \_____\     \ \_\ \_\  \ \_____\  \ \_____\  \ \_____\  \ \_\ \_\  \ \____-  \/\_____\ 
              \/_____/   \/_/  \/_/   \/_/     \/_____/   \/_____/   \/_____/   \/_____/   \/_____/      \/_/ /_/   \/_____/   \/_____/   \/_____/   \/_/ /_/   \/____/   \/_____/ 
                                                                                                                                                                       


Sort Employees by | (1) Name | (2) State | (3) ID | (4) Yearly Pay | (5) Tax due 
" + "\n>");
                string pinput = Console.ReadLine();
                Console.Clear();
                TaxCalculator.checkEmpRecordBool = false;
                List<EmployeeRecord> empListName = new List<EmployeeRecord>();// these lists are for sorting the employee records
                List<EmployeeRecord> empStateCodeL = new List<EmployeeRecord>();
                List<EmployeeRecord> empIDL = new List<EmployeeRecord>();
                List<EmployeeRecord> empYearlyPayL = new List<EmployeeRecord>();
                List<EmployeeRecord> empTaxdueL = new List<EmployeeRecord>();
                empListName = EmployeesList.empList;
                empStateCodeL = EmployeesList.empList;
                empIDL = EmployeesList.empList;
                empYearlyPayL = EmployeesList.empList;
                empTaxdueL = EmployeesList.empList;
                empListName = empListName.OrderBy(x => x.empName).ToList();//list of employee records sorted by name
                empStateCodeL = empStateCodeL.OrderBy(x => x.empStateCode).ToList();//list of employee records sorted by state
                empIDL = empIDL.OrderBy(x => x.empID).ToList();//list of employee records sorted by employee ID
                empYearlyPayL = empYearlyPayL.OrderBy(x => x.yearlyPay).ToList();//list of employee records sorted by yearly pay
                empTaxdueL = empYearlyPayL.OrderBy(x => x.yearlyTax).ToList();//list of employee records sorted by yearly tax
                TaxCalculator.checkEmpRecordBool = true;

                while (true)
                {


                    if (pinput.Equals("1"))// check user input and pass correct list through function
                    {
                        printList(empListName);
                        break;

                    }
                    else if (pinput.Equals("2"))
                    {
                        printList(empStateCodeL);
                        break;
                    }
                    else if (pinput.Equals("3"))
                    {
                        printList(empIDL);
                        break;
                    }
                    else if (pinput.Equals("4"))
                    {
                        printList(empYearlyPayL);
                        break;
                    }
                    else if (pinput.Equals("5"))
                    {
                        printList(empTaxdueL);
                        break;
                    }
                    else
                    {
                        Console.WriteLine("\n\n~~~~~~~~~~~~~~~~~\nInput Incorrect: Please input a number 1-5 \n\n Sort Employees by | (1) Name | (2) State | (3) ID | (4) Yearly Pay | (5) Tax due ");
                        pinput = Console.ReadLine();
                        Console.Clear();

                    }

                }

            }



            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }



        }
    }
}