using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TurismoReal.Class
{
    public class EncryptString   {  }

    public static class StringCipher
    {
        // log4net.
        //private static readonly ILog Log = LogManager.GetLogger(typeof(StringCipher));

        // Key privada para validar la comunicación entre sistemas.
        private static string passPhrase = "B4390E5BA12B4FB28FF5F3E2319242CBECB1CD8102364B7C897FF3E2319242CBE611F71603B347789BDDF3E2319242CB9BFB640BE71241828094F3E2319242CB";

        // Esta constante se utiliza para determinar el tamaño de clave del algoritmo de cifrado en bits.
        // Dividimos por 8 dentro del código a continuación para obtener el número equivalente de bytes.
        private const int Keysize = 128;

        // Esta constante determina el número de iteraciones para la función de generación de bytes de contraseña.
        private const int DerivationIterations = 1000;

        public static string Encrypt(string plainText)
        {
            try
            {
                // Salt y IV se generan aleatoriamente cada vez, pero se agrega al texto cifrado
                // para que se puedan usar los mismos valores Salt y IV al descifrar.
                var saltStringBytes = Generate128BitsOfRandomEntropy();
                var ivStringBytes = Generate128BitsOfRandomEntropy();
                var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
                using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
                {
                    var keyBytes = password.GetBytes(Keysize / 8);
                    using (var symmetricKey = new RijndaelManaged())
                    {
                        symmetricKey.BlockSize = 128;
                        symmetricKey.Mode = CipherMode.CBC;
                        symmetricKey.Padding = PaddingMode.PKCS7;
                        using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes))
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                                {
                                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                                    cryptoStream.FlushFinalBlock();
                                    // Crea los bytes finales como una concatenación de los bytes de salt aleatorios, los bytes iv aleatorios y los bytes de cifrado.
                                    var cipherTextBytes = saltStringBytes;
                                    cipherTextBytes = cipherTextBytes.Concat(ivStringBytes).ToArray();
                                    cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
                                    memoryStream.Close();
                                    cryptoStream.Close();
                                    return Convert.ToBase64String(cipherTextBytes);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                //Log.Error(e);
                return string.Empty;
            }
        }

        public static string Decrypt(string cipherText)
        {
            try
            {
                // Reemplaza los espacios por +.
                cipherText = cipherText.Replace(" ", "+");
                // Obtiene la secuencia completa de bytes que representan:
                // [16 bytes de Salt] + [16 bytes de IV] + [n bytes de CipherText]
                var cipherTextBytesWithSaltAndIv = Convert.FromBase64String(cipherText);
                // Obtiene los saltbytes extrayendo los primeros 16 bytes de los bytes de texto cifrado suministrados.
                var saltStringBytes = cipherTextBytesWithSaltAndIv.Take(Keysize / 8).ToArray();
                // Obtiene los bytes IV extrayendo los siguientes 16 bytes de los bytes de texto cifrado suministrados.
                var ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(Keysize / 8).Take(Keysize / 8).ToArray();
                // Obtiene los bytes de texto de cifrado reales eliminando los primeros 64 bytes de la cadena de texto de cifrado.
                var cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip((Keysize / 8) * 2).Take(cipherTextBytesWithSaltAndIv.Length - ((Keysize / 8) * 2)).ToArray();

                using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
                {
                    var keyBytes = password.GetBytes(Keysize / 8);
                    using (var symmetricKey = new RijndaelManaged())
                    {
                        symmetricKey.BlockSize = 128;
                        symmetricKey.Mode = CipherMode.CBC;
                        symmetricKey.Padding = PaddingMode.PKCS7;
                        using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes))
                        {
                            using (var memoryStream = new MemoryStream(cipherTextBytes))
                            {
                                using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                                {
                                    var plainTextBytes = new byte[cipherTextBytes.Length];
                                    var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                    memoryStream.Close();
                                    cryptoStream.Close();
                                    return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                //Log.Error(e);
                return string.Empty;
            }
        }

        private static byte[] Generate128BitsOfRandomEntropy()
        {
            var randomBytes = new byte[16]; // 16 bytes nos darán 128 bits.
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                // Rellena la matriz con bytes aleatorios criptográficamente seguros.
                rngCsp.GetBytes(randomBytes);
            }
            return randomBytes;
        }


        public static string GetFtrMd5Hash(MD5 md5Hash, string input)
        {
            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();
            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
        // Verify a hash against a string.
        public static bool VerifyFtrMd5Hash(MD5 md5Hash, string input, string hash)
        {
            // Hash the input.
            string hashOfInput = GetFtrMd5Hash(md5Hash, input);
            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
