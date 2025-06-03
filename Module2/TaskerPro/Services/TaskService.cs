using TaskerPro.Models;

// This code defines a TaskService class that manages a list of tasks.
// It provides methods to get tasks for a specific user, get all tasks, add a new task, and mark a task as completed.

namespace TaskerPro.Services
{
    public class TaskService
    {
        private readonly List<TaskItem> _tasks = new();

        public List<TaskItem> GetTasksForUser(string userId)
        {
            return _tasks.Where(t => t.UserId == userId).ToList();
        }

        public List<TaskItem> GetTasks()
        {
            return _tasks;
        }

        public void AddTask(TaskItem task)
        {
            if (task == null)
            {
                Console.WriteLine("Task is null, cannot add.");
                return;
            }
            if (string.IsNullOrWhiteSpace(task.UserId))
            {
                Console.WriteLine("UserId is empty, cannot add task.");
                return;
            }
            Console.WriteLine($"Adding task: {task.Title} for user: {task.UserId}");
            task.Id = _tasks.Count + 1;
            _tasks.Add(task);
            Console.WriteLine($"Task count for {task.UserId}: {GetTasksForUser(task.UserId).Count}");
        }

        public void CompleteTask(int taskId)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == taskId);
            if (task != null)
                task.IsCompleted = true;
        }

        public void UncheckTask(int taskId)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == taskId);
            if (task != null)
                task.IsCompleted = false;
        }
    }
}
