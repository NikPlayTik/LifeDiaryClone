using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeDiary
{
    public class Goals
    {
        private int GoalID { get; set; }
        private string Name { get; set; }
        private DateTime Deadline { get; set; }
        private float Progress { get; set; }

        public Achievement Achievement
        {
            get => default;
            set
            {
            }
        }

        public Goals(string name, DateTime deadline)
        {
            Name = name;
            Deadline = deadline;
        }

        public void SetGoal(string name, DateTime deadline)
        {
            // установка цели
        }

        public void UpdateProgress(float progress)
        {
            // обновление прогресса выполнения цели
        }

        public void MarkProgress(float progress)
        {
            // отметка прогресса выполнения цели
        }
    }

}
