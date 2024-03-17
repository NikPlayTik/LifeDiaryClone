using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeDiary.PageProgram
{
    public class DiaryGoalsModel
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public DateTime Deadline { get; set; }
        public DateTime StartDate { get; set; }
        public string Description { get; set; }
        public double Progress { get; set; }
        public int AchievementId { get; set; }

        public DiaryGoalsModel()
        {
            StartDate = DateTime.Now;
        }
    }
}
