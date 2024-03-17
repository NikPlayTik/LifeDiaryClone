using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeDiary
{
    public class Achievement
    {
        private int AchievementID { get; set; }
        private string Description { get; set; }
        private DateTime DateTime { get; set; }

        public Achievement(string description, DateTime dateTime)
        {
            Description = description;
            DateTime = dateTime;
        }

        public void AddAchievement(string description)
        {
            // добавление достижения
        }

        public void EditAchievement(string description)
        {
            // редактирование достижения
        }

        public void DeleteAchievement()
        {
            // удаление достижения
        }
    }

}
