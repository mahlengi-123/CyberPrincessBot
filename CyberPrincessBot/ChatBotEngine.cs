using System;
using CyberSafeBot.Models;
using CyberSafeBot.Services;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSafeBot
{
    internal class ChatBotEngine
    {
        private readonly User _user;
        private readonly ResponseManager _responseManager;
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

        private string ExtractName(string input)
        {
            string lower = input.ToLower().Trim();

            // Pattern: "my name is Amhle"
            if (lower.Contains("my name is"))
            {
                int index = lower.IndexOf("my name is") + 10;
                if (index < input.Length)
                    return input.Substring(index).Trim();
            }
            // Pattern: "i am Amhle" or "i'm Amhle"
            else if (lower.Contains("i am"))
            {
                int index = lower.IndexOf("i am") + 4;
                if (index < input.Length)
                    return input.Substring(index).Trim();
            }
            else if (lower.Contains("i'm"))
            {
                int index = lower.IndexOf("i'm") + 3;
                if (index < input.Length)
                    return input.Substring(index).Trim();
            }
            else
            {
                // Assume the whole input is the name
                return input.Trim();
            }
            return null;
        }
        public ChatBotEngine(User user, ResponseManager responseManager)
        {
            _user = user;
            _responseManager = responseManager;
            _waitingForName = true;
        }

        public string DetectSentiment(string input)
        {
            string lower = input.ToLower();
            foreach (var kv in _sentimentMap)
                if (lower.Contains(kv.Key))
                    return kv.Value;
            return "neutral";
        }

        // Apply empathetic prefix based on sentiment (adds cute emojis)
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
                return $"✨ Nice to meet you, {name}! ✨\nNow you can ask me anything about cybersecurity. Try: 'Tell me about phishing' or 'I'm curious about passwords'.";
            }

            // ----- After name is known, process normally -----
            string lower = userInput.ToLower();
            string sentiment = DetectSentiment(userInput);
            _user.LastSentiment = sentiment;

            // 1. Follow-up: "tell me more" / "another tip"
            if (lower.Contains("tell me more") || lower.Contains("another tip"))
            {
                if (!string.IsNullOrEmpty(_lastTopic))
                {
                    string tip = _responseManager.GetResponse(_lastTopic, sentiment, isFollowUp: true);
                    return ApplyEmpathy(sentiment, tip);
                }
                return "💬 What topic would you like more tips on? (phishing, password, scam, privacy, safe browsing, malware)";
            }

            // 2. Remember favorite topic ("I'm interested in ...")
            if (lower.Contains("interested in"))
            {
                foreach (var topic in new[] { "phishing", "password", "scam", "privacy", "safe browsing", "malware" })
                {
                    if (lower.Contains(topic))
                    {
                        _user.FavoriteTopic = topic;
                        string tip = _responseManager.GetResponse(topic, sentiment, false);
                        return ApplyEmpathy(sentiment, $"Yay! I'll remember you're into {topic}. {tip}");
                    }
                }
            }

            // 3. Detect topic from keywords
            string detectedTopic = null;
            foreach (var topic in new[] { "phishing", "password", "scam", "privacy", "safe browsing", "malware" })
            {
                if (lower.Contains(topic))
                {
                    detectedTopic = topic;
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
                return $"🌸 Hello {_user.Name}! I can teach you about phishing, passwords, scams, privacy, safe browsing, or malware. What would you like?";
            if (lower.Contains("bye") || lower.Contains("goodbye"))
                return "👋 Stay safe online, sunshine! Remember to think before you click. Bye bye!";

            // 6. Error handling (default response)
            return "🌸 Hmm, I didn't get that. Could you rephrase? Try: 'Tell me about phishing' or 'I'm curious about passwords'.";
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

    

