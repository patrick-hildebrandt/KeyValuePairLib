namespace KeyValuePairLib
{
    /// <summary>
    /// This class stores the masked search key value, attachment values for multirow search keys and the SearchDirection object to which values are to be extracted from files. SearchDirection.Horizontal is default.
    /// Diese Klasse speichert maskiert den Suchschlüsselwert, Anhangswerte für mehrzeilige Suchschlüssel und das SearchDirection-Objekt zu denen Werte aus Dateien extrahiert werden sollen. SearchDirection.Horizontal ist Standard.
    /// </summary>
    public class SearchKey
    {
        // ! FIELDS
        #region fields
        /// <summary>
        /// The masked search key value.
        /// Der Wert des maskierten Suchschlüssels.
        /// </summary>
        private string _key = "";
        /// <summary>
        /// The masked values of attachment for multirow search key.
        /// Die maskierten Werte des Anhangs für mehrzeiligen Suchschlüssel.
        /// </summary>
        private readonly List<string> _appendix = new();
        /// <summary>
        /// The value of the SearchDirection object indicating the search direction.
        /// Der Wert des SearchDirection-Objekts, das die Suchrichtung angibt.
        /// </summary>
        private SearchDirection _direction = SearchDirection.Horizontal;
        /// <summary>
        /// Instantiation of the RegexUtils class.
        /// Instanziierung der RegexUtils-Klasse.
        /// </summary>
        private readonly RegexUtils _regexUtils = new();
        #endregion

        // ! PROPERTIES
        #region properties
        /// <summary>
        /// Gets or sets the masked search key value.
        /// Gibt den maskierten Suchschlüsselwert zurück oder setzt ihn.
        /// </summary>
        public string Key
        {
            get => _key;
            set => _key = _regexUtils.MaskRegexChars(value);
        }
        /// <summary>
        /// Gets or sets the masked values of attachment for multirow search key. Relevant for output, irrelevant for search.
        /// Gibt die maskierten Werte des Anhangs für mehrzeiligen Suchschlüssel zurück oder setzt sie. Relevant für Ausgabe, irrelevant für Suche.
        /// </summary>
        public List<string> Appendix
        {
            get => _appendix;
            set => SetAppendix(value);
        }
        /// <summary>
        /// Gets or sets the SearchDirection object associated with the key. Integer and long values are also accepted for setting.
        /// Gibt das SearchDirection-Objekt zurück, das mit dem Schlüssel verbunden ist, oder setzt es. Integer- und Long-Werte werden ebenfalls zum Setzen akzeptiert.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when an invalid type is provided. Ausgelöst, wenn ein ungültiger Typ angegeben wird.</exception>
        public object Direction
        {
            get => _direction;
            set
            {
                // ! This property was a boolean value in the original implementation, representing only two directions and used for conditional checks.
                // ! This and the population method from Newtonsoft.Json made it necessary to cast from long to integer to make changes work.
                if (value is SearchDirection searchDirection) _direction = searchDirection;
                else if (value is int intDirection) SetDirection(intDirection);
                else if (value is long longDirection) SetDirection((int)(long)longDirection);
                else throw new ArgumentException("Only types of SearchDirection, integer or long are accepted. " +
                    "Nur die Typen SearchDirection, integer oder long werden akzeptiert.");
            }
        }
        #endregion

        // ! CONSTRUCTORS
        #region constructors
        /// <summary>
        /// Initializes a new instance of the SearchKey class with the SearchDirection.Horizontal object as default.
        /// Initialisiert eine neue Instanz der Klasse SearchKey mit dem SearchDirection.Horizontal-Objekt als Standard.
        /// </summary>
        public SearchKey() { }
        /// <summary>
        /// Initializes a new instance of the SearchKey class with the specified search key value from string and SearchDirection object.
        /// Initialisiert eine neue Instanz der Klasse SearchKey mit dem angegebenen Suchschlüsselwert aus Zeichenkette und SearchDirection-Objekt.
        /// </summary>
        /// <param name="key">The search key value. Der Suchschlüsselwert.</param>
        /// <param name="direction">The SearchDirection object. Das SearchDirection-Objekt.</param>
        public SearchKey(string key, SearchDirection direction)
        {
            Key = key;
            Direction = direction;
        }
        /// <summary>
        /// Initializes a new instance of the SearchKey class with the specified search key value from string, SearchDirection object and appendix value from string.
        /// Initialisiert eine neue Instanz der Klasse SearchKey mit dem angegebenen Suchschlüsselwert aus Zeichenkette, SearchDirection-Objekt und Anhangswert aus Zeichenkette.
        /// </summary>
        /// <param name="key">The search key value. Der Suchschlüsselwert.</param>
        /// <param name="direction">The SearchDirection object. Das SearchDirection-Objekt.</param>
        /// <param name="appendix">The value of attachment. Der Wert des Anhangs.</param>
        public SearchKey(string key, SearchDirection direction, string appendix) : this(key, direction)
        {
            SetAppendix(appendix);
        }
        /// <summary>
        /// Initializes a new instance of the SearchKey class with the specified search key value from string, SearchDirection object and appendix values from IEnumerable.
        /// Initialisiert eine neue Instanz der Klasse SearchKey mit dem angegebenen Suchschlüsselwert aus Zeichenkette, SearchDirection-Objekt und Anhangswerten aus IEnumerable.
        /// </summary>
        /// <param name="key">The search key value. Der Suchschlüsselwert.</param>
        /// <param name="direction">The SearchDirection object. Das SearchDirection-Objekt.</param>
        /// <param name="appendix">The values of attachment. Die Werte des Anhangs.</param>
        public SearchKey(string key, SearchDirection direction, IEnumerable<string> appendix) : this(key, direction)
        {
            SetAppendix(appendix);
        }
        /// <summary>
        /// Initializes a new instance of the SearchKey class with the specified search key value from string and integer value indicating the search direction.
        /// Initialisiert eine neue Instanz der Klasse SearchKey mit dem angegebenen Suchschlüsselwert aus Zeichenkette und dem Integer-Wert, der die Suchrichtung angibt.
        /// </summary>
        /// <param name="key">The search key value. Der Suchschlüsselwert.</param>
        /// <param name="direction">The integer value indicating the search direction. Der Integer-Wert, der die Suchrichtung angibt.</param>
        public SearchKey(string key, int direction)
        {
            Key = key;
            SetDirection(direction);
        }
        /// <summary>
        /// Initializes a new instance of the SearchKey class with the specified search key value from string, integer value indicating the search direction and appendix value from string.
        /// Initialisiert eine neue Instanz der Klasse SearchKey mit dem angegebenen Suchschlüsselwert aus Zeichenkette, dem Integer-Wert, der die Suchrichtung angibt und Anhangswert aus Zeichenkette.
        /// </summary>
        /// <param name="key">The search key value. Der Suchschlüsselwert.</param>
        /// <param name="direction">The integer value indicating the search direction. Der Integer-Wert, der die Suchrichtung angibt.</param>
        /// <param name="appendix">The value of attachment. Der Wert des Anhangs.</param>
        public SearchKey(string key, int direction, string appendix) : this(key, direction)
        {
            SetAppendix(appendix);
        }
        /// <summary>
        /// Initializes a new instance of the SearchKey class with the specified search key value from string, integer value indicating the search direction and appendix values from IEnumerable.
        /// Initialisiert eine neue Instanz der Klasse SearchKey mit dem angegebenen Suchschlüsselwert aus Zeichenkette, dem Integer-Wert, der die Suchrichtung angibt und Anhangswerten aus IEnumerable.
        /// </summary>
        /// <param name="key">The search key value. Der Suchschlüsselwert.</param>
        /// <param name="direction">The integer value indicating the search direction. Der Integer-Wert, der die Suchrichtung angibt.</param>
        /// <param name="appendix">The values of attachment. Die Werte des Anhangs.</param>
        public SearchKey(string key, int direction, IEnumerable<string> appendix) : this(key, direction)
        {
            SetAppendix(appendix);
        }
        #endregion

        // ! METHODS
        #region methods
        /// <summary>
        /// Sets the value of the search key.
        /// Setzt den Wert des Suchschlüssels.
        /// </summary>
        /// <param name="key">The search key value. Der Suchschlüsselwert.</param>
        public void SetKeyValue(string key) => Key = key;
        /// <summary>
        /// Sets the value of the attachment for multirow search key from string. The string is converted to an array based on row breaks.
        /// Setzt den Wert des Anhangs für mehrzeiligen Suchschlüssel aus Zeichenkette. Die Zeichenkette wird basierend auf Umbrüchen in ein Array umgewandelt.
        /// </summary>
        /// <param name="appendix">The value of attachment. Der Wert des Anhangs.</param>
        public void SetAppendix(string appendix)
        {
            _appendix.Clear();
            AddAppendix(appendix);
        }
        /// <summary>
        /// Sets the values of the attachment for multirow search key from IEnumerable.
        /// Setzt die Werte des Anhangs für mehrzeiligen Suchschlüssel aus IEnumerable.
        /// </summary>
        /// <param name="appendix">The values of attachment. Die Werte des Anhangs.</param>
        public void SetAppendix(IEnumerable<string> appendix)
        {
            _appendix.Clear();
            foreach (string append in appendix) _appendix.Add(_regexUtils.MaskRegexChars(append));
        }
        /// <summary>
        /// Adds a value to the attachment for multirow search key from string. The string is converted to an array based on row breaks.
        /// Fügt dem Anhang für mehrzeiligen Suchschlüssel einen Wert aus Zeichenkette hinzu. Die Zeichenkette wird basierend auf Umbrüchen in ein Array umgewandelt.
        /// </summary>
        /// <param name="appendix">The value for attachment. Der Wert zum Anhang.</param>
        public void AddAppendix(string appendix)
        {
            var rowBreaks = new string[] { "\r\n", "\r", "\n" };
            var rows = appendix.Split(rowBreaks, StringSplitOptions.RemoveEmptyEntries);
            _appendix.AddRange(rows.Select(_regexUtils.MaskRegexChars));
        }
        /// <summary>
        /// Sets the SearchDirection object.
        /// Setzt das SearchDirection-Objekt.
        /// </summary>
        /// <param name="direction">The SearchDirection object. Das SearchDirection-Objekt.</param>
        public void SetDirection(SearchDirection direction) => _direction = direction;
        /// <summary>
        /// Sets the SearchDirection object indicating the search direction with the provided integer value.
        /// Setzt mit dem angegebenen Integer-Wert das SearchDirection-Objekt, das die Suchrichtung angibt.
        /// </summary>
        /// <param name="direction">The integer value indicating the SearchDirection object. Der Integer-Wert, der das SearchDirection-Objekt angibt.</param>
        /// <exception cref="ArgumentException">Thrown when an invalid enumeration is provided. Ausgelöst, wenn eine ungültige Enumeration angegeben wird.</exception>
        public void SetDirection(int direction)
        {
            this._direction = direction switch
            {
                0 => SearchDirection.Horizontal,
                1 => SearchDirection.Vertical,
                2 => SearchDirection.Columns,
                4 => SearchDirection.Rows,
                8 => SearchDirection.Table,
                16 => SearchDirection.CheckBoxes,
                32 => SearchDirection.RadioButtons,
                64 => SearchDirection.Separators,
                128 => SearchDirection.Abstract,
                _ => throw new ArgumentException("No valid enumeration provided. Keine gültige Enumeration angegeben"),
            };
        }
        /// <summary>
        /// Gets the demasked value of the search key.
        /// Gibt den demaskierten Wert des Suchschlüssels zurück.
        /// </summary>
        /// <returns>The demasked search key value. Der demaskierte Suchschlüsselwert.</returns>
        public string GetKeyDemasked() => _regexUtils.DemaskRegexChars(Key);
        /// <summary>
        /// Gets the demasked values of the appendix for the search key as List(string) object.
        /// Gibt die demaskierten Werte der Anhänge des Suchschlüssels als List(string)-Objekt zurück.
        /// </summary>
        /// <returns>The demasked values of the appendix. Die demaskierten Werte der Anhänge.</returns>
        public List<string> GetAppendixDemasked()
        {
            List<string> demaskedAppendix = new();
            foreach (string append in Appendix) demaskedAppendix.Add(_regexUtils.DemaskRegexChars(append));
            return demaskedAppendix;
        }
        /// <summary>
        /// Gets the demasked values of the appendix for the search key as string with row breaks.
        /// Gibt die demaskierten Werte der Anhänge des Suchschlüssels als Zeichenkette mit Zeilenumbrüchen zurück.
        /// </summary>
        /// <returns>The string of demasked appendix values with row breaks. Die Zeichenkette der demaskierten Anhangswerte mit Zeilenumbrüchen.</returns>
        public string GetAppendixString()
        {
            string appendixString = "";
            foreach (string append in Appendix) appendixString += "\r\n" + _regexUtils.DemaskRegexChars(append);
            return appendixString;
        }
        /// <summary>
        /// Gets the integer value of the SearchDirection object indicating the search direction.
        /// Gibt den Integer-Wert des SearchDirection-Objekts zurück, das die Suchrichtung angibt.
        /// </summary>
        /// <returns>The integer value of the SearchDirection object. Der Integer-Wert des SearchDirection-Objekts.</returns>
        public int GetDirectionInteger() => (int)_direction.Value;
        /// <summary>
        /// Gets the wording for the direction of the search key.
        /// Gibt den Wortlaut für die Richtung des Suchschlüssels zurück.
        /// </summary>
        /// <returns>The wording for the direction of the search key. Den Wortlaut für die Richtung des Suchschlüssels.</returns>
        public string GetDirectionName()
        {
            if (_direction == SearchDirection.Horizontal) return "Horizontal";
            else if (_direction == SearchDirection.Vertical) return "Vertikal";
            else if (_direction == SearchDirection.Columns) return "Columns";
            else if (_direction == SearchDirection.Rows) return "Rows";
            else if (_direction == SearchDirection.Table) return "Table";
            else if (_direction == SearchDirection.CheckBoxes) return "CheckBoxes";
            else if (_direction == SearchDirection.RadioButtons) return "RadioButtons";
            else if (_direction == SearchDirection.Separators) return "Separators";
            else if (_direction == SearchDirection.Abstract) return "Abstract";
            else return string.Empty;
        }
        /// <summary>
        /// Masks all members of the SearchKey object.
        /// Maskiert alle Member des SearchKey-Objekts.
        /// </summary>
        internal void MaskAllMembers()
        {
            Key = _regexUtils.MaskRegexChars(Key);
            List<string> tempAppendix = new();
            foreach (string append in Appendix) tempAppendix.Add(_regexUtils.MaskRegexChars(append));
            _appendix.Clear();
            foreach (string append in tempAppendix) _appendix.Add(append);
        }
        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// Bestimmt, ob das angegebene Objekt mit dem aktuellen Objekt identisch ist.
        /// </summary>
        /// <param name="obj">The object to compare with the current object. Das Objekt, das mit dem aktuellen Objekt verglichen werden soll.</param>
        /// <returns>True if the specified object is equal to the current object; otherwise, false. True, wenn das angegebene Objekt mit dem aktuellen Objekt identisch ist. Andernfalls false.</returns>
        public override bool Equals(object? obj)
        {
            if (obj == null || this.GetType() != obj.GetType()) return false;
            SearchKey other = (SearchKey)obj;
            return this.Key == other.Key && this.Direction == other.Direction;
            // ? Multirow search keys needed for search and comparison ?
            //return this.Key == other.Key && this.Direction == other.Direction && this.Appendix == other.Appendix;
        }
        /// <summary>
        /// Serves as the default hash function.
        /// Dient als Standardhashfunktion.
        /// </summary>
        /// <returns>The hash code for the current object. Der Hashcode für das aktuelle Objekt.</returns>
        public override int GetHashCode() => HashCode.Combine(Key, Direction);
        // ? Multirow search keys needed for search and comparison ?
        //public override int GetHashCode() => HashCode.Combine(Key, Direction, Appendix);
        #endregion
    }
}