﻿using RockLib.Encryption;
using RockLib.Encryption.Symmetric;
using System;
using XSerializer.Encryption;

namespace Example.XSerializer;

#pragma warning disable CA1303 // Do not pass literals as localized parameters
class Program
{
    static void Main(string[] args)
    {
        ICrypto crypto =
               new SymmetricCrypto(new[] { new Credential(() => Convert.FromBase64String("3x6ZwzENlKLrQB3fSuuM4+8z1OYjygDAlKZNmSBXASQ=")) });

        var person = new Person
        {
            FirstName = "J",
            LastName = "Public",
            SSN = "123-45-6789"
        };

        Console.WriteLine("Initial:");
        Console.WriteLine($"FirstName: {person.FirstName}, LastName: {person.LastName}, SSN: {person.SSN}");
        Console.WriteLine();

        var encryptedXml = crypto.ToXml(person);
        Console.WriteLine("XML serialized:");
        Console.WriteLine(encryptedXml);
        Console.WriteLine();

        person = crypto.FromXml<Person>(encryptedXml);
        Console.WriteLine("XML deserialized:");
        Console.WriteLine($"FirstName: {person.FirstName}, LastName: {person.LastName}, SSN: {person.SSN}");
        Console.WriteLine();

        var encryptedJson = crypto.ToJson(person);
        Console.WriteLine("JSON serialized:");
        Console.WriteLine(encryptedJson);
        Console.WriteLine();

        person = crypto.FromJson<Person>(encryptedJson);
        Console.WriteLine("JSON deserialized:");
        Console.WriteLine($"FirstName: {person.FirstName}, LastName: {person.LastName}, SSN: {person.SSN}");
        Console.WriteLine();
    }
}

public class Person
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    [Encrypt] public string? SSN { get; set; }
}
#pragma warning restore CA1303 // Do not pass literals as localized parameters
