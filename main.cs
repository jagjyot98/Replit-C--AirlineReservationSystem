using System;
using System.Collections.Generic;

class Booking{
	public string name;
	public int seatNo;
	public int BookingID;

	private static Random random = new Random();
	
	public int newBooking(){
		BookingID = new Random().Next(100,500);
		Console.WriteLine("Enter your name: ");
		name = Console.ReadLine();
		Console.WriteLine("Enter seat number: ");
		seatNo = Convert.ToInt32(Console.ReadLine());
		return BookingID;
	}

	public void displayBooking(){
		Console.WriteLine("Booking ID: {0}", BookingID);
		Console.WriteLine("Name: " + name);
		Console.WriteLine("Seat number: " + seatNo);
	}
}

class Airline{
	List<Booking> BookingsList = new List<Booking>();

	public int bookingsCount(){
		return BookingsList.Count;
	}
	
	public void addNewBooking(){
		Booking newBooking = new Booking();
		int id = newBooking.newBooking();
		BookingsList.Add(newBooking);
		Console.ForegroundColor = ConsoleColor.Green;
		Console.WriteLine("Booking No. {0} Added successfully !",id);
		Console.ResetColor();
	}

	public void displayAllBookings(){
		if(BookingsList.Count != 0){
			for(int i=0;i<BookingsList.Count;i++){
				Console.WriteLine(i+1);
				BookingsList[i].displayBooking();
				Console.WriteLine("-----------------");
			}
		}else{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("No Bookings found in System !");
			Console.ResetColor();
		}
	}

	public void searchBooking(int bookingID){
		Boolean found = false;
			foreach(Booking booking in BookingsList){
				if(booking.BookingID == bookingID){
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine("\nMatch Found:");
					Console.ResetColor();
					booking.displayBooking();
					found = true;
				}
			}
		if(!found){
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("No Match Found !");
			Console.ResetColor();
		}
	}

	public void deleteBooking(int bookingID){
		Boolean found = false;
		foreach(Booking booking in BookingsList){
			if(booking.BookingID == bookingID){
				BookingsList.Remove(booking);
				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine("Booking Deleted successfully !");
				Console.ResetColor();
				found = true;
				break;
			}
		}
		if(!found){
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("No Match Found !");
			Console.ResetColor();
		}
	}
}

class Program {
  public static void Main (string[] args) {
    Airline airline = new Airline();
		
			while(true){
				Console.ForegroundColor = ConsoleColor.Blue;
				Console.WriteLine("\n		-----Airline Resrvation system-----\n");
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("No. of Bookings in the system: {0}\n",airline.bookingsCount());
				Console.ResetColor();
				Console.WriteLine("1. Add New Booking");
				Console.WriteLine("2. Display All Bookings");
				Console.WriteLine("3. Search Booking");
				Console.WriteLine("4. Delete Booking");
				Console.WriteLine("5. Exit");
				Console.Write("Enter your choice: ");
		
				switch(Console.ReadLine()){
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
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine("Thank you for using our system !");
					Console.ResetColor();
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