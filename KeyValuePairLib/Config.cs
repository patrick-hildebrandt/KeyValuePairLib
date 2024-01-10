using Newtonsoft.Json;

namespace KeyValuePairLib
{
    /// <summary>
    /// This class stores the configuration for reading files.
    /// Diese Klasse speichert die Konfiguration zum Auslesen von Dateien.
    /// </summary>
    public class Config
    {
        // ! PROPERTIES
        #region properties
        /// <summary>
        /// This defines from how many spaces between fields they will be interpreted as empty space. Default is 3, minimum is 2, but should be avoided, because this also takes input errors of double spaces into account.
        /// Hier wird definiert ab wievielen Leerzeichen zwischen Feldern diese als leerer Raum interpretiert werden. Standard ist 3, Minimum ist 2, sollte jedoch vermieden werden, da dies auch Eingabefehler von doppelten Leerzeichen berücksichtigt.
        /// </summary>
        public int MaxWhiteSpaces { get; set; } = 3;
        /// <summary>
        /// This defines how many characters are tolerated with horizontal offset between vertical key and value fields. Default is 3. A too large buffer may include adjacent fields.
        /// Hier wird definiert, wieviele Zeichen bei horizontalem Versatz zwischen vertikalen Schlüssel- und Wert-Feldern toleriert werden. Standard ist 3. Ein zu großer Puffer kann benachbarte Felder miteinbeziehen.
        /// </summary>
        public int RangeBuffer { get; set; } = 3;
        /// <summary>
        /// This defines the minimum number of separators for key-value pairs which are connected by a sequence of separators. Default is 2. Higher value increases performance.
        /// Hier wird die minimale Anzahl von Trennzeichen definiert für Schlüssel-Wert-Paare, welche durch eine Sequenz von Trennzeichen verbunden sind. Standard ist 2. Höherer Wert steigert die Performance.
        /// </summary>
        public int MinSeparatorSpaces { get; set; } = 2;
        /// <summary>
        /// This defines the maximum number of separators for key-value pairs which are connected by a sequence of separators. Default is 30. Lower value increases performance.
        /// Hier wird die maximale Anzahl von Trennzeichen definiert für Schlüssel-Wert-Paare, welche durch eine Sequenz von Trennzeichen verbunden sind. Standard ist 30. Niedrigerer Wert steigert die Performance.
        /// </summary>
        public int MaxSeparatorSpaces { get; set; } = 30;
        /// <summary>
        /// This defines how many pixels of PDF pages will be cropped at the top. Default is 0.
        /// Hier wird definiert, wieviele Pixel von PDF-Seiten oben abgeschnitten werden. Standard ist 0.
        /// </summary>
        public int CutTop { get; set; } = 0;
        /// <summary>
        /// This defines how many pixels of PDF pages will be cropped at the bottom. Default is 0.
        /// Hier wird definiert, wieviele Pixel von PDF-Seiten unten abgeschnitten werden. Standard ist 0.
        /// </summary>
        public int CutBottom { get; set; } = 0;
        /// <summary>
        /// This defines how many pixels of PDF pages are trimmed on the left. Default is 0.
        /// Hier wird definiert, wieviele Pixel von PDF-Seiten links abgeschnitten werden. Standard ist 0.
        /// </summary>
        public int CutLeft { get; set; } = 0;
        /// <summary>
        /// This defines how many pixels of PDF pages are trimmed on the right. Default is 0.
        /// Hier wird definiert, wieviele Pixel von PDF-Seiten rechts abgeschnitten werden. Standard ist 0.
        /// </summary>
        public int CutRight { get; set; } = 0;
        /// <summary>
        /// This defines whether properties without found values should be added to the results or not. Default is true.
        /// Hier wird definiert, ob Eigenschaften ohne gefundene Werte zu den Ergebnissen hinzugefügt werden sollen oder nicht. Standard ist true.
        /// </summary>
        public bool AddEmptyProperties { get; set; } = true;
        /// <summary>
        /// Here the SearchKey objects are defined for which values are to be extracted from files.
        /// Hier werden die SearchKey-Objekte definiert zu denen Werte aus Dateien extrahiert werden.
        /// </summary>
        public GroupedSearchKeys SearchKeys { get; set; } = new();
        /// <summary>
        /// Here different GroupedSearchKeys objects are defined in a Dictionary, which should be treated as recurring groups of search keys.
        /// Hier werden unterschiedliche GroupedSearchKeys-Objekte in einem Wörterbuch definiert, welche als wiederkehrende Gruppen von Suchschlüsseln behandelt werden sollen.
        /// </summary>
        public Dictionary<string, GroupedSearchKeys> SearchKeyGroups { get; set; } = new();
        /// <summary>
        /// These fields are ignored by skipping their containing field. Especially intended for fields that cannot be uniquely identified or contain one occurrence of the field.
        /// Diese Felder werden dadurch ignoriert, dass ihre enthaltenden Felder übersprungen werden. Speziell für Felder gedacht, die nicht eindeutig identifiziert werden können bzw. ein Vorkommen des Feldes enthalten.
        /// </summary>
        public RegexWrappedCollection IgnoreFields { get; set; } = new(@".*", @".*");
        /// <summary>
        /// These rows are ignored by skipping them as a whole. Especially meant for headings or fields which are shifted by their offset between Key and Value rows. Their strings have to be selected from the .CONSOLE(5)-Report.
        /// Diese Zeilen werden dadruch ignoriert, dass sie als Ganzes übersprungen werden. Speziell für Überschriften oder Felder gedacht, welche sich durch ihren Versatz zwischen Key- und Value-Zeilen schieben. Diese Zeichenketten müssen aus dem .CONSOLE(5)-Report ausgewählt werden.
        /// </summary>
        public RegexWrappedCollection IgnoreRows { get; set; } = new(@"(^| *)", @"$");
        /// <summary>
        /// Absolute stop fields are defined here for scenarios where the algorithm iterates beyond multiple rows or columns.
        /// Hier werden absolute Stoppfelder definiert für Szenarien, bei denen der Algorithmus über mehrfache Zeilen oder Spalten hinaus iteriert.
        /// </summary>
        public RegexWrappedCollection BreakFields { get; set; } = new(@".*", @".*");
        /// <summary>
        /// Absolute stop rows are defined here for scenarios where the algorithm iterates beyond multiple rows or columns. Their strings have to be selected from the .CONSOLE(5)-Report.
        /// Hier werden absolute Stoppzeilen definiert für Szenarien, bei denen der Algorithmus über mehrfache Zeilen oder Spalten hinaus iteriert. Diese Zeichenketten müssen aus dem .CONSOLE(5)-Report ausgewählt werden.
        /// </summary>
        public RegexWrappedCollection BreakRows { get; set; } = new(@"(^| *)", @"$");
        /// <summary>
        /// This collection defines strings that replace checkboxes. These strings should have maximum rarity or not occur in the document otherwise.
        /// In dieser Collection werden Zeichenketten definiert, welche Checkboxen ersetzen. Diese Zeichenketten sollten maximale Seltenheit aufweisen bzw. ansonsten nicht im Dokument vorkommen.
        /// </summary>
        public RegexWrappedCollection CheckBoxes { get; set; } = new(@".*", @".*");
        /// <summary>
        /// This collection defines strings that replace radio buttons. These strings should have maximum rarity or not occur in the document otherwise.
        /// In dieser Collection werden Zeichenketten definiert, welche Radio-Buttons ersetzen. Diese Zeichenketten sollten maximale Seltenheit aufweisen bzw. ansonsten nicht im Dokument vorkommen.
        /// </summary>
        public RegexWrappedCollection RadioButtons { get; set; } = new(@".*", @".*");
        /// <summary>
        /// This list is used to separate key-value pairs that are together in one field. The strings that act as separators are defined here.
        /// Diese Liste dient dazu, Schlüssel-Wert-Paare zu trennen, die sich zusammen in einem Feld befinden. Hier werden die Zeichenfolgen definiert, welche als Trennzeichen fungieren.
        /// </summary>
        public HashSet<string> Separators { get; set; } = new();
        #endregion

