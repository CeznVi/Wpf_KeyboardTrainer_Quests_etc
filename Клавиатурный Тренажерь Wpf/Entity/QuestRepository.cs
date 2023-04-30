using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Text;

namespace Клавиатурный_Тренажерь_Wpf.Entity
{
    class QuestRepository
    {
        /// <summary>
        /// Хранилище квестов
        /// </summary>
        private List<Quest> _quests;

        /// <summary>
        /// Конструктор
        /// </summary>
        public QuestRepository()
        {
            _quests = new List<Quest>();
        }

        ///-------------------------------------Методы---------------------
        
        //////////ДЕЙСТВИЯ С 1 квестом

        //Добавить квест
        public void AddQuest(Quest q)
        {
            _quests.Add(q);
        }
        /// <summary>
        /// Удалить квест
        /// </summary>
        /// <param name="q">Сущность квест</param>
        public void RemoveQuest(Quest q) 
        { 
            _quests.Remove(q);
        }
        /// <summary>
        /// Возвращает список сложностей
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllDifficults()
        {
            List<string> diff = new List<string>();

            foreach (Quest item in _quests)
            {
                if(!diff.Equals(item.Difficult))
                    diff.Add(item.Difficult);
            }

            return diff; 
        }
        /// <summary>
        /// Возвращает 1 квест по сложности
        /// </summary>
        /// <param name="difficults"></param>
        /// <returns></returns>
        public Quest GetQuestByDifficults(string difficults)
        {
            return _quests.Find(q => q.Difficult.Equals(difficults));
        }

        //////////ДЕЙСТВИЯ загрузка/сохранение квестов
        



        ///-------------------------------------Свойства---------------------
        public List<Quest> Quests { get { return _quests; } }


    }
}
