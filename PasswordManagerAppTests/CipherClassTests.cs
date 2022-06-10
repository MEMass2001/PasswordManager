using Microsoft.VisualStudio.TestTools.UnitTesting;
using PasswordManagerApp.Classes;

namespace PasswordManagerAppTests
{
    [TestClass]
    public class CipherClassTests
    {
        [TestMethod]
        public void cryptStringCipherTest_RealStringsInserted_ExpectedResultReturned()
        {
            // Arrange
            string cipherableString = "TestString";
            string cipherKey = "TestKey";
            bool isCipher = true;
            string expectedResult = "93 176 17 218 150 18 74 204 41 222 197 37 186 152 169 192 49 136 178 116 149 131 142 159";
            // Act
            string actualResult = IdeaCipher.cryptString(cipherableString, cipherKey, isCipher);
            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void cryptStringCipherTest_BlankKeyInserted_nullReturned()
        {
            // Arrange
            string cipherableString = "TestString";
            string cipherKey = "      ";
            bool isCipher = true;
            string expectedResult = null;
            // Act
            string actualResult = IdeaCipher.cryptString(cipherableString, cipherKey, isCipher);
            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void cryptStringCipherTest_nullKeyInserted_nullReturned()
        {
            // Arrange
            string cipherableString = "TestString";
            string cipherKey = null;
            bool isCipher = true;
            string expectedResult = null;
            // Act
            string actualResult = IdeaCipher.cryptString(cipherableString, cipherKey, isCipher);
            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void cryptStringCipherTest_BlankCipherableStringInserted_nullReturned()
        {
            // Arrange
            string cipherableString = "      ";
            string cipherKey = "TestKey";
            bool isCipher = true;
            string expectedResult = null;
            // Act
            string actualResult = IdeaCipher.cryptString(cipherableString, cipherKey, isCipher);
            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void cryptStringCipherTest_nullCipherableStringInserted_nullReturned()
        {
            // Arrange
            string cipherableString = null;
            string cipherKey = "TestKey";
            bool isCipher = true;
            string expectedResult = null;
            // Act
            string actualResult = IdeaCipher.cryptString(cipherableString, cipherKey, isCipher);
            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void cryptStringCipherTest_BlankStringsInserted_nullReturned()
        {
            // Arrange
            string cipherableString = "      ";
            string cipherKey = "      ";
            bool isCipher = true;
            string expectedResult = null;
            // Act
            string actualResult = IdeaCipher.cryptString(cipherableString, cipherKey, isCipher);
            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void cryptStringCipherTest_nullStringsInserted_nullReturned()
        {
            // Arrange
            string cipherableString = null;
            string cipherKey = null;
            bool isCipher = true;
            string expectedResult = null;
            // Act
            string actualResult = IdeaCipher.cryptString(cipherableString, cipherKey, isCipher);
            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void cryptStringDecipherTest_RealStringsInserted_ExpectedResultReturned()
        {
            // Arrange
            string cipherableString = "93 176 17 218 150 18 74 204 41 222 197 37 186 152 169 192 49 136 178 116 149 131 142 159";
            string cipherKey = "TestKey";
            bool isCipher = false;
            string expectedResult = "TestString";
            // Act
            string actualResult = IdeaCipher.cryptString(cipherableString, cipherKey, isCipher).TrimEnd('\0');
            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void cryptStringDecipherTest_BlankKeyInserted_nullReturned()
        {
            // Arrange
            string cipherableString = "93 176 17 218 150 18 74 204 41 222 197 37 186 152 169 192 49 136 178 116 149 131 142 159";
            string cipherKey = "     ";
            bool isCipher = false;
            string expectedResult = null;
            // Act
            string actualResult = IdeaCipher.cryptString(cipherableString, cipherKey, isCipher);
            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void cryptStringDecipherTest_nullKeyInserted_nullReturned()
        {
            // Arrange
            string cipherableString = "93 176 17 218 150 18 74 204 41 222 197 37 186 152 169 192 49 136 178 116 149 131 142 159";
            string cipherKey = null;
            bool isCipher = false;
            string expectedResult = null;
            // Act
            string actualResult = IdeaCipher.cryptString(cipherableString, cipherKey, isCipher);
            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void cryptStringDecipherTest_BlankCipherableStringInserted_nullReturned()
        {
            // Arrange
            string cipherableString = "      ";
            string cipherKey = "TestKey";
            bool isCipher = false;
            string expectedResult = null;
            // Act
            string actualResult = IdeaCipher.cryptString(cipherableString, cipherKey, isCipher);
            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void cryptStringDecipherTest_nullCipherableStringInserted_nullReturned()
        {
            // Arrange
            string cipherableString = null;
            string cipherKey = "TestKey";
            bool isCipher = false;
            string expectedResult = null;
            // Act
            string actualResult = IdeaCipher.cryptString(cipherableString, cipherKey, isCipher);
            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void cryptStringDecipherTest_BlankDataInserted_nullReturned()
        {
            // Arrange
            string cipherableString = "      ";
            string cipherKey = "     ";
            bool isCipher = false;
            string expectedResult = null;
            // Act
            string actualResult = IdeaCipher.cryptString(cipherableString, cipherKey, isCipher);
            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void cryptStringDecipherTest_nullDataInserted_nullReturned()
        {
            // Arrange
            string cipherableString = null;
            string cipherKey = null;
            bool isCipher = false;
            string expectedResult = null;
            // Act
            string actualResult = IdeaCipher.cryptString(cipherableString, cipherKey, isCipher);
            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void cryptStringDecipherTest_WrongStringInserted_nullReturned()
        {
            // Arrange
            string cipherableString = "TestString";
            string cipherKey = "TestKey";
            bool isCipher = false;
            string expectedResult = null;
            // Act
            string actualResult = IdeaCipher.cryptString(cipherableString, cipherKey, isCipher);
            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void cryptStringCipherAndDecipher_SameKeyInserted_InitialStringReturned()
        {
            // Arrange
            string cipherableString = "TestString";
            string cipherKey = "TestKey";
            // Act
            string ciphered = IdeaCipher.cryptString(cipherableString, cipherKey, true);
            string result = IdeaCipher.cryptString(ciphered, cipherKey, false).TrimEnd('\0');
            // Assert
            Assert.AreEqual(cipherableString, result);
        }
        [TestMethod]
        public void cryptStringCipherAndDecipher_CorruptKeyInserted_WrongStringReturned()
        {
            // Arrange
            string cipherableString = "TestString";
            string cipherKey = "TestKey";
            // Act
            string ciphered = IdeaCipher.cryptString(cipherableString, cipherKey, true);
            string result = IdeaCipher.cryptString(ciphered, "corription", false).TrimEnd('\0');
            // Assert
            Assert.AreNotEqual(cipherableString, result);
        }
    }
}
