using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Клавиатурный_Тренажерь_Wpf.Entity
{
    [Serializable]
    public class Quest
    {
        private string _text;
        private string _difficult;

        public string Text 
        { 
            get { return _text; } 
            
            set 
            {
                if (value.Length > 0)
                    _text = value;
            }   
        }
        public string Difficult
        {
            get
            {
                return _difficult;
            }
            set
            {
                if (value.Length > 0)
                    _difficult = value;
            }
        }

    }


}
