using Microsoft.AspNetCore.Mvc;

public class RsaController : Controller
{
    static RsaController()
    {
        RsaEncryptionHelper.AssignNewKey();
    }

    [HttpPost]
    public ActionResult RsaEncrypt(IFormCollection form)
    {
        var publicKeyXml = RsaEncryptionHelper.GetPublicKey();
        var encryptedText = RsaEncryptionHelper.Encrypt(form["stringForEncrypt"], publicKeyXml);

        // Parse the XML and extract the Modulus and Exponent
        var xmlDoc = new System.Xml.XmlDocument();
        xmlDoc.LoadXml(publicKeyXml);
        var modulus = xmlDoc.SelectSingleNode("//Modulus")?.InnerText ?? "Modulus not found";
        var exponent = xmlDoc.SelectSingleNode("//Exponent")?.InnerText ?? "Exponent not found";

        // Combine the Modulus and Exponent into a single string
        var publicKeyString = $"Modulus: {modulus}\nExponent: {exponent}";

        TempData["EncryptedText"] = encryptedText;
        TempData["PublicKey"] = publicKeyString; // Assign the user-friendly string

        return RedirectToAction("RSA", "Home");
    }

    [HttpPost]
    public ActionResult RsaDecrypt(IFormCollection form)
    {
        var privateKeyXml = RsaEncryptionHelper.GetPrivateKey();
        var decryptedText = RsaEncryptionHelper.Decrypt(form["encryptedString"], privateKeyXml);

        // Parse the XML and extract the relevant parts (e.g., Modulus, Exponent)
        var xmlDoc = new System.Xml.XmlDocument();
        xmlDoc.LoadXml(privateKeyXml);

        // Example: Extracting Modulus and Exponent from the XML
        var modulus = xmlDoc.SelectSingleNode("//Modulus")?.InnerText ?? "Modulus not found";
        var exponent = xmlDoc.SelectSingleNode("//Exponent")?.InnerText ?? "Exponent not found";

        // Combine the Modulus and Exponent into a single string
        var privateKeyString = $"Modulus: {modulus}\nExponent: {exponent}";

        TempData["DecryptedText"] = decryptedText;
        TempData["PrivateKey"] = privateKeyString; // Assign the user-friendly string

        return RedirectToAction("RSA", "Home");
    }

}
