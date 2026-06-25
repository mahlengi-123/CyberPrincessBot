using CyberSafeBot.Data;
using CyberSafeBot.Models;
using System.Collections.Generic;
using System.Linq;

namespace CyberSafeBot.Services
{
    public class TaskStorageHelper
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        public List<Models.Task> LoadTasks()
        {
            return _db.Tasks.ToList();
        }

        public void AddTask(Models.Task task)
        {
            _db.Tasks.Add(task);
            _db.SaveChanges();
        }

        public void AddTask(string title, string description, string reminder)
        {
            var task = new Models.Task
            {
                Title = title,
                Description = description,
                Reminder = reminder,
                IsComplete = false
            };
            _db.Tasks.Add(task);
            _db.SaveChanges();
        }

        public void MarkAsComplete(int id)
        {
            var task = _db.Tasks.FirstOrDefault(t => t.Id == id);
            if (task != null)
            {
                task.IsComplete = true;
                _db.Tasks.Update(task);
                _db.SaveChanges();
            }
        }

        public void DeleteTask(int id)
        {
            var task = _db.Tasks.FirstOrDefault(t => t.Id == id);
            if (task != null)
            {
                _db.Tasks.Remove(task);
                _db.SaveChanges();
            }
        }
    }
}