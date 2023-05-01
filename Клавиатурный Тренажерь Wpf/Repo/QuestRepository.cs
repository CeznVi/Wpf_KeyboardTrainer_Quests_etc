using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Text;
using System.Windows;
using System.Xml.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Клавиатурный_Тренажерь_Wpf.Entity
{
    class QuestRepository
    {
        public static void SaveData(QuestController controller, string fileName = $"/QuestRepo.xml", string dirPath = "../../../../SaveFile")
        {
            try
            {
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }

                XmlSerializer serializer = new(typeof(List<Quest>));

                using (Stream stream = File.Create(dirPath + fileName))
                {
                    serializer.Serialize(stream, controller.Quests);
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public static void LoadData(QuestController controller, string fileName = $"/QuestRepo.xml", string dirPath = "../../../../SaveFile")
        {
            try
            {
                if (!File.Exists(dirPath + fileName))
                    throw new FileNotFoundException($"Файл: {fileName} не создан.");

                XmlSerializer serializer = new(typeof(List<Quest>));

                using (Stream stream = File.OpenRead(dirPath + fileName))
                {
                    controller.Quests.AddRange((List<Quest>)serializer.Deserialize(stream));
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

    }


}
