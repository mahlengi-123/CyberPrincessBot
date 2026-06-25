using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CyberSafeBot.Services;

namespace CyberSafeBot
{
    public partial class QuizWindow : Window
    {
        private readonly QuizManager _quizManager;
        private readonly ActivityLogger _logger;
        private bool _answered = false;

        public QuizWindow(ActivityLogger logger)
        {
            InitializeComponent();
            _logger = logger;
            _quizManager = new QuizManager();
            _logger.Log("Quiz started");
            ShowQuestion();
        }

        private void ShowQuestion()
        {
            var q = _quizManager.GetCurrentQuestion();
            if (q == null)
            {
                FinishQuiz();
                return;
            }

            txtProgress.Text = $"Question {_quizManager.GetScore() + 1} of {_quizManager.GetTotal()}";
            txtScore.Text = $"Score: {_quizManager.GetScore()}";
            txtQuestion.Text = q.Question;
            optionsList.ItemsSource = q.Options;
            btnSubmit.IsEnabled = true;
            btnNext.IsEnabled = false;
            txtFeedback.Text = "";
            _answered = false;
        }

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (_answered) return;

            // Force generation of item containers
            optionsList.ApplyTemplate();

            // Find the selected RadioButton
            RadioButton selected = null;

            for (int i = 0; i < optionsList.Items.Count; i++)
            {
                var container = optionsList.ItemContainerGenerator.ContainerFromIndex(i);
                if (container == null) continue;

                var presenter = container as ContentPresenter;
                if (presenter == null) continue;

                // Use the helper to find the RadioButton inside the ContentPresenter
                var radioButton = FindVisualChild<RadioButton>(presenter);
                if (radioButton != null && radioButton.IsChecked == true)
                {
                    selected = radioButton;
                    break;
                }
            }

            if (selected == null)
            {
                MessageBox.Show("Please select an answer!", "No Selection",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string selectedAnswer = selected.Content.ToString();
            // Extract the option letter (A, B, C, D) from the content (e.g., "A) Reply with your password")
            string answer = selectedAnswer.Split(')')[0].Trim();

            bool correct = _quizManager.SubmitAnswer(answer);
            _answered = true;

            string feedback = _quizManager.GetFeedback(correct);
            txtFeedback.Text = feedback;
            txtScore.Text = $"Score: {_quizManager.GetScore()}";

            btnSubmit.IsEnabled = false;
            btnNext.IsEnabled = true;

            _logger.Log($"Quiz - Question answered: {(correct ? "Correct" : "Incorrect")}");
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            ShowQuestion();
        }

        private void FinishQuiz()
        {
            int score = _quizManager.GetScore();
            int total = _quizManager.GetTotal();
            string message = _quizManager.GetFinalMessage();

            _logger.Log($"Quiz completed - Score: {score}/{total}");

            MessageBox.Show($"🏆 Quiz complete!\n\nScore: {score} out of {total}\n{message}",
                           "Quiz Complete", MessageBoxButton.OK, MessageBoxImage.Information);

            this.Close();
        }

        /// <summary>
        /// Helper method to find a child element of type T in the visual tree.
        /// </summary>
        private static T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T found)
                    return found;
                T deeper = FindVisualChild<T>(child);
                if (deeper != null)
                    return deeper;
            }
            return null;
        }
    }
}