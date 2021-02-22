using Newtonsoft.Json;
using RockLib.Encryption;
using RockLib.Encryption.FieldLevel;
using RockLib.Encryption.Symmetric;
using RockLib.Encryption.Testing;
using System;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Example.FieldLevel
{
    class Program
    {
        static void Main(string[] args)
        {
            ICrypto crypto =
                new SymmetricCrypto(new[] { new Credential(() => Convert.FromBase64String("1J9Og/OaZKWdfdwM6jWMpvlr3q3o7r20xxFDN7TEj6s=")) });

            Person person = new Person
            {
                FirstName = "J",
                LastName = "Public",
                SSN = "123-45-6789"
            };

            string xml = SerializeXml(person);
            Console.WriteLine("Original XML:");
            Console.WriteLine(xml);
            Console.WriteLine();

            string encryptedXml = crypto.EncryptXml(xml, "/Person/SSN");
            Console.WriteLine("Encrypted XML:");
            Console.WriteLine(encryptedXml);
            Console.WriteLine();

            string decryptedXml = crypto.DecryptXml(encryptedXml, "/Person/SSN");
            Console.WriteLine("Decrypted XML:");
            Console.WriteLine(decryptedXml);
            Console.WriteLine();

            string json = SerializeJson(person);
            Console.WriteLine("Original JSON:");
            Console.WriteLine(json);
            Console.WriteLine();

            string encryptedJson = crypto.EncryptJson(json, "$.SSN");
            Console.WriteLine("Encrypted JSON:");
            Console.WriteLine(encryptedJson);
            Console.WriteLine();

            string decryptedJson = crypto.DecryptJson(encryptedJson, "$.SSN");
            Console.WriteLine("Decrypted JSON:");
            Console.WriteLine(decryptedJson);
            Console.WriteLine();
        }

        private static string SerializeXml(Person person)
        {
            var serializer = new XmlSerializer(typeof(Person));
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            var sb = new StringBuilder();
            using (var writer = XmlWriter.Create(sb, new XmlWriterSettings { Indent = false, OmitXmlDeclaration = true }))
                serializer.Serialize(writer, person, ns);
            return sb.ToString();
        }

        private static string SerializeJson(Person person) =>
            JsonConvert.SerializeObject(person, Newtonsoft.Json.Formatting.None);
    }

    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SSN { get; set; }
    }
}
