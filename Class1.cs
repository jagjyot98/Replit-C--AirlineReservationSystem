/*Notes :		

*/
using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using Mysqlx.Crud;
using System.Security.Policy;
using Replit_C__AirlineReservationSystem;
using System.Collections;
using System.Linq;
using System.Windows;

class Users
{
    public string userID;
    public string fullName;
    public string password;

    private static Random random = new Random();

    public string newUser(string userName, string password)
    {
        this.fullName = userName;
        this.password = password;
        this.userID = "UN" + random.Next(10000, 99999);
        return userID;
    }

}

class Flight																//Flight Class
{
    public string flightCode;
    public string flightDestination;
    public char[] seats = new char[10];

    private static Random random = new Random();

    public List<int> availableSeats()               //getting the list of available seats of a flight
    {
        List<int> count = new List<int>();
        for (int i = 0; i < seats.Length; i++)
        {
            if (seats[i] == 'A')
                count.Add(i);
        }
        return count;
    }

    public void newFlight(string desti)                     //to create a new flight
    {
        //Console.Write("Enter your destination: ");
        //flightDestination = Console.ReadLine();
        this.flightDestination = desti;
        this.flightCode = "FL" + random.Next(1000, 9999);            //generating a random unique flight code
        for (int i = 0; i < seats.Length; i++)
            seats[i] = 'A';
        //Console.ForegroundColor = ConsoleColor.Green;
        //Console.WriteLine("\n Flight " + this.flightCode + " to " + this.flightDestination + " created with 10 seats.");
        //Console.ResetColor();
    }

    public string displayFlight()             //to display flighs(s) details in the header pf program
    {
        List<int> list = availableSeats();
        string seats="";
        foreach (int seat in list)
            seats+=(seat + 1 + " ");  

        string flight = "\nFlight Code: "+flightCode+ " to Destination: " + flightDestination+ "\nAvailable seats: "+seats+ "\n---------------------";
        return flight;

        /*Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write(flightCode);
        Console.ResetColor();
        Console.Write("		Flight Destination: ");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(flightDestination);
        Console.ResetColor();
        Console.Write("Available seats: ");
        Console.ResetColor();*/
    }
}

class Booking														//Booking class
{
    public string name;
    public string flightcode;
    public int seatNo;
    public int BookingID;

    private static Random random = new Random();

    public int newBooking(string name, string flightcode, int seatno)        //to create new booking
    {
        BookingID = new Random().Next(100, 500);            //generating random unique booking id
        this.flightcode = flightcode;
        this.name = name;
        //Console.Write("Enter your name: ");
        //name = Console.ReadLine();
        this.seatNo = seatno;
        return BookingID;
    }

    public void displayBooking()                    //to display bookings when called
    {
        Console.WriteLine("Booking ID: " + BookingID);
        Console.WriteLine("Flight Code: " + flightcode);
        Console.WriteLine("Name: " + name);
        Console.WriteLine("Seat number: " + (seatNo + 1));
    }
}

class Program														//program class
{
    [STAThread]
    public static void Main(string[] args)
    {

        //Console.WriteLine("HELLO.............!!!!!!!!!!!!!!!!!!!!");
        Application app = new Application();
        MainWindow win = new MainWindow();
        app.Run(win);



        //airline.updateFlights();        //updating system collection of flights with database data
        //airline.updateBookings();        //updating system collection of bookings with database data

        /*Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("\n\n		-----Airline Resrvation system-----\n");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Flights Available in the system: {0}\n", airline.flightsCount());            //desplaying system count of flights' collection
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("No. of Bookings in the system: {0}\n", airline.bookingsCount());            //desplaying system count of bookings' collection
        Console.ResetColor();

        airline.displayAllFlights();                                  //displaying flights' details in header of program

        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("\n Flight Operations	------");
        Console.ResetColor();
        Console.WriteLine("1. Add New Flight");
        Console.WriteLine("2. Search Flight");
        Console.WriteLine("3. Delete Flight");

        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("\n Booking Operations	------");
        Console.ResetColor();
        Console.WriteLine("4. Add New Booking");
        Console.WriteLine("5. Display All Bookings");
        Console.WriteLine("6. Search Booking");
        Console.WriteLine("7. Delete Booking");
        Console.Write("\n Enter your choice: ");
        switch (Console.ReadLine())
        {
            /////////////////////////////Fliight Operations
            case "1":
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("\nSystem response -----\n");
                Console.ResetColor();
                airline.addNewFlight();                     //new flight
                break;
        */
        
        /*case "2":    seach flight////////////////////////////////////////
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("\nSystem response -----\n");
        Console.ResetColor();
        Console.Write("Enter the Flight code: ");
        string code = Console.ReadLine();
        airline.searchFlight(code);                 //search specific flight with flight code
        break;
        //case "3":
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("\nSystem response -----\n");
        Console.ResetColor();
        Console.Write("Enter the Flight code: ");
        code = Console.ReadLine();
        airline.deleteFlight(code);                 //delete flight with flight code
        break;
        */
                ////////////////////////Booking Operations
        /*case "4":
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\nSystem response -----\n");
            Console.ResetColor();
            airline.addNewBooking();                     //new booking
            break;*/
        /*case "5":   dispayBookings///////////////////////////////////
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\nSystem response -----\n");
            Console.ResetColor();
            airline.displayAllBookings();               //display all bookings
            break;*/
        /*case "6":       searchBooking/////////////////////////////////
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\nSystem response -----\n");
            Console.ResetColor();
            Console.Write("Enter the BookingId number: ");
            int id = Convert.ToInt32(Console.ReadLine());
            airline.searchBooking(id);                  //search specific booking with booking id
            break;*/
        /*case "7":   deleteBooking///////////////////////////////
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\nSystem response -----\n");
            Console.ResetColor();
            Console.Write("Enter the BookingId number: ");
            id = Convert.ToInt32(Console.ReadLine());
            airline.deleteBooking(id);                  //delete booking with booking id
            break;*/
        
    }
}
    

