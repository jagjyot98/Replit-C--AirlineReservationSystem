/*Notes :		1.Join the functionality to create flights and bookings.
 *              2.Add fucntionality for delete flight, search booking, delete booking, display bookings.
 *              3.MIGHT want to seperate out fucntionalites for Admin and User based on login credentials

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Replit_C__AirlineReservationSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    class Airline                                                                       //Airline class
    {
        List<Flight> FlightsList = new List<Flight>();              //System collection for Flights
        List<Booking> BookingsList = new List<Booking>();              //System collection for Bookings

        DatabaseOperations DBops = new DatabaseOperations();




        /*public void updateFlights()
        {
            FlightsList.Clear();      //clearing previous data collected before updation

            FlightsList = DBops.readDatabaseFT().Cast<Flight>().ToList();
        }

        public void updateBookings()
        {
            BookingsList.Clear();      //clearing previous data collected before updation

            BookingsList = DBops.readDatabaseBK().Cast<Booking>().ToList();
        }*/
        public char seatAvailability(string flightcode, int seatno)               //Chceking seat availablity and marking it (R)eserved it for booking
        {

            foreach (Flight flight in FlightsList)
            {
                if (flight.flightCode == flightcode)
                {
                    if (flight.availableSeats().Contains(seatno))
                    {
                        flight.seats[seatno] = 'R';
                        if (!DBops.seatsDatabaseUpdation(flight.seats, flightcode))
                        {
                            BookingsList = DBops.readDatabaseBK();
                            FlightsList = DBops.readDatabaseFT();

                            return 'D';             //  D = Database updation Error
                        }
                        return 'G';             //  G = all Good with Seats updation
                    }
                }
            }
            return 'S';             //  S = Seat not available
        }


        public string addNewBooking(string flightcode, int seatno)           //add new BOOKINGS            
        {
            Booking newBooking = new Booking();
            //string flightcode; int seatno;
            char seatAvailabilityStatus;

            /*Console.Write("Enter the flight code: ");
            flightcode = Console.ReadLine();
            Console.Write("Enter the seat no.: ");
            seatno = Convert.ToInt32(Console.ReadLine());*/
            seatno--;
            seatAvailabilityStatus = seatAvailability(flightcode, seatno);          //chcek seat availability

            if (seatAvailabilityStatus == 'G')
            {
                int id = newBooking.newBooking(flightcode, seatno);

                string newBookingStatus = DBops.createNewBooking(newBooking);

                if (newBookingStatus == "BG")
                {
                    BookingsList = DBops.readDatabaseBK();

                    /*Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\n Booking No. {0} Added successfully !", id);
                    Console.ResetColor();*/
                    return " Booking No. " + id + " Added successfully !";
                }
                else //if(newBookingStatus == "BE")
                {
                    return " Database Bk Updation Error !";
                    /*Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(" Database Bk Updation Error !");
                    Console.ResetColor();*/
                }
            }
            else if (seatAvailabilityStatus == 'S')
            {
                return " Seat not Available !";
                /*Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n Seat not Available !");
                Console.ResetColor();*/
            }
            else //if (seatAvailabilityStatus == 'D')
            {
                return "Database Ft Updation Error !";
                /*Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n Database Ft Updation Error !");
                Console.ResetColor();*/
            }
        }

        public string addNewFlight()                    //add new FLIGHTS
        {
            Flight newFlight = new Flight();
            newFlight.newFlight();

            string newFlightStatus = DBops.createNewFlight(newFlight);

            if (newFlightStatus == "FG")
            {
                FlightsList = DBops.readDatabaseFT();
                return "Flight Data Inserted successfully.";
                //Console.ForegroundColor = ConsoleColor.Green;
                //Console.WriteLine("Flight Data Inserted successfully.");
                //Console.ResetColor();
            }
            else //if (newFlightStatus == "FE")
            {
                return "Database Ft Updation Error !";
                /*Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(" Database Ft Updation Error !");
                Console.ResetColor();*/
            }
        }

        public void displayAllBookings()            // display all bookings
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

        /*public void displayAllFlights()				//display all flights
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
        }*/

        public void searchBooking(int bookingID)                    //search booking with booking id
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

        public void searchFlight(String flightCode)                 //search flight with flight code
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

        public void deleteBooking(int bookingID)                    //delete booking with booking id
        {
            Boolean found = false;
            foreach (Booking booking in BookingsList)
            {
                if (booking.BookingID == bookingID)         //bookingID exists or not
                {
                    found = true;
                    foreach (Flight flight in FlightsList)
                    {
                        if (flight.flightCode == booking.flightcode)
                        {
                            string deleteBookingStatus = DBops.deleteBooking(bookingID);

                            if (deleteBookingStatus == "BDG")
                            {
                                Console.WriteLine();
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("Booking Deleted from Bk successfully.");
                                Console.ResetColor();

                                BookingsList.Remove(booking);       //simply removing booking objcet from system collection
                            }
                            else if (deleteBookingStatus == "BDE")
                            {
                                Console.WriteLine();
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Database Bk Updation Error !");
                                Console.ResetColor();
                                return;
                            }

                            flight.seats[booking.seatNo] = 'A';         //updating seat status in system collection of seats

                            if (DBops.seatsDatabaseUpdation(flight.seats, flight.flightCode))     //Updating available seats in database with system collection 
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("Booking Deleted from Ft successfully !");
                                Console.ResetColor();
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("\n Database Ft Updation Error !");
                                Console.ResetColor();
                                return;
                            }
                            break;
                        }
                    }
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

        public void deleteFlight(string flightCode)                 //delete flight with flight code
        {
            Boolean found = false;

            foreach (Flight flight in FlightsList)
            {
                if (flight.flightCode == flightCode)            //chceking whether flight exists or not
                {
                    if (DBops.deleteFlight(flightCode) == "FDG")       //if found, delete flight data from Flights database
                    {
                        if (DBops.deleteFT_Bookings(flightCode) == "BDG")      //delete bookings related to deleted flight
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Flight Deleted Successfully with all Related Bookings!");
                            Console.ResetColor();

                            FlightsList.Remove(flight);
                            BookingsList = DBops.readDatabaseBK();
                        }
                        /*else              //Not neccessary deleted flight would have related bookings 
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\n Database Bk Updation Error !");
                            Console.ResetColor();
                        }*/
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n Database Ft Updation Error !");
                        Console.ResetColor();
                    }
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

    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            updateFlights();
            updateBookings();
            flightsCount();
            bookingsCount();
            displayAllFlights();
        }

        Airline airline = new Airline();
        List<Flight> FlightsList = new List<Flight>();              //System collection for Flights
        List<Booking> BookingsList = new List<Booking>();              //System collection for Bookings

        DatabaseOperations DBops = new DatabaseOperations();

        public void flightsCount()                                       //FLIGHTs count
        {
            noOfFlightsDisplay.Text += FlightsList.Count;
        }
        public void bookingsCount()              //BOOKINGs count
        {
            noOfBookingsDisplay.Text += BookingsList.Count;
        }

        public void updateFlights()
        {
            FlightsList.Clear();      //clearing previous data collected before updation

            FlightsList = DBops.readDatabaseFT().Cast<Flight>().ToList();
            Console.WriteLine(FlightsList);
        }

        public void updateBookings()
        {
            BookingsList.Clear();      //clearing previous data collected before updation

            BookingsList = DBops.readDatabaseBK().Cast<Booking>().ToList();
        }

        public void displayAllFlights()             //display all flights
        {
            GeneralMssgDisplay.Content = string.Empty;
            if (FlightsList.Count != 0)
            {
                for (int i = 0; i < FlightsList.Count; i++)
                {
                    GeneralMssgDisplay.Content += "\n" + (i + 1);
                    GeneralMssgDisplay.Content += FlightsList[i].displayFlight();
                }
            }
        }
        private void loginSubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (userNameTextbox.Text.Length == 0 || passwordTextbox.Text.Length == 0)
            { 
                
                loginErrorMessage.Content = "Missed Credential !!";
                userNameTextbox.Text = string.Empty;
                passwordTextbox.Text = string.Empty;
            }
            else if (userNameTextbox.Text == "ABC123ABC" && passwordTextbox.Text == "abc123")
            {
                LoginGroup.Visibility = Visibility.Collapsed;
                FlightGroup.Visibility = Visibility.Visible;
                BookingGroup.Visibility = Visibility.Visible;
                GeneralMssgDisplay.Visibility = Visibility.Visible;
            }
            else
            {
                loginErrorMessage.Content = "Invalid Credentials !!";
                userNameTextbox.Text = string.Empty;
                passwordTextbox.Text = string.Empty;
            }
        }

        private void FTSubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if(FTDestinationInput.Text.Length == 0)
            {
                //FTmessageDisplay.Content = string.Empty;
                FTmessageDisplay.Content = "Please provide Destination for flight.";
            }
            else 
            {
                //FTmessageDisplay.Content = string.Empty;
                FTmessageDisplay.Content = airline.addNewFlight();
            }
        }

        private void BKSubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (BKFlightCodeInput.Text.Length == 0 || BKSeatNoInput.Text.Length == 0 || BKNameInput.Text.Length == 0)
            {
                //BKmessageDisplay.Content = string.Empty;
                BKmessageDisplay.Content = "Missed details";
            }
            else
            {
                //BKmessageDisplay.Content = string.Empty;
                int seatNo;
                int.TryParse(BKSeatNoInput.Text, out seatNo);
                BKmessageDisplay.Content = airline.addNewBooking(BKFlightCodeInput.Text,seatNo);
            }
        }

      
    }
}
