using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeDiary.PageProgram
{
    public class DiaryAchievementsModel
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public DateTime Date { get; set; } // Дата достижения
        public string Description { get; set; } // Описание достижения
        public string Title { get; set; } // Заголовок достижения
        public int GoalId { get; set; } // ID связанной цели
    }
}
