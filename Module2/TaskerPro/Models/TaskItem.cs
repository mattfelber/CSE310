namespace TaskerPro.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }

        // Associate task with the logged-in user
        public string UserId { get; set; } = string.Empty;
    }
}

// This code defines a TaskItem model for a task management application.
// It includes properties for task ID, title, description, completion status, and the user ID of the task owner.
// The TaskItem class is used to represent individual tasks in the application, allowing for task management features such as adding, completing, 
// and retrieving tasks associated with specific users. 
// The UserId property links each task to a specific user, enabling personalized task management experiences.
// This model is typically used in conjunction with a service layer that manages task operations and a database context for persistence.
// The TaskItem class is a simple representation of a task in a task management system, allowing for easy manipulation and retrieval of task data.