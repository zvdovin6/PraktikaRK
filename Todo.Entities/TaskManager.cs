using System.Collections.ObjectModel;
using System;
using Todo;

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
    public ObservableCollection<TaskModel> CompletedTasks { get; private set; }

    private TaskManager()
    {
        AllTasks = new ObservableCollection<TaskModel>();
        CompletedTasks = new ObservableCollection<TaskModel>();
    }



    public void SaveTask(TaskModel task)
    {
        if (!AllTasks.Contains(task))
        {
            AllTasks.Add(task);
        }
    }

    public event Action TasksUpdated;

    public void UncompleteTask(TaskModel task)
    {
        if (CompletedTasks.Contains(task))
        {
            CompletedTasks.Remove(task);
            AllTasks.Add(task);
            task.IsCompleted = false; // Обновляем статус задачи

            // Вызываем событие обновления задач
            TasksUpdated?.Invoke();
        }
    }

    public void CompleteTask(TaskModel task)
    {
        if (AllTasks.Contains(task))
        {
            AllTasks.Remove(task);         // Удаляем из основной коллекции
            task.IsCompleted = true;       // Отмечаем как завершённую
            CompletedTasks.Add(task);      // Добавляем в коллекцию завершённых

            // Вызываем событие обновления задач
            TasksUpdated?.Invoke();
        }
    }


}
