using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MM3.Simulation
{
    public sealed class NameGenerator
    {
        private Dictionary<char, Dictionary<char, int>> database = new Dictionary<char, Dictionary<char, int>>();
        private Dictionary<char, int> initialDatabase = new Dictionary<char, int>();

        public string GenerateName(Random rng)
        {
            var result = string.Empty;
            var resultIsValid = false;

            while (!resultIsValid)
            {
                result = string.Empty;


                var initialDatabaseValue = this.initialDatabase.Sum(o => o.Value);
                var initialDatabaseRoll = rng.Next(initialDatabaseValue);
                var initialDatabaseCounter = 0;
                foreach (var initialDatabaseMember in this.initialDatabase)
                {
                    initialDatabaseCounter += initialDatabaseMember.Value;
                    if (initialDatabaseRoll < initialDatabaseCounter)
                    {
                        result += initialDatabaseMember.Key.ToString().ToUpper();
                        break;
                    }
                }

                var continueBuilding = true;
                while (continueBuilding)
                {
                    var lastCharacter = char.ToLowerInvariant(result[result.Length - 1]);

                    if (char.IsWhiteSpace(lastCharacter))
                    {
                        result = result.Trim();
                        continueBuilding = false;
                        break;
                    }
                    else
                    {
                        var lastCharacterDatabase = this.database[lastCharacter];
                        var databaseValue = lastCharacterDatabase.Sum(o => o.Value);
                        var databaseRoll = rng.Next(databaseValue);
                        var databaseCounter = 0;
                        foreach (var databaseMember in lastCharacterDatabase)
                        {
                            databaseCounter += databaseMember.Value;
                            if (databaseRoll < databaseCounter)
                            {
                                result += databaseMember.Key.ToString();
                                break;
                            }
                        }
                    }
                }

                var loweredResult = result.ToLowerInvariant();
                if (loweredResult.Length > 3) resultIsValid = true;
                if (loweredResult.Length> 15) resultIsValid = false;
                var vowels = 0;
                if (loweredResult.Contains("a")) vowels += 1;
                if (loweredResult.Contains("e")) vowels += 1;
                if (loweredResult.Contains("i")) vowels += 1;
                if (loweredResult.Contains("o")) vowels += 1;
                if (loweredResult.Contains("u")) vowels += 1;
                if (vowels == 0) resultIsValid = false;
            }

            return result;
        }

        public void AddSourceToDatabase(string sourceData)
        {
            var lastCharacter = ' ';
            for (int i = 0; i < sourceData.Length; i++)
            {
                var character = char.ToLowerInvariant(sourceData[i]);

                if (!char.IsWhiteSpace(character) && char.IsLetter(character))
                {
                    if (char.IsWhiteSpace(lastCharacter))
                    {
                        if (!this.initialDatabase.ContainsKey(character)) this.initialDatabase.Add(character, 0);
                        this.initialDatabase[character] = this.initialDatabase[character] + 1;
                    }

                    var nextIndex = i + 1;
                    if (nextIndex < sourceData.Length)
                    {
                        var nextCharacter = char.ToLowerInvariant(sourceData[nextIndex]);
                        if (char.IsWhiteSpace(nextCharacter) || char.IsLetter(nextCharacter))
                        {
                            if (!this.database.ContainsKey(character)) this.database.Add(character, new Dictionary<char, int>());
                            if (!this.database[character].ContainsKey(nextCharacter)) this.database[character].Add(nextCharacter, 0);
                            this.database[character][nextCharacter] = this.database[character][nextCharacter] + 1;
                        }
                    }
                }

                lastCharacter = character;
            }
        }

        public void SaveToDisk(string filePath)
        {
            using (var fileStream = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                using (var writer = new BinaryWriter(fileStream))
                {
                    var initialItemCount = this.initialDatabase.Count;
                    writer.Write(initialItemCount);
                    foreach(var item in this.initialDatabase.Keys)
                    {
                        writer.Write(item);
                        writer.Write(this.initialDatabase[item]);
                    }

                    var databaseItemCount = this.database.Count;
                    writer.Write(databaseItemCount);
                    foreach (var item in this.database.Keys)
                    {
                        writer.Write(item);
                        var subitemCount = this.database[item].Count;
                        writer.Write(subitemCount);
                        foreach (var subitem in this.database[item].Keys)
                        {
                            writer.Write(subitem);
                            writer.Write(this.database[item][subitem]);
                        }
                    }
                }
            }
        }

        public void LoadFromDisk(string filePath)
        {
            this.database = new Dictionary<char, Dictionary<char, int>>();
            this.initialDatabase = new Dictionary<char, int>();

            using (var fileStream = new FileStream(filePath, FileMode.Open))
            {
                using (var reader = new BinaryReader(fileStream))
                {
                    var initialItemCount = reader.ReadInt32();
                    for (int itemIndex = 0; itemIndex < initialItemCount; itemIndex++)
                    {
                        var initialDatabaseItem = reader.ReadChar();
                        var initialDatabaseValue = reader.ReadInt32();
                        this.initialDatabase.Add(initialDatabaseItem, initialDatabaseValue);
                    }

                    var itemCount = reader.ReadInt32();
                    for (int itemIndex = 0; itemIndex < itemCount; itemIndex++)
                    {
                        var databaseItem = reader.ReadChar();
                        this.database.Add(databaseItem, new Dictionary<char, int>());

                        var subitemCount = reader.ReadInt32();
                        for (int subitemIndex = 0; subitemIndex < subitemCount; subitemIndex++)
                        {
                            var subitem = reader.ReadChar();
                            var subitemValue = reader.ReadInt32();
                            this.database[databaseItem].Add(subitem, subitemValue);
                        }
                    }
                }
            }
        }
    }
}
