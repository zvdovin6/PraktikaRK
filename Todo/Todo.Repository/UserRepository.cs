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
        private static List<UserModel> _users = new List<UserModel>();

        public static bool RegisterUser(UserModel user)
        {
            // Проверка на уникальность email
            if (_users.Any(u => u.Email == user.Email))
            {
                return false; // Email уже зарегистрирован
            }

            // Добавляем нового пользователя
            _users.Add(user);
            return true;
        }

        public static UserModel AuthenticateUser(string email, string password)
        {
            // Находим пользователя по email и паролю
            return _users.FirstOrDefault(u => u.Email == email && u.Password == password);
        }
    }
}
