namespace KeyValuePairLib
{
    /// <summary>
    /// This class stores SearchKey objects as a HashSet.
    /// Diese Klasse speichert SearchKey-Objekte als HashSet.
    /// </summary>
    public class GroupedSearchKeys : HashSet<SearchKey>
    {
        // ! CONSTRUCTORS
        #region constructors
        /// <summary>
        /// Initializes a new instance of the GroupedSearchKeys class.
        /// Initialisiert eine neue Instanz der Klasse GroupedSearchKeys.
        /// </summary>
        public GroupedSearchKeys() { }
        /// <summary>
        /// Initializes a new instance of the GroupedSearchKeys class with the provided IEnumerable of search key values and a common SearchDirection object.
        /// Initialisiert eine neue Instanz der Klasse GroupedSearchKeys mit der angegebenen IEnumerable von Suchschlüsselwerten und einem gemeinsamen SearchDirection-Objekt.
        /// </summary>
        /// <param name="keys">The IEnumerable of search key values. Die IEnumerable von Suchschlüsselwerten.</param>
        /// <param name="direction">The common SearchDirection object. Das gemeinsame SearchDirection-Objekt.</param>
        public GroupedSearchKeys(IEnumerable<string> keys, SearchDirection direction) => AddRange(keys, direction);
        /// <summary>
        /// Initializes a new instance of the GroupedSearchKeys class with the provided IEnumerable of search key values and a common integer value indicating the search direction.
        /// Initialisiert eine neue Instanz der Klasse GroupedSearchKeys mit der angegebenen IEnumerable von Suchschlüsselwerten und einem gemeinsamen Integer-Wert, der die Suchrichtung angibt.
        /// </summary>
        /// <param name="keys">The IEnumerable of search key values. Die IEnumerable von Suchschlüsselwerten.</param>
        /// <param name="direction">The common integer value indicating the search direction. Der gemeinsame Integer-Wert, der die Suchrichtung angibt.</param>
        public GroupedSearchKeys(IEnumerable<string> keys, int direction) => AddRange(keys, direction);
        /// <summary>
        /// Initializes a new instance of the GroupedSearchKeys class with the provided IEnumerable of search key values and appendix values in Tuples and a common SearchDirection object.
        /// Initialisiert eine neue Instanz der Klasse GroupedSearchKeys mit der angegebenen IEnumerable von Suchschlüsselwerten und Anhangswerten in Tupeln und einem gemeinsamen SearchDirection-Objekt.
        /// </summary>
        /// <param name="keys">The IEnumerable of search key values and appendix values in Tuples. Die IEnumerable von Suchschlüsselwerten und Anhangswerten in Tupeln.</param>
        /// <param name="direction">The common SearchDirection object. Das gemeinsame SearchDirection-Objekt.</param>
        public GroupedSearchKeys(IEnumerable<(string, string)> keys, SearchDirection direction)
            => AddRange(keys, direction);
        /// <summary>
        /// Initializes a new instance of the GroupedSearchKeys class with the provided IEnumerable of search key values and appendix values in Tuples and a common integer value indicating the search direction.
        /// Initialisiert eine neue Instanz der Klasse GroupedSearchKeys mit der angegebenen IEnumerable von Suchschlüsselwerten und Anhangswerten in Tupeln und einem gemeinsamen Integer-Wert, der die Suchrichtung angibt.
        /// </summary>
        /// <param name="keys">The IEnumerable of search key values and appendix values in Tuples. Die IEnumerable von Suchschlüsselwerten und Anhangswerten in Tupeln.</param>
        /// <param name="direction">The common integer value indicating the search direction. Der gemeinsame Integer-Wert, der die Suchrichtung angibt.</param>
        public GroupedSearchKeys(IEnumerable<(string, string)> keys, int direction) => AddRange(keys, direction);
        /// <summary>
        /// Initializes a new instance of the GroupedSearchKeys class with the provided IEnumerable of search key values and appendix values as IEnumerable(string) in Tuples and a common SearchDirection object.
        /// Initialisiert eine neue Instanz der Klasse GroupedSearchKeys mit der angegebenen IEnumerable von Suchschlüsselwerten und Anhangswerten als IEnumerable(string) in Tupeln und einem gemeinsamen SearchDirection-Objekt.
        /// </summary>
        /// <param name="keys">The IEnumerable of search key values and appendix values as IEnumerable(string) in Tuples. Die IEnumerable von Suchschlüsselwerten und Anhangswerten als IEnumerable(string) in Tupeln.</param>
        /// <param name="direction">The common SearchDirection object. Das gemeinsame SearchDirection-Objekt.</param>
        public GroupedSearchKeys(IEnumerable<(string, IEnumerable<string>)> keys, SearchDirection direction)
            => AddRange(keys, direction);
        /// <summary>
        /// Initializes a new instance of the GroupedSearchKeys class with the provided IEnumerable of search key values and appendix values as IEnumerable(string) in Tuples and a common integer value indicating the search direction.
        /// Initialisiert eine neue Instanz der Klasse GroupedSearchKeys mit der angegebenen IEnumerable von Suchschlüsselwerten und Anhangswerten als IEnumerable(string) in Tupeln und einem gemeinsamen Integer-Wert, der die Suchrichtung angibt.
        /// </summary>
        /// <param name="keys">The IEnumerable of search key values and appendix values as IEnumerable(string) in Tuples. Die IEnumerable von Suchschlüsselwerten und Anhangswerten als IEnumerable(string) in Tupeln.</param>
        /// <param name="direction">The common integer value indicating the search direction. Der gemeinsame Integer-Wert, der die Suchrichtung angibt.</param>
        public GroupedSearchKeys(IEnumerable<(string, IEnumerable<string>)> keys, int direction)
            => AddRange(keys, direction);
        /// <summary>
        /// Initializes a new instance of the GroupedSearchKeys class with the provided IEnumerable of search key values and SearchDirection objects in Tuples.
        /// Initialisiert eine neue Instanz der Klasse GroupedSearchKeys mit der angegebenen IEnumerable von Suchschlüsselwerten und SearchDirection-Objekten in Tupeln.
        /// </summary>
        /// <param name="keys">The IEnumerable of search key values and SearchDirection objects in Tuples. Die IEnumerable von Suchschlüsselwerten und SearchDirection-Objekten in Tupeln.</param>
        public GroupedSearchKeys(IEnumerable<(string, SearchDirection)> keys) => AddRange(keys);
        /// <summary>
        /// Initializes a new instance of the GroupedSearchKeys class with the provided IEnumerable of search key values and integer values, indicating the search direction, in Tuples.
        /// Initialisiert eine neue Instanz der Klasse GroupedSearchKeys mit der angegebenen IEnumerable von Suchschlüsselwerten und Integer-Werten, welche die Suchrichtung angeben, in Tupeln.
        /// </summary>
        /// <param name="keys">The IEnumerable of search key values and integer values, indicating the search direction, in Tuples. Die IEnumerable von Suchschlüsselwerten und Integer-Werten, welche die Suchrichtung angeben, in Tupeln.</param>
        public GroupedSearchKeys(IEnumerable<(string, int)> keys) => AddRange(keys);
        #endregion

