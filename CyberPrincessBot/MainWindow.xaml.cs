using CyberSafeBot.Data;
using CyberSafeBot.Models;
using CyberSafeBot.Services;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Task = CyberSafeBot.Models.Task;  // ✅ Alias to fix ambiguity with System.Threading.Tasks.Task

namespace CyberSafeBot
{
    public partial class MainWindow : Window
    {
        private const string UserBubbleKey = "UserBubble";
        private const string BotBubbleKey = "BotBubble";
        private readonly User _user;
        private readonly ResponseManager _responseManager;
        private readonly ChatBotEngine _chatbot;
        private readonly AudioPlayer _audioPlayer;
        private TaskManager _taskManager;
        private ActivityLogger _logger;

        public MainWindow()
        {
            InitializeComponent();

            // ✅ CREATE SQLITE DATABASE IF IT DOESN'T EXIST
            using (var db = new ApplicationDbContext())
            {
                db.EnsureDatabaseCreated();
            }

            _user = new User();
            _responseManager = new ResponseManager();
            _chatbot = new ChatBotEngine(_user, _responseManager);
            _audioPlayer = new AudioPlayer();
            _logger = _chatbot.Logger;
            _taskManager = new TaskManager(_logger);

            // Play greeting sound
            _audioPlayer.PlayGreeting(@"Assets\Audio\greeting.wav");

            // Display ASCII art
            txtAscii.Text = _chatbot.GetAsciiArt();

            // Bot introduction messages
            AppendMessage("Bot", "🌟 Hello! I am CyberSafe Bot – your personal cybersecurity assistant. I'm here to help you stay safe online. You can ask me about passwords, scams, privacy, phishing, safe browsing, and malware.\n\n💬 Please tell me your name.", Brushes.DarkMagenta);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshTaskList();
        }

        private void RefreshTaskList()
        {
            var tasks = _taskManager.GetAllTasks();
            lstTasks.ItemsSource = tasks;
        }

        private void BtnAddTask_Click(object sender, RoutedEventArgs e)
        {
            string title = txtTaskTitle.Text.Trim();
            string reminder = txtReminder.Text.Trim();

            if (string.IsNullOrEmpty(title))
            {
                MessageBox.Show("Please enter a task title!", "Missing Info", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrEmpty(reminder) || reminder == "e.g., remind me in 3 days")
                reminder = "No reminder set";

            _taskManager.AddTask(title, title, reminder);
            RefreshTaskList();
            txtTaskTitle.Clear();
            txtReminder.Text = "e.g., remind me in 3 days";
            AppendMessage("Bot", $"✅ Task added: '{title}'", Brushes.DarkBlue);
        }

        private void BtnComplete_Click(object sender, RoutedEventArgs e)
        {
            if (lstTasks.SelectedItem == null)
            {
                MessageBox.Show("Please select a task to complete!", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var task = (Task)lstTasks.SelectedItem;
            _taskManager.MarkAsComplete(task.Id);
            RefreshTaskList();
            AppendMessage("Bot", $"✅ Task marked complete: '{task.Title}'", Brushes.DarkBlue);
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (lstTasks.SelectedItem == null)
            {
                MessageBox.Show("Please select a task to delete!", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show("Are you sure you want to delete this task?", "Confirm Delete",
                                          MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                var task = (Task)lstTasks.SelectedItem;
                _taskManager.DeleteTask(task.Id);
                RefreshTaskList();
                AppendMessage("Bot", $"🗑️ Task deleted: '{task.Title}'", Brushes.DarkBlue);
            }
        }

        private void LstTasks_SelectionChanged(object sender, SelectionChangedEventArgs e) { }

        private void BtnQuiz_Click(object sender, RoutedEventArgs e)
        {
            var quizWindow = new QuizWindow(_logger);
            quizWindow.ShowDialog();
        }

        private void BtnLog_Click(object sender, RoutedEventArgs e)
        {
            string logDisplay = _logger.GetLogDisplay(10, false);
            AppendMessage("Bot", logDisplay, Brushes.DarkBlue);
        }

        private void BtnTasks_Click(object sender, RoutedEventArgs e)
        {
            string reply = _chatbot.GetReply("show tasks");
            AppendMessage("Bot", reply, Brushes.DarkBlue);
        }

        private void AppendMessage(string sender, string message, Brush color)
        {
            // Create a border + textblock to simulate chat bubbles inside StackPanel
            Border bubble = new Border();
            if (sender == "You")
                bubble.Style = (Style)FindResource("UserBubble");
            else
                bubble.Style = (Style)FindResource(BotBubbleKey);

            TextBlock text = new TextBlock();
            text.TextWrapping = TextWrapping.Wrap;
            text.FontSize = 14;
            text.Foreground = (sender == "You") ? Brushes.Black : Brushes.DarkSlateBlue;
            text.Inlines.Add(new Run($"{sender}: ") { FontWeight = FontWeights.Bold, Foreground = (sender == "You") ? Brushes.Purple : Brushes.Crimson });
            text.Inlines.Add(new Run(message));
            bubble.Child = text;

            chatStackPanel.Children.Add(bubble);

            var scroll = FindVisualChild<ScrollViewer>(this);
            scroll?.ScrollToEnd();
        }

        private void TxtUserInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                SendMessage();
        }

        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            SendMessage();
        }

        private async void SendMessage()
        {
            string userMsg = txtUserInput.Text.Trim();

            // Input validation: empty or whitespace
            if (string.IsNullOrWhiteSpace(userMsg))
            {
                AppendMessage("Bot", "🌸 Please type something so I can help you! Your message was empty.", Brushes.DarkOrange);
                txtUserInput.Clear();
                return;
            }

            AppendMessage("You", userMsg, Brushes.Black);
            txtUserInput.Clear();

            // Simulate typing delay
            await System.Threading.Tasks.Task.Delay(300);

            string botReply = _chatbot.GetReply(userMsg);
            AppendMessage("Bot", botReply, Brushes.DarkBlue);
        }

        // Helper to find ScrollViewer inside visual tree
        private static T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T found) return found;
                T deeper = FindVisualChild<T>(child);
                if (deeper != null) return deeper;
            }
            return null;
        }
    }
}