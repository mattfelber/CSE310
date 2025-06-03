# Noobie Guide to TaskerPro

Hey there! 👋 This guide will help you understand how everything works in the project, starting from the very basics and building up to more complex concepts.

## 🎯 Part 1: Basic Blazor Pages

### 1. Basic Page Structure
```csharp
@page "/simple"  // This is the URL path

<h1>My First Blazor Page</h1>

<p>Hello, @name!</p>  // @name will show the value of the name variable

@code {
    private string name = "John";  // This is a variable
}
```
This is the simplest possible page. It just shows text and a variable.

### 2. Adding a Button
```csharp
@page "/simple"

<h1>My First Blazor Page</h1>

<p>Hello, @name!</p>

<button @onclick="ChangeName">Change Name</button>

@code {
    private string name = "John";

    private void ChangeName()
    {
        name = "Jane";  // This changes the name when the button is clicked
    }
}
```
Now we have:
- A button
- A method that runs when the button is clicked
- The ability to change what's displayed

### 3. Adding an Input
```csharp
@page "/simple"

<h1>My First Blazor Page</h1>

<input @bind="name" placeholder="Enter your name" />
<p>Hello, @name!</p>

@code {
    private string name = "John";
}
```
This shows:
- An input box
- `@bind="name"` connects the input to the name variable
- The page updates automatically when you type

### 4. Simple List
```csharp
@page "/simple"

<h1>My Simple List</h1>

<input @bind="newItem" placeholder="Add new item" />
<button @onclick="AddItem">Add</button>

<ul>
    @foreach (var item in items)
    {
        <li>@item</li>
    }
</ul>

@code {
    private string newItem = "";
    private List<string> items = new();

    private void AddItem()
    {
        if (!string.IsNullOrWhiteSpace(newItem))
        {
            items.Add(newItem);
            newItem = "";
        }
    }
}
```
This shows:
- A list of items
- How to add new items
- How to loop through items with `@foreach`

## 🎯 Part 2: Working with Models

Now let's see how to use models (classes that represent our data) from the `Models` folder.

### 1. Basic Model
First, create a file in the `Models` folder called `SimpleUser.cs`:
```csharp
namespace TaskerPro.Models
{
    public class SimpleUser
    {
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
    }
}
```

### 2. Using the Model in a Page
```csharp
@page "/simple"
@using TaskerPro.Models  // This lets us use our models

<h1>User Profile</h1>

<input @bind="user.Name" placeholder="Enter name" />
<input @bind="user.Email" placeholder="Enter email" />

<p>Hello, @user.Name!</p>
<p>Your email is: @user.Email</p>

@code {
    private SimpleUser user = new SimpleUser();
}
```
This shows:
- How to create a model
- How to use it in a page
- How to bind inputs to model properties

## 🎯 Part 3: Working with Services

Services handle the business logic of our app. Let's create a simple service.

### 1. Basic Service
Create a file in the `Services` folder called `UserService.cs`:
```csharp
using TaskerPro.Models;

namespace TaskerPro.Services
{
    public class UserService
    {
        private List<SimpleUser> _users = new();

        public void AddUser(SimpleUser user)
        {
            _users.Add(user);
        }

        public List<SimpleUser> GetUsers()
        {
            return _users;
        }
    }
}
```

### 2. Using the Service in a Page
```csharp
@page "/simple"
@using TaskerPro.Models
@using TaskerPro.Services
@inject UserService UserService  // This gives us access to our service

<h1>User Management</h1>

<input @bind="newUser.Name" placeholder="Enter name" />
<input @bind="newUser.Email" placeholder="Enter email" />
<button @onclick="AddUser">Add User</button>

<ul>
    @foreach (var user in users)
    {
        <li>@user.Name - @user.Email</li>
    }
</ul>

@code {
    private SimpleUser newUser = new SimpleUser();
    private List<SimpleUser> users = new();

    protected override void OnInitialized()
    {
        users = UserService.GetUsers();
    }

    private void AddUser()
    {
        UserService.AddUser(newUser);
        users = UserService.GetUsers();
        newUser = new SimpleUser();
    }
}
```
This shows:
- How to create a service
- How to inject it into a page
- How to use it to manage data

