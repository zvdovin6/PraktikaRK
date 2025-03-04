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
        public static TaskManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new TaskManager();
                }
                return _instance;
            }
        }

        public ObservableCollection<TaskModel> AllTasks { get; private set; }
        public ObservableCollection<TaskModel> CompletedTasks { get; private set; } // Коллекция для завершенных задач

        private TaskManager()
        {
            AllTasks = new ObservableCollection<TaskModel>();
            CompletedTasks = new ObservableCollection<TaskModel>(); // Инициализация коллекции завершенных задач
        }
        public void SaveTask(TaskModel task)
        {
            if (!AllTasks.Contains(task))
            {
                AllTasks.Add(task);
            }
        }

        public void CompleteTask(TaskModel task)
        {
            if (AllTasks.Remove(task))
            {
                task.IsCompleted = true;
                CompletedTasks.Add(task);
            }
        }

    }
}

