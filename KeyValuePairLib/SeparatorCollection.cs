using System.Collections;

namespace KeyValuePairLib
{
    /// <summary>
    /// This class stores separators and their variations for key-value pairs which are connected by a long sequence of separators.
    /// Diese Klasse speichert Trennzeichen und ihre Variationen für Schlüssel-Wert-Paare, welche durch eine lange Sequenz von Trennzeichen verbunden sind.
    /// </summary>
    public class SeparatorCollection : IEnumerable<string>
    {
        // ! FIELDS
        #region fields
        /// <summary>
        /// Reference value for AddSeparatorVariations method.
        /// Referenzwert für AddSeparatorVariations-Methode.
        /// </summary>
        private readonly int _maxWhiteSpaces = 3;
        /// <summary>
        /// This defines the minimum number of separators for key-value pairs which are connected by a long sequence of separators.
        /// Hier wird die minimale Anzahl von Trennzeichen definiert für Schlüssel-Wert-Paare, welche durch eine lange Sequenz von Trennzeichen verbunden sind.
        /// </summary>
        private readonly int _minSeparatorSpaces = 2;
        /// <summary>
        /// This defines the maximum number of separators for key-value pairs which are connected by a long sequence of separators.
        /// Hier wird die maximale Anzahl von Trennzeichen definiert für Schlüssel-Wert-Paare, welche durch eine lange Sequenz von Trennzeichen verbunden sind.
        /// </summary>
        private readonly int _maxSeparatorSpaces = 30;
        /// <summary>
        /// In this HashSet the masked separator strings are stored.
        /// In diesem HashSet werden die maskierten Trennzeichen-Zeichenfolgen gespeichert.
        /// </summary>
        private readonly HashSet<string> _separators = new();
        /// <summary>
        /// In this HashSet the variations of masked separator strings are stored.
        /// In diesem HashSet werden die Variationen der maskierten Trennzeichen-Zeichenfolgen gespeichert.
        /// </summary>
        private readonly HashSet<string> _separatorVariations = new();
        /// <summary>
        /// Instantiation of the RegexUtils class.
        /// Instanziierung der RegexUtils-Klasse.
        /// </summary>
        private readonly RegexUtils _regexUtils = new();
        #endregion

        // ! PROPERTIES
        #region properties
        /// <summary>
        /// Gets or sets the HashSet containing the masked separator strings.
        /// Ruft das HashSet ab, das die maskierten Trennzeichen-Zeichenfolgen enthält, oder legt es fest.
        /// </summary>
        public HashSet<string> Separators
        {
            get => _separators;
            set
            {
                foreach (var item in value) Add(item);
            }
        }
        /// <summary>
        /// Gets the HashSet containing the variations of masked separator strings.
        /// Ruft das HashSet ab, das die Variationen der maskierten Trennzeichen-Zeichenfolgen enthält.
        /// </summary>
        public HashSet<string> SeparatorVariations
        {
            get => _separatorVariations;
        }
        #endregion

        // ! CONSTRUCTORS
        #region constructors
        /// <summary>
        /// Initializes a new instance of the Separators class with default values for MaxWhiteSpaces, MinSeparatorSpaces and MaxSeparatorSpaces.
        /// Initialisiert eine neue Instanz der Klasse Separators mit Standardwerten für MaxWhiteSpaces, MinSeparatorSpaces und MaxSeparatorSpaces.
        /// </summary>
        public SeparatorCollection() { }
        /// <summary>
        /// Initializes a new instance of the Separators class with the provided separator strings as IEnumerable.
        /// Initialisiert eine neue Instanz der Klasse Separators mit den angegebenen Trennzeichen-Zeichenfolgen als IEnumerable.
        /// </summary>
        /// <param name="separators">The IEnumerable of separator strings. Die IEnumerable der Trennzeichen-Zeichenfolgen.</param>
        public SeparatorCollection(IEnumerable<string> separators) : this()
        {
            AddRange(separators);
        }
        /// <summary>
        /// Initializes a new instance of the Separators class with the provided values for MaxWhiteSpaces, MinSeparatorSpaces and MaxSeparatorSpaces.
        /// Initialisiert eine neue Instanz der Klasse Separators mit den angegebenen Werten für MaxWhiteSpaces, MinSeparatorSpaces und MaxSeparatorSpaces.
        /// </summary>
        /// <param name="maxWhiteSpaces">The value for MaxWhiteSpaces. Der Wert für MaxWhiteSpaces.</param>
        /// <param name="minSeparatorSpaces">The value for MinSeparatorSpaces. Der Wert für MinSeparatorSpaces.</param>
        /// <param name="maxSeparatorSpaces">The value for MaxSeparatorSpaces. Der Wert für MaxSeparatorSpaces.</param>
        public SeparatorCollection(int maxWhiteSpaces, int minSeparatorSpaces, int maxSeparatorSpaces)
        {
                this._maxWhiteSpaces = maxWhiteSpaces;
            this._minSeparatorSpaces = minSeparatorSpaces;
            this._maxSeparatorSpaces = maxSeparatorSpaces;
        }
        /// <summary>
        /// Initializes a new instance of the Separators class with the provided values for MaxWhiteSpaces, MinSeparatorSpaces, MaxSeparatorSpaces and separator strings as IEnumerable.
        /// Initialisiert eine neue Instanz der Klasse Separators mit den angegebenen Werten für MaxWhiteSpaces, MinSeparatorSpaces, MaxSeparatorSpaces und den Trennzeichen-Zeichenfolgen als IEnumerable.
        /// </summary>
        /// <param name="maxWhiteSpaces">The value for MaxWhiteSpaces. Der Wert für MaxWhiteSpaces.</param>
        /// <param name="minSeparatorSpaces">The value for MinSeparatorSpaces. Der Wert für MinSeparatorSpaces.</param>
        /// <param name="maxSeparatorSpaces">The value for MaxSeparatorSpaces. Der Wert für MaxSeparatorSpaces.</param>
        /// <param name="separators">The IEnumerable of separator strings. Die IEnumerable der Trennzeichen-Zeichenfolgen.</param>
        public SeparatorCollection(int maxWhiteSpaces, int minSeparatorSpaces, int maxSeparatorSpaces,
            IEnumerable<string> separators) : this(maxWhiteSpaces, minSeparatorSpaces, maxSeparatorSpaces)
        {
            AddRange(separators);
        }
        #endregion

