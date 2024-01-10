namespace KeyValuePairLib
{
    /// <summary>
    /// Utility class with default values for masking and unmasking character occurrences in the document, which are interpreted in regular expressions.
    /// Hilfsklasse mit Standardwerten zum Maskieren und Demaskieren von Zeichenvorkommen im Dokument, welche in regulären Ausdrücken interpretiert werden.
    /// </summary>
    public class RegexUtils
    {
        // ! PROPERTIES
        #region properties
        /// <summary>
        /// This dictionary defines character occurrences which are interpreted in regular expressions.
        /// In diesem Wörterbuch werden Zeichenvorkommen definiert, welche in regulären Ausdrücken interpretiert werden.
        /// </summary>
        public Dictionary<string, string> MaskedRegexChars { get; set; } = new()
        {
            { @"*", @"￼"},// ! "Object Replacement Character" (U+FFFC)
            { @"^", @"⋀" },// ! "N-ARY LOGICAL AND" (U+22C0)
            { @"$", @"⩳" },// ! "TWO CONSECUTIVE EQUALS SIGNS" (U+2A73)
            { @"|", @"∣" },// ! "DIVIDES" (U+2223)
            { @"(", @"⸦"},// ! "LEFT SIDEWAYS U BRACKET" (U+2E26)
            { @")", @"⸧"},// ! "RIGHT SIDEWAYS U BRACKET" (U+2E27)
            { @"{", @"⦓" },// ! "LEFT WHITE SQUARE BRACKET" (U+27D3)
            { @"}", @"⦔" },// ! "RIGHT WHITE SQUARE BRACKET" (U+27D4)
            { @"[", @"⦗" },// ! "LEFT WHITE CURLY BRACKET" (U+27D7)
            { @"]", @"⦘" },// ! "RIGHT WHITE CURLY BRACKET" (U+27D8)
        };
        #endregion

        // ! CONSTRUCTORS
        #region constructors
        /// <summary>
        /// Initializes a new instance of the RegexUtils class with default values.
        /// Initialisiert eine neue Instanz der Klasse RegexUtils mit Standardwerten.
        /// </summary>
        public RegexUtils() { }
        /// <summary>
        /// Initializes a new instance of the RegexUtils class with the provided collection of target values and replace values.
        /// Initialisiert eine neue Instanz der Klasse RegexUtils mit der angegebenen Sammlung von Ziel- und Ersetzungswerten.
        /// </summary>
        /// <param name="keyValuePairs">The collection of target values and replace values as tuples. Die Sammlung der Ziel- und Ersetzungswerte als Tupeln.</param>
        public RegexUtils(IEnumerable<(string, string)> keyValuePairs)
        {
            MaskedRegexChars.Clear();
            AddRange(keyValuePairs);
        }
        #endregion

        // ! METHODS
        #region methods
        /// <summary>
        /// Adds a new pair of target values and replace values.
        /// Fügt ein neues Paar von Zielwerten und Ersatzwerten hinzu.
        /// </summary>
        /// <param name="key">The target value to be replaced. Das Zielwert, das ersetzt wird.</param>
        /// <param name="value">The replace value. Der Ersetzungswert.</param>
        public void Add(string key, string value) => MaskedRegexChars.Add(key, value);
        /// <summary>
        /// Adds a collection of target values and replace values.
        /// Fügt eine Sammlung von Zielwerten und Ersatzwerten hinzu.
        /// </summary>
        /// <param name="keyValuePairs">The collection of target values and replace values as tuples. Die Sammlung der Ziel- und Ersetzungswerte als Tupeln.</param>
        public void AddRange(IEnumerable<(string, string)> keyValuePairs)
        {
            foreach (var keyValuePair in keyValuePairs.ToList()) Add(keyValuePair.Item1, keyValuePair.Item2);
        }
        /// <summary>
        /// Method for masking character occurrences for regular expressions.
        /// Methode zum Maskieren von Zeichenvorkommen für reguläre Ausdrücke.
        /// </summary>
        /// <param name="input">The input text to be masked. Der Eingabetext, der maskiert wird.</param>
        /// <returns>The masked output text. Der maskierte Ausgabetext.</returns>
        internal string MaskRegexChars(string input)
        {
            string output = input;
            foreach (var maskedRegexChar in MaskedRegexChars)
            {
                output = output.Replace(maskedRegexChar.Key, maskedRegexChar.Value);
            }
            return output;
        }
        /// <summary>
        /// Method for demasking character occurrences for regular expressions.
        /// Methode zum Demaskieren von Zeichenvorkommen für reguläre Ausdrücke.
        /// </summary>
        /// <param name="input">The input text to be demasked. Der Eingabetext, der demaskiert wird.</param>
        /// <returns>The demasked output text. Der demaskierte Ausgabetext.</returns>
        internal string DemaskRegexChars(string input)
        {
            string output = input;
            foreach (var maskedRegexChar in MaskedRegexChars)
            {
                output = output.Replace(maskedRegexChar.Value, maskedRegexChar.Key);
            }
            return output;
        }
        #endregion
    }
}