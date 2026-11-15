using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using RegexSearch.Core.Models;
using RegexSearch.Core.Services;

namespace RegexSearch.WPF
{
    public partial class MainWindow : Window
    {
        private readonly RecognitionService _recognitionService;

        public MainWindow()
        {
            InitializeComponent();
            _recognitionService = new RecognitionService();
            PatternText.Text = RecognitionService.SnilsPattern;
        }

        private void RegexButton_Click(object sender, RoutedEventArgs e)
        {
            string text = InputText.Text;
            List<MatchResult> results = _recognitionService.SearchByRegex(text);

            ResultsGrid.ItemsSource = results;
            SummaryText.Text = $"Режим Regex: найдено совпадений — {results.Count}.";
            StatusText.Text = "Поиск через регулярные выражения выполнен.";
            StatusText.Foreground = new SolidColorBrush(Color.FromRgb(40, 120, 200));
        }

        private void AutomatonButton_Click(object sender, RoutedEventArgs e)
        {
            string text = InputText.Text;
            List<MatchResult> results = _recognitionService.SearchByAutomation(text);

            ResultsGrid.ItemsSource = results;
            SummaryText.Text = $"Режим Автомата: найдено совпадений — {results.Count}.";
            StatusText.Text = "Поиск через конечный автомат выполнен.";
            StatusText.Foreground = new SolidColorBrush(Color.FromRgb(33, 135, 90));
        }

        private void CompareButton_Click(object sender, RoutedEventArgs e)
        {
            string text = InputText.Text;
            List<MatchResult> regexResults = _recognitionService.SearchByRegex(text);
            List<MatchResult> automatonResults = _recognitionService.SearchByAutomation(text);

            ResultsGrid.ItemsSource = automatonResults;

            bool isEqual = regexResults.Count == automatonResults.Count &&
                           regexResults.Zip(automatonResults, (r, a) =>
                               r.Value == a.Value && r.Position == a.Position && r.Length == a.Length)
                           .All(match => match);

            if (isEqual)
            {
                SummaryText.Text = $"Сравнение: совпадений — {regexResults.Count}. Результаты Regex и Автомата полностью идентичны.";
                StatusText.Text = "Алгоритмы работают согласованно!";
                StatusText.Foreground = new SolidColorBrush(Color.FromRgb(33, 135, 90));
            }
            else
            {
                SummaryText.Text = $"Расхождение! Regex нашел: {regexResults.Count}, Автомат нашел: {automatonResults.Count}.";
                StatusText.Text = "Обнаружены различия в результатах!";
                StatusText.Foreground = new SolidColorBrush(Colors.Crimson);
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            InputText.Clear();
            ResultsGrid.ItemsSource = null;
            SummaryText.Text = "Результаты поиска появятся здесь.";
            StatusText.Text = "Поля очищены.";
            StatusText.Foreground = new SolidColorBrush(Color.FromRgb(101, 116, 139));
        }
    }
}