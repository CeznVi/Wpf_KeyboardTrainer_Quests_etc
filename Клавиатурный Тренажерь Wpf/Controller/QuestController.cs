using System;
using System.Collections.Generic;
using System.Text;

namespace Клавиатурный_Тренажерь_Wpf.Entity
{
    public class QuestController
    {
        /*   -------------------------- Переменные --------------------------   */

        /// <summary>
        /// Хранилище квестов
        /// </summary>
        private List<Quest> _quests;

        /*   -------------------------- Конструкторы --------------------------   */

        /// <summary>
        /// Конструктор
        /// </summary>
        public QuestController()
        {
            _quests = new List<Quest>();
        }

        /*   -------------------------- Свойства --------------------------   */
        /// <summary>
        /// Дает возможность получить доступ к списку квестов
        /// </summary>
        public List<Quest> Quests { get { return _quests; } }


        /*   -------------------------- Методы --------------------------   */
        
        /// <summary>
        /// Добавить квест
        /// </summary>
        /// <param name="q">квест</param>
        public void AddQuest(Quest q)
        {
            _quests.Add(q);
        }

        /// <summary>
        /// Удалить квест
        /// </summary>
        /// <param name="q">Квест</param>
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
                if (!diff.Equals(item.Difficult))
                    diff.Add(item.Difficult);
            }

            return diff;
        }

        /// <summary>
        /// Возвращает 1 квест по сложности
        /// </summary>
        /// <param name="difficults"></param>
        /// <returns></returns>
        public string GetQuestByDifficults(string difficults)
        {
            return _quests.Find(q => q.Difficult.Equals(difficults)).Text;
        }


    }
}
