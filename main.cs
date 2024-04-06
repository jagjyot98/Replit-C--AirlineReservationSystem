/*Notes :		1. Not updating deleted booking seat status from 'R' to 'A'		
			 			2. if flight deleted, related bookings also be deleted.
*/
using System;
using System.Collections.Generic;

class Flight																//Flight Class
{
    public string flightCode;
    public string flightDestination;
    public char[] seats = new char[10];

    private static Random random = new Random();

    public List<int> availableSeats()
    {
        List<int> count = new List<int>();
        for (int i = 0; i < seats.Length; i++)
        {
            if (seats[i] == 'A')
                count.Add(i);
        }
        return count;
    }

    public void newFlight()
    {
        Console.WriteLine("Enter your destination: ");
        flightDestination = Console.ReadLine();
        flightCode = "FL" + random.Next(1000, 9999);
        for (int i = 0; i < seats.Length; i++)
            seats[i] = 'A';
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Flight " + flightCode + " to " + flightDestination + " created with 10 seats.");
        Console.ResetColor();
    }

    public void displayFlight()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("Flight Code: " + flightCode);
        Console.ResetColor();
        Console.WriteLine("		Flight Destination: " + flightDestination);
        Console.Write("Available seats: ");
        List<int> list = availableSeats();
        foreach (int seat in list)
            Console.Write(seat+1 + " ");
    }
}

class Booking														//Booking class
{
    public string name;
		public string flightcode;
    public int seatNo;
    public int BookingID;

    private static Random random = new Random();

    public int newBooking(string flightcode, int seatno)
    {
        BookingID = new Random().Next(100, 500);
				this.flightcode = flightcode;
        Console.Write("Enter your name: ");
        name = Console.ReadLine();
        this.seatNo = seatno;
        return BookingID;
    }

    public void displayBooking()
    {
        Console.WriteLine("Booking ID: "+ BookingID);
				Console.WriteLine("Flight Code: "+ flightcode);
        Console.WriteLine("Name: " + name);
        Console.WriteLine("Seat number: " + (seatNo+1));
    }
}

class Airline																		//Airline class
{
    List<Flight> FlightsList = new List<Flight>();
    List<Booking> BookingsList = new List<Booking>();

    public int flightsCount()       								//FLIGHTs count
    {
        return FlightsList.Count;
    }

		public bool seatAvailability(string flightcode,int seatno){				//Chceking seat availablity
						
			foreach(Flight flight in FlightsList)
			{
				if(flight.flightCode == flightcode)
				{
					if(flight.availableSeats().Contains(seatno))
					{
						flight.seats[seatno] = 'R';
						return true;
					}
				}
			}
			return false;
		}

    public int bookingsCount()				//BOOKINGs count
    {       
        return BookingsList.Count;
    }

    public void addNewBooking()           //add new BOOKINGS
    {
        Booking newBooking = new Booking();
		string flightcode; int seatno;
		
		Console.Write("Enter the flight code: ");
		flightcode = Console.ReadLine();		
		Console.Write("Enter the seat no.: ");
		seatno = Convert.ToInt32(Console.ReadLine());
		seatno--;
		if(seatAvailability(flightcode,seatno))
		{
			int id = newBooking.newBooking(flightcode,seatno);
	        BookingsList.Add(newBooking);
	        Console.ForegroundColor = ConsoleColor.Green;
	        Console.WriteLine("Booking No. {0} Added successfully !", id);
	        Console.ResetColor();
		}else{							//////////////////////////////////////////////////// improve this
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("Seat not Available !");
			Console.ResetColor();
		}
    }

    public void addNewFlight()					//add new FLIGHTS
    {           
        Flight newFlight = new Flight();
        newFlight.newFlight();
        FlightsList.Add(newFlight);
    }

    public void displayAllBookings()			// display all bookings
    {
        if (BookingsList.Count != 0)
        {
            for (int i = 0; i < BookingsList.Count; i++)
            {
                Console.WriteLine(i + 1);
                BookingsList[i].displayBooking();
                Console.WriteLine("-----------------");
            }
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("No Bookings found in System !");
            Console.ResetColor();
        }
    }

