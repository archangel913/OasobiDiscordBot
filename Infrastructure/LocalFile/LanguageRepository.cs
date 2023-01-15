using System.Text.Json;
using System.IO;
using Application.Interface;
using Application.Languages;
using System.Text.Encodings.Web;
using System.Text.Unicode;
namespace Infrastructure.LocalFile
{
    public class LanguageRepository : ILanguageRepository
    {
        public LanguageRepository()
        {
            var settings = new SettingsReader().GetSettings();

            if (!Directory.Exists(this.LangDirectoryPath))
            {
                Directory.CreateDirectory(this.LangDirectoryPath);
            }

            this.LangFilePath = $@"{this.LangDirectoryPath}\{settings.BotLanguage}.json";
            if (!File.Exists(this.LangFilePath))
            {
                throw new FileNotFoundException($"{this.LangFilePath} was not found.");
            }
        }

        private string LangDirectoryPath { get; } = "Languages";

        private string LangFilePath { get; }

        private JsonSerializerOptions Options { get; } = new JsonSerializerOptions()
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
        };

        public LanguageDictionary Find()
        {
            using (var reader = new StreamReader(this.LangFilePath))
            {
                var json = reader.ReadToEnd();
                var languageDictionaryNullable = JsonSerializer.Deserialize<LanguageDictionary>(json, this.Options);
                if (languageDictionaryNullable is null)
                {
                    throw new NotSupportedException($"could not read {this.LangFilePath}.");
                }
                return languageDictionaryNullable;
            }
        }
    }
}