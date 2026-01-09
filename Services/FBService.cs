using Microsoft.Data.SqlClient;
namespace FeedBack.Services
{
    public class FBService
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly SqlConnection _connection;
        private SqlCommand ?_command;
        private SqlDataReader ?sqlDataReader;
        public FBService(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            _connection = new SqlConnection(_connectionString);
            
        }
        public void Dispose()
        {
            _connection.Close();
            _connection.Dispose();
        }
        public async Task<bool> SaveFeedback(Models.FB feedback)
        {
            try
            {
                await _connection.OpenAsync();   
            }
            catch(Exception ex)
            {
                throw new InvalidOperationException("Could not open database connection.", ex);
            }
            string query = "INSERT INTO [FeedBack].[dbo].[FB] VALUES (@Name, @Email, @Fb, @Emojivalue,@CreatedAt)";
            _command=new SqlCommand(query,_connection);
            _command.Parameters.AddWithValue("@Name", feedback.Name ?? (object)DBNull.Value);
            _command.Parameters.AddWithValue("@Email", feedback.Email ?? (object)DBNull.Value);
            _command.Parameters.AddWithValue("@Fb", feedback.Fb ?? (object)DBNull.Value);
            _command.Parameters.AddWithValue("@Emojivalue", feedback.Emojivalue ?? (object)DBNull.Value);
            _command.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
            try
            {
                await _command.ExecuteNonQueryAsync();
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error saving feedback: " + ex.Message);
                return false;
            }
            await _connection.CloseAsync();
            return true;
        }
        public async Task<List<Models.FB>> getFeedbackByYear(string year)
        {
            try
            {
                await _connection.OpenAsync();   
            }
            catch(Exception ex)
            {
                throw new InvalidOperationException("Could not open database connection.", ex);
            }
            List<Models.FB> feedbacks = new List<Models.FB>();
            string query = "SELECT * FROM [FeedBack].[dbo].[FB] WHERE YEAR(CreatedDate) = @Year ORDER BY CreatedDate DESC";
            _command=new SqlCommand(query,_connection);
            _command.Parameters.AddWithValue("@Year", year);
            try
            {
                sqlDataReader=_command.ExecuteReaderAsync().Result;
                while(sqlDataReader.Read())
                {
                    Models.FB fb = new Models.FB
                    {
                        id = Convert.ToInt32(sqlDataReader["id"]),
                        Name = sqlDataReader["Name"] as string,
                        Email = sqlDataReader["Email"] as string,
                        Fb = sqlDataReader["Fb"] as string,
                        Emojivalue = sqlDataReader["Emojivalue"] as string,
                    };
                    feedbacks.Add(fb);
                }
                sqlDataReader.Close();
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error retrieving feedback: " + ex.Message);
            }
            await _connection.CloseAsync();
            return feedbacks;
        }
        public async Task<List<CountGroup>> GetCountGroup(string year){
                try
                {
                    await _connection.OpenAsync();   
                }
                catch(Exception ex)
                {
                    throw new InvalidOperationException("Could not open database connection.", ex);
                }
            string query="SELECT Emojivalue, COUNT(*) AS Total FROM [FeedBack].[dbo].[FB] WHERE YEAR(CreatedDate)=@Year GROUP BY Emojivalue order by count(*) DESC";
            List<CountGroup> countGroups=new List<CountGroup>();
            _command=new SqlCommand(query,_connection);
            _command.Parameters.AddWithValue("@Year", year);
            try
            {
                sqlDataReader=_command.ExecuteReaderAsync().Result;
                while(sqlDataReader.Read()){
                    CountGroup cg=new CountGroup{
                        EmojiValue=sqlDataReader["Emojivalue"] as string,
                        Total=Convert.ToInt32(sqlDataReader["Total"])
                    };
                    countGroups.Add(cg);
                }
                sqlDataReader.Close();
            }
            catch(Exception ex){
                Console.WriteLine("Error retrieving count groups: "+ex.Message);    
            }
            await _connection.CloseAsync();
            return countGroups;
        }
        public async Task<bool> DeleteFeedbackById(int id){
            try
            {
                await _connection.OpenAsync();   
            }
            catch(Exception ex)
            {
                throw new InvalidOperationException("Could not open database connection.", ex);
            }
            string query="DELETE FROM [FeedBack].[dbo].[FB] WHERE id=@Id";
            _command=new SqlCommand(query,_connection);
            _command.Parameters.AddWithValue("@Id",id);
            try{
                await _command.ExecuteNonQueryAsync();
            }
            catch(Exception ex){
                Console.WriteLine("Error deleting feedback: "+ex.Message);
                return false;
            }
            await _connection.CloseAsync();
            return true;
        }
        
    }
}