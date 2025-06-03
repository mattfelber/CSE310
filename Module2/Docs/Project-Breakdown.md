# TaskerPro Project Structure Guide

This guide explains the folder and file structure of the TaskerPro project.  
It's written for beginners and will help you understand what each part does and how everything fits together.

---

## 📁 Typical Folder Tree

```
TaskerPro/
│
├── Components/
│   ├── Pages/
│   │   ├── TestTaskManagement.razor
│   │   └── ... (other Blazor pages)
│   └── ... (shared UI components like headers and so on)
│
├── Data/
│   └── ApplicationDbContext.cs
│
├── Models/
│   └── TaskItem.cs
│
├── Services/
│   └── TaskService.cs
│
├── wwwroot/
│   └── ... (static files like CSS, JS, images)
│
├── Program.cs
├── App.razor
├── _Imports.razor
└── ... (other config and startup files)
```

---

## 📂 What Each Folder/File Does

### **Components/**
- **Purpose:** Holds all Blazor UI components.
- **Pages/**: Contains pages users can visit (like `/testtaskmanagement`, `/counter`, etc.).
- **Other components**: Reusable UI pieces (like nav menus, buttons).

### **Data/**
- **Purpose:** Contains classes for database access and configuration.
- **ApplicationDbContext.cs**: Sets up the connection to the database (using Entity Framework).

### **Models/**
- **Purpose:** Defines the data structures used in the app.
- **TaskItem.cs**: Represents a single task (with properties like Title, Description, UserId, etc.).

### **Services/**
- **Purpose:** Contains business logic and data management.
- **TaskService.cs**:  Provides methods to add, get, and update tasks. Manages tasks in local memory (right now they`re not persistent...you gotta set up the database part for that).

### **wwwroot/**
- **Purpose:** Stores static files (CSS, JavaScript, images) that are sent directly to the browser.

### **Program.cs**
- **Purpose:** The main entry point for the app. Sets up services, middleware, and configures how the app runs.

### **App.razor**
- **Purpose:** The root component for the Blazor app. Handles routing between pages.

### **_Imports.razor**
- **Purpose:** Makes common namespaces available to all components, so you don't have to keep adding `@using` everywhere.

---

## 🧩 How They Interact

- **User visits a page** (e.g., `/testtaskmanagement` - Components/Pages folder)
- The **Blazor component** (e.g., `TestTaskManagement.razor` - Models folder) loads and displays UI.
- The component uses a **service** (e.g., `TaskService` - Services folder) to get or update data. 
- The **service** uses **models** (e.g., `TaskItem` - Models folder) to represent the data.
- If needed, the **service** talks to the **database** (via `ApplicationDbContext` in `Data/`).
- **Static files** (CSS, JS) from `wwwroot/` make the app look and feel better.
- **Program.cs** wires everything together at startup.

---

## 📝 Example: Adding a Task

1. **User types a task and clicks "Add Task" on a Blazor page.**
   - The user enters a task title in an input box and clicks the "Add Task" button on a page like `TestTaskManagement.razor`.

2. **The page calls a method in `TaskService`.** (Services folder)
   - The Blazor component (the page) has a method (e.g., `AddTask`) that gets triggered by the button click.
   - This method creates a new `TaskItem` object with the entered title and the current user's ID, then calls `TaskService.AddTask()`.

3. **`TaskService` creates a new `TaskItem` and stores it.**
   - `TaskService` is a C# class that manages a list of tasks.
   - When you call `AddTask`, it adds the new `TaskItem` to a list in memory (a private `List<TaskItem>`).
   - **Right now, this means tasks are only stored temporarily in the server's memory.** If you restart the app, the tasks disappear.

   **Example:**
   ```csharp
   public void AddTask(TaskItem task)
   {
       task.Id = _tasks.Count + 1;
       _tasks.Add(task);
   }
   ```
   - Here, `_tasks` is a list that lives as long as the server is running.

4. **The page refreshes the task list to show the new task.**
   - After adding, the page asks `TaskService` for all tasks belonging to the current user and displays them.

---

### 🗄️ What about the database?

- **Currently:**  
  `TaskService` only stores tasks in memory. This is great for learning and testing, but not for real-world use, because all data is lost when the server restarts.

- **With a database:**  
  You would update `TaskService` to use the `ApplicationDbContext` (from the `Data/` folder) to save and retrieve tasks from a real database (like SQLite or SQL Server).
  
  **Example of saving to a database:**
  ```csharp
  public class TaskService
  {
      private readonly ApplicationDbContext _db;
      public TaskService(ApplicationDbContext db) { _db = db; }

      public void AddTask(TaskItem task)
      {
          _db.Tasks.Add(task);
          _db.SaveChanges();
      }

      public List<TaskItem> GetTasksForUser(string userId)
      {
          return _db.Tasks.Where(t => t.UserId == userId).ToList();
      }
  }
  ```
  - Now, tasks are stored permanently in the database, and each user's tasks are saved even if the server restarts.

---

**Summary:**  
- `TaskItem` is the C# object that represents a task.
- `TaskService` manages these objects, either in memory (for now) or in a database (in the future).
- The Blazor page interacts with `TaskService` to add and display tasks.
- When you connect to a database, your app becomes persistent and ready for real-world use
---

## 💡 Tips for Working on the Project

- **Frontend contributors**: Focus on `Components/` for UI and user experience.
- **Backend contributors**: Work in `Services/`, `Models/`, and `Data/` for logic and data.
- **Everyone**: Use `Program.cs` to register new services or configure the app.

---

## 📚 Summary: How Everything Connects

Here's a simple visual to help you understand how the main parts of your project work together:

```
[User]
  │
  ▼
[Pages / Components]  <--- UI: What the user sees and clicks
  │
  ▼
[Services]            <--- Logic: Services decide *how* things happen—like adding, updating, or deleting tasks. They contain methods that process data, enforce rules, and coordinate between the UI and the data. For example, `TaskService` knows how to add a new task, get all tasks for a user, or mark a task as complete.
  │
  ▼
[Models]              <--- Data Shape: What the data looks like (e.g., TaskItem). The "form" or "blueprint" of your data. Models define *what* your data looks like—what properties it has and what types they are. For example, `TaskItem` is a model with properties like `Id`, `Title`, `Description`, `IsCompleted`, and `UserId`. Models are used by services and pages to represent and transfer data.
  │
  ▼
[Data]                <--- Database Access: How the app talks to the database
```

- **wwwroot**: Sits alongside everything, providing static files (CSS, JS, images) to make the app look and feel good.
- **Program.cs**: The "main" file that ties all these layers together and starts the app.

---

### 🔗 How a User Action Flows

1. **User** interacts with a **Page** (e.g., clicks "Add Task").
2. The **Page** calls a method in a **Service** (e.g., `TaskService.AddTask()`).
3. The **Service** creates or updates a **Model** (e.g., a `TaskItem`).
4. The **Service** may use the **Data** layer to save or load data from the database.
5. The **Page** updates to show the latest data to the **User**.

This structure helps keep code organized, makes collaboration easier, and lets you grow the app as needed!