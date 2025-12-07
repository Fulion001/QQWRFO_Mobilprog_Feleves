using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace QQWRFO_Mobilprog_Feleves
{
    public interface IDatabaseService
    {
        Task<int> SaveUserAsync(User user);
        Task<User> GetUserAsync(string username);
        Task<List<User>> GetUsersAsync();
        Task<int> DeleteUserAsync(User user);
    }
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(250), Unique]
        public string Username { get; set; }

        public string HashedPassword { get; set; }

        public DateTime RegistrationDate { get; set; } = DateTime.Now;
    }
}
