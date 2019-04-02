using RockLib.Configuration.ObjectFactory;
using RockLib.Encryption;
using System.Collections.Generic;

[assembly: ConfigSection("RockLib.Encryption", typeof(List<ICrypto>))]