// Класс для заполнения базы данных значений из лаборатории

using System;
using System.IO;

namespace WebAppFixed.Models
{
    public class FillDb
    {
        public static void FillLabVals(string path)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(path); // Переменная для работы с файлами
            string[] linesFromFile = new string[3];

            if (dirInfo.Exists)
            {
                byte[] readArray;
                using (FileStream read = File.OpenRead(path + $@"\{DateTime.Now.Day}.txt"))
                {
                    readArray = new byte[read.Length]; // Преобразуем строку в байты
                    read.Read(readArray, 0, readArray.Length); // Считываем данные
                    string textFromFile = System.Text.Encoding.Default.GetString(readArray); // Декодируем байты в строку

                    #region File strings of laboratory numbers into array of strings

                    for (int i = 0; i < 3; i++)
                    {
                        int indexOfNewLine = textFromFile.IndexOf('\n');
                        linesFromFile[i] = textFromFile.Substring(0, indexOfNewLine-1);
                        textFromFile = textFromFile.Remove(0, indexOfNewLine + 1);
                    }

                    #endregion

                    #region Write this array of strings into Data Base 

                    using (ValuesContext dbV = new ValuesContext()) 
                    {
                        int lastIndexOfNewNum;
                        int startIndexOfNewNum;
                        string[] texttodb = new string[3];
                        for (int i = 0; i < 3; i++)
                        {
                            for (int j = 0; j < texttodb.Length; j++)
                            {
                                startIndexOfNewNum = linesFromFile[i].IndexOf('_');
                                lastIndexOfNewNum = linesFromFile[i].IndexOf(' ');
                                
                                if (j == texttodb.Length-1)
                                {
                                    lastIndexOfNewNum = linesFromFile[i].LastIndexOf(' ');
                                }
                                
                                texttodb[j] = linesFromFile[i].Substring(startIndexOfNewNum+1, lastIndexOfNewNum-1);
                                linesFromFile[i] = linesFromFile[i].Remove(0, lastIndexOfNewNum + 1);
                            }

                            Value value = new Value(Convert.ToDouble(texttodb[0]), Convert.ToDouble(texttodb[1]), Convert.ToDateTime(texttodb[2]));
                            dbV.Values.Add(value);
                            dbV.SaveChanges();
                        }
                    }

                    #endregion
                }

                #region Adding a new line in .txt file after writing data in data base 

                using (FileStream writing = new FileStream(path + $@"\{DateTime.Now.Day}.txt", FileMode.OpenOrCreate))
                {
                    for (int i = readArray.Length - 1; i >= 1; i--)
                    {
                        readArray[i] = readArray[i - 1];
                    }

                    readArray[0] = 10;
                    writing.Write(readArray, 0, readArray.Length);
                }

                #endregion
            }
        }

        public static bool WritingCheck(string path)
        {
            using (FileStream read = File.OpenRead(path + $@"\{DateTime.Now.Day}.txt"))
            {
                byte[] readArray = new byte[read.Length];
                read.Read(readArray, 0, readArray.Length);

                return readArray[0] != 10;
            }
        }
    }
}