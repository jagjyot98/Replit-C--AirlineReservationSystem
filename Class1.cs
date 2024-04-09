/*Notes :		

*/
using System;
using System.Data;
using MySql.Data.MySqlClient;
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
        Console.Write("Enter your destination: ");
        flightDestination = Console.ReadLine();
        flightCode = "FL" + random.Next(1000, 9999);
        for (int i = 0; i < seats.Length; i++)
            seats[i] = 'A';
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\n Flight " + flightCode + " to " + flightDestination + " created with 10 seats.");
        Console.ResetColor();
    }

    public void displayFlight()
    {
        Console.Write("Flight Code: ");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write(flightCode);
        Console.ResetColor();
        Console.WriteLine("		Flight Destination: ");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write(flightDestination);
        Console.ResetColor();
        Console.Write("Available seats: ");
        List<int> list = availableSeats();
        Console.ForegroundColor = ConsoleColor.Yellow;
        foreach (int seat in list)
            Console.Write(seat + 1 + " ");

        Console.ResetColor();
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
        Console.WriteLine("Booking ID: " + BookingID);
        Console.WriteLine("Flight Code: " + flightcode);
        Console.WriteLine("Name: " + name);
        Console.WriteLine("Seat number: " + (seatNo + 1));
    }
}

class Airline																		//Airline class
{
    List<Flight> FlightsList = new List<Flight>();
    List<Booking> BookingsList = new List<Booking>();

    string FLIGHTSconnectionString = "Server=127.0.0.1;Database=airlinesys;Uid=root;Pwd=;";
    string BOOKINGSconnectionString = "Server=127.0.0.1;Database=airlinesys;Uid=root;Pwd=;";

    public int flightsCount()       								//FLIGHTs count
    {
        return FlightsList.Count;
    }
    public int bookingsCount()				//BOOKINGs count
    {
        return BookingsList.Count;
    }
    /// <summary>
    /// ////////////////////////////////////DATABASE FUNCTIONS//////////////////////////////////////
    /// </summary>

