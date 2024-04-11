using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;
using System.Collections;

namespace Replit_C__AirlineReservationSystem
{
    internal class DatabaseOperations
    {
        string connectionString;        //"Server=127.0.0.1;Database=airlinesys;Uid=root;Pwd=;"
        
        List<Flight> FTlist = new List<Flight>();
        List<Booking> BKlist = new List<Booking>();   

        public void DatabaseOpereations(string Server, string databse, string uid, string passwrd)
        {
            this.connectionString = "Server=" + DBconsts.Server + ";Database=" + DBconsts.Database + ";Uid=" + DBconsts.Uid+ ";Pwd=" + DBconsts.Pwd + ";";
        }

        public List<Flight> readDatabaseFT()
        {
            FTlist.Clear();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM flights";                         //Query to read data from flights database
                MySqlCommand command = new MySqlCommand(query, connection);
                //command.Parameters.AddWithValue("@DatabaseTable", table);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Flight flight = new Flight();
                        flight.flightCode = reader.GetString(0);
                        flight.flightDestination = reader.GetString(1);
                        flight.seats = reader.GetString(2).ToCharArray();

                        FTlist.Add(flight);                      //adding each row of database as flight object in flights' system collection
                    }
                }
                connection.Close();
            }
            return FTlist;
        }

        public List<Booking> readDatabaseBK()
        {
            FTlist.Clear();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM bookings";                         //Query to read data from flights database
                MySqlCommand command = new MySqlCommand(query, connection);
                //command.Parameters.AddWithValue("@DatabaseTable", table);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Booking booking = new Booking();
                        booking.BookingID = reader.GetInt32(0);
                        booking.name = reader.GetString(1);
                        booking.seatNo = reader.GetInt32(2);
                        booking.flightcode = reader.GetString(3);

                        BKlist.Add(booking);                      //adding each row of database as flight object in flights' system collection
                    }
                }
                connection.Close();
            }
            return BKlist;
        }

        public string createNewBooking(Booking newBooking)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO bookings (BookingID, Name, SeatNoIndex, FlightCode) VALUES (@BookingID, @Name, @SeatNoIndex, @FlightCode)";

                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@BookingID", newBooking.BookingID);
                command.Parameters.AddWithValue("@Name", newBooking.name);
                command.Parameters.AddWithValue("@SeatNoIndex", newBooking.seatNo);
                command.Parameters.AddWithValue("@FlightCode", newBooking.flightcode);

                int rowsAffected = command.ExecuteNonQuery();
                connection.Close();

                if (rowsAffected > 0)
                {
                    readDatabaseBK();      //updating system collection with updated data
                }
                else
                {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(" Database Bk Updation Error !");
                    Console.ResetColor();
                    return;
                }
                
            }
        }

        public bool seatsDatabaseUpdation(char[] seatsUpdated, string flightcode)            //Updating Available Seats list in databse after every updation in seats
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE `flights` SET `AvailableSeats` = @AvailableSeats WHERE `flights`.`FlightCode` = @FlightCode";


                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@FlightCode", flightcode);
                command.Parameters.AddWithValue("@AvailableSeats", new string(seatsUpdated));

                int rowsAffected = command.ExecuteNonQuery();
                connection.Close();
                if (rowsAffected > 0)
                {
                    readDatabaseBK();      //updating system collection with updated data
                    readDatabaseFT();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }


    }
}
