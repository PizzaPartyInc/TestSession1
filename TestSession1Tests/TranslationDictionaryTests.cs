using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using TestSession1;

namespace TestSession1Tests
{
    [TestClass]
    public class TranslationDictionaryTests
    {
        [DataTestMethod]
        [DataRow("Hello ", "English", "Tere", "Estonian")]
        [DataRow("hellO", " English", "Привет", "Russian")]
        [DataRow("HELLO", "English", " こんにちは ", "Japanese")]
        [DataRow("你好", "Chinese (Simplified)", "Hello", "   English  ")]
        [DataRow("여보세요", "Korean", "Hello", "English")]
        [DataRow("Pẹlẹ o", "ZULU", "HELLO", "ENGLISH")]
        public void Test_TranslationDictionary_AddEntry_Saves_Valid_Data(
            string fromWord, string fromLanguage, string toWord, string toLanguage)
        {
            //Arrange
            TranslationDictionary translationDictionary = new TranslationDictionary();

            //Act
            translationDictionary.AddEntry(fromWord, fromLanguage, toWord, toLanguage);

            //Assert
            Assert.AreEqual(1, translationDictionary.Dictionary.Count);
            Assert.AreEqual(fromWord.Trim().ToLowerInvariant(), translationDictionary.Dictionary.First().FromWord);
            Assert.AreEqual(fromLanguage.Trim().ToLowerInvariant(), translationDictionary.Dictionary.First().FromLanguage);
            Assert.AreEqual(toWord.Trim().ToLowerInvariant(), translationDictionary.Dictionary.First().ToWord);
            Assert.AreEqual(toLanguage.Trim().ToLowerInvariant(), translationDictionary.Dictionary.First().ToLanguage);
        }

        [TestMethod]
        public void Test_TranslationDictionary_AddEntry_For_Two_Languages_And_Same_FromWord_Saves_Both_Entries()
        {
            //Arrange
            TranslationDictionary translationDictionary = new TranslationDictionary();

            //Act
            translationDictionary.AddEntry("Hello", "English", "Tere", "Estonian");
            translationDictionary.AddEntry("Hello", "English", "여보세요", "Korean");

            //Assert
            Assert.AreEqual(2, translationDictionary.Dictionary.Count);
            Assert.AreEqual("hello", translationDictionary.Dictionary.First().FromWord);
            Assert.AreEqual("english", translationDictionary.Dictionary.First().FromLanguage);
            Assert.AreEqual("tere", translationDictionary.Dictionary.First().ToWord);
            Assert.AreEqual("estonian", translationDictionary.Dictionary.First().ToLanguage);
            Assert.AreEqual("hello", translationDictionary.Dictionary.Last().FromWord);
            Assert.AreEqual("english", translationDictionary.Dictionary.Last().FromLanguage);
            Assert.AreEqual("여보세요", translationDictionary.Dictionary.Last().ToWord);
            Assert.AreEqual("korean", translationDictionary.Dictionary.Last().ToLanguage);
        }

        [DataTestMethod]
        [DataRow("Hello", "English", "Tere", "Estonian")]
        [DataRow("Hello", "English", "Tere2", "Estonian")]
        [DataRow("Hello2", "English", "Tere", "Estonian")]
        [DataRow("Tere", "Estonian", "Hello", "English")]
        [DataRow("Tere", "Estonian", "Hello2", "English")]
        [DataRow("Tere2", "Estonian", "Hello", "English")]
        public void Test_TranslationDictionary_AddEntry_For_Different_Duplicate_Cases_Saves_Only_First_Entry(
            string fromWord, string fromLanguage, string toWord, string toLanguage)
        {
            //Arrange
            TranslationDictionary translationDictionary = new TranslationDictionary();

            //Act
            translationDictionary.AddEntry("Hello", "English", "Tere", "Estonian");
            translationDictionary.AddEntry(fromWord, fromLanguage, toWord, toLanguage);

            //Assert
            Assert.AreEqual(1, translationDictionary.Dictionary.Count);
            Assert.AreEqual("hello", translationDictionary.Dictionary.First().FromWord);
            Assert.AreEqual("english", translationDictionary.Dictionary.First().FromLanguage);
            Assert.AreEqual("tere", translationDictionary.Dictionary.First().ToWord);
            Assert.AreEqual("estonian", translationDictionary.Dictionary.First().ToLanguage);
        }

