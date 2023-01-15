using Application.Languages;

namespace Application.Interface
{
    public interface ILanguageRepository
    {
        public LanguageDictionary Find();
    }
}
