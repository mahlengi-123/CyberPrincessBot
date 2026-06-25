using CyberSafeBot.Data;
using CyberSafeBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CyberSafeBot.Services
{
    public class ActivityLogger
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        public void Log(string action)
        {
            var logEntry = new Log
            {
                Description = action,
                CreatedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm")
            };
            _db.Logs.Add(logEntry);
            _db.SaveChanges();
        }

        public List<Log> GetRecentLog(int count = 10)
        {
            return _db.Logs.OrderByDescending(l => l.Id).Take(count).ToList();
        }

        public List<Log> GetFullLog()
        {
            return _db.Logs.OrderByDescending(l => l.Id).ToList();
        }

        public int GetCount() => _db.Logs.Count();

        public string GetLogDisplay(int count = 10, bool showAll = false)
        {
            List<Log> logs = showAll ? GetFullLog() : GetRecentLog(count);

            if (logs.Count == 0)
                return "📋 No actions recorded yet.";

            string result = "📋 Here's a summary of recent actions:\n";
            for (int i = 0; i < logs.Count; i++)
            {
                result += $"{i + 1}. {logs[i].CreatedAt} - {logs[i].Description}\n";
            }

            if (!showAll && GetCount() > count)
            {
                result += $"\n📌 Showing {count} of {GetCount()} entries. Type 'show more' to see all.";
            }

            return result;
        }
    }
}