        [DataTestMethod]
        [DataRow(null, "English", "Tere", "Estonian")]
        [DataRow("", "English", "Tere", "Estonian")]
        [DataRow(" ", "English", "Tere", "Estonian")]
        [DataRow("Hello", null, "Tere", "Estonian")]
        [DataRow("Hello", "", "Tere", "Estonian")]
        [DataRow("Hello", " ", "Tere", "Estonian")]
        [DataRow("Hello", "English", null, "Estonian")]
        [DataRow("Hello", "English", "", "Estonian")]
        [DataRow("Hello", "English", " ", "Estonian")]
        [DataRow("Hello", "English", "Tere", null)]
        [DataRow("Hello", "English", "Tere", "")]
        [DataRow("Hello", "English", "Tere", " ")]
        public void Test_TranslationDictionary_AddEntry_With_Null_Or_Empty_Or_Whitespace_Arguments_Throws_ArgumentNullException(
            string fromWord, string fromLanguage, string toWord, string toLanguage)
        {
            //Arrange
            TranslationDictionary translationDictionary = new TranslationDictionary();
            
            //Assert
            Assert.ThrowsException<ArgumentNullException>(() => translationDictionary.AddEntry(fromWord, fromLanguage, toWord, toLanguage));
        }

        [TestMethod]
        public void Test_TranslationDictionary_Remove_For_Empty_Dictionary_Throws_No_Exceptions()
        {
            //Arrange
            TranslationDictionary translationDictionary = new TranslationDictionary();

            //Assert
            Assert.AreEqual(0, translationDictionary.Dictionary.Count);
            translationDictionary.Remove("Hello", "English");
        }

        [TestMethod]
        public void Test_TranslationDictionary_Remove_With_From_Values_Removes_Single_Entry()
        {
            //Arrange
            TranslationDictionary translationDictionary = new TranslationDictionary();
            translationDictionary.AddEntry("Hello", "English", "Tere", "Estonian");

            //Act
            translationDictionary.Remove("Hello", "English");

            //Assert
            Assert.AreEqual(0, translationDictionary.Dictionary.Count);
        }

        [TestMethod]
        public void Test_TranslationDictionary_Remove_With_To_Values_Removes_Single_Entry()
        {
            //Arrange
            TranslationDictionary translationDictionary = new TranslationDictionary();
            translationDictionary.AddEntry("Hello", "English", "Tere", "Estonian");

            //Act
            translationDictionary.Remove("Tere", "Estonian");

            //Assert
            Assert.AreEqual(0, translationDictionary.Dictionary.Count);
        }

        [TestMethod]
        public void Test_TranslationDictionary_Remove_Removes_Multiple_Entries()
        {
            //Arrange
            TranslationDictionary translationDictionary = new TranslationDictionary();
            translationDictionary.AddEntry("Hello", "English", "Tere", "Estonian");
            translationDictionary.AddEntry("Hello", "English", "Привет", "Russian");
            translationDictionary.AddEntry("Hello", "English", "こんにちは", "Japanese");

            //Act
            translationDictionary.Remove("Hello", "English");

            //Assert
            Assert.AreEqual(0, translationDictionary.Dictionary.Count);
        }

        [TestMethod]
        public void Test_TranslationDictionary_Remove_With_Other_Entries_Removes_Valid_Entries()
        {
            //Arrange
            TranslationDictionary translationDictionary = new TranslationDictionary();
            translationDictionary.AddEntry(" Привет", "Russian", "Tere", "Estonian");
            translationDictionary.AddEntry("Hello", "English ", "Tere", "Estonian");
            translationDictionary.AddEntry("Hello", "English", "Привет", "Russian");
            translationDictionary.AddEntry("Hello", "English", "こんにちは", "Japanese");
            translationDictionary.AddEntry("Tere", "Estonian", "こんにちは", "Japanese");

            //Act
            translationDictionary.Remove("Hello", "English");

            //Assert
            Assert.AreEqual(2, translationDictionary.Dictionary.Count);
            Assert.AreEqual("привет", translationDictionary.Dictionary.First().FromWord);
            Assert.AreEqual("russian", translationDictionary.Dictionary.First().FromLanguage);
            Assert.AreEqual("tere", translationDictionary.Dictionary.First().ToWord);
            Assert.AreEqual("estonian", translationDictionary.Dictionary.First().ToLanguage);
            Assert.AreEqual("tere", translationDictionary.Dictionary.Last().FromWord);
            Assert.AreEqual("estonian", translationDictionary.Dictionary.Last().FromLanguage);
            Assert.AreEqual("こんにちは", translationDictionary.Dictionary.Last().ToWord);
            Assert.AreEqual("japanese", translationDictionary.Dictionary.Last().ToLanguage);
        }

