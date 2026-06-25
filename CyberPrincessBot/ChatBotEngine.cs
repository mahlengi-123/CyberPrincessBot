using System;
using CyberSafeBot.Models;
using CyberSafeBot.Services;
using System.Collections.Generic;
using System.Linq;

namespace CyberSafeBot
{
    internal class ChatBotEngine
    {
        private readonly User _user;
        private readonly ResponseManager _responseManager;
        private readonly TaskManager _taskManager;
        private readonly ActivityLogger _logger;
        private readonly QuizManager _quizManager;
        private string _lastTopic = "";
        private bool _waitingForFollowUp = false;
        private bool _waitingForName = true;

        // Sentiment keywords mapping
        private readonly Dictionary<string, string> _sentimentMap = new()
        {
            { "worried", "worried" }, { "anxious", "worried" }, { "scared", "worried" },
            { "curious", "curious" }, { "interested", "curious" }, { "tell me", "curious" },
            { "frustrated", "frustrated" }, { "confused", "frustrated" }
        };

        // Constructor
        public ChatBotEngine(User user, ResponseManager responseManager)
        {
            _user = user;
            _responseManager = responseManager;
            _waitingForName = true;
            _logger = new ActivityLogger();
            _taskManager = new TaskManager(_logger);
            _quizManager = new QuizManager();
        }

        public ActivityLogger Logger => _logger;

        private static string ExtractName(string input)
        {
            string lower = input.ToLower().Trim();

            
            if (lower.Contains("my name is"))
            {
                int index = lower.IndexOf("my name is") + 10;
                if (index < input.Length)
                    return input[index..].Trim();  
            }
            
            else if (lower.Contains("i am"))
            {
                int index = lower.IndexOf("i am") + 4;
                if (index < input.Length)
                    return input[index..].Trim();  
            }
            else if (lower.Contains("i'm"))
            {
                int index = lower.IndexOf("i'm") + 3;
                if (index < input.Length)
                    return input[index..].Trim();  
            }
            else
            {
                
                return input.Trim();
            }
            return null;
        }
        public string DetectSentiment(string input)
        {
            string lower = input.ToLower();
            foreach (var kv in _sentimentMap)
                if (lower.Contains(kv.Key))
                    return kv.Value;
            return "neutral";
        }

        private string ApplyEmpathy(string sentiment, string response)
        {
            return sentiment switch
            {
                "worried" => $"😟 *soft hug* I understand you're worried. {response}",
                "curious" => $"🤓 Ooh, love your curiosity! {response}",
                "frustrated" => $"😤 I know it's frustrating – let me help. {response}",
                _ => $"💖 {response}"
            };
        }

        

