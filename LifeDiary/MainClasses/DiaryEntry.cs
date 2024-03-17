using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeDiary
{
    public class DiaryEntry
    {
        private int RecordID { get; set; }
        private string Title { get; set; }
        private string Text { get; set; }
        private DateTime DateTime { get; set; }
        private string[] MediaFiles { get; set; }

        public DiaryEntry(string title, string text)
        {
            Title = title;
            Text = text;
            DateTime = DateTime.Now;
        }

        public void CreateEntry(string title, string text)
        {
            // создание записи в дневнике
        }

        public void EditEntry(string text)
        {
            // редактирование записи в дневнике
        }

        public void DeleteEntry()
        {
            // удаление записи из дневника
        }
    }

}
