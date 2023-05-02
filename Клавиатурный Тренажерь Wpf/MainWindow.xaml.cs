using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Клавиатурный_Тренажерь_Wpf.Entity;
using Клавиатурный_Тренажерь_Wpf.MyWindows;

namespace Клавиатурный_Тренажерь_Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private string _quest = string.Empty;
        private string _currentQuestResult = "";
        private int _indexCurrentLetter = 0;
        private int _countFails = 0;
        private int _countTotal = 0;
        private float _speed = 0.0f;

        QuestController _controllerQuests;

        private DispatcherTimer _taskTimer;
        private DateTime _startTime;
        private DateTime _endTime;
        private TimeSpan _elapsedSpan;      //_endTime - _startTime

        private List<Border> _buttons;
        public MainWindow()
        {
            InitializeComponent();

            _controllerQuests = new QuestController();
            _taskTimer = new DispatcherTimer();
            _taskTimer.Interval = new TimeSpan(1000);
            _taskTimer.Tick += _taskTimer_Tick;
            _buttons = new List<Border>();
            Button_StartGame.IsEnabled = false;

            foreach (var item in Grid_Keyboard.Children.OfType<Border>())
            {
                _buttons.Add(item);
            }
                        
            QuestRepository.LoadData(_controllerQuests);
            ComboBox_SelectDifficult.ItemsSource = _controllerQuests.GetAllDifficults();

        }

        private void _taskTimer_Tick(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Старт игры 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_StartGame_Click(object sender, RoutedEventArgs e)
        {
            if (!_taskTimer.IsEnabled)          //если таймер не включен
            {
                ComboBox_SelectDifficult.IsEnabled = false;

                _taskTimer.Start();
                _startTime = DateTime.Now;
                Button_StartGame.IsEnabled = false;
                Button_EndGame.IsEnabled = true;
                
                _indexCurrentLetter = 0;
                _countFails = 0;
                _countTotal = 0;
                _speed = 0.0f;

                RichTextBox_Quest.Document.Blocks.Clear();
                RichTextBox_Answer.Document.Blocks.Clear();

                RichTextBox_Quest.Document.Blocks.Add(new Paragraph(new Run(_quest)));
                RichTextBox_Quest.CaretPosition = RichTextBox_Quest.CaretPosition.DocumentStart;
                Label_ErrorInfo.Content = 0;
                Label_SpeedInfo.Content = 0;
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
                ComboBox_SelectDifficult.IsEnabled = true;
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            int[] ignorKey = { 2, 3, 6, 8, 116, 117, 119, 118, 70, 156, 27 };

            if (!_taskTimer.IsEnabled) return;

            int keyKode = Convert.ToInt32(e.Key);
            string keySymbol = e.Key.ToString();

            if (Keyboard.GetKeyStates(Key.CapsLock) == KeyStates.None)
            {
                keySymbol = keySymbol.ToLower();
            }

            if (keySymbol.Length == 1 || keyKode == 2 || keyKode == 18)      //либо символ либо пробел либо backspace
            {

                var startPosition = RichTextBox_Quest.CaretPosition;
                var endPosition = RichTextBox_Quest.CaretPosition.GetNextInsertionPosition(LogicalDirection.Forward);

                _currentQuestResult = "";
                if (Keyboard.IsKeyDown(Key.LeftShift) == true)
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

                if (_currentQuestResult.Length > 0 && _quest[_indexCurrentLetter] == _currentQuestResult[0]) //если пользователь угадал букву
                {
                    var textRange = new TextRange(startPosition, endPosition);
                    textRange.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.Green);
                    RichTextBox_Quest.CaretPosition = endPosition;

                    _indexCurrentLetter++;
                    RichTextBox_Answer.AppendText(_currentQuestResult);
                    
                }
                else                             //если не угадал                     
                {
                    var textRange = new TextRange(startPosition, endPosition);
                    textRange.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.Red);
                    _countFails++;
                    Label_ErrorInfo.Content = _countFails;
                }
                _countTotal++;
            }
            else if (keySymbol.Length > 1 && !ignorKey.Contains(keyKode))
            {
                var startPosition = RichTextBox_Quest.CaretPosition;
                var endPosition = RichTextBox_Quest.CaretPosition.GetNextInsertionPosition(LogicalDirection.Forward);

                _currentQuestResult = "";
                _currentQuestResult += e.Key.ToString();

                if (Keyboard.GetKeyStates(Key.LeftShift) == KeyStates.Down || Keyboard.GetKeyStates(Key.RightShift) == KeyStates.Down)
                {
                   if (_currentQuestResult.ToLower() == "oemcomma")
                      _currentQuestResult = "<";
                   else if (_currentQuestResult.ToLower() == "oemperiod")
                        _currentQuestResult = ">";
                   else if (_currentQuestResult.ToLower() == "oem3")
                        _currentQuestResult = "~";
                    else if (_currentQuestResult.ToLower() == "d1")
                        _currentQuestResult = "!";
                    else if (_currentQuestResult.ToLower() == "d2")
                        _currentQuestResult = "@";
                    else if (_currentQuestResult.ToLower() == "d3")
                        _currentQuestResult = "#";
                    else if (_currentQuestResult.ToLower() == "d4")
                        _currentQuestResult = "$";
                    else if (_currentQuestResult.ToLower() == "d5")
                        _currentQuestResult = "%";
                    else if (_currentQuestResult.ToLower() == "d6")
                        _currentQuestResult = "^";
                    else if (_currentQuestResult.ToLower() == "d7")
                        _currentQuestResult = "&";
                    else if (_currentQuestResult.ToLower() == "d8")
                        _currentQuestResult = "*";
                    else if (_currentQuestResult.ToLower() == "d9")
                        _currentQuestResult = "(";
                    else if (_currentQuestResult.ToLower() == "d0")
                        _currentQuestResult = ")";
                    else if (_currentQuestResult.ToLower() == "oemplus")
                        _currentQuestResult = "+";
                    else if (_currentQuestResult.ToLower() == "oemopenbrackets")
                        _currentQuestResult = "{";
                    else if (_currentQuestResult.ToLower() == "oem6")
                        _currentQuestResult = "}";
                    else if (_currentQuestResult.ToLower() == "oem5")
                        _currentQuestResult = "|";
                    else if (_currentQuestResult.ToLower() == "oem1")
                        _currentQuestResult = ":";
                    else if (_currentQuestResult.ToLower() == "oemquotes")
                        _currentQuestResult = "\"";
                }
                else 
                {
                    if (_currentQuestResult.ToLower() == "oem3")
                        _currentQuestResult = "`";
                    else if (_currentQuestResult.ToLower() == "d1")
                        _currentQuestResult = "1";
                    else if (_currentQuestResult.ToLower() == "d2")
                        _currentQuestResult = "2";
                    else if (_currentQuestResult.ToLower() == "d3")
                        _currentQuestResult = "3";
                    else if (_currentQuestResult.ToLower() == "d4")
                        _currentQuestResult = "4";
                    else if (_currentQuestResult.ToLower() == "d5")
                        _currentQuestResult = "5";
                    else if (_currentQuestResult.ToLower() == "d6")
                        _currentQuestResult = "6";
                    else if (_currentQuestResult.ToLower() == "d7")
                        _currentQuestResult = "7";
                    else if (_currentQuestResult.ToLower() == "d8")
                        _currentQuestResult = "8";
                    else if (_currentQuestResult.ToLower() == "d9")
                        _currentQuestResult = "9";
                    else if (_currentQuestResult.ToLower() == "d0")
                        _currentQuestResult = "0";
                    else if (_currentQuestResult.ToLower() == "oemminus")
                        _currentQuestResult = "-";
                    else if (_currentQuestResult.ToLower() == "oemplus")
                        _currentQuestResult = "=";
                    else if (_currentQuestResult.ToLower() == "oemcomma")
                        _currentQuestResult = ",";
                    else if (_currentQuestResult.ToLower() == "oemperiod")
                        _currentQuestResult = ".";
                    else if (_currentQuestResult.ToLower() == "oemopenbrackets")
                        _currentQuestResult = "[";
                    else if (_currentQuestResult.ToLower() == "oem6")
                        _currentQuestResult = "]";
                    else if (_currentQuestResult.ToLower() == "oem5")
                        _currentQuestResult = "\'";
                    else if (_currentQuestResult.ToLower() == "oem1")
                        _currentQuestResult = ";";
                    else if (_currentQuestResult.ToLower() == "oemquotes")
                        _currentQuestResult = "'";
                } 

                if (_currentQuestResult.Length > 0 && _quest[_indexCurrentLetter] == _currentQuestResult[0]) //если пользователь угадал букву
                {
                    var textRange = new TextRange(startPosition, endPosition);
                    textRange.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.Green);
                    RichTextBox_Quest.CaretPosition = endPosition;

                    _indexCurrentLetter++;
                    RichTextBox_Answer.AppendText(_currentQuestResult);

                }
                else                                                 
                {
                    var textRange = new TextRange(startPosition, endPosition);
                    textRange.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.Red);
                    _countFails++;
                    Label_ErrorInfo.Content = _countFails;
                }
                _countTotal++;
            }

            if (_indexCurrentLetter == _quest.Length)   /* ---------------------------------  Вызывает конец игры   */
            {
                EndGame();
                return;
            }

            Label_StatusInfo.Content = $"Time elapsed = {(DateTime.Now - _startTime).Seconds} sec.";

            ///перебор техкстбоксов и реализация подсветки 
            foreach (Border oneButton in _buttons)
            {
                if(((TextBlock)oneButton.Child).Text.ToUpper().Equals(keySymbol.ToUpper())) 
                {
                    IlluminateTheKey(oneButton);
                }
                else if (((TextBlock)oneButton.Child).Tag != null)
                {
                    if(((TextBlock)oneButton.Child).Tag.ToString().ToUpper().Equals(keySymbol.ToUpper()))
                    {
                        IlluminateTheKey(oneButton);
                    }    
                }
            }


        }

        private void EndGame()
        {
            Label_StatusInfo.Content = "Game over";
            Button_StartGame.IsEnabled = true;
            Button_EndGame.IsEnabled = false;
            ComboBox_SelectDifficult.IsEnabled = true;

            _taskTimer.Stop();
            _endTime = DateTime.Now;
            _elapsedSpan = _startTime - _endTime;
            
            _speed = ((float)(Math.Round(_quest.Length / _elapsedSpan.TotalMinutes) * -1));

            Label_SpeedInfo.Content = _speed.ToString();
            
            GameResult gameResult = new GameResult(
                ComboBox_SelectDifficult.SelectedItem.ToString(),
                _countFails,
                _speed,
                _elapsedSpan); 
            
            gameResult.ShowDialog();
        }

        /// <summary>
        /// Механизм подстветки клавиш
        /// </summary>
        /// <param name="oneButton">текстовый блок клавишы</param>
        private void IlluminateTheKey(Border oneButton)
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

        private void ComboBox_SelectDifficult_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Button_StartGame.IsEnabled = true;
            _quest = _controllerQuests.GetQuestByDifficults(ComboBox_SelectDifficult.SelectedItem.ToString());
        }

        private void Button_ShowResults_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("В следующей версии :)");
        }
    }
}

