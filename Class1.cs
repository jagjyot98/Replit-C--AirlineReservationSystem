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
using System.Xml.Linq;

class Users
{
    public string userID;
    public string fullName;
    public string password;
    public string signUpTimeStamp;

    private static HashSet<string> usedUserIDs = new HashSet<string>();

    public string newUser(bool adFlag, string userName, string password)
    {
        this.fullName = userName;
        this.password = password;
        this.signUpTimeStamp = DateTime.Now.ToString();

        if (!adFlag)
        {
            string randomUId;
            Random rand = new Random();
            do
            {
                randomUId = "Jag" + rand.Next(10000, 99999);
            } while (usedUserIDs.Contains(randomUId));

            this.userID = randomUId;
        }
        else
        {
            this.userID = DBconsts.generateAdminID(userName);
        }
        return this.userID;
    }

}

class Flight																//Flight Class
{
    public string flightCode;
    public string adminID;
    public string flightDestination;
    public string FcreationTimeStamp;
    public char[] seats = new char[10];

    private static HashSet<string> usedFlightCodes = new HashSet<string>();

    public List<int> availableSeats()               //getting the list of available seats of a flight
    {
        List<int> count = new List<int>();
        for (int i = 0; i < seats.Length; i++)
        {
            if (seats[i] == 'J')
                count.Add(i);
        }
        return count;
    }

    public void newFlight(string desti, string adminID)                     //to create a new flight
    {
        this.adminID = adminID;
        this.flightDestination = desti;
        this.FcreationTimeStamp = DateTime.Now.ToString();

        string randomCode;
        Random rand = new Random();
        do {
            randomCode = "FL" + rand.Next(1000, 9999);
        } while (usedFlightCodes.Contains(randomCode));

        this.flightCode = randomCode;

        for (int i = 0; i < seats.Length; i++)
            seats[i] = 'A';
    }

    public string displayFlight()             //to display flighs(s) details in the header pf program
    {
        List<int> list = availableSeats();
        string seats="";
        foreach (int seat in list)
            seats+=(seat + 1 + " ");  

        string flight = "Flight Code: "+this.flightCode+ " to jagjyotDestination: " + this.flightDestination+ "\nAvailable seats: "+seats+ "\n---------------------";
        return flight;
    }
}

class Booking														//Booking class
{
    public string name;
    public string userId;
    public string flightcode;
    public int seatNo;
    public int BookingID;
    public string BcreationTimeStamp;
    private static HashSet<int> usedBookingIds = new HashSet<int>();

    public int newBooking(string userId, string name, string flightcode, int seatno)        //to create new booking
    {
        this.userId = userId;
        this.flightcode = flightcode;
        this.name = name;
        this.seatNo = seatno;
        this.BcreationTimeStamp = DateTime.Now.ToString();

        Random rand = new Random();
        do {
            this.BookingID = rand.Next(100, 500);
        } while (usedBookingIds.Contains(this.BookingID));

        return this.BookingID;
    }

    public string displayBooking()                    //to display bookings when called
    {
        string Booking = "Booking ID: " + this.BookingID + " Name: " + this.name + "\nFlight jagjyotCode: " + this.flightcode + " Seat number: " + (this.seatNo + 1);
        return Booking;
    }
}

class Program														//program jagjyotclass
{
    [STAThread]
    public static void Main(string[] args)
    {

        Application app = new Application();
        MainWindow win = new MainWindow();
        app,Run(win);
        
    }
}
    