        // ! METHODS
        #region methods
        /// <summary>
        /// Adds a new separator string to the HashSet of separators.
        /// Fügt dem HashSet der Trennzeichen eine neue Trennzeichen-Zeichenfolge hinzu.
        /// </summary>
        /// <param name="separator">The separator string. Die Trennzeichen-Zeichenfolge.</param>
        public void Add(string separator)
        {
            _separators.Add(_regexUtils.MaskRegexChars(separator));
            AddSeparatorVariations(separator);
        }
        /// <summary>
        /// Adds a IEnumerable of separator strings to the HashSet of separators.
        /// Fügt dem HashSet der Trennzeichen eine IEnumerable von Trennzeichen-Zeichenfolgen hinzu.
        /// </summary>
        /// <param name="separators">The IEnumerable of separator strings. Die IEnumerable der Trennzeichen-Zeichenfolgen.</param>
        public void AddRange(IEnumerable<string> separators)
        {
            foreach (string separator in separators) Add(separator);
        }
        /// <summary>
        /// Adds variations of the separator string to the HashSet of separator variations.
        /// Fügt Variationen der Trennzeichen-Zeichenfolge dem HashSet von Trennzeichen-Variationen hinzu.
        /// </summary>
        /// <param name="separator">The separator string to generate variations from. Die Trennzeichen-Zeichenfolge, von der Variationen generiert werden.</param>
        private void AddSeparatorVariations(string separator)
        {
            // ! Iterate over white spaces
            for (int i = _maxWhiteSpaces - 1; i > 0; i--)
            {
                // ! Add separator string with white spaces
                _separatorVariations.Add(separator + new string(' ', i));
                _separatorVariations.Add(new string(' ', i) + separator);
                // ! Iterate over separator spaces
                for (int j = _minSeparatorSpaces; j <= _maxSeparatorSpaces; j++)
                {
                    // ! Generate separator chain for single character separator
                    if (separator.Length == 1)
                    {
                        // ! Multiply single character separator
                        char[] chainChar = separator.ToCharArray();
                        string separatorChain = new(chainChar[0], j);
                        // ! Add separator chain
                        _separatorVariations.Add(separatorChain);
                        // ! Add separator chain with white spaces
                        _separatorVariations.Add(separatorChain + new string(' ', i));
                        _separatorVariations.Add(new string(' ', i) + separatorChain);
                        _separatorVariations.Add(new string(' ', i) + separatorChain + new string(' ', i));
                    }
                    // ! Generate separator chain for string separator
                    else
                    {
                        // ! Multiply string separator
                        string separatorChain = "";
                        for (int k = j; k > 0; k--) separatorChain += separator;
                        // ! Determine chain length for repeated string separators
                        int chainLength = separatorChain.Length / separator.Length;
                        while (chainLength % separator.Length != 0) chainLength++;
                        // ! Reduce chain length
                        separatorChain = separatorChain[..chainLength];
                        // ! Add separator chain
                        _separatorVariations.Add(separatorChain);
                        // ! Add separator chain with white spaces
                        _separatorVariations.Add(separatorChain + new string(' ', i));
                        _separatorVariations.Add(new string(' ', i) + separatorChain);
                        _separatorVariations.Add(new string(' ', i) + separatorChain + new string(' ', i));
                    }
                }
            }
        }
        /// <summary>
        /// Outputs the variations of masked separator strings to the console.
        /// Gibt die Variationen der maskierten Trennzeichen auf der Konsole aus.
        /// </summary>
        public void CONSOLE_SeparatorCollection()
        {
            Console.WriteLine("CONSOLE_SeparatorCollection:");
            Console.WriteLine("############################\r\n");
            foreach (var separatorVariation in _separatorVariations) Console.WriteLine($"- \"{separatorVariation}\"");
            Console.ReadLine();
        }
        /// <summary>
        /// Returns an enumerator that iterates through the collection of separators.
        /// Gibt einen Enumerator zurück, der die Sammlung von Trennzeichen durchläuft.
        /// </summary>
        /// <returns>The enumerator that can be used to iterate through the collection of separators. Der Enumerator, der verwendet werden kann, um durch die Sammlung von Trennzeichen zu iterieren.</returns>
        public IEnumerator<string> GetEnumerator() => _separators.GetEnumerator();
        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// Gibt einen Enumerator zurück, der eine Sammlung durchläuft.
        /// </summary>
        /// <returns>The enumerator that can be used to iterate through the collection. Der Enumerator, der verwendet werden kann, um durch die Sammlung zu iterieren.</returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        #endregion
    }
}