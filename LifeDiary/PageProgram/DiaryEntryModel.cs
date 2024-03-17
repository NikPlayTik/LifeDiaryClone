﻿using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeDiary.PageProgram
{
    // Класс для модели БД Записи
    public class DiaryEntryModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
