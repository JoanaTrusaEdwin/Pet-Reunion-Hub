//using System;
//using System.IO;
//using System.Security.Cryptography;
//using System.Text;

//namespace Pet_Reunion_Hub.Helper
//{
//    public class EncryptionHelper
//    {
//        // Generate a random encryption key each time the application starts
//        private static readonly string EncryptionKey = GenerateEncryptionKey();

//        private static string GenerateEncryptionKey()
//        {
//            byte[] keyBytes = new byte[32];
//            using (var rng = new RNGCryptoServiceProvider())
//            {
//                rng.GetBytes(keyBytes);
//            }
//            return Convert.ToBase64String(keyBytes);
//        }

//        public static string Encrypt(string plainText)
//        {
//            byte[] encryptedBytes;

//            using (Aes aesAlg = Aes.Create())
//            {
//                aesAlg.Key = Convert.FromBase64String(EncryptionKey);
//                //aesAlg.IV = aesAlg.Key;
//                aesAlg.GenerateIV(); // Generate a random IV
//                byte[] iv = aesAlg.IV;

//                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
//                using (MemoryStream msEncrypt = new MemoryStream())
//                {
//                    msEncrypt.Write(iv, 0, iv.Length);

//                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
//                    {
//                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
//                        {
//                            swEncrypt.Write(plainText);
//                        }
//                        encryptedBytes = msEncrypt.ToArray();
//                    }
//                }
//            }
//            return Convert.ToBase64String(encryptedBytes);
//        }



//        public static string Decrypt(string cipherText, string encryptionKey)
//        {
//            if (string.IsNullOrEmpty(cipherText))
//            {
//                // Handle null or empty cipherText
//                return string.Empty; // Or throw an exception if appropriate
//            }

//            try
//            {
//                // Convert the base64 encoded ciphertext to bytes
//                byte[] cipherBytes = Convert.FromBase64String(cipherText);

//                // Ensure that the cipherBytes array has at least 16 bytes (IV size)
//                if (cipherBytes.Length < 16)
//                {
//                    throw new ArgumentException("Invalid cipherText length.");
//                }

//                // Create a byte array to store the IV
//                byte[] iv = new byte[16];

//                // Copy the first 16 bytes of the cipherBytes array into the IV array
//                Buffer.BlockCopy(cipherBytes, 0, iv, 0, iv.Length);

//                // Create a byte array to store the encrypted data (excluding the IV)
//                byte[] encryptedData = new byte[cipherBytes.Length - iv.Length];

//                // Copy the encrypted data (excluding the IV) into the encryptedData array
//                Buffer.BlockCopy(cipherBytes, iv.Length, encryptedData, 0, encryptedData.Length);

//                // Declare a string variable to store the plaintext
//                string plaintext = null;

//                // Create an AES instance
//                using (Aes aesAlg = Aes.Create())
//                {
//                    // Set the encryption key
//                    aesAlg.Key = Convert.FromBase64String(EncryptionKey);

//                    // Set the IV
//                    aesAlg.IV = iv;

//                    // Create a decryptor
//                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

//                    // Create a MemoryStream to hold the decrypted data
//                    using (MemoryStream msDecrypt = new MemoryStream(encryptedData))
//                    {
//                        // Create a CryptoStream to perform decryption
//                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
//                        {
//                            // Create a StreamReader to read the decrypted data
//                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
//                            {
//                                // Read the decrypted data and store it in the plaintext variable
//                                plaintext = srDecrypt.ReadToEnd();
//                            }
//                        }
//                    }
//                }

//                // Return the plaintext
//                return plaintext;
//            }
//            catch (Exception ex)
//            {
//                // Handle decryption errors
//                // Log the exception or rethrow it as needed
//                throw new ArgumentException("Error decrypting cipherText.", ex);
//            }
//        }


//    }
//}

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Pet_Reunion_Hub.Helper
{
    public class EncryptionHelper
    {
        // Define a constant encryption key
        private const string EncryptionKey = "w7b6S2vZ9f3bN5dR7g2v4e8i1m3q6t9w";

        public static string Encrypt(string plainText)
        {
            byte[] encryptedBytes;
            byte[] iv;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.GenerateIV(); // Generate a random IV
                iv = aesAlg.IV;

                aesAlg.Key = Encoding.UTF8.GetBytes(EncryptionKey);

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, iv);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    msEncrypt.Write(iv, 0, iv.Length);

                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        encryptedBytes = msEncrypt.ToArray();
                    }
                }
            }
            return Convert.ToBase64String(encryptedBytes);
        }

        public static string Decrypt(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText))
            {
                // Handle null or empty cipherText
                return string.Empty; // Or throw an exception if appropriate
            }

            try
            {
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                byte[] iv = new byte[16];
                Buffer.BlockCopy(cipherBytes, 0, iv, 0, iv.Length);
                byte[] encryptedData = new byte[cipherBytes.Length - iv.Length];
                Buffer.BlockCopy(cipherBytes, iv.Length, encryptedData, 0, encryptedData.Length);
                string plaintext = null;

                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = Encoding.UTF8.GetBytes(EncryptionKey);
                    aesAlg.IV = iv;
                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                    using (MemoryStream msDecrypt = new MemoryStream(encryptedData))
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            {
                                plaintext = srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }

                return plaintext;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error decrypting cipherText.", ex);
            }
        }
    }
}
