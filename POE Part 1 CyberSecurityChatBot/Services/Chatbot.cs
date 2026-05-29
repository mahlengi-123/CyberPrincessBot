using CybersecurityAwarenessBotApp.Models;
using CybersecurityAwarenessBotApp.Services;

namespace CybersecurityAwarenessBotApp
{
    internal class Chatbot
    {
        private readonly ConsoleUI _ui;
        private readonly ResponseService _responseService;
        private readonly AudioPlayer _audioPlayer;
        private readonly UserProfile _user;

        public Chatbot()
        {
            _ui = new ConsoleUI();
            _responseService = new ResponseService();
            _audioPlayer = new AudioPlayer();
            _user = new UserProfile();
        }

        public void Start()
        {
            _ui.DisplayHeader();
            _audioPlayer.PlayGreeting(@"Assets\Audio\greeting.wav");

            _ui.DisplayDivider();
            _ui.TypeText("Welcome to the Cybersecurity Awareness Bot.", ConsoleColor.Cyan);
            _ui.TypeText("I am here to help you stay safe online.", ConsoleColor.Green);
            _ui.DisplayDivider();

            AskUserName();
            RunChatLoop();
        }

        private void AskUserName()
        {
            string name = "";

            while (string.IsNullOrWhiteSpace(name))
            {
                _ui.TypeText("Please enter your name:", ConsoleColor.Yellow);
                name = Console.ReadLine() ?? "";

                if (string.IsNullOrWhiteSpace(name))
                {
                    _ui.TypeText("Name cannot be empty. Please try again.", ConsoleColor.Red);
                }
            }

            name = FormatName(name);
            _user.Name = name;

            _ui.DisplayDivider();
            _ui.TypeText($"Hello, {_user.Name}! Welcome to the Cybersecurity Awareness Bot.", ConsoleColor.Magenta);
            _ui.TypeText("You can ask me about password safety, phishing, scams, and safe browsing.", ConsoleColor.White);
            _ui.TypeText("Type 'exit' to close the chatbot.", ConsoleColor.DarkYellow);
            _ui.DisplayDivider();
        }

        private void RunChatLoop()
        {
            bool running = true;

            while (running)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write($"\n{_user.Name}: ");
                Console.ResetColor();

                string input = Console.ReadLine() ?? "";

                if (string.IsNullOrWhiteSpace(input))
                {
                    _ui.TypeText("Please enter something before pressing Enter.", ConsoleColor.Red);
                    continue;
                }

                string cleanedInput = input.Trim().ToLower();

                if (cleanedInput == "exit")
                {
                    _ui.TypeText($"Goodbye, {_user.Name}. Stay safe online!", ConsoleColor.Green);
                    running = false;
                    continue;
                }

                string response = _responseService.GetResponse(cleanedInput, _user.Name);
                _ui.TypeText(response, ConsoleColor.Cyan);
            }
        }

        private string FormatName(string name)
        {
            name = name.Trim().ToLower();
            return char.ToUpper(name[0]) + name.Substring(1);
        }
    }
}