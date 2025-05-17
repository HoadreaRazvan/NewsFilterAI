using Porter2Stemmer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FileClassification
{
    public class FileReader
    {
        public string filePath;

        public FileReader()
        {
            this.filePath = Directory.GetCurrentDirectory();
        }


        public void addDocumentToFile(string filePath, Dictionary<int, int> documentWords,string path,string topic)
        {
            using (StreamWriter writer = new StreamWriter(path + "\\Files\\DocumentsWords.txt", true))
            {
                writer.Write((filePath.Split('\\')[filePath.Split('\\').Length - 1]).Split('.')[0] + " "+ topic+" ");

                foreach (var word in documentWords)
                {
                    writer.Write($"{word.Key}:{word.Value} ");
                }
                writer.WriteLine();
            }
        }

        public void addDocumentToEntropyWords(string key,string value)
        {
            using (StreamWriter writer = new StreamWriter(this.filePath + "\\Files\\EntropyWords.txt",true))
            {
                writer.WriteLine(key+" "+value);
            }
        }

        public string getTextFromFile(string filePath)
        {
            XDocument doc = XDocument.Load(filePath);
            XElement textElement = doc.Root.Element("text");

            if (textElement == null) return string.Empty;

            string content = string.Join(" ", textElement.Elements("p")
                .Select(p => p.Value.Trim())
                .Where(p => !string.IsNullOrWhiteSpace(p))
            );

            return Regex.Replace(content, @"\s+", " ");
        }

        public string getTitleFromFile(string filePath)
        {
            XDocument doc = XDocument.Load(filePath);
            XElement titleElement = doc.Root.Element("title");
            return Regex.Replace(titleElement.Value, @"\s+", " ");
        }

        public string getTopicsFromFile(string filepath)
        {
            XDocument doc = XDocument.Load(filepath);

            var codesElement = doc.Descendants("codes")
                .FirstOrDefault(el => (string)el.Attribute("class") == "bip:topics:1.0");

            if (codesElement == null)
                return String.Empty;

            List<string> codeList = codesElement.Elements("code")
                .Select(el => el.Attribute("code")?.Value)
                .Where(val => val != null)
                .ToList();

            return codeList[0];
        }

        public void addGlobalWordsToFile(Dictionary<string, int> globalWords)
        {
            using (StreamWriter writer = new StreamWriter(this.filePath + "\\Files\\GlobalWords.txt"))
            {
                foreach (var word in globalWords)
                {
                    writer.WriteLine($"{word.Key} {word.Value}");
                }
            }
        }

    }
}
