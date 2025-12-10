using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace QQWRFO_Mobilprog_Feleves
{
    public interface IDatabaseService
    {
        Task<List<User>> GetUsersAsync();
        Task<User> GetUserAsync(int id);
        Task CreateUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(User user);
    }
    public class SqliteDatabaseService : IDatabaseService
    {
        // SQLite kapcsolat objektum
        private SQLiteAsyncConnection _database;

        // Konstans az adatbázis fájlnévhez
        private const string DbName = "PuzzleGame.db3";

        // A .NET MAUI-nak szüksége van a platformspecifikus útvonalra
        private readonly string _dbPath = Path.Combine(FileSystem.AppDataDirectory, DbName);

        public SqliteDatabaseService()
        {
            // Inicializálás
            _database = new SQLiteAsyncConnection(_dbPath);
            InitializeAsync().Wait();
        }

        private async Task InitializeAsync()
        {
            // Ha a tábla még nem létezik, létrehozza
            await _database.CreateTableAsync<User>();
        }

        // --- CRUD Műveletek ---

        
    }
}