        [TestMethod]
        public void Test_TranslationDictionary_Clear_Removes_All_Entries()
        {
            //Arrange
            TranslationDictionary translationDictionary = new TranslationDictionary();
            translationDictionary.AddEntry("Привет", "Russian", "Tere", "Estonian");
            translationDictionary.AddEntry("Hello", "English", "Tere", "Estonian");
            translationDictionary.AddEntry("Hello", "English", "Привет", "Russian");
            translationDictionary.AddEntry("Hello", "English", "こんにちは", "Japanese");
            translationDictionary.AddEntry("Tere", "Estonian", "こんにちは", "Japanese");

            //Act
            translationDictionary.Clear();

            //Assert
            Assert.AreEqual(0, translationDictionary.Dictionary.Count);
        }

        [DataTestMethod]
        [DataRow("HELLO", "ENGLISH")]
        [DataRow("hello", "english")]
        [DataRow("Hello ", "English ")]
        [DataRow(" Tere", " Estonian")]
        [DataRow("  Tere  ", "  Estonian  ")]
        [DataRow("  Tere    ", "    Estonian    ")]
        public void Test_TranslationDictionary_Remove_With_Different_Case_And_Whitespaces_Removes_Valid_Entry(
            string word, string language)
        {
            //Arrange
            TranslationDictionary translationDictionary = new TranslationDictionary();
            translationDictionary.AddEntry("Hello", "English ", "Tere", "Estonian");

            //Act
            translationDictionary.Remove(word, language);

            //Assert
            Assert.AreEqual(0, translationDictionary.Dictionary.Count);
        }

        [DataRow(null, "English")]
        [DataRow("", "English")]
        [DataRow(" ", "English")]
        [DataRow("Hello", null)]
        [DataRow("Hello", "")]
        [DataRow("Hello", " ")]
        public void Test_TranslationDictionary_Remove_With_Null_Or_Empty_Or_Whitespace_Arguments_Throws_ArgumentNullException(
            string word, string language)
        {
            //Arrange
            TranslationDictionary translationDictionary = new TranslationDictionary();

            //Assert
            Assert.ThrowsException<ArgumentNullException>(() => translationDictionary.Remove(word, language));
        }

        [TestMethod]
        public void Test_TranslationDictionary_Translate_For_FromWord_Returns_Valid_Word()
        {
            //Arrange
            TranslationDictionary translationDictionary = new TranslationDictionary();
            translationDictionary.AddEntry("Привет", "Russian", "Tere", "Estonian");

            //Act
            var result = translationDictionary.Translate("Привет", "Russian", "Estonian");

            //Assert
            Assert.AreEqual("tere", result);
        }

        [TestMethod]
        public void Test_TranslationDictionary_Translate_For_ToWord_Returns_Valid_Word()
        {
            //Arrange
            TranslationDictionary translationDictionary = new TranslationDictionary();
            translationDictionary.AddEntry("Привет", "Russian", "Tere", "Estonian");

            //Act
            var result = translationDictionary.Translate("Tere", "Estonian", "Russian");

            //Assert
            Assert.AreEqual("привет", result);
        }

        [TestMethod]
        public void Test_TranslationDictionary_Translate_With_Empty_Dictionary_Throws_NullReferenceException()
        {
            //Arrange
            TranslationDictionary translationDictionary = new TranslationDictionary();

            //Assert
            Assert.ThrowsException<NullReferenceException>(() => translationDictionary.Translate("Tere", "Estonian", "Russian"));
        }

        [DataTestMethod]
        [DataRow("Привет", "Russian", "Japanese")]
        [DataRow("Tere", "Estonian", "Japanese")]
        [DataRow("Tere", "Japanese", "Russian")]
        [DataRow("Tere", "Japanese", "Estonian")]
        public void Test_TranslationDictionary_Translate_For_Non_Existant_Entry_Throws_NullReferenceException(
            string fromWord, string fromLanguage, string toLanguage)
        {
            //Arrange
            TranslationDictionary translationDictionary = new TranslationDictionary();
            translationDictionary.AddEntry("Привет", "Russian", "Tere", "Estonian");

            //Assert
            Assert.ThrowsException<NullReferenceException>(() => translationDictionary.Translate(fromWord, fromLanguage, toLanguage));
        }

        [DataTestMethod]
        [DataRow("HELLO ", "ENGLISH ", "ESTONIAN ")]
        [DataRow(" hello", " english", " estonian")]
        [DataRow("  Hello   ", "  eNglish ", " esTonian  ")]
        public void Test_TranslationDictionary_Translate_With_Different_Case_And_Whitespaces_Returns_Valid_Word(
            string fromWord, string fromLanguage, string toLanguage)
        {
            //Arrange
            TranslationDictionary translationDictionary = new TranslationDictionary();
            translationDictionary.AddEntry("Hello", "English", "Tere", "Estonian");

            //Act
            var result = translationDictionary.Translate(fromWord, fromLanguage, toLanguage);

            //Assert
            Assert.AreEqual("tere", result);
        }
    }
}
