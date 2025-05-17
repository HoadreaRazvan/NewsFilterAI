using Porter2Stemmer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileClassification
{
    public class ProcessReuters
    {
        public string[] stopWords;
        public Dictionary<string, int> globalWords;
        public Dictionary<int, int> documentWords;
        public int globalIndex;

        public FileReader fileReader;

        public ProcessReuters()
        {
            this.fileReader = new FileReader();
            this.globalWords = new Dictionary<string, int>();
            this.documentWords = new Dictionary<int, int>();

            this.stopWords = this.getStopWords();
            this.globalIndex = 0;
        }

        public void processReuters()
        {
            File.WriteAllText(this.fileReader.filePath + "\\Files\\DocumentsWords.txt", string.Empty);

            string[] filesPath = Directory.GetFiles(this.fileReader.filePath + "\\Reuters\\Reuters_7083", "*.XML");
            foreach (string filePath in filesPath)
            {
                string documentName = (filePath.Split('\\')[filePath.Split('\\').Length - 1]).Split('.')[0];
                this.processFile(this.fileReader.getTitleFromFile(filePath), this.fileReader.getTextFromFile(filePath));
                this.fileReader.addDocumentToFile(filePath,this.documentWords,this.fileReader.filePath, this.fileReader.getTopicsFromFile(filePath));
                this.documentWords = new Dictionary<int, int>();
                this.fileReader.addGlobalWordsToFile(this.globalWords);
            }
        }

        public void processFile(string title, string text)
        {
            string[] titleWords = title.Split(' ');
            string[] textWords = text.Split(' ');
            string stemWord = "";

            foreach (string word in textWords)
            {
                stemWord = this.rootWord(word);
                this.evaluateWord(stemWord, false);
            }

            foreach (string word in titleWords)
            {
                stemWord = this.rootWord(word);
                this.evaluateWord(stemWord, true);
            }
        }

        public string rootWord(string word)
        {
            string trimChars = ",.\"!?;: ()[]{}-";
            int start = 0;
            int end = word.Length - 1;

            while (start <= end && trimChars.Contains(word[start]))
                start++;
            while (end >= start && trimChars.Contains(word[end]))
                end--;

            word = word.Substring(start, end - start + 1);


            foreach (string w in this.stopWords)
            {
                if (word == w)
                {
                    return String.Empty;
                }
            }

            if (word.Length < 3)
                return String.Empty;

            char[] digits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

            foreach (char c in digits)
                if (word.Contains(c))
                    return String.Empty;

            if (word.Any(c => trimChars.Contains(c)))
                return string.Empty;    

            word = new EnglishPorter2Stemmer().Stem(word).Value;
            return word;
        }


        public void evaluateWord(string word, bool isTitle)
        {
            if (string.IsNullOrEmpty(word))
                return;
            if (!globalWords.ContainsKey(word))
            {
                globalWords.Add(word, this.globalIndex++);
            }

            if (this.documentWords.ContainsKey(this.globalWords[word]))
            {
                if (isTitle == false)
                {
                    this.documentWords[this.globalWords[word]]++;
                }
                else
                {
                    this.documentWords[this.globalWords[word]] += 5;
                }
            }
            else
            {
                if (isTitle == false)
                {
                    this.documentWords.Add(this.globalWords[word], 1);
                }
                else
                {

                    this.documentWords.Add(this.globalWords[word], 5);
                }

            }


        }

        public string[] getStopWords()
        {
            string content = File.ReadAllText(this.fileReader.filePath + "\\Files\\StopWords.txt");
            string[] words = content.Split(new[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            return words;
        }

    }
}
