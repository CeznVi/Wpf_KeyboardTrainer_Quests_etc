using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Клавиатурный_Тренажерь_Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private string[] _quests =
        {
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
            "Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.",
            "Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
        };
        private string _currentQuestResult = "";
        private int _indexQuest = -1;
        private int _indexCurrentLetter = 0;
        private int _countFails = 0;
        private int _countTotal = 0;
        private float _speed = 0.0f;

        private DispatcherTimer _taskTimer;
        private DateTime _startTime;
        private DateTime _endTime;
        private TimeSpan _elapsedSpan;      //_endTime - _startTime

        private List<Border> _buttons;
        public MainWindow()
        {
            InitializeComponent();
            _taskTimer = new DispatcherTimer();
            _taskTimer.Interval = new TimeSpan(1000);
            _taskTimer.Tick += _taskTimer_Tick;
            _buttons = new List<Border>();


            foreach (var item in Grid_Keyboard.Children.OfType<Border>())
            {
                _buttons.Add(item);
            }
        }

        private void _taskTimer_Tick(object sender, EventArgs e)
        {

        }

        private void Button_StartGame_Click(object sender, RoutedEventArgs e)
        {
            if (!_taskTimer.IsEnabled)          //если таймер не включен
            {
                _taskTimer.Start();
                _startTime = DateTime.Now;
                Button_StartGame.IsEnabled = false;
                Button_EndGame.IsEnabled = true;
                Random random = new Random();
                _indexQuest = random.Next(0, _quests.Length);


                _indexCurrentLetter = 0;
                _countFails = 0;
                _countTotal = 0;
                _speed = 0.0f;

                RichTextBox_Quest.Document.Blocks.Clear();
                RichTextBox_Quest.Document.Blocks.Add(new Paragraph(new Run(_quests[_indexQuest])));
                RichTextBox_Quest.CaretPosition = RichTextBox_Quest.CaretPosition.DocumentStart;
                Label_ErrorInfo.Content = 0;

            }
        }

        private void Button_EndGame_Click(object sender, RoutedEventArgs e)
        {
            if (_taskTimer.IsEnabled)
            {
                _taskTimer.Stop();
                _endTime = DateTime.Now;
                _elapsedSpan = _startTime - _endTime;
                Button_EndGame.IsEnabled = false;
                Button_StartGame.IsEnabled = true;
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (!_taskTimer.IsEnabled) return;

            int keyKode = Convert.ToInt32(e.Key);
            string keySymbol = e.Key.ToString();

            if (Keyboard.GetKeyStates(Key.CapsLock) == KeyStates.None)
            {
                keySymbol = keySymbol.ToLower();
            }

            if(keySymbol.Length == 1 || keyKode == 2 || keyKode == 18)      //либо символ либо пробел либо backspace
            {

                var startPosition = RichTextBox_Quest.CaretPosition;
                var endPosition = RichTextBox_Quest.CaretPosition.GetNextInsertionPosition(LogicalDirection.Forward);

                _currentQuestResult = "";
                if(Keyboard.IsKeyDown(Key.LeftShift) == true)
                {
                    keySymbol = keySymbol.ToUpper();
                }
                if (Keyboard.IsKeyDown(Key.RightShift) == true)
                {
                    keySymbol = keySymbol.ToUpper();
                }

                _currentQuestResult = keySymbol;

                if (keyKode == 18)
                {
                    _currentQuestResult = " ";
                }

                if (_currentQuestResult.Length > 0 && _quests[_indexQuest][_indexCurrentLetter] == _currentQuestResult[0]) //если пользователь угадал букву
                {
                    var textRange = new TextRange(startPosition, endPosition);
                    textRange.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.Green);
                    RichTextBox_Quest.CaretPosition = endPosition;

                    _indexCurrentLetter++;
                    RichTextBox_Answer.AppendText(_currentQuestResult);
                } else                             //если не угадал                     
                {
                    var textRange = new TextRange(startPosition, endPosition);
                    textRange.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.Red);
                    _countFails++;
                    Label_ErrorInfo.Content = _countFails;
                }
                _countTotal++;
            }


            Label_StatusInfo.Content = keySymbol;

            
            foreach (Border oneButton in _buttons)
            {
                if(((TextBlock)oneButton.Child).Text.ToUpper().Equals(keySymbol.ToUpper())) 
                {

                    Brush oldColor = oneButton.Background;
                    if(oldColor != Brushes.Black)
                    {
                        oneButton.Background = Brushes.Black;
                        ((TextBlock)oneButton.Child).Foreground = Brushes.White;
                        BlinkButtonBackgroundAsync(oldColor);
                    }
                    async Task BlinkButtonBackgroundAsync(Brush oldColor)
                    {
                        await Task.Delay(200);
                        oneButton.Background = oldColor;
                        ((TextBlock)oneButton.Child).Foreground = Brushes.Black;
                    }
                }
                else if (((TextBlock)oneButton.Child).Tag != null)
                {
                    if(((TextBlock)oneButton.Child).Tag.ToString().ToUpper().Equals(keySymbol.ToUpper()))
                    {
                        Brush oldColor = oneButton.Background;
                        if (oldColor != Brushes.Black)
                        {
                            oneButton.Background = Brushes.Black;
                            ((TextBlock)oneButton.Child).Foreground = Brushes.White;
                            BlinkButtonBackgroundAsync(oldColor);
                        }
                        async Task BlinkButtonBackgroundAsync(Brush oldColor)
                        {
                            await Task.Delay(200);
                            oneButton.Background = oldColor;
                            ((TextBlock)oneButton.Child).Foreground = Brushes.Black;
                        }
                    }    
                }

                        



            }
        }
    }
}

