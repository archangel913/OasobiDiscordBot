namespace Application.Languages
{
    public class LanguageDictionary
    {
        public LanguageDictionary(Dictionary<string, string> keySentencePairs)
        {
            this.KeySentencePairs = keySentencePairs;
        }

        public Dictionary<string, string> KeySentencePairs { get; } = new();

        public string this[string key]
            => this.KeySentencePairs[key];

        public bool TryGetSentence(string key, out string sentence)
            => this.KeySentencePairs.TryGetValue(key, out sentence);
    } 
}