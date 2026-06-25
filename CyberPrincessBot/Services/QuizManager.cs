using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CyberSafeBot.Models;

namespace CyberSafeBot.Services
{
    public class QuizManager
    {
            private readonly List<QuizQuestion> _questions;
            private int _currentIndex = 0;
            private int _score = 0;

            public QuizManager()
            {
                _questions = CreateQuestions();
            }

            private List<QuizQuestion> CreateQuestions()
            {
                return new List<QuizQuestion>
            {
                new QuizQuestion
                {
                    Question = "What should you do if you receive an email asking for your password?",
                    Options = new List<string> { "A) Reply with your password", "B) Delete the email", "C) Report the email as phishing", "D) Ignore it" },
                    CorrectAnswer = "C",
                    Explanation = "Report phishing emails. Legit companies never ask for passwords via email.",
                    IsTrueFalse = false
                },
                new QuizQuestion
                {
                    Question = "A strong password should contain:",
                    Options = new List<string> { "A) Only numbers", "B) Only letters", "C) Uppercase, lowercase, numbers, and symbols", "D) Your birthday" },
                    CorrectAnswer = "C",
                    Explanation = "A strong password uses a mix of character types to make it harder to crack.",
                    IsTrueFalse = false
                },
                new QuizQuestion
                {
                    Question = "Two-factor authentication (2FA) adds an extra layer of security.",
                    Options = new List<string> { "A) True", "B) False" },
                    CorrectAnswer = "A",
                    Explanation = "2FA requires a second verification step, making accounts more secure.",
                    IsTrueFalse = true
                },
                new QuizQuestion
                {
                    Question = "What does HTTPS mean?",
                    Options = new List<string> { "A) Hyper Text Transfer Protocol Secure", "B) High Tech Transfer Process Secure", "C) Hyper Transfer Text Protocol System", "D) None of the above" },
                    CorrectAnswer = "A",
                    Explanation = "HTTPS indicates a secure connection between your browser and the website.",
                    IsTrueFalse = false
                },
                new QuizQuestion
                {
                    Question = "Social engineering is a type of computer virus.",
                    Options = new List<string> { "A) True", "B) False" },
                    CorrectAnswer = "B",
                    Explanation = "Social engineering uses psychological manipulation to trick people into revealing information.",
                    IsTrueFalse = true
                },
                new QuizQuestion
                {
                    Question = "Malware can be hidden in email attachments.",
                    Options = new List<string> { "A) True", "B) False" },
                    CorrectAnswer = "A",
                    Explanation = "Malware often comes disguised as harmless files in email attachments.",
                    IsTrueFalse = true
                },
                new QuizQuestion
                {
                    Question = "What is the best way to protect your privacy on social media?",
                    Options = new List<string> { "A) Share everything", "B) Set profiles to private and limit personal info", "C) Post your location daily", "D) Accept all friend requests" },
                    CorrectAnswer = "B",
                    Explanation = "Keep your privacy settings strict and avoid sharing sensitive information publicly.",
                    IsTrueFalse = false
                },
                new QuizQuestion
                {
                    Question = "Ransomware attacks can be prevented by regularly backing up your data.",
                    Options = new List<string> { "A) True", "B) False" },
                    CorrectAnswer = "A",
                    Explanation = "Regular backups ensure you can restore your files without paying the ransom.",
                    IsTrueFalse = true
                },
                new QuizQuestion
                {
                    Question = "You should use the same password for all your accounts.",
                    Options = new List<string> { "A) True", "B) False" },
                    CorrectAnswer = "B",
                    Explanation = "Using the same password everywhere means if one account is hacked, all are vulnerable.",
                    IsTrueFalse = true
                },
                new QuizQuestion
                {
                    Question = "Which of these is a sign of a phishing email?",
                    Options = new List<string> { "A) Urgent language asking for immediate action", "B) Spelling mistakes", "C) Requests for personal information", "D) All of the above" },
                    CorrectAnswer = "D",
                    Explanation = "Phishing emails often use urgency, typos, and requests for sensitive information.",
                    IsTrueFalse = false
                },
                new QuizQuestion
                {
                    Question = "A VPN helps protect your privacy online.",
                    Options = new List<string> { "A) True", "B) False" },
                    CorrectAnswer = "A",
                    Explanation = "A VPN encrypts your internet connection, making it harder for others to see what you're doing online.",
                    IsTrueFalse = true
                },
                new QuizQuestion
                {
                    Question = "What should you do if you receive a suspicious link?",
                    Options = new List<string> { "A) Click it to see what happens", "B) Share it with friends", "C) Hover over it to check the URL before clicking", "D) Forward it to everyone" },
                    CorrectAnswer = "C",
                    Explanation = "Always hover over links to see the real URL before clicking.",
                    IsTrueFalse = false
                }
            };
            }

        public QuizQuestion GetCurrentQuestion()
            {
                if (_currentIndex < _questions.Count)
                    return _questions[_currentIndex];
                return null;
            }

            public bool SubmitAnswer(string answer)
            {
                var q = GetCurrentQuestion();
                if (q == null) return false;

                // Trim and compare (case insensitive)
                string userAnswer = answer.Trim().ToUpper();
                string correctAnswer = q.CorrectAnswer.Trim().ToUpper();

                bool correct = userAnswer == correctAnswer || userAnswer == correctAnswer + ")";

                if (correct) _score++;
                _currentIndex++;
                return correct;
            }

            // ✅ FIXED: Get feedback for the question that was just answered
            public string GetFeedback(bool correct)
            {
                // Get the question that was just answered (previous index)
                int questionIndex = _currentIndex - 1;

                // Check if the index is valid
                if (questionIndex < 0 || questionIndex >= _questions.Count)
                    return correct ? "✅ Correct!" : "❌ Incorrect.";

                var q = _questions[questionIndex];
                return correct ? $"✅ Correct! {q.Explanation}" : $"❌ Incorrect. {q.Explanation}";
            }

            public bool IsFinished() => _currentIndex >= _questions.Count;
            public int GetScore() => _score;
            public int GetTotal() => _questions.Count;

            public string GetFinalMessage()
            {
                int score = _score;
                int total = _questions.Count;
                if (score >= total * 0.8) return "🏆 Excellent! You're a cybersecurity expert! 🌟";
                if (score >= total * 0.6) return "👍 Good job! Keep learning to become an expert!";
                return "📚 Keep studying! Cybersecurity is important for everyone. You've got this! 💪";
            }

            public void ResetQuiz()
            {
                _currentIndex = 0;
                _score = 0;
            }
        }
    }