using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Entities;

namespace Todo.Repository
{
    public static class UserRepository
    {
        // Список зарегистрированных пользователей
        private static readonly List<UserModel> Users = new List<UserModel>();

        // Свойство для текущего пользователя
        public static UserModel CurrentUser { get; set; }

        // Метод регистрации пользователя
        public static string Register(UserModel user)
        {
            // Проверка на уникальность email
            if (Users.Any(u => u.Email == user.Email))
            {
                return "Пользователь с таким email уже существует.";
            }

            Users.Add(user);
            return "Пользователь успешно зарегистрирован.";
        }

        // Метод авторизации пользователя
        public static UserModel Login(string email, string password)
        {
            CurrentUser = Users.FirstOrDefault(u => u.Email == email && u.Password == password);
            return CurrentUser;
        }
    }
}
