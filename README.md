 CyberSafe Bot – Cybersecurity Awareness Chatbot

Developer Information

- Name: Amahle Ndlela
- Student Number:ST10494955
- Subject:Programming 2B (PROG6221)
- Part:POE Part 3 (Final Submission)



Part 3 – Advanced Features (Final Submission)

 Features Added in Part 3

 1. Task Assistant with Reminders
- Add, view, complete, and delete cybersecurity tasks
- Reminder system (e.g., "remind me in 3 days")
- SQLite database integration using Entity Framework Core
- Full CRUD operations (Create, Read, Update, Delete) sync with database
- Task panel on the right side of the main window

 2. Cybersecurity Quiz (Mini-Game)
- 12 questions covering:
  - Phishing
  - Password safety
  - Safe browsing
  - Social engineering
  - Two-factor authentication
  - Malware and ransomware
  - Privacy settings
  - Data backup
- Multiple-choice and True/False question types
- One question at a time
- Immediate feedback with explanations after each answer
- Score tracking with final message

 3. NLP Simulation
- Intent detection for:
  - **Add Task:** "add task", "add a task", "create task", "enable", "set up"
  - **Set Reminder:** "remind me", "reminder", "set a reminder", "don't forget"
  - **Start Quiz:** "start quiz", "take quiz", "quiz me", "test my knowledge"
  - **Show Log:** "show activity log", "what have you done", "show log"
  - **Show More Log:** "show more"
  - **Show Tasks:** "show tasks", "view tasks", "list tasks"
- Uses `string.Contains()` for keyword detection
- Handles varied phrasings naturally

 4. Activity Log
- Logs all significant actions with timestamps:
  - Task added
  - Task marked complete
  - Task deleted
  - Reminder set
  - Quiz started
  - Quiz completed
  - NLP interactions
  - Keyword matches
- Shows last 5-10 entries on request
- "Show more" option for full history
- Stored in SQLite database

---

Part 2 – WPF GUI Application

Features Added in Part 2

 1. Graphical User Interface (GUI)
- WPF application with `StackPanel` chat layout
- Pink and purple themed interface
- ASCII art logo at the top
- Chat bubbles (user on right, bot on left)
- Voice greeting on startup
- Input validation for empty messages

 2. Keyword Recognition
- 6+ cybersecurity keywords:
  - `password`
  - `phishing`
  - `scam`
  - `privacy`
  - `safe browsing`
  - `malware`
- Random responses from `List<string>` lists

3. Sentiment Detection
- Detects sentiments: `worried`, `curious`, `frustrated`
- Empathetic responses with emojis
- Immediate tips without extra input

4. Memory and Recall
- Stores user's name
- Stores favourite topic (e.g., "I'm interested in privacy")
- Personalised responses using stored information

5. Conversation Flow
- "tell me more" continues the last topic
- "another tip" gives another random tip
- Tracks `_lastTopic` for follow-ups



 Part 1 – Console Application

Features Added in Part 1

 1. Text-Based Chatbot
- Console application with keyboard input
- User name collection and personalisation
- Basic cybersecurity tips for passwords, phishing, and safe browsing

 2. Voice Greeting
- Plays `greeting.wav` when the application starts
- Audio file stored in `Assets/Audio/`

 3. Basic Response System
- Answers questions like:
  - "How are you?"
  - "What's your purpose?"
  - "What can I ask you about?"
- Default response for unrecognised input

---

Full Project Overview

CyberSafe Bot is a WPF desktop application built in C# that educates users about cybersecurity awareness through an interactive chat interface. The bot responds to cybersecurity topics with keyword recognition, sentiment detection, random tips, memory recall, and natural conversation flow — all wrapped in a vibrant, girly theme with pastel colours and cute emojis.

---

Prerequisites

| Requirement | Details |
|-------------|---------|
| Operating System | Windows 10 or 11 |
| Framework | .NET 8.0 (Windows) |
| IDE | Visual Studio 2022 (with .NET Desktop Development workload) |
| NuGet Packages | Microsoft.EntityFrameworkCore.Sqlite, Microsoft.EntityFrameworkCore.Proxies |
| Audio (optional) | A `greeting.wav` file |

---

Installation Instructions

1. Clone the Repository
```bash
git clone https://github.com/mahlenqi-123/CyberSafeBot.git








   
