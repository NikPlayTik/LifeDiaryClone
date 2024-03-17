using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace LifeDiary.PageProgram
{
    public class DiaryEntryDatabase
    {
        readonly SQLiteAsyncConnection _database;

        public DiaryEntryDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<DiaryEntryModel>().Wait();
        }
        public Task<List<DiaryEntryModel>> GetEntriesAsync()
        {
            return _database.Table<DiaryEntryModel>().ToListAsync();
        }
        public Task<int> SaveEntryAsync(DiaryEntryModel entry)
        {
            if (entry.Id != 0)
            {
                return _database.UpdateAsync(entry); // Обновляем запись, если она уже существует
            }
            else
            {
                return _database.InsertAsync(entry); // Вставляем новую запись, если это новая запись
            }
        }
        public Task<int> DeleteEntryAsync(DiaryEntryModel entry)
        {
            return _database.DeleteAsync(entry);
        }
    }
}
