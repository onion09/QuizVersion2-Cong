using Quiz2.Models.DBEntities;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Security.Principal;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Quiz2.Models.DBEntities;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace QuizProject.Dao
{
    public class AccountDao
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDBContext _dbContext;

        private readonly string _checkIfMatchQuery = "SELECT COUNT(*) FROM userInfo WHERE userName = @username AND password = @password";
        public AccountDao(IConfiguration configuration, ApplicationDBContext dbContext)
        {
            _configuration = configuration;
            _dbContext = dbContext;

        }

        public string GetPermissionByUserName(string userName)
        {
            string result = "";
            using (var conn = new SqlConnection(_configuration.GetConnectionString("MyConn")))
            {
                conn.Open();
                var cmd = new SqlCommand($"select role from userInfo where username = '{userName}'", conn);
                result = cmd.ExecuteScalar().ToString();
            }
            return result;
        }

        public bool AuthenticationCheck(string username, string password)
        {
            bool result = false;
            using (var conn = new SqlConnection(_configuration.GetConnectionString("MyConn")))
            {
                conn.Open();
                var cmd = new SqlCommand(_checkIfMatchQuery, conn);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);
                //varify user credential
                var count = Convert.ToInt32(cmd.ExecuteScalar());
                if (count > 0)
                    result = true;
            }
            return result;
        }
        public Account GetAccountByUserNamePassowrd (string username, string password)
        {

            string query = "select * from userInfo WHERE userName = @username AND password = @password";
            using (var conn = new SqlConnection(_configuration.GetConnectionString("MyConn")))
            {
                conn.Open();
                var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);
                var reader = cmd.ExecuteReader();
                reader.Read();
                Account account = new Account()
                {
                    username = username,
                    password = password,
                    userId = Convert.ToInt32(reader["UserId"]),
                    email = Convert.ToString(reader["Email"]),
                    firstName = Convert.ToString(reader["FirstName"]),
                    lastName = Convert.ToString(reader["LastName"]),
                    role = Convert.ToString(reader["Role"]),
                    address = Convert.ToString(reader["address"]),
                    phone = Convert.ToString(reader["phone"])
                };
                return account;
            }
        }
        public int AddNewAccount(Account newAccount)
        {
            string quary = $"INSERT INTO userInfo(UserName, Email, Password, FirstName, LastName, Role, address, phone) " +
                $"VALUES(@username, @email, @password, @firstname, @lastname,  @role, @address,@phone )";

            int rowAffected;
            using(var coon = new SqlConnection(_configuration.GetConnectionString("MyConn")))
            {
                coon.Open();
                var cmd = new SqlCommand(quary, coon);
                cmd.Parameters.AddWithValue("@username", newAccount.username);
                cmd.Parameters.AddWithValue("@password", newAccount.password);
                cmd.Parameters.AddWithValue("@email", newAccount.email);
                cmd.Parameters.AddWithValue("@firstname", newAccount.firstName);
                cmd.Parameters.AddWithValue("@lastname", newAccount.lastName);
                cmd.Parameters.AddWithValue("@role", newAccount.role);
                cmd.Parameters.AddWithValue("@address", newAccount.address);
                cmd.Parameters.AddWithValue("@phone", newAccount.phone);
                rowAffected = cmd.ExecuteNonQuery();
            }
            return rowAffected;
        }

        public int SubmitFeedback(string feedback)
        {
            string quary = $"insert into feedback(content) Values(@feedback)";

            int rowAffected;
            using (var coon = new SqlConnection(_configuration.GetConnectionString("MyConn")))
            {
                coon.Open();
                var cmd = new SqlCommand(quary, coon);
                cmd.Parameters.AddWithValue("@feedback", feedback);

                rowAffected = cmd.ExecuteNonQuery();
            }
            return rowAffected;
        }
        public List<Account> GetAllAccounts()
        {
            var accounts = _dbContext.Accounts.ToList();
            return accounts;
        }
        public IEnumerable<SelectListItem> GetUserIds()
        {
            var userIds = _dbContext.Accounts.Select(x => new SelectListItem { Value = x.userId.ToString(), Text = x.userId.ToString() }).ToList();
            if (userIds == null) return new List<SelectListItem>();
            return userIds;
        }
    }
}