        // ! METHODS
        #region methods
        /// <summary>
        /// Adds a new SearchKey object with the provided string of the search key value and SearchDirection object.
        /// Fügt ein neues SearchKey-Objekt mit der angegebenen Zeichenkette des Suchschlüsselwerts und dem SearchDirection-Objekt hinzu.
        /// </summary>
        /// <param name="key">The search key value. Der Suchschlüsselwert.</param>
        /// <param name="direction">The SearchDirection object. Das SearchDirection-Objekt.</param>
        public void Add(string key, SearchDirection direction)
        {
            if (!Contains(new SearchKey(key, direction))) Add(new SearchKey(key, direction));
        }
        /// <summary>
        /// Adds a new SearchKey object with the provided string of the search key value, SearchDirection object and string of attachment value.
        /// Fügt ein neues SearchKey-Objekt mit der angegebenen Zeichenkette des Suchschlüsselwerts, dem SearchDirection-Objekt und der Zeichenkette des Anhangs hinzu.
        /// </summary>
        /// <param name="key">The search key value. Der Suchschlüsselwert.</param>
        /// <param name="direction">The SearchDirection object. Das SearchDirection-Objekt.</param>
        /// <param name="appendix">The string of attachment value. Die Zeichenkette des Anhangs.</param>
        public void Add(string key, SearchDirection direction, string appendix)
        {
            if (!Contains(new SearchKey(key, direction, appendix))) Add(new SearchKey(key, direction, appendix));
        }
        /// <summary>
        /// Adds a new SearchKey object with the provided string of the search key value, SearchDirection object and IEnumerable(string) of attachment values.
        /// Fügt ein neues SearchKey-Objekt mit der angegebenen Zeichenkette des Suchschlüsselwerts, dem SearchDirection-Objekt und der IEnumerable(string) der Anhänge hinzu.
        /// </summary>
        /// <param name="key">The search key value. Der Suchschlüsselwert.</param>
        /// <param name="direction">The SearchDirection object. Das SearchDirection-Objekt.</param>
        /// <param name="appendix">The IEnumerable(string) of attachment values. Die IEnumerable(string) der Anhänge.</param>
        public void Add(string key, SearchDirection direction, IEnumerable<string> appendix)
        {
            if (!Contains(new SearchKey(key, direction, appendix))) Add(new SearchKey(key, direction, appendix));
        }
        /// <summary>
        /// Adds a new SearchKey object with the provided string of the search key value and integer value indicating the search direction.
        /// Fügt ein neues SearchKey-Objekt mit der angegebenen Zeichenkette des Suchschlüsselwerts und dem Integer-Wert, der die Suchrichtung angibt, hinzu.
        /// </summary>
        /// <param name="key">The search key value. Der Suchschlüsselwert.</param>
        /// <param name="direction">The integer value indicating the search direction. Der Integer-Wert, der die Suchrichtung angibt.</param>
        public void Add(string key, int direction)
        {
            if (!Contains(new SearchKey(key, direction))) Add(new SearchKey(key, direction));
        }
        /// <summary>
        /// Adds a new SearchKey object with the provided string of the search key value, integer value indicating the search direction and string of attachment value.
        /// Fügt ein neues SearchKey-Objekt mit der angegebenen Zeichenkette des Suchschlüsselwerts, dem Integer-Wert, der die Suchrichtung angibt und der Zeichenkette des Anhangs hinzu.
        /// </summary>
        /// <param name="key">The search key value. Der Suchschlüsselwert.</param>
        /// <param name="direction">The integer value indicating the search direction. Der Integer-Wert, der die Suchrichtung angibt.</param>
        /// <param name="appendix">The string of attachment value. Die Zeichenkette des Anhangs.</param>
        public void Add(string key, int direction, string appendix)
        {
            if (!Contains(new SearchKey(key, direction, appendix))) Add(new SearchKey(key, direction, appendix));
        }
        /// <summary>
        /// Adds a new SearchKey object with the provided string of the search key value, integer value indicating the search direction and IEnumerable(string) of attachment values.
        /// Fügt ein neues SearchKey-Objekt mit der angegebenen Zeichenkette des Suchschlüsselwerts, dem Integer-Wert, der die Suchrichtung angibt und der IEnumerable(string) der Anhänge hinzu.
        /// </summary>
        /// <param name="key">The search key value. Der Suchschlüsselwert.</param>
        /// <param name="direction">The integer value indicating the search direction. Der Integer-Wert, der die Suchrichtung angibt.</param>
        /// <param name="appendix">The IEnumerable(string) of attachment values. Die IEnumerable(string) der Anhänge.</param>
        public void Add(string key, int direction, IEnumerable<string> appendix)
        {
            if (!Contains(new SearchKey(key, direction, appendix))) Add(new SearchKey(key, direction, appendix));
        }
        /// <summary>
        /// Adds a range of new SearchKey objects with the provided IEnumerable of search key values and a common SearchDirection object.
        /// Fügt eine Reihe von neuen SearchKey-Objekten hinzu mit der angegebenen IEnumerable von Suchschlüsselwerten und einem gemeinsamen SearchDirection-Objekt.
        /// </summary>
        /// <param name="keys">The IEnumerable of search key values. Die IEnumerable von Suchschlüsselwerten.</param>
        /// <param name="direction">The common SearchDirection object. Das gemeinsame SearchDirection-Objekt.</param>
        public void AddRange(IEnumerable<string> keys, SearchDirection direction)
        {
            foreach (string key in keys) if (!Contains(new SearchKey(key, direction)))
                    Add(new SearchKey(key, direction));
        }
        /// <summary>
        /// Adds a range of new SearchKey objects with the provided IEnumerable of search key values and a common integer value indicating the search direction.
        /// Fügt eine Reihe von neuen SearchKey-Objekten hinzu mit der angegebenen IEnumerable von Suchschlüsselwerten und einem gemeinsamen Integer-Wert, der die Suchrichtung angibt.
        /// </summary>
        /// <param name="keys">The IEnumerable of search key values. Die IEnumerable von Suchschlüsselwerten.</param>
        /// <param name="direction">The common integer value indicating the search direction. Der gemeinsame Integer-Wert, der die Suchrichtung angibt.</param>
        public void AddRange(IEnumerable<string> keys, int direction)
        {
            foreach (string key in keys) if (!Contains(new SearchKey(key, direction)))
                    Add(new SearchKey(key, direction));
        }
        /// <summary>
        /// Adds a range of new SearchKey objects with the provided IEnumerable of search key values and appendix values in Tuples and a common SearchDirection object.
        /// Fügt eine Reihe von neuen SearchKey-Objekten hinzu mit der angegebenen IEnumerable von Suchschlüsselwerten und Anhangswerten in Tupeln und einem gemeinsamen SearchDirection-Objekt.
        /// </summary>
        /// <param name="keys">The IEnumerable of search key values and appendix values in Tuples. Die IEnumerable von Suchschlüsselwerten und Anhangswerten in Tupeln.</param>
        /// <param name="direction">The common SearchDirection object. Das gemeinsame SearchDirection-Objekt.</param>
        public void AddRange(IEnumerable<(string, string)> keys, SearchDirection direction)
        {
            foreach (var key in keys)
                if (!Contains(new SearchKey(key.Item1, direction, key.Item2)))
                    Add(new SearchKey(key.Item1, direction, key.Item2));
        }
        /// <summary>
        /// Adds a range of new SearchKey objects with the provided IEnumerable of search key values and appendix values in Tuples and a common integer value indicating the search direction.
        /// Fügt eine Reihe von neuen SearchKey-Objekten hinzu mit der angegebenen IEnumerable von Suchschlüsselwerten und Anhangswerten in Tupeln und einem gemeinsamen Integer-Wert, der die Suchrichtung angibt.
        /// </summary>
        /// <param name="keys">The IEnumerable of search key values and appendix values in Tuples. Die IEnumerable von Suchschlüsselwerten und Anhangswerten in Tupeln.</param>
        /// <param name="direction">The common integer value indicating the search direction. Der gemeinsame Integer-Wert, der die Suchrichtung angibt.</param>
        public void AddRange(IEnumerable<(string, string)> keys, int direction)
        {
            foreach (var key in keys)
                if (!Contains(new SearchKey(key.Item1, direction, key.Item2)))
                    Add(new SearchKey(key.Item1, direction, key.Item2));
        }
        /// <summary>
        /// Adds a range of new SearchKey objects with the provided IEnumerable of search key values and appendix values as IEnumerable(string) in Tuples and a common SearchDirection object.
        /// Fügt eine Reihe von neuen SearchKey-Objekten hinzu mit der angegebenen IEnumerable von Suchschlüsselwerten und Anhangswerten als IEnumerable(string) in Tupeln und einem gemeinsamen SearchDirection-Objekt.
        /// </summary>
        /// <param name="keys">The IEnumerable of search key values and appendix values as IEnumerable(string) in Tuples. Die IEnumerable von Suchschlüsselwerten und Anhangswerten als IEnumerable(string) in Tupeln.</param>
        /// <param name="direction">The common SearchDirection object. Das gemeinsame SearchDirection-Objekt.</param>
        public void AddRange(IEnumerable<(string, IEnumerable<string>)> keys, SearchDirection direction)
        {
            foreach (var key in keys)
                if (!Contains(new SearchKey(key.Item1, direction, key.Item2)))
                    Add(new SearchKey(key.Item1, direction, key.Item2));
        }
        /// <summary>
        /// Adds a range of new SearchKey objects with the provided IEnumerable of search key values and appendix values as IEnumerable(string) in Tuples and a common integer value indicating the search direction.
        /// Fügt eine Reihe von neuen SearchKey-Objekten hinzu mit der angegebenen IEnumerable von Suchschlüsselwerten und Anhangswerten als IEnumerable(string) in Tupeln und einem gemeinsamen Integer-Wert, der die Suchrichtung angibt.
        /// </summary>
        /// <param name="keys">The IEnumerable of search key values and appendix values as IEnumerable(string) in Tuples. Die IEnumerable von Suchschlüsselwerten und Anhangswerten als IEnumerable(string) in Tupeln.</param>
        /// <param name="direction">The common integer value indicating the search direction. Der gemeinsame Integer-Wert, der die Suchrichtung angibt.</param>
        public void AddRange(IEnumerable<(string, IEnumerable<string>)> keys, int direction)
        {
            foreach (var key in keys)
                if (!Contains(new SearchKey(key.Item1, direction, key.Item2)))
                    Add(new SearchKey(key.Item1, direction, key.Item2));
        }
        /// <summary>
        /// Adds a range of new SearchKey objects with the provided IEnumerable of search key values and SearchDirection objects in Tuples.
        /// Fügt eine Reihe von neuen SearchKey-Objekten hinzu mit der angegebenen IEnumerable von Suchschlüsselwerten und SearchDirection-Objekten in Tupeln.
        /// </summary>
        /// <param name="keys">The IEnumerable of search key values and SearchDirection objects in Tuples. Die IEnumerable von Suchschlüsselwerten und SearchDirection-Objekten in Tupeln.</param>
        public void AddRange(IEnumerable<(string, SearchDirection)> keys)
        {
            foreach (var key in keys)
                if (!Contains(new SearchKey(key.Item1, key.Item2)))
                    Add(new SearchKey(key.Item1, key.Item2));
        }
        /// <summary>
        /// Adds a range of new SearchKey objects with the provided IEnumerable of search key values and integer values, indicating the search direction, in Tuples.
        /// Fügt eine Reihe von neuen SearchKey-Objekten hinzu mit der angegebenen IEnumerable von Suchschlüsselwerten und Integer-Werten, welche die Suchrichtung angeben, in Tupeln.
        /// </summary>
        /// <param name="keys">The IEnumerable of search key values and integer values, indicating the search direction, in Tuples. Die IEnumerable von Suchschlüsselwerten und Integer-Werten, welche die Suchrichtung angeben, in Tupeln.</param>
        public void AddRange(IEnumerable<(string, int)> keys)
        {
            foreach (var key in keys)
                if (!Contains(new SearchKey(key.Item1, key.Item2)))
                    Add(new SearchKey(key.Item1, key.Item2));
        }
        #endregion
    }
}