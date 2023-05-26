
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics;

namespace WordsWriter
{
    internal class Program
    {
        static void Main(string[] args)
        {
           
            try
            {
                var Timer = new Stopwatch();
                string filePath;
                string resultPath;
                string MultyThreadResultPath;
                var wf = new WordsFinder.WordsFinder();
                Console.Write("Введите путь к файлу: ");
                filePath = Console.ReadLine();
                Console.Write("Введите путь к результату: ");
                resultPath = Console.ReadLine();
                Console.Write("Введите путь к результату работы многопоточного метода: ");
                MultyThreadResultPath = Console.ReadLine();

                Dictionary<string, int> _words = new Dictionary<string, int>();
                Dictionary<string, int> _words1 = new Dictionary<string, int>();
                Dictionary<string, int> _words2 = new Dictionary<string, int>();
                string text = File.ReadAllText(filePath);
                //метод из первого задания
                Timer.Start();
                _words = CountWords(text);
                Timer.Stop();
                Console.WriteLine($"Время выполнения приватного метода:{Timer.Elapsed.ToString()}");
                Timer.Reset();
                WriteToFile(resultPath, _words);
                //Многопоточный метод
                Timer.Start();
                _words1 = wf.CountWordsPublic(text);
                Timer.Stop(); 
                Console.WriteLine($"Время выполнения публичного многопоточного метода:{Timer.Elapsed.ToString()}");
                Timer.Reset();
                WriteToFile(MultyThreadResultPath, _words1);
                Timer.Start();
                _words2 = wf.CountWordsWithParallel(text);
                Timer.Stop();
                Console.WriteLine($"Время выполнения публичного parallel метода:{Timer.Elapsed.ToString()}");
                Timer.Reset();
                WriteToFile(MultyThreadResultPath, _words2);
                Console.ReadKey();

            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message); 
                Console.ReadKey();
            }
            
            
            
        }
        private static Dictionary<string, int> CountWords(string text)
        {
            string dllPath = "WordsFinder.dll";
            Assembly assembly = Assembly.LoadFrom(dllPath);
            var type = assembly.GetType("WordsFinder.WordsFinder");
            var instance = Activator.CreateInstance(type);
            var method = type.GetMethod("CountWords", BindingFlags.NonPublic | BindingFlags.Instance);
            var result = (Dictionary<string, int>)method.Invoke(instance, new object[] { text });
            return result;
        }
        private static void WriteToFile(string resultPath, Dictionary<string, int> result )
        {
            try
            {


                var sw = new StreamWriter(resultPath, false, Encoding.Unicode);
                foreach (var res in result)
                {
                    sw.WriteLine(res.Key + " " + res.Value);
                }
                sw.Close();
            }
            catch (Exception e)
            { Console.WriteLine(e.Message); }


        }
      
    }
}