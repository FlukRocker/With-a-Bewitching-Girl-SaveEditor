using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        string inputPath = "saveData2.bytes";
        string outputXml = "saveData2-edited.xml";
        string outputBytes = "saveData2-edited.bytes";

        byte[] key = Encoding.UTF8.GetBytes("4919871432539453");
        byte[] iv = Encoding.UTF8.GetBytes("4919871432539453");

        byte[] encrypted = File.ReadAllBytes(inputPath);
        string xml = DecryptAes(encrypted, key, iv);

        XmlDocument doc = new XmlDocument();
        doc.LoadXml(xml);

        // 🌟 Dictionary: XML key => Display label
        var editableFields = new Dictionary<string, string>
        {
            { "cafeCount", "Café Count" },
            { "cash", "Cash" },
            { "workPower", "Work Power" },
            { "koukando", "Affection (koukando)" },
            { "inran", "Lewdness (inran)" }
        };

        var indexToKey = new Dictionary<int, string>();

        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Save Editor Menu ===");
            int index = 1;

            foreach (var kv in editableFields)
            {
                string xmlKey = kv.Key;
                string label = kv.Value;
                string value = GetXmlValue(doc, xmlKey);

                Console.WriteLine($"[{index}] {label,-24} : {value}");
                indexToKey[index] = xmlKey;
                index++;
            }

            Console.WriteLine("\n[S] Save & Exit");
            Console.WriteLine("[Q] Quit Without Saving");
            Console.Write("\nSelect option: ");

            string input = Console.ReadLine()?.Trim().ToLower();

            if (input == "s")
            {
                // Save updated XML
                File.WriteAllText(outputXml, doc.OuterXml, Encoding.UTF8);
                byte[] newEncrypted = EncryptAes(Encoding.UTF8.GetBytes(doc.OuterXml), key, iv);
                File.WriteAllBytes(outputBytes, newEncrypted);

                Console.WriteLine($"\n✅ Saved to: {outputBytes}");
                break;
            }
            else if (input == "q")
            {
                Console.WriteLine("❌ Exit without saving.");
                break;
            }
            else if (int.TryParse(input, out int selected) && indexToKey.ContainsKey(selected))
            {
                string xmlKey = indexToKey[selected];
                string label = editableFields[xmlKey];
                string oldVal = GetXmlValue(doc, xmlKey);

                Console.Write($"Enter new value for {label} (current: {oldVal}): ");
                string newVal = Console.ReadLine()?.Trim();

                if (!string.IsNullOrWhiteSpace(newVal))
                {
                    SetXmlValue(doc, xmlKey, newVal);
                    Console.WriteLine("✅ Value updated. Press any key...");
                }
                else
                {
                    Console.WriteLine("⚠️ No change made.");
                }
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("❌ Invalid selection. Press any key...");
                Console.ReadKey();
            }
        }
    }

    static string GetXmlValue(XmlDocument doc, string key)
    {
        XmlNode node = doc.SelectSingleNode($"/plist/Dictionary/key[text()='{key}']/following-sibling::*[1]");
        return node?.InnerText ?? "(not found)";
    }

    static void SetXmlValue(XmlDocument doc, string key, string newValue)
    {
        XmlNode node = doc.SelectSingleNode($"/plist/Dictionary/key[text()='{key}']/following-sibling::*[1]");
        if (node != null)
        {
            node.InnerText = newValue;
        }
    }

    static string DecryptAes(byte[] data, byte[] key, byte[] iv)
    {
        using (Aes aes = Aes.Create())
        {
            aes.KeySize = 128; aes.BlockSize = 128;
            aes.Key = key; aes.IV = iv;
            aes.Mode = CipherMode.CBC; aes.Padding = PaddingMode.PKCS7;
            using (var decryptor = aes.CreateDecryptor())
            {
                byte[] result = decryptor.TransformFinalBlock(data, 0, data.Length);
                return Encoding.UTF8.GetString(result);
            }
        }
    }

    static byte[] EncryptAes(byte[] data, byte[] key, byte[] iv)
    {
        using (Aes aes = Aes.Create())
        {
            aes.KeySize = 128; aes.BlockSize = 128;
            aes.Key = key; aes.IV = iv;
            aes.Mode = CipherMode.CBC; aes.Padding = PaddingMode.PKCS7;
            using (var encryptor = aes.CreateEncryptor())
            {
                return encryptor.TransformFinalBlock(data, 0, data.Length);
            }
        }
    }
}
