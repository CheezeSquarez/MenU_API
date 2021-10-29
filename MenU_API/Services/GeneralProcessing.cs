using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Linq;



namespace MenU_API.Sevices
{
    public class GeneralProcessing
    {
        
        public static int GenerateCryptoRandomINT(int minValue, int maxExclusiveValue)
        {
            if (minValue >= maxExclusiveValue)
                throw new ArgumentOutOfRangeException("minValue must be lower than maxExclusiveValue");

            long diff = (long)maxExclusiveValue - minValue;
            long upperBound = uint.MaxValue / diff * diff;

            uint ui;
            do
            {
                ui = GetRandomUInt();
            } while (ui >= upperBound);
            return (int)(minValue + (ui % diff));
        }

        private static uint GetRandomUInt()
        {
            var randomBytes = GenerateRandomBytes(sizeof(uint));
            return BitConverter.ToUInt32(randomBytes, 0);
        }

        private static byte[] GenerateRandomBytes(int bytesNumber)
        {
            byte[] buffer = new byte[bytesNumber];
            using (RNGCryptoServiceProvider csp = new RNGCryptoServiceProvider())
            {
                csp.GetBytes(buffer);
            }
            return buffer;
        }

        public static string GenerateAlphanumerical(int size)
        {
            char[] chars =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            byte[] data = new byte[4 * size];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetBytes(data);
            }
            StringBuilder result = new StringBuilder(size);
            for (int i = 0; i < size; i++)
            {
                var rnd = BitConverter.ToUInt32(data, i * 4);
                var idx = rnd % chars.Length;

                result.Append(chars[idx]);
            }
            return result.ToString();
        }

        //This method will be used to calculate a hash from a desired string
        public static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }

        }


        /// <summary>
        /// Recieves the raw password, the salt and the number of hash iterations
        /// and runs the password with the salt through the hash function, the desired amount of times (iterations)
        /// </summary>
        /// <param name="pass">string containing the raw password</param>
        /// <param name="salt">string containing the salt of the logged in user</param>
        /// <param name="iterations">int specifying how many times the password and salt should be hashed</param>
        /// <returns>Hashed password calculated with string and hashed the desired ammount of times</returns>
        public static string PlainTextToHashedPassword(string pass, string salt, int iterations)
        {
            string hash = pass + salt;

            for (int i = 0; i < iterations; i++)
            {
                hash = ComputeSha256Hash(hash);
            }
            return hash;
        }
    }
}
