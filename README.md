# CyberPrincessBot — Cybersecurity Awareness Chatbot

## Table of Contents
1. [Project Overview](#project-overview)
2. [Features](#features)
3. [Requirements](#requirements)
4. [Installation Instructions](#installation-instructions)
5. [Usage](#usage)
6. [Example Interactions](#example-interactions)
7. [Project Structure](#project-structure)
8. [GitHub Releases](#github-releases)
9. [YouTube Presentation](#youtube-presentation)
10. [References](#references)

---

## Project Overview
CyberPrincessBot is a WPF desktop application built in C# that educates users about cybersecurity awareness through an interactive chat interface. The bot responds to cybersecurity topics with keyword recognition, sentiment detection, random tips, memory recall, and natural conversation flow — all wrapped in a vibrant, girly theme with pastel colours and cute emojis.

**Developer:** Mahlengi  
**Student Number:** ST10494955 
**Subject:** Programming 2B  
**Part:** POE Part 2

---

## Features

- **🎨 GUI Design** – Pink/lavender/coral theme, ASCII art logo, StackPanel chat bubbles, voice greeting on startup.
- **🔑 Keyword Recognition** – Detects 6+ cybersecurity topics: password, phishing, scam, privacy, safe browsing, malware.
- **🎲 Random Responses** – Each topic has a list of tips; a random tip is shown every time.
- **💬 Conversation Flow** – Supports “tell me more” and “another tip” to continue the last topic.
- **🧠 Memory & Recall** – Remembers user’s name and favourite topic (e.g., “I’m interested in privacy”).
- **😊 Sentiment Detection** – Recognises worried, curious, frustrated and adjusts responses with empathy & immediate tips.
- **🛡️ Error Handling** – Graceful fallback for empty input, missing audio, or unrecognised queries.
- **📦 Clean Code** – Organised into Models, Services, and core logic; uses dictionaries, lists, and a delegate.

---

## Requirements

| Requirement | Details |
|------------|---------|
| Operating System | Windows 10 or 11 |
| Framework | .NET 6.0 or .NET 8.0 (Windows) |
| IDE | Visual Studio 2022 (with .NET Desktop Development workload) |
| Audio | A `greeting.wav` file (optional) |
| Voice Output (optional) | System.Speech (for text‑to‑speech) |

---

## Installation Instructions

1. **Clone the repository**  
   ```bash
   git clone https://github.com/mahlenqi-123/CyberPrincessBot.git
