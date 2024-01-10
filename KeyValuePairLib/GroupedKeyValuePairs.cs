using System.Dynamic;

namespace KeyValuePairLib
{
    /// <summary>
    /// Represents a collection of key-value pairs grouped together.
    /// Stellt eine Sammlung von gruppierten Schlüssel-Wert-Paaren dar.
    /// </summary>
    public class GroupedKeyValuePairs
    {
        // ! FIELDS
        #region fields
        /// <summary>
        /// The internal storage for the key-value pairs as an ExpandoObject.
        /// Der interne Speicher für die Schlüssel-Wert-Paare als ExpandoObject.
        /// </summary>
        private readonly ExpandoObject _properties = new();
        #endregion

        // ! PROPERTIES
        #region properties
        /// <summary>
        /// Gets or sets the value as object associated with the specified key as string.
        /// Ruft den Wert als Objekt ab, der mit dem angegebenen Schlüssel als Zeichenkette verknüpft ist, oder legt ihn fest.
        /// </summary>
        /// <param name="name">The name of the key as String. Der Name des Schlüssels als Zeichenkette.</param>
        public object this[string name]
        {
            get => (_properties as IDictionary<string, object>)[name];
            set => (_properties as IDictionary<string, object>)[name] = value;
        }
        #endregion

        // ! METHODS
        #region methods
        /// <summary>
        /// Adds a property with the specified name as string and value as object to the collection.
        /// Fügt der Sammlung eine Eigenschaft mit dem angegebenen Namen als Zeichenkette und Wert als Objekt hinzu.
        /// </summary>
        /// <param name="name">The name of the property as string. Der Name der Eigenschaft als Zeichenkette.</param>
        /// <param name="value">The value of the property as object. Der Wert der Eigenschaft als Objekt.</param>
        public void AddProperty(string name, object value) => (_properties as IDictionary<string, object>)[name] =
            value;
        /// <summary>
        /// Adds a collection of key-value properties as tuples to the existing collection.
        /// Fügt eine Sammlung von Schlüssel-Wert-Eigenschaften als Tupel zur bestehenden Sammlung hinzu.
        /// </summary>
        /// <param name="properties">The collection of key-value properties as tuples. Die Sammlung von Schlüssel-Wert-Eigenschaften als Tupel.</param>
        public void AddProperties(IEnumerable<(string, object)> properties)
        {
            foreach ((string, object) property in properties) AddProperty(property.Item1, property.Item2);
        }
        /// <summary>
        /// Gets a Dictionary of all key-value properties in the collection.
        /// Gibt ein Wörterbuch aller Schlüssel-Wert-Eigenschaften in der Sammlung zurück.
        /// </summary>
        /// <returns>The Dictionary of key-value properties. Das Wörterbuch von Schlüssel-Wert-Eigenschaften.</returns>
        public IDictionary<string, object> GetProperties() => _properties as IDictionary<string, object>;
        /// <summary>
        /// Gets the dynamic object representing the key-value properties.
        /// Gibt das dynamische Objekt zurück, das die Schlüssel-Wert-Eigenschaften darstellt.
        /// </summary>
        /// <returns>The dynamic object representing the key-value properties. Das dynamische Objekt, das die Schlüssel-Wert-Eigenschaften darstellt.</returns>
        public dynamic GetDynamicObject() => _properties;
        /// <summary>
        /// Checks if the collection is empty.
        /// Überprüft, ob die Sammlung leer ist.
        /// </summary>
        /// <returns>True if the collection is empty, otherwise false. True, wenn die Sammlung leer ist, sonst false.</returns>
        internal bool IsEmpty()
        {
            IDictionary<string, object> properties = GetProperties();
            return properties == null || properties.Count == 0;
        }
        /// <summary>
        /// Reports the key-value properties to the console for debugging purpose.
        /// Meldet die Schlüssel-Wert-Eigenschaften zu Debugging-Zwecken an die Konsole.
        /// </summary>
        public void CONSOLE_GroupedKeyValuePairs()
        {
            Console.WriteLine("CONSOLE_GroupedKeyValuePairs:");
            Console.WriteLine("#############################\r\n");
            IDictionary<string, object> properties = _properties as IDictionary<string, object>;
            foreach (KeyValuePair<string, object> property in properties)
            {
                string? propertyValue = property.Value.ToString()?.Replace("\t", "\r\n").Replace("\r\n", "\r\n\t\t\t\t");
                Console.WriteLine($"°{property.Key}°\r\n\t\t\t\t°{propertyValue}°");
            }
            Console.ReadLine();
        }
        #endregion
    }
}