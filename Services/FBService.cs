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
            try
            {
                _connection.Open();   
            }
            catch(Exception ex)
            {
                throw new InvalidOperationException("Could not open database connection.", ex);
            }
        }
        public async Task<bool> SaveFeedback(Models.FB feedback)
        {
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
            return true;
        }
    }
}