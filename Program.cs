using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace word_counter
{
    public class WordCounter
    {
       
        public static void CountWordFrequencySimple(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException("fileName", "You must specify a file");
            }

            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException("Could not find the specified file.", fileName);
            }

            Dictionary<string, int> wordCounts = new Dictionary<string, int>();
            string pattern = @"[^a-zA-Z0-9'\- ]";

            using (TextReader reader = File.OpenText(fileName))
            {
                string line = reader.ReadLine();
                while (line != null)
                {
                    string cleanedLine = Regex.Replace(line, pattern, string.Empty).ToLowerInvariant();
                    string[] words = cleanedLine.Split(' ');
                    foreach (string word in words)
                    {
  
                        if (!string.IsNullOrEmpty(word))
                        {
                            int frequency = 1;
                            if (wordCounts.ContainsKey(word))
                            {
                                frequency = wordCounts[word] + 1;
                            }

                            wordCounts[word] = frequency;
                        }
                    }

                    line = reader.ReadLine();
                }

                List<KeyValuePair<string, int>> pairList = new List<KeyValuePair<string, int>>(wordCounts);

                pairList.Sort((first, second) => { return second.Value.CompareTo(first.Value); });

                foreach (KeyValuePair<string, int> pair in pairList)
                {
                    Console.WriteLine("{0}, {1}", pair.Key, pair.Value);
                }
            }
        }

        public static void CountWordFrequency(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException("fileName", "You must specify a file");
            }

            string fullPath = FindFile(fileName);

            Dictionary<string, int> wordCounts = new Dictionary<string, int>();


            int longestWordLength = 0;
            int highestFrequency = 0;


            using (TextReader reader = File.OpenText(fullPath))
            {
                string line = reader.ReadLine();
                while (line != null)
                {
                    string cleanedLine = CleanLine(line);
                    string[] words = cleanedLine.Split(' ');
                    foreach (string word in words)
                    {

                        if (!string.IsNullOrEmpty(word))
                        {
                            int frequency = 1;
                            if (wordCounts.ContainsKey(word))
                            {
                                frequency = wordCounts[word] + 1;
                            }

                            wordCounts[word] = frequency;

                            if (word.Length > longestWordLength)
                            {
                                longestWordLength = word.Length;
                            }

                            if (frequency > highestFrequency)
                            {
                                highestFrequency = frequency;
                            }
                        }
                    }

                    line = reader.ReadLine();
                }

                List<KeyValuePair<string, int>> pairList = new List<KeyValuePair<string, int>>(wordCounts);

                pairList.Sort(ComparePairs);

                string formatString = GetFormatString(longestWordLength, highestFrequency);
                for (int i = 0; i < pairList.Count; i++)
                {
                    KeyValuePair<string, int> pair = pairList[i];
                    Console.WriteLine(formatString, pair.Key, pair.Value);
                }
            }
        }

        private static string FindFile(string fileName)
        {
            string foundFile = fileName;
            if (!File.Exists(foundFile))
            {
                foundFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), Path.GetFileName(fileName));
            }

            if (!File.Exists(foundFile))
            {
                throw new FileNotFoundException("Could not find the specified file.", fileName);
            }

            return foundFile;
        }

        private static string CleanLine(string line)
        {
            string pattern = @"[^a-zA-Z0-9'\- ]";
            string cleanedLine = Regex.Replace(line, pattern, string.Empty);
            return cleanedLine.ToLowerInvariant();
        }

        private static int ComparePairs(KeyValuePair<string, int> first, KeyValuePair<string, int> second)
        {

            return second.Value.CompareTo(first.Value);
        }

        private static string GetFormatString(int longestWordLength, int highestFrequency)
        {
            int frequencyDigits = 0;
            while (highestFrequency > 0)
            {
                highestFrequency /= 10;
                frequencyDigits++;
            }

            return "{0, -" + longestWordLength + "}  {1, " + frequencyDigits + "}";
        }
    }
}
