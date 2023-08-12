
using System.Data.SqlClient;
using System.Data;
using System.Net;
using System.Net.Mail;
using System.ComponentModel.DataAnnotations;
using System.Collections;

namespace FirstApp.Models
{

public class Database{
      public bool Admin=false;
      [Required]
[DataType(DataType.Password)]
[MinLength(6,ErrorMessage="Code must contain 6 digits")]
        public string? userInput{get;set;}
  public string to="devanathan640@gmail.com@gmail.com";
  public bool KeepLoggedIn{get;set;}
        public static string? password{get;set;}
      private  static string? randomCode;
     
public string? Date{get;set;}
public string? Destination{get;set;}
        static SqlConnection sqlconnection=new SqlConnection("Data Source=deva-pc\\sqlexpress;Initial Catalog=BusBooking;Integrated Security=True");
        static public string login(Employee employee)
        {
                sqlconnection.Open();
                SqlCommand command=new SqlCommand("VerifyUser",sqlconnection); 
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@EmailId",employee.EmailId);
                command.Parameters.AddWithValue("@userPassword", employee.userPassword);
                int Count=Convert.ToInt32(command.ExecuteScalar());
                sqlconnection.Close();
                if(Count==1)
                {                 
                  return "success";         
                }
                else if ((employee.EmailId=="admin@gmail.com") && (employee.userPassword=="Admin@1234"))
                {
                  return "Admin"; 
                }
                  return "fails";  
    }
    static public string sendEmail(string to, Employee employee){
                sqlconnection.Open();
                SqlCommand command=new SqlCommand("AvailableUser",sqlconnection); 
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@EmailId",employee.EmailId);
                int Count=Convert.ToInt32(command.ExecuteScalar());
                sqlconnection.Close();
                if(Count==1)
                {
                  
                string from, pass, messageBody;
                Random rand =new Random();
                 randomCode=(rand.Next(999999)).ToString();
                MailMessage message=new MailMessage();
                from="abinashabinash711@gmail.com";
                  pass  = "vzkgbrayouwkqrit";
                 messageBody="Your Verification Code is "+randomCode;
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
        return "no-user";
        
}

static public string verifyCode(Database database){
  if(randomCode==database.userInput){
    
    return "code sent";
  }
  else{
    return "Invalid code";
  }
}
 public static string? sendPassword(string Mail)
        {
                Employee employee1=new Employee();
                sqlconnection.Open();
                SqlCommand command=new SqlCommand("sendPassword",sqlconnection); 
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@EmailId",Mail);
                SqlDataReader sqlDataReader=command.ExecuteReader();
                while(sqlDataReader.Read()){
                    password=Convert.ToString(sqlDataReader[0]);
                }
                sqlconnection.Close();
                return password;
    }
    public static IEnumerable<Database> GetHistory(string userName)  
        {  List<Database> History=new  List<Database> ();
                SqlCommand cmd = new SqlCommand("GetCurrentUserHistory", sqlconnection);  
                cmd.CommandType = CommandType.StoredProcedure;  
                cmd.Parameters.AddWithValue("@Name",userName);
                sqlconnection.Open();  
                SqlDataReader rdr = cmd.ExecuteReader();  
                while (rdr.Read())  
                {     Database user=new Database();
                    user.Date= rdr["Date"].ToString();  
                    user.Destination = rdr["Destination"].ToString();  
                    History.Add(user);  
                }  
                sqlconnection.Close(); 
                return History;
        }  
}
}