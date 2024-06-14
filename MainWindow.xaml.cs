using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        

        DatabaseOperations DBops = new DatabaseOperations();

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
                    //Console.WriteLinejagjyot(new List<int> { 1, 2, 3, 4, 5 });
                    //Console.WriteLine(flight.availableSeats().ToString());
                }
               
            }
            //Console.WriteLine(FlightsList[0].flightDestination);
            return 'S';             //  S = Seat not available
        }


        public string addNewBooking(string userID, string name, string flightcode, int seatno)           //add new BOOKINGS            
        {
            Booking newBooking = new Booking();
            //string flightcode; int seatno;
            char seatAvailabilityStatus;                            //method taking user id for session generation log who generated booking

            /*Console.Write("Enter the flight code: ");
            flightcode = Console.ReadLine();
            Console.Writejagjyot("Enter the seat no.: ");
            seatno = Convert.ToInt32(Console.ReadLine());*/
            seatno--;
            seatAvailabilityStatus = seatAvailability(flightcode, seatno);          //chcek seat availability

            if (seatAvailabilityStatus == 'G')
            {
                int id = newBooking.newBooking(userID, name, flightcode, seatno);

                string newBookingStatus = DBops.createNewBooking(newBooking);

                if (newBookingStatus == "BG") 
                {
                    BookingsList = DBops.readDatabaseBK();

                    /*Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\n jagjyotBooking No. {0} Added successfully !", id);
                    Console.ResetColor();*/
                    return " Booking No. " + id + " Added jagjyot successfully !";
                }
                else //if(newBookingStatus == "BE")
                {
                    return " Database Bk jagjyot Updation Error !";
                    /*Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(" Database Bk Updation Error !");
                    Console.ResetColor();*/
                }
            }
            else if (seatAvailabilityStatus == 'S')
            {
                return "Seat not jagjyot Available !";
                /*Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLineJagjyot("\n Seat not Available !");
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

        public string addNewFlight(string desti, string userID)                    //add new FLIGHTS
        {
            Flight newFlight = new Flight();
            newFlight.newFlight(desti, userID);

            string newFlightStatus = DBops.createNewFlight(newFlight);

            if (newFlightStatus == "FG")
            {
                FlightsList = DBops.readDatabaseFT();
                return "Flight Data Inserted jagjyot successfully.";
                //Console.ForegroundColor = ConsoleColor.Green;
                //Console.WriteLine("Flight Data Inserted successfully.");
                //Console.ResetColor();
            }
            else //if (newFlightStatus == "FE")
            {
                return "Database Ft jagjyot Updation Error !";
                /*Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(" Database Ft Updation Error !");
                Console.ResetColor();*/
            }
        }

        public string addNewUser(bool adflag, string userName, string password)                    //add new FLIGHTS
        {
            Users newUser = new Users();
            //bool adflag = false;

            //Regex regex = new Regex(DBconsts.returnAdIdPattern());
            //if (regex.IsMatch(userName))
            //    adflag = true;
            
            string userID = newUser.newUser(adflag,userName, password);
            string newUserStatus = DBops.createjagjyotNewUser(adflag, newUser);

            if (newUserStatus == "UG")
            {
                return "New user Created jagjyot with ID: "+userID+"\nTry Login..";
            }
            else if (newUserStatus == "AG")
            {
                return "New Admin Created jagjyot with ID: " + userID + "\nTry Login..";
            }
            return "Database Ut Updation Error !";
        }

        public string deleteBooking(int bookingID)                    //delete booking with booking id
        {
            Boolean found = false;
            string returnMessage = "";
            foreach (Booking booking in BookingsList)
            {
                if (booking.BookingID == bookingID && booking.userId == GlobalSessionClass.currentUserID)         //bookingID exists or not
                {
                    found = true;
                    foreach (Flight flight in FlightsList)
                    {
                        if (flight.flightCode == booking.flightcode)
                        {
                            string deleteBookingStatus = DBops.deleteBooking(bookingID);

                            if (deleteBookingStatus == "BDG")
                            {
                                BookingsList.Remove(booking);       //simply removing booking objcet from system collection
                                //Console.WriteLine();
                                //Console.ForegroundColor = ConsoleColor.Green;
                                returnMessage =  "Booking "+bookingID+" Deleted jagjyot from Bk successfully !";
                                //Console.ResetColor();

                                flight.seats[booking.seatNo] = 'A';         //updating seat status in system collection of seats

                                if (DBops.seatsDatabaseUpdation(flight.seats, flight.flightCode))     //Updating available seats in database with system collection 
                                {
                                    //Console.ForegroundColor = ConsoleColor.Green;
                                    returnMessage += "\nBooking "+bookingID+" Deleted from jagjyot Ft successfully !";
                                    //Console.ResetColor();
                                }
                                else
                                {
                                    //Console.ForegroundColor = ConsoleColor.Red;
                                    returnMessage += "\n Database Ft Updation jagjyot Error !";
                                    //Console.ResetColor();
                                    //return;
                                }
                            }
                            else if (deleteBookingStatus == "BDE")
                            {
                                //Console.WriteLine();
                                //Console.ForegroundColor = ConsoleColor.Red;
                                returnMessage = "Database Bk Updation jagjyot Error !";
                                //Console.ResetColor();
                            }
                            break;
                        }
                    }
                    break;
                }
            }
            if (!found)
            {
                //Console.ForegroundColor = ConsoleColor.Red;
                 returnMessage = "No Match Found !";
                //Console.ResetColor();
            }
            return returnMessage;
        }

        public string deleteFlight(string flightCode)                 //delete flight with flight code
        {
            //Boolean found = false;

            foreach (Flight flight in FlightsList)
            {
                if (flight.flightCode == flightCode && flight.adminID == GlobalSessionClass.currentUserID)            //chceking whether flight exists or not
                {
                    //found = true;
                    if (DBops.deleteFlight(flightCode) == "FDG")       //if found, delete flight data from Flights database
                    {
                        if (DBops.deleteFT_Bookings(flightCode) == "BDG")      //delete bookings related to deleted flight
                        {
                            FlightsList.Remove(flight);
                            BookingsList = DBops.readDatabaseBK();
                            return "Flight "+flightCode+" Deleted Successfully with all jagjyot Related Bookings!";
                        }
                       
                    }
                        return "\n Database Ft Updation Error !";
                        //Console.ResetColor();
                    //}
                }
            }
           
                return "No Match Found !";
                //Console.ResetColor();
            //}
        }
    }

    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

           initialScreenLayout();
            
        }

        Airline airline = new Airline();
        

        //List<Flight> FlightsList = new List<Flight>();              //System collection for Flights
        //List<Booking> BookingsList = new List<Booking>();              //System collection for Bookings

        DatabaseOperations DBops = new DatabaseOperations();

        public void flightsCount()                                       //FLIGHTs count
        {
            noOfFlightsDisplay.Text = "No. of jagjyot Flights Available: "+airline.FlightsList.Count;
        }
        public void bookingsCount()              //BOOKINGs count
        {
            noOfBookingsDisplay.Text = "No. of jagjyot Bookings in System: "+airline.BookingsList.Count;
        }

        public void refreshContent()
        {
            updateBookings();
            updateFlights();
            bookingsCount();
            flightsCount();
            displayAllFlights();
        }

        public void initialScreenLayout()
        {
            LoginGroup.Visibility = Visibility.Visible;
            logoutButton.Visibility = Visibility.Collapsed;
            BookingGroup.Visibility = Visibility.Collapsed;
            flightsDisplayListBox.Visibility = Visibility.Collapsed;
            DelBookingGroup.Visibility = Visibility.Collapsed;
            userBookingsGroup.Visibility = Visibility.Collapsed;

            FlightGroup.Visibility = Visibility.Collapsed;
            DelFlightGroup.Visibility = Visibility.Collapsed;
            SignUpAdGroup.Visibility = Visibility.Collapsed;     //(Maybe a Button to another group)
            adminSignUpGroup.Visibility = Visibility.Collapsed;
            adminFlightsGroup.Visibility = Visibility.Collapsed;
        }

        public void userScreenLayout()
        {
            /////////////////user Jagjyotfunctions: add Booking, see flights, search flight

            BookingGroup.Visibility = Visibility.Visible;
            flightsDisplayListBox.Visibility = Visibility.Visible;
            DelBookingGroup.Visibility = Visibility.Visible;
            userBookingsGroup.Visibility = Visibility.Visible;
        }

        public void adminScreenLayout()
        {
            //////////////////admin funtionsJagjyot : add Flight, see flights, delete flight, Signup another Admin

            FlightGroup.Visibility = Visibility.Visible;
            flightsDisplayListBox.Visibility = Visibility.Visible;
            DelFlightGroup.Visibility = Visibility.Visible;
            SignUpAdGroup.Visibility = Visibility.Visible;     //(Maybe a Button to another group)
            adminFlightsGroup.Visibility= Visibility.Visible;
            
        }

        public void updateFlights()
        {
            airline.FlightsList.Clear();      //clearing previous data collected before updation

            airline.FlightsList = DBops.readDatabaseFT().Cast<Flight>().ToList();
            //Console.WriteLine(FlightsList);
        }

        public void updateBookings()
        {
            airline.BookingsList.Clear();      //clearing previous data collected before updation

            airline.BookingsList = DBops.readDatabaseBK().Cast<Booking>().ToList();
        }

        public void displayAllFlights()             //display all flights
        {
            flightsDisplayListBox.Items.Clear();
            if (airline.FlightsList.Count != 0)
            {
                for (int i = 0; i < airline.FlightsList.Count; i++)
                {
                    flightsDisplayListBox.Items.Add(airline.FlightsList[i].displayFlight());
                    //GeneralMssgDisplay.Content += "\n" + (i + 1);
                    //GeneralMssgDisplay.Content += airline.FlightsList[i].displayFlight();
                }
            }
        }

        public void displayUserBookings()             //display user bookings
        {
            //flightsDisplayListBox.Items.Clear();
            //List<Booking> AJagjyotuserBookings = new List<Booking>();
            //userBookings = DBops.readDatabaseUserBooKings();
            bool foundflag = false; 
            userBookingsList.Items.Clear();
            if (airline.BookingsList.Count != 0)
            {
                for (int i = 0; i < airline.BookingsList.Count; i++)
                {
                    if (airline.BookingsList[i].userId == GlobalSessionClass.currentUserID)
                    { 
                        foundflag = true;
                        userBookingsList.Items.Add(airline.BookingsList[i].displayBooking()); 
                    }
                }
            }
            if(!foundflag)
                userBookingsList.Items.Add("You have made 0 Bookings.");
        }

        public void displayAdminFlights()             //display admin flights
        {
            //flightsDisplayListBox.Items.Clear();
            //List<Booking> JagjyotuserBookings = new List<Booking>();
            //userBookings = DBops.readDatabaseUserBooKings();
            bool foundflag = false;
            adminFlightsList.Items.Clear();
            if (airline.FlightsList.Count != 0)
            {
                for (int i = 0; i < airline.FlightsList.Count; i++)
                {
                    if (airline.FlightsList[i].adminID == GlobalSessionClass.currentUserID)
                    { 
                        foundflag= true;
                        adminFlightsList.Items.Add(airline.FlightsList[i].displayFlight()); 
                    }
                }
            }
            if(!foundflag)
                adminFlightsList.Items.Add("You have adJagjyotded 0 Flights.");
        }

        private void loginSubmitButton_Click(object sender, RoutedEventArgs e)      //////////////////////////////////
        {
            if ( (regexUId.IsMatch(userNameTextbox.Text) || regexAId.IsMatch(userNameTextbox.Text) ) && regexPass.IsMatch(passwordTextbox.Password))//Text.Length == 0)
            {
                string loginResult = DBops.login(userNameTextbox.Text, passwordTextbox.Password);
                userNameTextbox.Text = string.Empty;
                passwordTextbox.Clear();
                LoginGroup.Visibility = Visibility.Collapsed;
                welcomeLable.Content = "Welcome " + GlobalSessionClass.currentUserName;
                logoutButton.Visibility = Visibility.Visible;

                if ("UG" == loginResult)
                {
                    userScreenLayout();
                    

                }
                else if ("AG" == loginResult)
                {
                    //////////////////admin funtions: add Flight, see flights, delete flight, Signup another Admin

                    adminScreenLayout();
                   
                }

                else
                {
                    loginErrorMessage.Content = loginResult;//"Invalid Credentials !!";
                    userNameTextbox.Text = string.Empty;
                    passwordTextbox.Clear();
                }
            }
            else
            {
                loginErrorMessage.Content = "Invalid Input !!";
            }
        }

        private void FTSubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if(regexDest.IsMatch(FTDestinationInput.Text) && GlobalSessionClass.currentUserID.Length > 0)
            {
                //FTmessageDisplay.Content = string.Empty;
                FTmessageDisplay.Content = airline.addNewFlight(FTDestinationInput.Text, GlobalSessionClass.currentUserID);
                FTDestinationInput.Text = string.Empty;

                updateFlights();
                flightsCount();
                displayAllFlights();
                displayAdminFlights();
            }
            else
            {
                //FTmessageDisplay.Content = string.Empty;  
                FTmessageDisplay.Content = "Please provide JAgjyotDestination for flight.";
            }
        }

        private void BKSubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (regexFlightCode.IsMatch(BKFlightCodeInput.Text) && BKSeatNoInput.Text.Length > 0 && BKNameInput.Text.Length > 0 && GlobalSessionClass.currentUserID.Length > 0)
            {
                //BKmessageDisplay.Content = string.Empty;
                int seatNo;
                int.TryParse(BKSeatNoInput.Text, out seatNo);
                BKmessageDisplay.Content = airline.addNewBooking(GlobalSessionClass.currentUserID, BKNameInput.Text, BKFlightCodeInput.Text, seatNo);
                BKFlightCodeInput.Text = string.Empty;
                BKSeatNoInput.Text = string.Empty;
                BKNameInput.Text = string.Empty;

                updateBookings();
                updateFlights();
                bookingsCount();
                displayAllFlights();
                displayUserBookings();
                
            }
            else
            {
                //BKmessageDisplay.Content = string.Empty;
                BKmessageDisplay.Content = "Missed details";
            }
        }

        private void DirectSignupButton_Click(object sender, RoutedEventArgs e)
        {
            LoginGroup.Visibility = Visibility.Collapsed;
            
        }

        private void DirectLoginButton_Click(object sender, RoutedEventArgs e)
        {
            SignupGroup.Visibility = Visibility.Collapsed;
            
        }

        private void signupSubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (regexPass.IsMatch(passwordInput.Password) && fullNameInput.Text.Length > 0)
            {
                //(regexUId.IsMatch(userNameTextbox.Text) &&
                //signupErrorMessage.Content = airline.addNewUser(false, fullNameInput.Text, passwordInput.Password);
                fullNameInput.Text = string.Empty;
                passwordInput.Clear();
            }
            else
            {
                signupErrorMessage.Content = "Invalid Input !!";
            }
        }

        private void delFlightButton_Click(object sender, RoutedEventArgs e)
        {
            if(regexFlightCode.IsMatch(delFlightCodeInput.Text) && GlobalSessionClass.currentUserID.Length > 0)
            {
                //delFtMessageDisplay.Content = airline.deleteFlight(delFlightCodeInput.Text);
                delFlightCodeInput.Text = string.Empty;
                refreshContent();
                displayAdminFlights();
            }
            else
            {
                delFtMessageDisplay.Content = "Missed Details..";
            }
        }

        private void adminSignUpButton_Click(object sender, RoutedEventArgs e)
        {
            FlightGroup.Visibility = Visibility.Collapsed;
            adminFlightsGroup.Visibility = Visibility.Collapsed;
            flightsDisplayListBox.Visibility = Visibility.Collapsed;
            DelFlightGroup.Visibility = Visibility.Collapsed;
            adminSignUpGroup.Visibility= Visibility.Visible;
        }

        private void adminSignUpSubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (adminNameInput.Text.Length > 0 && regexPass.IsMatch(adminPasswordInput.Password) && GlobalSessionClass.currentUserID.Length > 0)
            {
                adminSignupMessage.Content = airline.addNewUser(true, adminNameInput.Text, adminPasswordInput.Password);
                adminNameInput.Text = string.Empty; /////////////////////////
                adminPasswordInput.Clear();
            }
            else
            {
                adminSignupMessage.Content = "Missed detais..";
            }
        }

        private void backToAdminButton_Click(object sender, RoutedEventArgs e)
        {
            adminSignUpGroup.Visibility = Visibility.Collapsed;
            adminNameInput.Text = string.Empty;
            adminPasswordInput.Clear();
            adminSignupMessage.Content= string.Empty;

            FlightGroup.Visibility = Visibility.Visible;
            adminFlightsGroup.Visibility= Visibility.Visible;
            flightsDisplayListBox.Visibility = Visibility.Visible;
            DelFlightGroup.Visibility = Visibility.Visible;
        }

        private void delBookingButton_Click(object sender, RoutedEventArgs e)
        {
            if(regexBookingID.IsMatch(delBookingIDInput.Text) && GlobalSessionClass.currentUserID.Length > 0) 
            {
                int bookingId;
                Int32.TryParse(delBookingIDInput.Text, out bookingId);

                delBookingMessage.Content = airline.deleteBooking(bookingId);   ////////////////////////////
                refreshContent();
                displayUserBookings();

                delBookingIDInput.Text = string.Empty;
            }
        }

        private void logoutButton_Click(object sender, RoutedEventArgs e)
        {
            GlobalSessionClass.LogOutTimeStamp = DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss tt");
            
            welcomeLable.Content = string.Empty;
        }
    }
}