        private string HandleNLP(string input, string userName)
        {
            string lower = input.ToLower();

            // ---------- INTENT: Add Task ----------
            if (lower.Contains("add task") || lower.Contains("add a task") ||
                lower.Contains("create task") || lower.Contains("create a task") ||
                (lower.Contains("enable") && lower.Contains("2fa")) ||
                (lower.Contains("set up") && lower.Contains("2fa")))
            {
                string taskText = input;
                foreach (var phrase in new[] { "add task", "add a task", "create task", "create a task" })
                {
                    if (lower.Contains(phrase))
                    {
                        int index = lower.IndexOf(phrase) + phrase.Length;
                        if (index < input.Length)
                            taskText = input.Substring(index).Trim();
                        break;
                    }
                }

                if (taskText.StartsWith("-")) taskText = taskText.Substring(1).Trim();
                if (string.IsNullOrEmpty(taskText) || taskText.Length < 3)
                    return "Please specify a task description. Example: 'Add task - Enable two-factor authentication'";

                if (lower.Contains("2fa") || lower.Contains("two factor") || lower.Contains("two-factor"))
                {
                    taskText = "Enable two-factor authentication";
                }

                var result = _taskManager.AddTask(taskText, taskText, "No reminder set");
                _logger.Log($"NLP detected task intent: '{taskText}'");
                return result + " Reply 'remind me in X days' to set a reminder.";
            }

            // ---------- INTENT: Set Reminder ----------
            if (lower.Contains("remind me") || lower.Contains("reminder") ||
                lower.Contains("set a reminder") || lower.Contains("set reminder") ||
                lower.Contains("don't forget"))
            {
                string reminderText = input;
                foreach (var phrase in new[] { "remind me", "reminder", "set a reminder", "set reminder", "don't forget", "remind me to" })
                {
                    if (lower.Contains(phrase))
                    {
                        int index = lower.IndexOf(phrase) + phrase.Length;
                        if (index < input.Length)
                            reminderText = input.Substring(index).Trim();
                        break;
                    }
                }

                if (string.IsNullOrEmpty(reminderText) || reminderText.Length < 3)
                    return "What would you like me to remind you about?";

                string reminder = "No specific timeframe";
                if (lower.Contains("tomorrow")) reminder = "Tomorrow";
                else if (lower.Contains("in") && lower.Contains("day"))
                {
                    var words = lower.Split(' ');
                    for (int i = 0; i < words.Length - 1; i++)
                    {
                        if (words[i] == "in" && int.TryParse(words[i + 1], out int days))
                        {
                            reminder = $"In {days} days";
                            break;
                        }
                    }
                }
                else if (lower.Contains("next week")) reminder = "Next week";

                _taskManager.AddTask(reminderText, reminderText, reminder);
                _logger.Log($"Reminder set: '{reminderText}' on {reminder}");
                return $"✅ Reminder set for '{reminderText}' on {reminder}.";
            }

            // ---------- INTENT: Start Quiz ----------
            if (lower.Contains("start quiz") || lower.Contains("take quiz") ||
                lower.Contains("quiz me") || lower.Contains("test my knowledge") ||
                lower.Contains("play the game"))
            {
                _logger.Log("Quiz started");
                return "🎮 Starting quiz! Click the '🎮 Quiz' button above to begin. You'll get 12 cybersecurity questions!";
            }

            // ---------- INTENT: Show Log ----------
            if (lower.Contains("show activity log") || lower.Contains("what have you done") ||
                lower.Contains("what did you do") || lower.Contains("show log") ||
                lower.Contains("recent actions") || lower.Contains("what have you done for me"))
            {
                return _logger.GetLogDisplay(10, false);
            }

            // ---------- INTENT: Show More Log ----------
            if (lower.Contains("show more") && lower.Contains("log"))
            {
                return _logger.GetLogDisplay(10, true);
            }

            // ---------- INTENT: Show Tasks ----------
            if (lower.Contains("show tasks") || lower.Contains("view tasks") || lower.Contains("list tasks"))
            {
                var tasks = _taskManager.GetAllTasks();
                if (tasks.Count == 0)
                    return "You have no tasks yet. Add one with 'Add task - [description]'";

                string result = "📋 Your tasks:\n";
                foreach (var t in tasks)
                {
                    string status = t.IsComplete ? "✅ Completed" : "⏳ Pending";
                    string reminder = !string.IsNullOrEmpty(t.Reminder) && t.Reminder != "No reminder set"
                        ? $"(Reminder: {t.Reminder})" : "";
                    result += $"- {t.Title} {reminder} [{status}]\n";
                }
                return result;
            }

            return null; // No NLP intent detected
        }

        // ============================================================
        // MAIN GetReply METHOD (UPDATED with NLP)
        // ============================================================

