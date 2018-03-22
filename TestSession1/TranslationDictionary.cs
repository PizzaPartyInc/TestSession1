using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace TestSession1
{
    public class TranslationDictionary
    {
        public List<TranslationEntry> Dictionary;
        public TranslationDictionary()
        {
            Dictionary = new List<TranslationEntry>();
        }
        
        public void AddEntry(string fromWord, string fromLanguage, string toWord, string toLanguage)
        {
            if (string.IsNullOrWhiteSpace(fromWord)) throw new ArgumentNullException(nameof(fromWord));
            if (string.IsNullOrWhiteSpace(fromLanguage)) throw new ArgumentNullException(nameof(fromLanguage));
            if (string.IsNullOrWhiteSpace(toWord)) throw new ArgumentNullException(nameof(toWord));
            if (string.IsNullOrWhiteSpace(toLanguage)) throw new ArgumentNullException(nameof(toLanguage));

            fromWord = fromWord.Trim().ToLowerInvariant();
            fromLanguage = fromLanguage.Trim().ToLowerInvariant();
            toWord = toWord.Trim().ToLowerInvariant();
            toLanguage = toLanguage.Trim().ToLowerInvariant();

            if (Dictionary.Any(de => (de.FromWord.Equals(fromWord, StringComparison.InvariantCultureIgnoreCase) &&
                                      de.FromLanguage.Equals(fromLanguage, StringComparison.InvariantCultureIgnoreCase) &&
                                      de.ToLanguage.Equals(toLanguage, StringComparison.InvariantCultureIgnoreCase))
                                      ||
                                     (de.ToWord.Equals(fromWord, StringComparison.InvariantCultureIgnoreCase) &&
                                      de.ToLanguage.Equals(fromLanguage, StringComparison.InvariantCultureIgnoreCase) &&
                                      de.FromLanguage.Equals(toLanguage, StringComparison.InvariantCultureIgnoreCase)) 
                                      ||
                                     (de.FromWord.Equals(toWord, StringComparison.InvariantCultureIgnoreCase) &&
                                      de.FromLanguage.Equals(toLanguage, StringComparison.InvariantCultureIgnoreCase) &&
                                      de.ToLanguage.Equals(fromLanguage, StringComparison.InvariantCultureIgnoreCase))
                                      ||
                                     (de.ToWord.Equals(toWord, StringComparison.InvariantCultureIgnoreCase) &&
                                      de.ToLanguage.Equals(toLanguage, StringComparison.InvariantCultureIgnoreCase) &&
                                      de.FromLanguage.Equals(fromLanguage, StringComparison.InvariantCultureIgnoreCase))))
            {
                Trace.WriteLine($"Entry for with following data was already added: FromWord '{fromWord}', FromLanguage '{fromLanguage}', ToLanguage '{toLanguage}', ToWord '{toWord}'");
                return;
            }

            Dictionary.Add(new TranslationEntry(fromWord, fromLanguage, toWord, toLanguage));
        }

        public void Remove(string word, string language)
        {
            if (string.IsNullOrWhiteSpace(word)) throw new ArgumentNullException(nameof(word));
            if (string.IsNullOrWhiteSpace(language)) throw new ArgumentNullException(nameof(language));

            word = word.Trim().ToLowerInvariant();
            language = language.Trim().ToLowerInvariant();

            Dictionary.RemoveAll(de => (de.FromWord.Equals(word, StringComparison.InvariantCultureIgnoreCase) &&
                                        de.FromLanguage.Equals(language, StringComparison.InvariantCultureIgnoreCase))
                                       ||
                                       (de.ToWord.Equals(word, StringComparison.InvariantCultureIgnoreCase) &&
                                        de.ToLanguage.Equals(language, StringComparison.InvariantCultureIgnoreCase)));
        }

        public void Clear()
        {
            Dictionary.Clear();
        }

        public string Translate(string fromWord, string fromLanguage, string toLanguage)
        {
            if (string.IsNullOrWhiteSpace(fromWord)) throw new ArgumentNullException(nameof(fromWord));
            if (string.IsNullOrWhiteSpace(fromLanguage)) throw new ArgumentNullException(nameof(fromLanguage));
            if (string.IsNullOrWhiteSpace(toLanguage)) throw new ArgumentNullException(nameof(toLanguage));

            fromWord = fromWord.Trim().ToLowerInvariant();
            fromLanguage = fromLanguage.Trim().ToLowerInvariant();
            toLanguage = toLanguage.Trim().ToLowerInvariant();

            var result = Dictionary.SingleOrDefault(de => de.FromWord.Equals(fromWord, StringComparison.InvariantCultureIgnoreCase) &&
                                                          de.FromLanguage.Equals(fromLanguage, StringComparison.InvariantCultureIgnoreCase) && 
                                                          de.ToLanguage.Equals(toLanguage, StringComparison.InvariantCultureIgnoreCase));

            if(result != null)
                return result.ToWord;

            result = Dictionary.SingleOrDefault(de => de.ToWord.Equals(fromWord, StringComparison.InvariantCultureIgnoreCase) &&
                                                      de.ToLanguage.Equals(fromLanguage, StringComparison.InvariantCultureIgnoreCase) &&
                                                      de.FromLanguage.Equals(toLanguage, StringComparison.InvariantCultureIgnoreCase));

            if (result == null) throw new NullReferenceException($"Entry for with following data was not found: FromWord '{fromWord}', FromLanguage '{fromLanguage}', ToLanguage '{toLanguage}'");

            return result.FromWord;
        }
    }
}
