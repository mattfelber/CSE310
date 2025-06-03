# Team-Suggestions for TaskerPro

This document is here to help you understand how everything fits together and what you can work on. It's written to be simple and clear for anyone learning.

## 🎯 Current Status

Right now, the app has:
- A basic task list that shows tasks with ✅ or ❌
- The ability to add new tasks
- User authentication (login system built-in recommended only for production)
- A simple way to store tasks in memory

### Database Connection
Right now, tasks disappear when you restart the app. To improve this:
- Connect to a database
- Save tasks there instead of in memory
- Load tasks from the database when the app starts

### Better UI
The current UI is very basic. Consider adding:
- Better styling
- Task categories
- Due dates
- Task descriptions
- Delete/edit buttons and methods

## 👥 How to Work on the Project

### For Frontend Contributors
1. Look in the `Components/` folder
2. You can create new pages for different views
3. Use the `TaskService` to get and update tasks
4. Example of using TaskService in a component:
```csharp
@inject TaskService TaskService
@using TaskerPro.Services
@using TaskerPro.Models

// Get tasks for current user
var tasks = TaskService.GetTasksForUser(userId);

// Add a new task
TaskService.AddTask(new TaskItem 
{
    Title = "New Task",
    IsCompleted = false,
    UserId = userId
});
```

### For Backend Contributors
1. Look in the `Services/` and `Models/` folders
2. You can add new features to `TaskService`
3. Create new models for additional data
4. Example of adding a new feature:
```csharp
// In TaskService.cs
public void DeleteTask(int taskId)
{
    var task = _tasks.FirstOrDefault(t => t.Id == taskId);
    if (task != null)
        _tasks.Remove(task);
}
```

### For Database Contributors
1. Look in the `Data/` folder
2. You'll need to:
   - Set up the database connection
   - Create tables for tasks
   - Update `TaskService` to use the database
3. Example of database changes:
```csharp
// In ApplicationDbContext.cs
public DbSet<TaskItem> Tasks { get; set; }

// In TaskService.cs
private readonly ApplicationDbContext _db;

public TaskService(ApplicationDbContext db)
{
    _db = db;
}

public List<TaskItem> GetTasksForUser(string userId)
{
    return _db.Tasks.Where(t => t.UserId == userId).ToList();
}
```

## 📝 Tips for Success

1. **Communication is Key**
   - Share what you're working on
   - Ask for help when stuck
   - Share your code changes

2. **Testing Your Changes**
   - Test your code before sharing
   - Make sure it works with other parts
   - Check if it breaks anything else

3. **Code Organization**
   - Keep related code together
   - Use clear names for files and methods
   - Add comments to explain tricky parts

4. **Common Issues**
   - If tasks don't show up, check if you're logged in
   - If buttons don't work, check the browser console
   - If you get errors, read them carefully - they often tell you what's wrong

## 🎓 Learning Resources

1. **Blazor Basics**
   - [Microsoft Learn: Blazor Tutorial](https://learn.microsoft.com/en-us/aspnet/core/blazor/tutorials/build-a-blazor-app)
   - [Blazor University](https://blazor-university.com/)

2. **C# Basics**
   - [Microsoft Learn: C# Tutorial](https://learn.microsoft.com/en-us/dotnet/csharp/tour-of-csharp/)
   - [W3Schools C# Tutorial](https://www.w3schools.com/cs/index.php)

3. **Database Basics**
   - [Microsoft Learn: Entity Framework](https://learn.microsoft.com/en-us/ef/core/get-started/overview/first-app)
   - [SQL Tutorial](https://www.w3schools.com/sql/)

## 🤝 Need Help?

1. Check the documentation in the `Docs/` folder
2. Ask for help from other contributors
3. Look at the example code in this document
4. Use the learning resources above

Remember: Everyone is learning! Don't be afraid to ask questions or make mistakes. That's how you learn! 🌟 