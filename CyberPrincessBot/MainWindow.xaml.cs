using CyberSafeBot.Models;
using CyberSafeBot.Services;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CyberSafeBot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly User _user;
        private readonly ResponseManager _responseManager;
        private readonly ChatBotEngine _chatbot;
        private readonly AudioPlayer _audioPlayer;

        public MainWindow()
        {
            InitializeComponent();
            _user = new User();
            _responseManager = new ResponseManager();
            _chatbot = new ChatBotEngine(_user, _responseManager);
            _audioPlayer = new AudioPlayer();

            // Play greeting sound
            _audioPlayer.PlayGreeting(@"Assets\Audio\greeting.wav");

            // Display ASCII art
            txtAscii.Text = _chatbot.GetAsciiArt();

            // Bot introduction messages
            AppendMessage("Bot", "🌟 Hello! I am CyberSafe Bot – your personal cybersecurity assistant. I'm here to help you stay safe online. You can ask me about passwords, scams, privacy, phishing, safe browsing, and malware.\n\n💬 Please tell me your name.", Brushes.DarkMagenta);
        }

        private void AppendMessage(string sender, string message, Brush color)
        {
            // Create a border + textblock to simulate chat bubbles inside StackPanel
            Border bubble = new Border();
            if (sender == "You")
                bubble.Style = (Style)FindResource("UserBubble");
            else
                bubble.Style = (Style)FindResource("BotBubble");

            TextBlock text = new TextBlock();
            text.TextWrapping = TextWrapping.Wrap;
            text.FontSize = 14;
            text.Foreground = (sender == "You") ? Brushes.Black : Brushes.DarkSlateBlue;
            text.Inlines.Add(new Run($"{sender}: ") { FontWeight = FontWeights.Bold, Foreground = (sender == "You") ? Brushes.Purple : Brushes.Crimson });
            text.Inlines.Add(new Run(message));
            bubble.Child = text;

            chatStackPanel.Children.Add(bubble);

            // Auto-scroll
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

            // ✅ Input validation: empty or whitespace
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

        
    
