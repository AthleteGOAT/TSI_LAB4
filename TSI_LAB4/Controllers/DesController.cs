using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace TSI_LAB4.Controllers
{
	public class DesController : Controller
	{
        [HttpPost]
        public ActionResult EncryptData(IFormCollection form)
        {

            string original = form["stringForEncrypt"];
            string key = "12345678"; // Cheia trebuie să fie de 8 caractere
            string encrypted = Encrypt(original, key);


            TempData["DesEncrypted"] = encrypted;


            return RedirectToAction("DES", "Home");
        }

        [HttpPost]
        public ActionResult DecryptData(IFormCollection form)
        {
            string encrypted = form["stringForDecrypt"];
            string key = "12345678"; // Cheia trebuie să fie de 8 caractere
            string decrypted = Decrypt(encrypted, key);
            TempData["DesDecrypted"] = decrypted;

            return RedirectToAction("DES", "Home");
        }

        private static string Encrypt(string textToEncrypt, string key)
        {
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                des.Key = ASCIIEncoding.ASCII.GetBytes(key); // Setează cheia
                des.IV = ASCIIEncoding.ASCII.GetBytes(key);  // Setează vectorul de inițializare

                byte[] textBytes = Encoding.UTF8.GetBytes(textToEncrypt);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(textBytes, 0, textBytes.Length);
                        cs.FlushFinalBlock();
                        return Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
        }

        private static string Decrypt(string encryptedText, string key)
        {
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                des.Key = ASCIIEncoding.ASCII.GetBytes(key); // Setează cheia
                des.IV = ASCIIEncoding.ASCII.GetBytes(key);  // Setează vectorul de inițializare

                byte[] textBytes = Convert.FromBase64String(encryptedText);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(textBytes, 0, textBytes.Length);
                        cs.FlushFinalBlock();
                        return Encoding.UTF8.GetString(ms.ToArray());
                    }
                }
            }
        }

    }
}