        public string GetReply(string userInput)
        {
            // ----- FIRST: If waiting for name, capture it -----
            if (_waitingForName)
            {
                if (string.IsNullOrWhiteSpace(userInput))
                {
                    return "🌸 Please enter your name so we can chat!";
                }

                string name = ExtractName(userInput);
                if (string.IsNullOrEmpty(name))
                {
                    return "🌸 I didn't catch your name. Could you please tell me your name?";
                }

                _user.Name = name;
                _waitingForName = false;
                _logger.Log($"User introduced: {name}");
                return $"✨ Nice to meet you, {name}! ✨\nNow you can ask me anything about cybersecurity. Try: 'Tell me about phishing' or 'Add a task - Enable 2FA'.";
            }

            
            string nlpResult = HandleNLP(userInput, _user.Name);
            if (nlpResult != null)
                return ApplyEmpathy(_user.LastSentiment, nlpResult);

            string lower = userInput.ToLower();
            string sentiment = DetectSentiment(userInput);
            _user.LastSentiment = sentiment;

            
            if (lower.Contains("tell me more") || lower.Contains("another tip"))
            {
                if (!string.IsNullOrEmpty(_lastTopic))
                {
                    string tip = _responseManager.GetResponse(_lastTopic, sentiment, isFollowUp: true);
                    return ApplyEmpathy(sentiment, tip);
                }
                return "💬 What topic would you like more tips on? (phishing, password, scam, privacy, safe browsing, malware)";
            }

          
            if (lower.Contains("interested in"))
            {
                foreach (var topic in new[] { "phishing", "password", "scam", "privacy", "safe browsing", "malware" })
                {
                    if (lower.Contains(topic))
                    {
                        _user.FavoriteTopic = topic;
                        string tip = _responseManager.GetResponse(topic, sentiment, false);
                        _logger.Log($"Favourite topic set: {topic}");
                        return ApplyEmpathy(sentiment, $"Yay! I'll remember you're into {topic}. {tip}");
                    }
                }
            }

           
            string detectedTopic = null;
            foreach (var topic in new[] { "phishing", "password", "scam", "privacy", "safe browsing", "malware" })
            {
                if (lower.Contains(topic))
                {
                    detectedTopic = topic;
                    _logger.Log($"Keyword detected: {topic}");
                    break;
                }
            }

            if (detectedTopic != null)
            {
                _lastTopic = detectedTopic;
                _waitingForFollowUp = true;
                string response = _responseManager.GetResponse(detectedTopic, sentiment, false);
                string personalized = string.IsNullOrEmpty(_user.Name) ? response : $"{_user.Name}, {response.ToLower()}";
                return ApplyEmpathy(sentiment, personalized);
            }

            // 4. Personalised greeting using stored favourite topic
            if (!string.IsNullOrEmpty(_user.FavoriteTopic) && (lower.Contains("hello") || lower.Contains("hi")))
            {
                return $"🌸 Hi {_user.Name}! Since you love {_user.FavoriteTopic}, would you like a fresh tip? Just ask!";
            }

            // 5. Standard greetings & goodbye
            if (lower.Contains("hello") || lower.Contains("hi"))
                return $"🌸 Hello {_user.Name}! I can teach you about phishing, passwords, scams, privacy, safe browsing, or malware. You can also add tasks or start a quiz!";
            if (lower.Contains("bye") || lower.Contains("goodbye"))
                return "👋 Stay safe online, sunshine! Remember to think before you click. Bye bye!";

            // 6. Error handling (default response)
            return "🌸 Hmm, I didn't get that. Could you rephrase? Try: 'Tell me about phishing', 'Add a task - Enable 2FA', 'start quiz', or 'show activity log'.";
        }

        

        public string GetAsciiArt()
        {
            return @"
      ██████╗ ██╗   ██╗██████╗ ███████╗██████╗ 
  ██╔═══██╗╚██╗ ██╔╝██╔══██╗██╔════╝██╔══██╗
  ██║   ██║ ╚████╔╝ ██████╔╝█████╗  ██████╔╝
  ██║   ██║  ╚██╔╝  ██╔══██╗██╔══╝  ██╔══██╗
  ╚██████╔╝   ██║   ██║  ██║███████╗██║  ██║
   ╚═════╝    ╚═╝   ╚═╝  ╚═╝╚══════╝╚═╝  ╚═╝
                                             
              CYBER PRINCESS BOT
          [ SAFE • SMART • SECURE ]
";
        }
    }
}