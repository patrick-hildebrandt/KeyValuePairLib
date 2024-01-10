namespace KeyValuePairLib
{
    /// <summary>
    /// This class represents a custom HashSet of masked strings with regular expression prefixes and suffixes applied to each item.
    /// Diese Klasse stellt ein benutzerdefiniertes HashSet von maskierten Zeichenfolgen dar, bei dem auf jedes Element reguläre Ausdrucks-Präfixe und Suffixe angewendet werden.
    /// </summary>
    public class RegexWrappedCollection : HashSet<string>
    {
        // ! FIELDS
        #region fields
        /// <summary>
        /// Field for the regular expression prefix applied to each item in the HashSet.
        /// Feld für das reguläre Ausdruckspräfix, das auf jedes Element im HashSet angewendet wird.
        /// </summary>
        private string _regexPrefix = @"";
        /// <summary>
        /// Field for the regular expression suffix applied to each item in the HashSet.
        /// Feld für das reguläre Ausdrucksuffix, das auf jedes Element im HashSet angewendet wird.
        /// </summary>
        private string _regexSuffix = @"";
        /// <summary>
        /// Instantiation of the RegexUtils class.
        /// Instanziierung der RegexUtils-Klasse.
        /// </summary>
        private readonly RegexUtils _regexUtils = new();
        #endregion

        // ! PROPERTIES
        #region properties
        /// <summary>
        /// Gets or sets the regular expression prefix to be applied to each item in the HashSet.
        /// Ruft das reguläre Ausdruckspräfix ab oder setzt es fest, das auf jedes Element im HashSet angewendet wird.
        /// </summary>
        public string RegexPrefix
        {
            get => @_regexPrefix;
            set => @_regexPrefix = @value;
        }
        /// <summary>
        /// Gets or sets the regular expression suffix to be applied to each item in the HashSet.
        /// Ruft das reguläre Ausdrucksuffix ab oder setzt es fest, das auf jedes Element im HashSet angewendet wird.
        /// </summary>
        public string RegexSuffix
        {
            get => @_regexSuffix;
            set => @_regexSuffix = @value;
        }
        #endregion

        // ! CONSTRUCTORS
        #region constructors
        /// <summary>
        /// Initializes a new instance of the RegexWrappedCollection class with empty regular expression prefix and suffix.
        /// Initialisiert eine neue Instanz der Klasse RegexWrappedCollection mit leeren regulären Ausdrucks-Präfix und Suffix.
        /// </summary>
        public RegexWrappedCollection() { }
        /// <summary>
        /// Initializes a new instance of the RegexWrappedCollection class with the provided regular expression prefix and suffix.
        /// Initialisiert eine neue Instanz der Klasse RegexWrappedCollection mit dem angegebenen regulären Ausdrucks-Präfix und Suffix.
        /// </summary>
        /// <param name="regexPrefix">The regular expression prefix to be applied to each item. Das reguläre Ausdruckspräfix, das auf jedes Element angewendet wird.</param>
        /// <param name="regexSuffix">The regular expression suffix to be applied to each item. Das reguläre Ausdrucksuffix, das auf jedes Element angewendet wird.</param>
        public RegexWrappedCollection(string @regexPrefix, string @regexSuffix) : this()
        {
            this._regexPrefix += @regexPrefix;
            this._regexSuffix += @regexSuffix;
        }
        /// <summary>
        /// Initializes a new instance of the RegexWrappedCollection class with the provided regular expression prefix and suffix, and populates it with the provided items.
        /// Initialisiert eine neue Instanz der Klasse RegexWrappedCollection mit dem angegebenen regulären Ausdrucks-Präfix und Suffix und füllt sie mit den angegebenen Elementen.
        /// </summary>
        /// <param name="regexPrefix">The regular expression prefix to be applied to each item. Das reguläre Ausdruckspräfix, das auf jedes Element angewendet wird.</param>
        /// <param name="regexSuffix">The regular expression suffix to be applied to each item. Das reguläre Ausdrucksuffix, das auf jedes Element angewendet wird.</param>
        /// <param name="items">The IEnumerable collection of strings to populate the HashSet with. Die IEnumerable-Sammlung von Zeichenfolgen, mit der das HashSet gefüllt wird.</param>
        public RegexWrappedCollection(string @regexPrefix, string @regexSuffix, IEnumerable<string> items) : this(
            @regexPrefix, @regexSuffix) => AddRange(items);
        #endregion

        // ! METHODS
        #region methods
        /// <summary>
        /// Adds an masked item to the HashSet and applies the regular expressions to the value before adding.
        /// Fügt ein maskiertes Element dem HashSet hinzu und wendet dabei die regulären Ausdrücke auf den Wert an.
        /// </summary>
        /// <param name="item">The item to be added. Das Element, das hinzugefügt wird.</param>
        public new void Add(string @item) => base.Add(RegexWrappedItem(_regexUtils.MaskRegexChars(@item)));
        /// <summary>
        /// Adds a collection of items to the HashSet and applies the regular expressions to each value before adding.
        /// Fügt eine Sammlung von Elementen dem HashSet hinzu und wendet dabei die regulären Ausdrücke auf jeden Wert an.
        /// </summary>
        /// <param name="items">The items to be added. Die Elemente, die hinzugefügt werden.</param>
        public void AddRange(IEnumerable<string> items)
        {
            foreach (var @item in items) Add(@item);
        }
        /// <summary>
        /// Applies the regular expression prefix and suffix to the provided item.
        /// Wendet das reguläre Ausdrucks-Präfix und Suffix auf das angegebene Element an.
        /// </summary>
        /// <param name="item">The item to be wrapped with the regular expression prefix and suffix. Das Element, das mit dem regulären Ausdrucks-Präfix und Suffix umschlossen wird.</param>
        /// <returns>The item with the regular expression prefix and suffix applied. Das Element mit dem angewendeten regulären Ausdrucks-Präfix und Suffix.</returns>
        private string RegexWrappedItem(string @item) => @_regexPrefix + @item + @_regexSuffix;
        /// <summary>
        /// Applies the regular expression prefix and suffix to all items when populated with JSON content.
        /// Wendet das reguläre Ausdrucks-Präfix und Suffix auf alle Elemente an, wenn sie mit JSON-Inhalten gefüllt werden.
        /// </summary>
        internal void GenerateWrappedItems()
        {
            HashSet<string> items = new();
            foreach (string @item in this) items.Add(@item);
            this.Clear();
            foreach (string @item in items) this.Add(@item);
        }
        #endregion
    }
}