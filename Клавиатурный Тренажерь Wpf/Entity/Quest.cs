using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Клавиатурный_Тренажерь_Wpf.Entity
{
    [Serializable]
    class Quest
    {
        private string _text;
        private string _difficult;
        private readonly string[] _AllDifficult ={"None","Easy", "Medium", "Hard" };

        public string Text 
        { 
            get { return _text; } 
            
            set 
            {
                if (value.Length > 0)
                    _text = value;
                else
                {
                    _text = string.Empty;
                    _difficult = "None";
                }
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
                if (_AllDifficult.Contains(value))
                    _difficult = value;
                else
                    _difficult = "None";
            }
        }
    }


}
