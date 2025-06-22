using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

class Program
{
    static readonly byte[] AesKey = Encoding.UTF8.GetBytes("4919871432539453");
    static readonly byte[] AesIV = Encoding.UTF8.GetBytes("4919871432539453");

    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        Console.Write("Enter save slot number (e.g., 1, 2, 3...): ");
        int slot;
        if (!int.TryParse(Console.ReadLine(), out slot))
        {
            Console.WriteLine("❌ Invalid slot number.");
            return;
        }

        string inputPath = $"saveData{slot}.bytes";
        string outputXml = $"saveData{slot}-edited.xml";
        string outputBytes = $"saveData{slot}-edited.bytes";

        if (!File.Exists(inputPath))
        {
            Console.WriteLine("❌ File not found: " + inputPath);
            return;
        }

        try
        {
            byte[] encrypted = File.ReadAllBytes(inputPath);
            string xml = DecryptAes(encrypted, AesKey, AesIV);

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            RunEditor(doc);

            File.WriteAllText(outputXml, doc.OuterXml, Encoding.UTF8);
            byte[] newEncrypted = EncryptAes(Encoding.UTF8.GetBytes(doc.OuterXml), AesKey, AesIV);
            File.WriteAllBytes(outputBytes, newEncrypted);

            Console.WriteLine("\n✅ Saved edited XML to: " + outputXml);
            Console.WriteLine("✅ Saved encrypted save to: " + outputBytes);
        }
        catch (Exception ex)
        {
            Console.WriteLine("❌ Error: " + ex.Message);
        }
    }

    static void RunEditor(XmlDocument doc)
    {
        Dictionary<string, string> editableFields = new Dictionary<string, string>()
        {
            { "cafeCount", "Café Count" },
            { "cash", "Cash" },
            { "workPower", "Work Power" },
            { "koukando", "Affection (koukando)" },
            { "inran", "Lewdness (inran)" }
        };

        Dictionary<int, string> indexToKey = new Dictionary<int, string>();

        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Save Editor Menu ===");

            int index = 1;
            foreach (KeyValuePair<string, string> kv in editableFields)
            {
                string xmlKey = kv.Key;
                string label = kv.Value;
                string value = GetXmlValue(doc, xmlKey);

                Console.WriteLine("[" + index + "] " + label.PadRight(24) + " : " + value);
                indexToKey[index] = xmlKey;
                index++;
            }

            Console.WriteLine("\n[S] Save & Exit");
            Console.WriteLine("[Q] Quit Without Saving");
            Console.Write("\nSelect option: ");

            string input = Console.ReadLine();
            if (input == null) continue;

            input = input.Trim().ToLower();

            if (input == "s") break;
            if (input == "q")
            {
                Console.WriteLine("❌ Exit without saving.");
                Environment.Exit(0);
            }

            int selected;
            if (int.TryParse(input, out selected) && indexToKey.ContainsKey(selected))
            {
                string xmlKey = indexToKey[selected];
                string label = editableFields[xmlKey];
                string oldVal = GetXmlValue(doc, xmlKey);

                Console.Write("Enter new value for " + label + " (current: " + oldVal + "): ");
                string newVal = Console.ReadLine();
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
                Console.WriteLine("❌ Invalid option. Press any key...");
                Console.ReadKey();
            }
        }
    }

    static string GetXmlValue(XmlDocument doc, string key)
    {
        XmlNode node = doc.SelectSingleNode("/plist/Dictionary/key[text()='" + key + "']/following-sibling::*[1]");
        return node != null ? node.InnerText : "(not found)";
    }

    static void SetXmlValue(XmlDocument doc, string key, string newValue)
    {
        XmlNode node = doc.SelectSingleNode("/plist/Dictionary/key[text()='" + key + "']/following-sibling::*[1]");
        if (node != null) node.InnerText = newValue;
    }

    static string DecryptAes(byte[] data, byte[] key, byte[] iv)
    {
        using (Aes aes = Aes.Create())
        {
            aes.KeySize = 128;
            aes.BlockSize = 128;
            aes.Key = key;
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            ICryptoTransform decryptor = aes.CreateDecryptor();
            try
            {
                byte[] result = decryptor.TransformFinalBlock(data, 0, data.Length);
                return Encoding.UTF8.GetString(result);
            }
            finally
            {
                decryptor.Dispose();
            }
        }
    }

    static byte[] EncryptAes(byte[] data, byte[] key, byte[] iv)
    {
        using (Aes aes = Aes.Create())
        {
            aes.KeySize = 128;
            aes.BlockSize = 128;
            aes.Key = key;
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            ICryptoTransform encryptor = aes.CreateEncryptor();
            try
            {
                return encryptor.TransformFinalBlock(data, 0, data.Length);
            }
            finally
            {
                encryptor.Dispose();
            }
        }
    }
}