        // ! CONSTRUCTORS
        #region constructors
        /// <summary>
        /// Initializes a new instance of the Config class with default values.
        /// Initialisiert eine neue Instanz der Klasse Config mit Standardwerten.
        /// </summary>
        public Config() { }
        /// <summary>
        /// Initializes a new instance of the Config class with the values from a .cfg file in JSON format.
        /// Initialisiert eine neue Instanz der Klasse Config mit den Werten aus einer .cfg-Datei im JSON-Format.
        /// </summary>
        /// <param name="filePath">The file path to the .cfg file. Der Dateipfad zur .cfg-Datei.</param>
        public Config(string filePath) => ReadConfigFile(filePath);
        #endregion

        // ! METHODS
        #region methods
        /// <summary>
        /// Reads values from a .cfg file in JSON format and updates the configuration.
        /// Liest Werte aus einer .cfg-Datei im JSON-Format und aktualisiert die Konfiguration.
        /// </summary>
        /// <param name="filePath">The file path to the .cfg file. Der Dateipfad zur .cfg-Datei.</param>
        public void ReadConfigFile(string filePath)
        {
            string jsonContent = File.ReadAllText(filePath);
            JsonConvert.PopulateObject(jsonContent, this);
            // ! GenerateWrappedItems() & MaskAllMembers() are necessary because JsonConvert.PopulateObject() does not call members as mentioned for them.
            IgnoreFields.GenerateWrappedItems();
            IgnoreRows.GenerateWrappedItems();
            BreakFields.GenerateWrappedItems();
            BreakRows.GenerateWrappedItems();
            CheckBoxes.GenerateWrappedItems();
            RadioButtons.GenerateWrappedItems();
            foreach (SearchKey searchKey in SearchKeys) searchKey.MaskAllMembers();
            foreach (KeyValuePair<string, GroupedSearchKeys> kvp in SearchKeyGroups)
            {
                foreach (SearchKey searchKey in kvp.Value) searchKey.MaskAllMembers();
            }
        }
        /// <summary>
        /// Outputs the configuration values to the console.
        /// Gibt die Konfigurationswerte auf der Konsole aus.
        /// </summary>
        public void CONSOLE_Config()
        {
            Console.WriteLine("CONSOLE_Config:");
            Console.WriteLine("###############\r\n");
            // ! Print primitive types
            Console.WriteLine($"- MaxWhiteSpaces = {MaxWhiteSpaces}");
            Console.WriteLine($"- RangeBuffer = {RangeBuffer}");
            Console.WriteLine($"- MinSeparatorSpaces = {MinSeparatorSpaces}");
            Console.WriteLine($"- MaxSeparatorSpaces = {MaxSeparatorSpaces}");
            Console.WriteLine($"- CutTop = {CutTop}");
            Console.WriteLine($"- CutBottom = {CutBottom}");
            Console.WriteLine($"- CutLeft = {CutLeft}");
            Console.WriteLine($"- CutRight = {CutRight}");
            Console.WriteLine($"- AddEmptyProperties = {AddEmptyProperties}");
            // ! Print SearchKeys
            Console.WriteLine("SearchKeys:");
            foreach (SearchKey searchKey in SearchKeys)
            {
                Console.WriteLine($"- \"{searchKey.Key}\" = {searchKey.GetDirectionInteger()} => " +
                    $"{searchKey.GetDirectionName()}");
                foreach (string append in searchKey.Appendix) Console.WriteLine($" - \"{append}\"");
            }
            // ! Print SearchKeyGroups
            Console.WriteLine("SearchKeyGroups:");
            foreach (KeyValuePair<string, GroupedSearchKeys> searchKeyGroup in SearchKeyGroups)
            {
                Console.WriteLine($"- {searchKeyGroup.Key}:");
                foreach (SearchKey searchKey in searchKeyGroup.Value)
                {
                    Console.WriteLine($" - \"{searchKey.Key}\" = {searchKey.GetDirectionInteger()} => " +
                        $"{searchKey.GetDirectionName()}");
                    foreach (string append in searchKey.Appendix) Console.WriteLine($"  - \"{append}\"");
                }
            }
            // ! Print IgnoreFields
            Console.WriteLine("IgnoreFields:");
            foreach (string ignoreField in IgnoreFields) Console.WriteLine($"- \"{ignoreField}\"");
            // ! Print IgnoreRows
            Console.WriteLine("IgnoreRows:");
            foreach (string ignoreRow in IgnoreRows) Console.WriteLine($"- \"{ignoreRow}\"");
            // ! Print BreakFields
            Console.WriteLine("BreakFields:");
            foreach (string breakField in BreakFields) Console.WriteLine($"- \"{breakField}\"");
            // ! Print BreakRows
            Console.WriteLine("BreakRows:");
            foreach (string breakRow in BreakRows) Console.WriteLine($"- \"{breakRow}\"");
            // ! Print CheckBoxes
            Console.WriteLine("CheckBoxes:");
            foreach (string checkBox in CheckBoxes) Console.WriteLine($"- \"{checkBox}\"");
            // ! Print RadioButtons
            Console.WriteLine("RadioButtons:");
            foreach (string radioButton in RadioButtons) Console.WriteLine($"- \"{radioButton}\"");
            // ! Print SeparatorStrings
            Console.WriteLine("Separators:");
            foreach (string separator in Separators) Console.WriteLine($"- \"{separator}\"");
            Console.ReadLine();
        }
        #endregion
    }
}