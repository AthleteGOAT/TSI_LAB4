using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace TSI_LAB4.Controllers
{
    public class SignatureDigitalController : Controller
    {
        [HttpPost]
        public IActionResult Generate(IFormCollection form)
        {
            var stringForSignature = form["stringForSignature"];

            if (string.IsNullOrEmpty(stringForSignature))
            {
                TempData["Error"] = "Please enter some data to sign.";
                return RedirectToAction("SignatureDigital", "Home");
            }

            using (var rsa = RSA.Create()) // Using RSA instead of DSA
            {
                byte[] data = Encoding.UTF8.GetBytes(stringForSignature);
                byte[] signature = rsa.SignData(data, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1); // RSA supports SHA256

                TempData["Signature"] = Convert.ToBase64String(signature);
                TempData["PublicKey"] = Convert.ToBase64String(rsa.ExportSubjectPublicKeyInfo()); // Exporting public key for RSA
            }

            return RedirectToAction("SignatureDigital","Home");
        }

        [HttpPost]
        public IActionResult Verify(IFormCollection form)
        {
            var stringForSignature = form["stringForSignature"];
            var signatureBase64 = form["signature"];
            var publicKeyBase64 = form["publicKey"];

            if (string.IsNullOrEmpty(stringForSignature) || string.IsNullOrEmpty(signatureBase64) || string.IsNullOrEmpty(publicKeyBase64))
            {
                TempData["Error"] = "All fields are required for verification.";
                return RedirectToAction("SignatureDigital", "Home");
            }

            byte[] signature = Convert.FromBase64String(signatureBase64);
            byte[] data = Encoding.UTF8.GetBytes(stringForSignature);
            byte[] publicKey = Convert.FromBase64String(publicKeyBase64);

            using (var rsa = RSA.Create())
            {
                rsa.ImportSubjectPublicKeyInfo(publicKey, out _); // Import the public key info
                bool isValid = rsa.VerifyData(data, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

                TempData["Verified"] = isValid;
                TempData["SignatureToVerify"] = signatureBase64;
                TempData["MessageToVerify"] = stringForSignature;
            }

            return RedirectToAction("SignatureDigital", "Home");
        }

    }
}