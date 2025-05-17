using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FileClassification
{
    public class Entropy
    {
        private int numberOfDocuments;
        private Dictionary<string, int> topicsSet;
        private FileReader fileReader;

        public Entropy()
        {
            this.fileReader = new FileReader();
            File.WriteAllText(this.fileReader.filePath + "\\Files\\EntropyWords.txt", string.Empty);
            this.numberOfDocuments = Directory.GetFiles(Directory.GetCurrentDirectory() + "\\Reuters\\Reuters_7083", "*.XML").Length;
            this.getUniqueTopics();

            this.numberOfDocuments = 12;
            this.entropyForEveryWord();
        }

        public void getUniqueTopics()
        {
            this.topicsSet = new Dictionary<string, int>();
            string[] strings = File.ReadAllLines(Directory.GetCurrentDirectory() + "\\Files\\DocumentsWords.txt");

            foreach (string line in strings)
            {
                if (!topicsSet.ContainsKey(line.Split(' ')[1]))
                {
                    this.topicsSet.Add(line.Split(' ')[1], 1);
                }
                else
                {
                    this.topicsSet[line.Split(' ')[1]] += 1;
                }
            }

        }

        public void entropyForEveryWord()
        {
            string[] globalWords = File.ReadAllLines(Directory.GetCurrentDirectory() + "\\Files\\GlobalWords.txt");
            string[] documentsWords = File.ReadAllLines(Directory.GetCurrentDirectory() + "\\Files\\DocumentsWords.txt");
            string word = "";
            double entropy;
            string[] size;
            int ok = 0, index = 0, zeroIndex = 0;
            foreach (string line in globalWords)
            {
                word = line.Split(' ')[1];
                index = 0;
                zeroIndex = 0;
                foreach (string line2 in documentsWords)
                {
                    ok = 0;
                    size = line2.Split(' ');
                    for (int i = 2; i < size.Length; i++)
                    {
                        if (word.Equals(size[i].Split(':')[0]) == true)
                        {
                            ok = 1;
                            index++;
                        }
                    }
                    if (ok == 0)
                    {
                        zeroIndex++;
                    }
                }

                entropy = this.entropy() - (double)zeroIndex / numberOfDocuments*this.entropyForWord(zeroIndex,word,true)-(double)index/numberOfDocuments*this.entropyForWord(index,word,false);
                this.fileReader.addDocumentToEntropyWords(line.Split(" ")[0], entropy.ToString());
            }
        }

        public double entropyForWord(int index, string word,bool zero)
        {
            double entropy = 0;
            string[] documentsWords = File.ReadAllLines(Directory.GetCurrentDirectory() + "\\Files\\DocumentsWords.txt");
            Dictionary<string, int> wordSet = new Dictionary<string, int>();
            string[] size;
            int ok = 0;
            foreach (string line in documentsWords)
            {
                ok = 0;
                size = line.Split(' ');
                for (int i = 2; i < size.Length; i++)
                {
                    if (zero == false)
                    {
                        if (size[i].StartsWith($"{word}:"))
                        {
                            ok = 1;
                            if (!wordSet.ContainsKey(size[1]))
                            {
                                wordSet.Add(size[1], 1);
                            }
                            else
                            {
                                wordSet[size[1]] += 1;
                            }
                            break;
                        }
                    }
                    else
                    {
                        if (size[i].StartsWith($"{word}:"))
                            ok = 1;
                        break;
                    }
                }
                if(ok==0 && zero==true)
                {
                    if (!wordSet.ContainsKey(size[1]))
                    {
                        wordSet.Add(size[1], 1);
                    }
                    else
                    {
                        wordSet[size[1]] += 1;
                    }
                }
            }

            MessageBox.Show("WordSet: "+zero+" "+word);
            foreach (var topic in wordSet)
            {
                MessageBox.Show(topic.Key + " " + topic.Value);
                double number = (double)topic.Value / index;
                entropy = entropy - number * this.logBase2(number);
            }

            return entropy;
        }




        public double entropyRange()
        {
            return logBase2(this.topicsSet.Count);
        }

        public double entropy()
        {
            double entropy = 0, number;
            foreach (var topic in this.topicsSet)
            {
                number = (double)topic.Value / numberOfDocuments;
                entropy = entropy - number * this.logBase2(number);
            }

            return entropy;
        }

        public double logBase2(double n)
        {
            if (n == 0)
                return 0;
            return Math.Log(n) / Math.Log(2);
        }
    }
}
