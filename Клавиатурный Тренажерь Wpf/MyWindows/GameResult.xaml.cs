using System;
using System.Windows;

namespace Клавиатурный_Тренажерь_Wpf.MyWindows
{
    /// <summary>
    /// Логика взаимодействия для GameResult.xaml
    /// </summary>
    public partial class GameResult : Window
    {
        public GameResult(string lvl, int fails, float speed, TimeSpan d)
        {
            InitializeComponent();
            LabelGameResult_lvlText.Content = lvl;
            LabelGameResult_FailsText.Content = fails;
            LabelGameResult_SpeedText.Content = speed;
            LabelGameResult_GameDurationText.Content =  $"{d.Seconds * -1} sec";
        }

        private void Button_Ok_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