## 🎯 Part 4: Working with Authentication

Now let's see how to work with the authentication system.

### 1. Getting the Current User
```csharp
@page "/simple"
@using TaskerPro.Models
@using TaskerPro.Services
@inject UserService UserService
@inject AuthenticationStateProvider AuthProvider

<h1>Welcome</h1>

@if (isAuthenticated)
{
    <p>Hello, @userName!</p>
}
else
{
    <p>Please log in</p>
}

@code {
    private bool isAuthenticated;
    private string userName = "";

    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        isAuthenticated = user.Identity?.IsAuthenticated ?? false;
        userName = user.Identity?.Name ?? "";
    }
}
```
This shows:
- How to check if a user is logged in
- How to get the current user's name
- How to show different content based on login status

## 🎯 Part 5: Putting It All Together

Now let's create a simple task list that uses all these concepts:

### 1. Task Model
In `Models/TaskItem.cs`:
```csharp
namespace TaskerPro.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public bool IsCompleted { get; set; }
        public string UserId { get; set; } = "";
    }
}
```

### 2. Task Service
In `Services/TaskService.cs`:
```csharp
using TaskerPro.Models;

namespace TaskerPro.Services
{
    public class TaskService
    {
        private List<TaskItem> _tasks = new();

        public void AddTask(TaskItem task)
        {
            task.Id = _tasks.Count + 1;
            _tasks.Add(task);
        }

        public List<TaskItem> GetTasksForUser(string userId)
        {
            return _tasks.Where(t => t.UserId == userId).ToList();
        }

        public void CompleteTask(int taskId)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == taskId);
            if (task != null)
                task.IsCompleted = true;
        }
    }
}
```

### 3. Task List Page
In `Components/Pages/MyTasks.razor`:
```csharp
@page "/mytasks"
@using TaskerPro.Models
@using TaskerPro.Services
@inject TaskService TaskService
@inject AuthenticationStateProvider AuthProvider

<h1>My Tasks</h1>

@if (isAuthenticated)
{
    <div class="mb-3">
        <input @bind="newTaskTitle" placeholder="Enter new task" class="form-control" />
        <button @onclick="AddTask" class="btn btn-primary mt-2">Add Task</button>
    </div>

    <ul class="list-group">
        @foreach (var task in tasks)
        {
            <li class="list-group-item">
                @task.Title
                <button @onclick="() => MarkTaskComplete(task.Id)" 
                        class="btn btn-sm @(task.IsCompleted ? "btn-success" : "btn-secondary") float-end">
                    @(task.IsCompleted ? "✅" : "❌")
                </button>
            </li>
        }
    </ul>
}
else
{
    <p>Please log in to see your tasks</p>
}

@code {
    private string newTaskTitle = "";
    private string userId = "";
    private List<TaskItem> tasks = new();
    private bool isAuthenticated;

    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        isAuthenticated = user.Identity?.IsAuthenticated ?? false;
        if (isAuthenticated)
        {
            userId = user.Identity?.Name ?? "";
            tasks = TaskService.GetTasksForUser(userId);
        }
    }

    private async Task AddTask()
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

    private async Task MarkTaskComplete(int taskId)
    {
        TaskService.CompleteTask(taskId);
        tasks = TaskService.GetTasksForUser(userId);
        await InvokeAsync(StateHasChanged);
    }
}
```

This final example shows how everything works together:
- Models define our data structure
- Services handle our business logic
- Pages display the UI and handle user interaction
- Authentication ensures users can only see their own data

## 🎯 Tips for Learning

1. **Start Small**
   - Begin with the basic examples
   - Make sure you understand each concept before moving on
   - Try modifying the examples to see what happens

2. **Experiment**
   - Change the code and see what happens
   - Add new features to the examples
   - Break things and learn how to fix them

3. **Ask Questions**
   - If something isn't clear, ask!
   - Look at the code in the project
   - Try to understand how it all fits together

Remember: Everyone starts somewhere! Don't be afraid to make mistakes - that's how you learn! 🌟 