using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeDiary
{
    public class Users
    {
        private int UserID { get; set; }
        private string Login { get; set; }
        private string Password { get; set; }

        public DiaryEntry DiaryEntry
        {
            get => default;
            set
            {
            }
        }

        public Goals Goals
        {
            get => default;
            set
            {
            }
        }

        public Achievement Achievement
        {
            get => default;
            set
            {
            }
        }

        public Users(string login, string password)
        {
            Login = login;
            Password = password;
        }

        public void Register(string login, string password)
        {
            // регистрация пользователя
        }

        public void ChangePassword(string currentPassword, string newPassword)
        {
            // смена пароля пользователя
        }
    }

}
