
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Text;
using static System.Net.Mime.MediaTypeNames;


namespace WordsWriter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string filePath;
                Console.Write("Введите путь к файлу: ");
                filePath = Console.ReadLine();
                string resultPath;
                Console.Write("Введите путь к результату: ");
                resultPath = Console.ReadLine();
                Dictionary<string, int> _words = new Dictionary<string, int>();
                string text = File.ReadAllText(filePath);
                _words = CountWords(text);
                WriteToFile(resultPath, _words);
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