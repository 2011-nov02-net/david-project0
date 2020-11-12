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
            Console.WriteLine(ses.ShowCurrentCustomer());
            Console.WriteLine("Please Select From the Following Options: ");
            Console.WriteLine("(1) Add Customer");
            Console.WriteLine("(2) View All Customers");
            Console.WriteLine("(3) Change Customer");
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
                case "x":
                    cont = false;
                    ses.CloseSession();
                    break;
            }
        }

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
            var customers = ses.GetAllCustomers();

            Console.Clear();

            foreach (Customer cust in customers)
            {
                Console.WriteLine("First Name\t| Last Name\t| Customer ID");
                Console.WriteLine($"{cust.FirstName}\t| {cust.LastName}\t| {cust.Id}");
            }
        }

        public static void GetAndSetCustomerSelection()
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
                if(!ses.Customers.IsCustomer(input))
                {
                    Console.WriteLine("Please enter a valid Customer ID");
                    input = -1;
                }
            }

            // have a customer id, tell the session to remember that
            ses.SetCurrentCustomer(input);
        }

        public static void WaitOnKeyPress()
        {
            //wait for key press
            Console.WriteLine("Press any key to go back");
            Console.ReadKey();
        }
    }
}
