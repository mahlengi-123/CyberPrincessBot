using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CyberSafeBot.Models;

namespace CyberSafeBot.Services
{
    public class TaskManager
    {
        private readonly TaskStorageHelper _storage;
        private readonly ActivityLogger _logger;

        public TaskManager(ActivityLogger logger)
        {
            _storage = new TaskStorageHelper();
            _logger = logger;
        }

        public string AddTask(string title, string description, string reminder)
        {
            _storage.AddTask(title, description, reminder);
            _logger.Log($"Task added: '{title}' (Reminder: {reminder})");
            return $"✅ Task added: '{title}'. Would you like to set a reminder?";
        }

        public List<Models.Task> GetAllTasks()
        {
            return _storage.LoadTasks();
        }

        public string MarkAsComplete(int id)
        {
            _storage.MarkAsComplete(id);
            _logger.Log($"Task marked complete: ID {id}");
            return "✅ Task marked as complete!";
        }

        public string DeleteTask(int id)
        {
            _storage.DeleteTask(id);
            _logger.Log($"Task deleted: ID {id}");
            return "🗑️ Task deleted successfully!";
        }
    }
}
