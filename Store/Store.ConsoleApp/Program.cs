using System;
using System.Collections.Generic;
using Store.Library;

namespace Store.ConsoleApp
{
    public class Program
    {
        // start a session
        public static Session ses = new Session();

        static void Main(string[] args)
        {
            bool cont = true;

            while (cont)
            {
                PrintMainMenu();
                string input = GetUserInput();
                MainMenuSelection(input, ref cont);
            }
        }

        public static void PrintMainMenu()
        {
            Console.Clear();
            Console.WriteLine("Welcome to Store!");
            Console.WriteLine("Customer: " + ses.ShowCurrentCustomer());
            Console.WriteLine("Location: " + ses.ShowCurrentLocation());
            Console.WriteLine("Please Select From the Following Options: ");
            Console.WriteLine("(1) Add Customer");
            Console.WriteLine("(2) View All Customers");
            Console.WriteLine("(3) Change Customer");
            Console.WriteLine("(4) Add Location");
            Console.WriteLine("(5) View All Locations");
            Console.WriteLine("(6) Select Location");
            Console.WriteLine("E(x)it");
            Console.Write("Selection: ");
        }

        public static string GetUserInput()
        {
            return Console.ReadLine();
        }

        public static void MainMenuSelection(string input, ref bool cont)
        {
            switch(input.ToLower())
            {
                case "1":
                    AddCustomerConsole();
                    break;
                case "2":
                    PrintAllCustomersConsole();
                    WaitOnKeyPress();
                    break;
                case "3":
                    PrintAllCustomersConsole();
                    GetAndSetCustomerSelection();
                    break;
                case "4":
                    AddLocationConsole();
                    break;
                case "5":
                    PrintAllLocationsConsole();
                    WaitOnKeyPress();
                    break;
                case "6":
                    PrintAllLocationsConsole();
                    GetAndSetLocationSelection();
                    break;
                case "x":
                    cont = false;
                    ses.CloseSession();
                    break;
            }
        }

        //--------------------------------------------------------------------
        // Customer Console

        public static void AddCustomerConsole()
        {
            Console.Clear();
            Console.WriteLine("Please provide first name:");
            string firstName = Console.ReadLine();
            Console.WriteLine("Please provide last name:");
            string lastName = Console.ReadLine();
            ses.AddCustomer(firstName, lastName);
        }

        public static void PrintAllCustomersConsole()
        {
            Console.Clear();

            if (!(ses.NumOfCurrentCustomers() == 0))
            {
                var customers = ses.GetAllCustomers();
                foreach (Customer cust in customers)
                {
                    Console.WriteLine("First Name\t|Last Name\t | Customer ID");
                    Console.WriteLine($"{cust.FirstName,16}|{cust.LastName,16}| {cust.Id}");
                    Console.WriteLine("----------------------------------------------");
                }
            }
            else
            {
                Console.WriteLine("No Customers Currently in System.");
            }
        }

        public static void GetAndSetCustomerSelection()
        {
            if (!(ses.NumOfCurrentCustomers() == 0))
            {
                //since we have already displayed all customers we can just get the selection here
                int input = -1;
                while (input <= 0)
                {
                    Console.Write("Please enter your Customer Id: ");
                    try
                    {
                        input = Int32.Parse(Console.ReadLine());
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Please Enter a valid number");
                        continue;
                    }

                    //check to see if the id given is an actual customer
                    if (!ses.IsCustomer(input))
                    {
                        Console.WriteLine("Please enter a valid Customer ID");
                        input = -1;
                    }
                }
                // have a customer id, tell the session to remember that
                ses.SetCurrentCustomer(input);
            }
            else
            {
                WaitOnKeyPress();
            }
        }

        //--------------------------------------------------------------------
        // Location Console

        public static void AddLocationConsole()
        {
            Console.Clear();
            Console.WriteLine("Please provide Store Name:");
            string name = Console.ReadLine();
            ses.AddLocation(name);
        }

        public static void PrintAllLocationsConsole()
        {
            if (!(ses.NumOfCurrentLocations() == 0))
            {
                var locations = ses.GetAllLocations();

                Console.Clear();

                foreach (Location location in locations)
                {
                    Console.WriteLine("Store Name\t| Location ID");
                    Console.WriteLine($"{location.Name}\t| {location.Id}");
                }
            }
            else
            {
                Console.WriteLine("No Locations Currently in System.");
            }    
        }

        public static void GetAndSetLocationSelection()
        {
            if (ses.NumOfCurrentLocations() > 0)
            {
                //since we have already displayed all customers we can just get the selection here
                int input = -1;
                while (input <= 0)
                {
                    Console.Write("Please enter the Location Id: ");
                    try
                    {
                        input = Int32.Parse(Console.ReadLine());
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Please Enter a valid number");
                        continue;
                    }

                    //check to see if the id given is an actual customer
                    if (!ses.Customers.IsCustomer(input))
                    {
                        Console.WriteLine("Please enter a valid Location ID");
                        input = -1;
                    }
                }
                // have a customer id, tell the session to remember that
                ses.SetCurrentLocation(input);
            }
            else
            {
                WaitOnKeyPress();
            }
        }

        public static void WaitOnKeyPress()
        {
            //wait for key press
            Console.WriteLine("Press any key to go back");
            Console.ReadKey();
        }
    }
}
