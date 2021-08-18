using Newtonsoft.Json;

namespace MG.Settings.Json
{
    public struct JsonFormat : IIndentSettings
    {
        private char _backingChar;
        private int _backingCount;
        private Formatting _backingFormat;

        public Formatting Formatting => _backingFormat;
        public char IndentChar => _backingChar;
        public int IndentCount => _backingCount;

        private JsonFormat(char defaultChar, int defaultCount, Formatting defaultFormatting)
        {
            _backingChar = defaultChar;
            _backingCount = defaultCount;
            _backingFormat = defaultFormatting;
        }

        public static JsonFormat Default => new JsonFormat((char)9, 1, Formatting.Indented);
    }
}
