using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace QQWRFO_Mobilprog_Feleves
{
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

        public Task<int> SaveUserAsync(User user)
        {
            if (user.Id != 0)
            {
                // Update
                return _database.UpdateAsync(user);
            }
            else
            {
                // Insert
                return _database.InsertAsync(user);
            }
        }

        public Task<User> GetUserAsync(string username)
        {
            // Lekérdezés a felhasználónév alapján
            return _database.Table<User>()
                            .Where(u => u.Username == username)
                            .FirstOrDefaultAsync();
        }

        public Task<List<User>> GetUsersAsync()
        {
            // Összes felhasználó lekérdezése
            return _database.Table<User>().ToListAsync();
        }

        public Task<int> DeleteUserAsync(User user)
        {
            // Felhasználó törlése az Id alapján
            return _database.DeleteAsync(user);
        }
    }
}
