
using System.Data.SqlClient;
using System.Data;
using System.Net;
using System.Net.Mail;
using System.Collections;

namespace FirstApp.Models
{
public class CurrentUser{
static SqlConnection sqlConnection=new SqlConnection("Data Source=deva-pc\\sqlexpress;Initial Catalog=BusBooking;Integrated Security=True");

public static string? Name{get;set;}
public static Guid UserId{get;set;}
public string? Date{get;set;}
public string? Destination{get;set;}
public string to="devanathan640@gmail.com";
public static string? display(String? Mail)
        {
            sqlConnection.Open();
            SqlCommand command=new SqlCommand("getUser",sqlConnection); 
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@EmailId",Mail);
            SqlDataReader sqlDataReader=command.ExecuteReader();
            while(sqlDataReader.Read()){
                Name=(string)sqlDataReader["Name"];
                UserId=(Guid)sqlDataReader["EmployeeId"];
            }
            sqlConnection.Close();
            return Name;
    }

public static string? bookBus(string? Destination,string Date)
        {   
                var dateTime = DateTime.Now;
                var shortDateValue = dateTime.ToShortDateString();
                string[] bookingDate = Date.Split("-");
                int[] booking = Array.ConvertAll(bookingDate, s => int.Parse(s));
                string[] todayDate = shortDateValue.Split("-");
                int[] today = Array.ConvertAll(todayDate, s => int.Parse(s));
                DateTime dt = Convert.ToDateTime(bookingDate[1]+"/"+bookingDate[2]+"/"+bookingDate[0]);
                DayOfWeek dow = dt.DayOfWeek;
                string day = dow.ToString();
                Console.WriteLine(day);
                Console.WriteLine(Date);
                Console.WriteLine(shortDateValue);
                if(day!="Sunday" && day!="Saturday"){
                if(booking[0]>=today[0]){
                    if(booking[1]>=today[1]){
                        if(booking[2]>=today[2]){
                            sqlConnection.Open();
                            SqlCommand command=new SqlCommand("bookBus",sqlConnection); 
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@name",Name);
                            command.Parameters.AddWithValue("@userId",UserId);
                            command.Parameters.AddWithValue("@destination",Destination);
                            command.Parameters.AddWithValue("@date",Date);
                            command.ExecuteNonQuery();
                            sqlConnection.Close();
                            return "Success";
                        } 
                    }
                }
                }
                if(day=="Sunday" || day=="Saturday"){
                        return "Leave";
                }
            return "Fail";                 
    }
BookingHistory user=new BookingHistory();
private static List <BookingHistory> History=new  List<BookingHistory> ();
public static IEnumerable GetHistory()  
        {  
                SqlCommand sqlCommand = new SqlCommand("GetCurrentUserHistory", sqlConnection);  
                sqlCommand.CommandType = CommandType.StoredProcedure;  
                sqlCommand.Parameters.AddWithValue("@Name",Name);
                sqlConnection.Open();  
                SqlDataReader rdr = sqlCommand.ExecuteReader();  
                while (rdr.Read())  
                {   BookingHistory user=new BookingHistory();
                    user.Date = rdr["Date"].ToString();  
                    user.Destination = rdr["Destination"].ToString();  
                    History.Add(user);  
                }  
                sqlConnection.Close(); 
                return History;
        } 


        static public string sendNotification(string to, Employee employee,CurrentUser currentUser){
                  
                string from, pass, messageBody;
                MailMessage message=new MailMessage();
                from="devanathan640@gmail.com";
                  pass  = "vzkgbrayouwkqrit";
                 messageBody="Hii, "+Name+"\nyour Bookings in Online Bus Booking System is Confirmed.\nYour Bookings on"+currentUser.Date+"in the Destination of "+currentUser.Destination;
                   message.To.Add(new MailAddress(to));
                   message.From=new MailAddress(from);
                     message.Body=messageBody;
                    message.Subject="password code";
                SmtpClient smtp=new SmtpClient("smtp.gmail.com");
        smtp.EnableSsl=true;
        smtp.Port=587;
        smtp.DeliveryMethod=SmtpDeliveryMethod.Network;
        smtp.Credentials=new NetworkCredential(from,pass);
        smtp.Send(message);
        return "sent";
        
}
    }
}