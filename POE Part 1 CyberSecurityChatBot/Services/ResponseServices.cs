using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CybersecurityAwarenessBotApp.Services
{
    internal class ResponseService
    {
        public string GetResponse(string input, string userName)
        {
            if (input.Contains("how are you"))
            {
                return $"I am doing well, {userName}. Thank you for asking.";
            }
            else if (input.Contains("what is your purpose") || input.Contains("what's your purpose"))
            {
                return "My purpose is to teach users about cybersecurity awareness and safe online behaviour.";
            }
            else if (input.Contains("what can i ask you about"))
            {
                return "You can ask me about passwords, phishing, safe browsing, scams, and suspicious links.";
            }
            else if (input.Contains("password"))
            {
                return "Use strong passwords with letters, numbers, and symbols. Avoid using personal details.";
            }
            else if (input.Contains("phishing"))
            {
                return "Phishing is when attackers trick you into giving away personal information through fake emails, messages, or websites.";
            }
            else if (input.Contains("safe browsing"))
            {
                return "Only visit trusted websites, avoid suspicious links, and check that a website uses HTTPS.";
            }
            else if (input.Contains("scam"))
            {
                return "Be careful of messages that create urgency or ask for money or private information.";
            }
            else if (input.Contains("hello") || input.Contains("hi"))
            {
                return $"Hello, {userName}! How can I help you today?";
            }
            else if (input.Contains("thank you") || input.Contains("thanks"))
            {
                return "You are welcome. Stay safe online!";
            }
            else
            {
                return "I did not quite understand that. Could you rephrase? You can ask about passwords, phishing, scams, or safe browsing.";
            }
        }
    }
}