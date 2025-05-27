using SQLite;
using System;

namespace KalorieApp
{
    public class LocalDb
    {
        private const string DB_NAME = "local_kalorie_db.db3";
        private readonly SQLiteAsyncConnection _connection;

        public LocalDb()
        {
            _connection = new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, DB_NAME));
            _connection.CreateTableAsync<User>();
            _connection.CreateTableAsync<CalorieEntry>();

        }

        public async Task<List<User>> GetUsers()
        {
            return await _connection.Table<User>().ToListAsync();
        }

        public async Task Create(User user)
        {
            await _connection.InsertAsync(user);
        }

        public async Task Update(User user)
        {
            await _connection.UpdateAsync(user);
        }

        public async Task Delete(User user)
        {
            await _connection.DeleteAsync(user);
        }

        public Task<List<CalorieEntry>> GetCalorieEntriesForDate(DateTime date, int userId)
        {
            return _connection.Table<CalorieEntry>()
                .Where(e => e.UserId == userId && e.Date == date)
                .OrderBy(e => e.EntryTime)
                .ToListAsync();
        }

        public Task<User> GetUserById(int id)
        {
            return _connection.Table<User>()
                .Where(u => u.Id == id)
                .FirstOrDefaultAsync();
        }
        public async Task CreateCalorieEntry(CalorieEntry entry)
        {
            await _connection.InsertAsync(entry);
        }

        public async Task<int> GetTodaysCalories(int userId)
        {
            var today = DateTime.Today;
            var entries = await _connection.Table<CalorieEntry>()
                .Where(e => e.UserId == userId && e.Date == today)
                .ToListAsync();

            return entries.Sum(e => e.Calories);
        }

        public async Task<double> GetTodaysWater(int userId)
        {
            var today = DateTime.Today;
            var entries = await _connection.Table<CalorieEntry>()
                .Where(e => e.UserId == userId && e.Date == today)
                .ToListAsync();

            return entries.Sum(e => e.Water);
        }

        public async Task DeleteCalorieEntry(CalorieEntry entry)
        {
            await _connection.DeleteAsync(entry);
        }



    }
}
