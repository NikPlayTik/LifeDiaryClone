using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeDiary.PageProgram
{
    public class DiaryAchievementsDatabase
    {
        readonly SQLiteAsyncConnection _database;

        public DiaryAchievementsDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<DiaryAchievementsModel>().Wait();
        }

        public Task<List<DiaryAchievementsModel>> GetAchievementsAsync()
        {
            return _database.Table<DiaryAchievementsModel>().ToListAsync();
        }

        public Task<int> SaveAchievementAsync(DiaryAchievementsModel achievement)
        {
            if (achievement.ID != 0)
            {
                return _database.UpdateAsync(achievement);
            }
            else
            {
                return _database.InsertAsync(achievement);
            }
        }

        public Task<int> DeleteAchievementAsync(DiaryAchievementsModel achievement)
        {
            return _database.DeleteAsync(achievement);
        }
    }
}