    public void displayAllFlights()				//display all flights
    {
        if (FlightsList.Count != 0)
        {
            for (int i = 0; i < FlightsList.Count; i++)
            {
                Console.WriteLine("\n"+(i + 1));
                FlightsList[i].displayFlight();
                Console.WriteLine("\n---------------------");
            }
        }
        // else{
        // 	Console.ForegroundColor = ConsoleColor.Red;
        // 	Console.WriteLine("No Flights found in System !");
        // 	Console.ResetColor();
        // }
    }

    public void searchBooking(int bookingID)			//search booking with booking id
    {
        Boolean found = false;
        foreach (Booking booking in BookingsList)
        {
            if (booking.BookingID == bookingID)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nMatch Found:");
                Console.ResetColor();
                booking.displayBooking();
                found = true;
            }
        }
        if (!found)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("No Match Found !");
            Console.ResetColor();
        }
    }

    public void searchFlight(String flightCode)			//search flight with flight code
    {
        Boolean found = false;
        foreach (Flight flight in FlightsList)
        {
            if (flight.flightCode == flightCode)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nMatch Found:");
                Console.ResetColor();
                flight.displayFlight();
                found = true;
            }
        }
        if (!found)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("No Match Found !");
            Console.ResetColor();
        }
    }

    public void deleteBooking(int bookingID)			//delete booking with booking id
    {
        Boolean found = false;
        foreach (Booking booking in BookingsList)
        {
            if (booking.BookingID == bookingID)
            {
                BookingsList.Remove(booking);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Booking Deleted successfully !");
                Console.ResetColor();
                found = true;
                break;
            }
        }
        if (!found)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("No Match Found !");
            Console.ResetColor();
        }
    }

    public void deleteFlight(string flightCode)			//delete flight with flight code
    {
        Boolean found = false;
        foreach (Flight flight in FlightsList)
        {
            if (flight.flightCode == flightCode)
            {
                FlightsList.Remove(flight);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Flight Deleted successfully !");
                Console.ResetColor();
                found = true;
                break;
            }
        }
        if (!found)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("No Match Found !");
            Console.ResetColor();
        }
    }
}

class Program														//program class
{
    public static void Main(string[] args)
    {
        Airline airline = new Airline();

        while (true)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\n\n		-----Airline Resrvation system-----\n");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("No. of Bookings in the system: {0}\n", airline.bookingsCount());
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Flights Available in the system: {0}\n", airline.flightsCount());
            Console.ResetColor();
            airline.displayAllFlights();

            Console.WriteLine("\n Booking Operations	------");			////////////////// this should be flight operations
            Console.WriteLine("1. Add New Booking");
            Console.WriteLine("2. Display All Bookings");
            Console.WriteLine("3. Search Booking");
            Console.WriteLine("4. Delete Booking");

            Console.WriteLine("\n Flight Operations	------");				////////////////// this should be booking operations
            Console.WriteLine("5. Add New Flight");
            Console.WriteLine("6. Search Flight");
            Console.WriteLine("7. Delete Flight");
            Console.Write("Enter your choice: ");

            switch (Console.ReadLine())
            {
                case "1":
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("\nSystem response -----\n");
                    Console.ResetColor();
                    airline.addNewBooking();
                    break;
                case "2":
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("\nSystem response -----\n");
                    Console.ResetColor();
                    airline.displayAllBookings();
                    break;
                case "3":
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("\nSystem response -----\n");
                    Console.ResetColor();
                    Console.Write("Enter the BookingId number: ");
                    int id = Convert.ToInt32(Console.ReadLine());
                    airline.searchBooking(id);
                    break;
                case "4":
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("\nSystem response -----\n");
                    Console.ResetColor();
                    Console.Write("Enter the BookingId number: ");
                    id = Convert.ToInt32(Console.ReadLine());
                    airline.deleteBooking(id);
                    break;


                case "5":
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("\nSystem response -----\n");
                    Console.ResetColor();
                    airline.addNewFlight();
                    break;
                case "6":
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("\nSystem response -----\n");
                    Console.ResetColor();
                    Console.Write("Enter the Flight code: ");
                    string code = Console.ReadLine();
                    airline.searchFlight(code);
                    break;
                case "7":
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("\nSystem response -----\n");
                    Console.ResetColor();
                    Console.Write("Enter the Flight code: ");
                    code = Console.ReadLine();
                    airline.deleteFlight(code);
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("\nSystem response -----\n");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid Choice !");
                    Console.ResetColor();
                    break;
            }
        }
    }
}