using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeDiary.PageProgram
{
    public class DiaryGoalsDatabase
    {
        readonly SQLiteAsyncConnection _database;

        public DiaryGoalsDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<DiaryGoalsModel>().Wait();
        }

        public Task<List<DiaryGoalsModel>> GetGoalsAsync()
        {
            return _database.Table<DiaryGoalsModel>().ToListAsync();
        }

        public Task<int> SaveGoalAsync(DiaryGoalsModel goal)
        {
            if (goal.ID != 0)
            {
                return _database.UpdateAsync(goal);
            }
            else
            {
                return _database.InsertAsync(goal);
            }
        }

        public Task<int> DeleteGoalAsync(DiaryGoalsModel goal)
        {
            return _database.DeleteAsync(goal);
        }
    }
}