    public void updateFlights()
    {
        FlightsList.Clear();      //clearing previous data collected before updation

        using (MySqlConnection connection = new MySqlConnection(FLIGHTSconnectionString))
        {
            connection.Open();
            string query = "SELECT * FROM flights";
            MySqlCommand command = new MySqlCommand(query, connection);
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Flight flight = new Flight();
                    flight.flightCode = reader.GetString(0);
                    flight.flightDestination = reader.GetString(1);
                    flight.seats = reader.GetString(2).ToCharArray();
                    FlightsList.Add(flight);
                    //Console.WriteLine(reader["BookID"] + " " + reader["Title"]+" " + reader["Author"]); // Replace with your column names

                }
            }
            connection.Close();
        }
    }

    public void updateBookings()
    {
        BookingsList.Clear();      //clearing previous data collected before updation

        using (MySqlConnection connection = new MySqlConnection(BOOKINGSconnectionString))
        {
            connection.Open();
            string query = "SELECT * FROM bookings";
            MySqlCommand command = new MySqlCommand(query, connection);
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Booking booking = new Booking();
                    booking.BookingID = reader.GetInt32(0);
                    booking.name = reader.GetString(1);
                    booking.seatNo = reader.GetInt32(2);
                    booking.flightcode = reader.GetString(3);
                    BookingsList.Add(booking);
                    //Console.WriteLine(reader["BookID"] + " " + reader["Title"]+" " + reader["Author"]); // Replace with your column names

                }
            }
            connection.Close();
        }
    }

    public bool seatAvailability(string flightcode, int seatno)
    {               //Chceking seat availablity

        foreach (Flight flight in FlightsList)
        {
            if (flight.flightCode == flightcode)
            {
                if (flight.availableSeats().Contains(seatno))
                {
                    flight.seats[seatno] = 'R';
                    return true;
                }
            }
        }
        return false;
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
        if (seatAvailability(flightcode, seatno))
        {
            int id = newBooking.newBooking(flightcode, seatno);
            //BookingsList.Add(newBooking);
            using (MySqlConnection connection = new MySqlConnection(BOOKINGSconnectionString))
            { 
                connection.Open();
                string query = "INSERT INTO bookings (BookingID, Name, SeatNo, FlightCode) VALUES (@BookingID, @Name, @SeatNo, @FlightCode)";

                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@BookingID", id);
                command.Parameters.AddWithValue("@Name", newBooking.name);
                command.Parameters.AddWithValue("@SeatNo", newBooking.seatNo);
                command.Parameters.AddWithValue("@FlightCode", newBooking.flightcode);

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Data Inserted successfully.");
                    Console.ResetColor();
                    updateBooks();      //updating system collection with updated data
                }
                else
                {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Failed to insert data.");
                    Console.ResetColor();
                }
                connection.Close();
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n Booking No. {0} Added successfully !", id);
            Console.ResetColor();
        }
        else
        {                           //////////////////////////////////////////////////// improve this
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n Seat not Available !");
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
                foreach (Flight flight in FlightsList)
                {
                    if (flight.flightCode == BookingsList[i].flightcode)
                    {
                        Console.WriteLine("Destination: " + flight.flightDestination);
                    }
                }
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
                Console.WriteLine("\n" + (i + 1));
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

    public void searchBooking(int bookingID)					//search booking with booking id
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

    public void searchFlight(String flightCode)					//search flight with flight code
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

    public void deleteBooking(int bookingID)					//delete booking with booking id
    {
        Boolean found = false;
        foreach (Booking booking in BookingsList)
        {
            if (booking.BookingID == bookingID)
            {
                BookingsList.Remove(booking);
                foreach (Flight flight in FlightsList)
                {
                    if (flight.flightCode == booking.flightcode)
                    {
                        flight.seats[booking.seatNo] = 'A';
                        break;
                    }
                }
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

    public void deleteFlight(string flightCode)					//delete flight with flight code
    {
        Boolean found = false;
        foreach (Flight flight in FlightsList)
        {
            if (flight.flightCode == flightCode)
            {
                FlightsList.Remove(flight);                             //deleting flight from system data
                foreach (Booking booking in BookingsList)       //deleting bookings related to deleted flight
                {
                    if (booking.flightcode == flightCode)
                    {
                        BookingsList.Remove(booking);
                        break;
                    }
                }
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

        airline.updateFlights();
        airline.updateBookings();

        while (true)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\n\n		-----Airline Resrvation system-----\n");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Flights Available in the system: {0}\n", airline.flightsCount());
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("No. of Bookings in the system: {0}\n", airline.bookingsCount());
            Console.ResetColor();

            airline.displayAllFlights();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\n Flight Operations	------");               ////////////////// this should be booking operations
            Console.ResetColor();
            Console.WriteLine("1. Add New Flight");
            Console.WriteLine("2. Search Flight");
            Console.WriteLine("3. Delete Flight");

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\n Booking Operations	------");           ////////////////// this should be flight operations
            Console.ResetColor();
            Console.WriteLine("4. Add New Booking");
            Console.WriteLine("5. Display All Bookings");
            Console.WriteLine("6. Search Booking");
            Console.WriteLine("7. Delete Booking");
            Console.Write("\n Enter your choice: ");
            switch (Console.ReadLine())
            {
                case "1":
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("\nSystem response -----\n");
                    Console.ResetColor();
                    airline.addNewFlight();
                    break;
                case "2":
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("\nSystem response -----\n");
                    Console.ResetColor();
                    Console.Write("Enter the Flight code: ");
                    string code = Console.ReadLine();
                    airline.searchFlight(code);
                    break;
                case "3":
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("\nSystem response -----\n");
                    Console.ResetColor();
                    Console.Write("Enter the Flight code: ");
                    code = Console.ReadLine();
                    airline.deleteFlight(code);
                    break;

                case "4":
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("\nSystem response -----\n");
                    Console.ResetColor();
                    airline.addNewBooking();
                    break;
                case "5":
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("\nSystem response -----\n");
                    Console.ResetColor();
                    airline.displayAllBookings();
                    break;
                case "6":
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("\nSystem response -----\n");
                    Console.ResetColor();
                    Console.Write("Enter the BookingId number: ");
                    int id = Convert.ToInt32(Console.ReadLine());
                    airline.searchBooking(id);
                    break;
                case "7":
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("\nSystem response -----\n");
                    Console.ResetColor();
                    Console.Write("Enter the BookingId number: ");
                    id = Convert.ToInt32(Console.ReadLine());
                    airline.deleteBooking(id);
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