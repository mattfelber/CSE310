# TaskerPro Backend Tasks: Beginner-Friendly Guide

Welcome to the backend documentation for the TaskerPro project!  
This guide explains the backend components, how they work, and how they interact with the rest of the site.  
It's written for beginners and includes examples and explanations.

---

## Table of Contents

1. [Overview](#overview)
2. [Key Backend Files](#key-backend-files)
   - [TaskItem Model](#taskitem-model)
   - [TaskService](#taskservice)
   - [TestTaskManagement.razor Page](#testtaskmanagementrazor-page)
   - [Program.cs (App Startup)](#programcs-app-startup)
3. [How They Work Together](#how-they-work-together)
4. [Example Workflow](#example-workflow)
5. [How This Fits With Other Contributors](#how-this-fits-with-other-contributors)
6. [Troubleshooting Tips](#troubleshooting-tips)

---

## Overview

Your backend work enables users to:
- Log in and see their own tasks.
- Add new tasks.
- See a list of their tasks.
- (Optionally) Mark tasks as complete.

This is done using C# classes, Blazor components, and dependency injection.

---

## Key Backend Files

### 1. TaskItem Model

**File:** `TaskerPro\Models\TaskItem.cs`

This is a C# class that represents a single task.  
Each task has an ID, title, description, completion status, and the user it belongs to.

```csharp
namespace TaskerPro.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
        public string UserId { get; set; } = string.Empty;
    }
}
```

**How it works:**  
- When a user adds a task, a `TaskItem` object is created and stored.
- The `UserId` property links the task to the logged-in user.

---

### 2. TaskService

**File:** `TaskerPro\Services\TaskService.cs`

This is a C# service class that manages all tasks in memory.  
It provides methods to add tasks, get tasks for a user, and mark tasks as complete.

```csharp
public class TaskService
{
    private readonly List<TaskItem> _tasks = new();

    public List<TaskItem> GetTasksForUser(string userId)
    {
        return _tasks.Where(t => t.UserId == userId).ToList();
    }

    public void AddTask(TaskItem task)
    {
        if (task == null || string.IsNullOrWhiteSpace(task.UserId))
            return;
        task.Id = _tasks.Count + 1;
        _tasks.Add(task);
    }

    public void CompleteTask(int taskId)
    {
        var task = _tasks.FirstOrDefault(t => t.Id == taskId);
        if (task != null)
            task.IsCompleted = true;
    }
}
```

**How it works:**  
- Stores all tasks in a list (in memory).
- Only returns tasks for the currently logged-in user.
- Lets you add and complete tasks.

---

### 3. TestTaskManagement.razor Page

**File:** `TaskerPro\Components\Pages\TestTaskManagement.razor`

This is a Blazor page where users can:
- See their user ID.
- Add a new task.
- See a list of their tasks.

**Key code:**

```razor
@page "/testtaskmanagement"
@inject TaskService TaskService
@inject AuthenticationStateProvider AuthProvider

<input @bind="newTaskTitle" placeholder="New task title" />
<button @onclick="AddTask">Add Task</button>
<button @onclick="TestConsole">Test Console</button>

<p>Current User ID: @userId</p>

@if (tasks.Any())
{
    <ul>
        @foreach (var task in tasks)
        {
            <li>
                @task.Title (@(task.IsCompleted ? "✅" : "❌"))
            </li>
        }
    </ul>
}
else
{
    <p>No tasks found.</p>
}

@code {
    string newTaskTitle = "";
    string userId = "";
    List<TaskItem> tasks = new();

    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        if (user.Identity?.IsAuthenticated == true)
        {
            userId = user.Identity.Name ?? "";
            tasks = TaskService.GetTasksForUser(userId);
        }
    }

    async Task AddTask()
    {
        if (!string.IsNullOrWhiteSpace(newTaskTitle))
        {
            TaskService.AddTask(new TaskItem
            {
                Title = newTaskTitle,
                IsCompleted = false,
                UserId = userId
            });

            tasks = TaskService.GetTasksForUser(userId);
            newTaskTitle = "";
            await InvokeAsync(StateHasChanged);
        }
    }

    void TestConsole()
    {
        Console.WriteLine("Test button clicked");
    }
}
```

**How it works:**  
- When the page loads, it gets the current user's ID and their tasks.
- When "Add Task" is clicked, it adds a new task for that user and refreshes the list.

---

### 4. Program.cs (App Startup)

**File:** `TaskerPro\Program.cs`

This file configures how your app starts up and which services are available.

**Key parts:**

```csharp
builder.Services.AddScoped<TaskService>(); // Register TaskService for dependency injection

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
```

**How it works:**  
- Registers `TaskService` so it can be used in Blazor components.
- Sets up Blazor Server so your pages are interactive.
- Maps your root component so the app runs as a Blazor Server app.

---

## How They Work Together

1. **User logs in** (handled by Identity, not your code).
2. **User navigates to `/testtaskmanagement`**.
3. **TestTaskManagement.razor** gets the user's ID and their tasks from `TaskService`.
4. **User adds a task** using the input and button.
5. **TaskService** stores the new task, linked to the user's ID.
6. **TestTaskManagement.razor** refreshes the list to show the new task.

---

## Example Workflow

1. **User visits `/testtaskmanagement` and logs in.**
2. The page shows their user ID and any tasks they have.
3. User types "Finish homework" and clicks "Add Task".
4. The new task appears in the list below, only for that user.

---

## How This Fits With Other Contributors

- **Frontend contributors** can use the `TaskService` to display or manage tasks in other pages/components.
- **Authentication contributors** provide the login system; the code uses the logged-in user's ID.
- **Database contributors** can later replace the in-memory `TaskService` with a database-backed version for persistence.
- **UI contributors** can style the task list or add features like editing and deleting.

---

## Troubleshooting Tips

- If tasks don't show up, make sure you are logged in.
- If button clicks do nothing, check that your app is running as a Blazor Server app and that you have `.AddInteractiveServerRenderMode()` in `Program.cs`.
- If you see errors, check the browser console and server output for details.

---

## Summary

You created a simple, interactive backend for managing user-specific tasks in a Blazor Server app.  
Your code is modular and ready for others to build on, whether that's adding a database, improving the UI, or integrating with other features.

---