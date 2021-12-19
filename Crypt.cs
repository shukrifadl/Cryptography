using System.Security.Cryptography;

namespace EncryptUsingAES
{
    class Crypt
    {
        //generate key
        public static readonly byte[] key = Aes.Create().Key;
        //generate IV
        public static readonly byte[] IV = Aes.Create().IV;
        //set mode
        public static CipherMode mode = CipherMode.ECB;

        public static byte[] Encrypt(byte[] plain)
        {

            AesCryptoServiceProvider aes = new()
            {
                Key = key,
                Mode = mode,
                BlockSize = 128,
                KeySize = 256,
                Padding = PaddingMode.Zeros
            };

            ICryptoTransform crypto = aes.CreateEncryptor(key, IV);
            byte[] cipher = crypto.TransformFinalBlock(plain, 0, plain.Length);
            crypto.Dispose();
            return cipher;
        }

        public static byte[] Decrypt(byte[] cipher)
        {
            AesCryptoServiceProvider aes = new()
            {
                Key = key,
                Mode = mode,
                BlockSize = 128,
                KeySize = 256,
                Padding = PaddingMode.Zeros
            };
            ICryptoTransform crypto = aes.CreateDecryptor(key, IV);
            byte[] plain = crypto.TransformFinalBlock(cipher, 0, cipher.Length);
            crypto.Dispose();
            return plain;
        }
    }
}
