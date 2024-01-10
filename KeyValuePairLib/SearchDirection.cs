namespace KeyValuePairLib
{
    /// <summary>
    /// This class defines the search directions that are applied starting from a search key value.
    /// Diese Klasse definiert die Suchrichtungen, welche ausgehend von einem Suchschlüsselwert angewendet werden.
    /// </summary>
    public class SearchDirection
    {
        // ! ENUMERATIONS
        /// <summary>
        /// Enumeration representing the search directions that can be used.
        /// Enumaration der Suchrichtungen, die verwendet werden können.
        /// </summary>
        public enum SearchDirectionType
        {
            /// <summary>
            /// The search is carried out horizontally. Assigned to the value 0.
            /// Die Suche wird horizontal durchgeführt. Zugewiesen dem Wert 0.
            /// </summary>
            Horizontal = 0,
            /// <summary>
            /// The search is carried out vertically. Assigned to the value 1.
            /// Die Suche wird vertikal durchgeführt. Zugewiesen dem Wert 1.
            /// </summary>
            Vertical = 1,
            /// <summary>
            /// The search is carried out over multiple columns. Assigned to the value 2.
            /// Die Suche wird über mehrere Spalten durchgeführt. Zugewiesen dem Wert 2.
            /// </summary>
            Columns = 2,
            /// <summary>
            /// The search is carried out over multiple rows. Assigned to the value 4.
            /// Die Suche wird über mehrere Zeilen durchgeführt. Zugewiesen dem Wert 4.
            /// </summary>
            Rows = 4,
            /// <summary>
            /// The search is carried out over columns and rows similar to a table. Assigned to the value 8.
            /// Die Suche erfolgt über Spalten und Zeilen, ähnlich einer Tabelle. Zugewiesen dem Wert 8.
            /// </summary>
            Table = 8,
            /// <summary>
            /// The search is carried out by detecting checkboxes in and after key fields. Assigned to the value 16.
            /// Die Suche erfolgt über die Erkennung von Checkboxen in und nach Schlüssel-Feldern. Zugewiesen dem Wert 16.
            /// </summary>
            CheckBoxes = 16,
            /// <summary>
            /// The search is carried out by detecting radio buttons in value fields. Assigned to the value 32.
            /// Die Suche erfolgt über die Erkennung von Radio-Buttons in Wert-Feldern. Zugewiesen dem Wert 32.
            /// </summary>
            RadioButtons = 32,
            /// <summary>
            /// The search is carried out by detecting separator variations in fields. Assigned to the value 64.
            /// Die Suche erfolgt über die Erkennung von Trennzeichen-Variationen in Feldern. Zugewiesen dem Wert 64.
            /// </summary>
            Separators = 64,
            /// <summary>
            /// Reserved for abstract implementation in derived class for special cases that cannot be pictured with config rules. Assigned to the value 128.
            /// Reserviert für die abstrakte Implementierung von Sonderfällen in abgeleiteter Klasse, die nicht mit Konfigurationsregeln abgebildet werden können. Zugewiesen dem Wert 128.
            /// </summary>
            Abstract = 128,
        }

        // ! FIELDS
        #region fields
        /// <summary>
        /// Field for the value of the predefined readonly instances.
        /// Feld für den Wert der vordefinierten Readonly-Instanzen.
        /// </summary>
        private readonly SearchDirectionType _value;
        /// <summary>
        /// Represents a predefined readonly instance of the search direction with a horizontal orientation.
        /// Stellt eine vordefinierte Readonly-Instanz der Suchrichtung mit horizontaler Ausrichtung dar.
        /// </summary>
        public static readonly SearchDirection Horizontal = new(SearchDirectionType.Horizontal);
        /// <summary>
        /// Represents a predefined readonly instance of the search direction with a vertical orientation.
        /// Stellt eine vordefinierte Readonly-Instanz der Suchrichtung mit vertikaler Ausrichtung dar.
        /// </summary>
        public static readonly SearchDirection Vertical = new(SearchDirectionType.Vertical);
        /// <summary>
        /// Represents a predefined readonly instance of the search direction with orientation over multiple columns.
        /// Stellt eine vordefinierte Readonly-Instanz der Suchrichtung mit Ausrichtung über mehrere Spalten dar.
        /// </summary>
        public static readonly SearchDirection Columns = new(SearchDirectionType.Columns);
        /// <summary>
        /// Represents a predefined readonly instance of the search direction with orientation over multiple rows.
        /// Stellt eine vordefinierte Readonly-Instanz der Suchrichtung mit Ausrichtung über mehrere Zeilen dar.
        /// </summary>
        public static readonly SearchDirection Rows = new(SearchDirectionType.Rows);
        /// <summary>
        /// Represents a predefined readonly instance of the search direction with a horizontal and vertical orientation similar to a table.
        /// Stellt eine vordefinierte Readonly-Instanz der Suchrichtung mit horizontaler und vertikaler Ausrichtung dar, ähnlich einer Tabelle.
        /// </summary>
        public static readonly SearchDirection Table = new(SearchDirectionType.Table);
        /// <summary>
        /// Represents a predefined readonly instance of the search direction with the principle of detecting checkboxes in fields.
        /// Stellt eine vordefinierte Readonly-Instanz der Suchrichtung mit dem Prinzip der Erkennung von Checkboxen in Feldern dar.
        /// </summary>
        public static readonly SearchDirection CheckBoxes = new(SearchDirectionType.CheckBoxes);
        /// <summary>
        /// Represents a predefined readonly instance of the search direction with the principle of detecting radio buttons in fields.
        /// Stellt eine vordefinierte Readonly-Instanz der Suchrichtung mit dem Prinzip der Erkennung von Radio-Buttons in Feldern dar.
        /// </summary>
        public static readonly SearchDirection RadioButtons = new(SearchDirectionType.RadioButtons);
        /// <summary>
        /// Represents a predefined readonly instance of the search direction with the principle of detecting separator variations in fields.
        /// Stellt eine vordefinierte Readonly-Instanz der Suchrichtung mit dem Prinzip der Erkennung von Trennzeichen-Variationen in Feldern dar.
        /// </summary>
        public static readonly SearchDirection Separators = new(SearchDirectionType.Separators);
        /// <summary>
        /// Reserved for abstract implementation in derived class for special cases that cannot be pictured with config rules.
        /// Reserviert für die abstrakte Implementierung von Sonderfällen in abgeleiteter Klasse, die nicht mit Konfigurationsregeln abgebildet werden können.
        /// </summary>
        public static readonly SearchDirection Abstract = new(SearchDirectionType.Abstract);
        #endregion

        // ! PROPERTIES
        #region properties
        /// <summary>
        /// Gets the enum value of the search direction instance.
        /// Gibt den Enum-Wert der Suchrichtungsinstanz zurück.
        /// </summary>
        public SearchDirectionType Value => _value;
        #endregion

        // ! CONSTRUCTORS
        #region constructors
        /// <summary>
        /// Private constructor to prevent instantiation outside the class.
        /// Privater Konstruktor, um Instanziierung außerhalb der Klasse zu verhindern.
        /// </summary>
        private SearchDirection() { }
        /// <summary>
        /// Private constructor to prevent instantiation outside the class.
        /// Privater Konstruktor, um Instanziierung außerhalb der Klasse zu verhindern.
        /// </summary>
        /// <param name="value">The SearchDirectionType value indicating the search direction. Der SearchDirectionType-Wert, der die Suchrichtung angibt.</param>
        private SearchDirection(SearchDirectionType value) => this._value = value;
        #endregion

        // ! METHODS
        #region methods
        /// <summary>
        /// Implicitly converts a search direction instance to a SearchDirectionType value by Operator.
        /// Konvertiert eine Suchrichtungsinstanz implizit in einen SearchDirectionType-Wert per Operator.
        /// </summary>
        /// <param name="direction">The search direction instance to convert. Die Suchrichtungsinstanz, die konvertiert werden soll.</param>
        /// <returns>The SearchDirectionType value of the search direction instance. Der SearchDirectionType-Wert der Suchrichtungsinstanz.</returns>
        public static implicit operator SearchDirectionType(SearchDirection direction) => direction._value;
        #endregion
    }
}