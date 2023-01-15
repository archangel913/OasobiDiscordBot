using Discord;
using Domain.Musics;
using Domain.Factory;
using Application.Languages;
using Application.Interface;

namespace Application.Exprimental
{
    public class Experimental
    {
        public static LanguageDictionary TestLang()
        {
            var languageRepository = Factory.GetService<ILanguageRepository>();
            return languageRepository.Find();
        }
    }
}
