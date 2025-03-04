using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.Entities
{
    public class TaskManager
    {
        private static TaskManager _instance;
        public static TaskManager Instance => _instance ??= new TaskManager();

        public ObservableCollection<TaskModel> AllTasks { get; private set; }

        private TaskManager()
        {
            AllTasks = new ObservableCollection<TaskModel>();
        }
    }

}
