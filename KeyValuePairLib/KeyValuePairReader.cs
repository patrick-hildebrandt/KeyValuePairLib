using System.Data;
using System.Text.RegularExpressions;

namespace KeyValuePairLib
{
    /// <summary>
    /// This class extracts key-value pairs from files with semi-structured or unstructured data.
    /// Diese Klasse extrahiert Schlüssel-Wert-Paaren aus Dateien mit semi- oder unstrukturierten Daten.
    /// </summary>
    public class KeyValuePairReader
    {
        // ! ENUMERATIONS
        /// <summary>
        /// Represents the file types that can be processed by the KeyValuePairReader class.
        /// Stellt die Dateitypen dar, die von der Klasse KeyValuePairReader verarbeitet werden können.
        /// </summary>
        private enum FileType { Pdf, Csv, Xlsx, Xml, Unknown }

        // ! FIELDS
        #region fields
        /// <summary>
        /// This field maps the row position of the cursor for the DataTable FieldTable.
        /// Dieses Feld bildet die Zeilenposition des Cursors für die DataTable FieldTable ab.
        /// </summary>
        private int _currentRow = 0;
        /// <summary>
        /// This field maps the column position of the cursor for the DataTable FieldTable.
        /// Dieses Feld bildet die Spaltenposition des Cursors für die DataTable FieldTable ab.
        /// </summary>
        private int _currentColumn = 0;
        /// <summary>
        /// This field represents the actual position of the cursor for the DataTable FieldTable.
        /// Dieses Feld stellt die tatsächliche Position des Cursors für die DataTable FieldTable dar.
        /// </summary>
        private int _currentField = 0;
        /// <summary>
        /// This field maps the row position of the relative cursor for the DataTable FieldTable.
        /// Dieses Feld bildet die Zeilenposition des relativen Cursors für die DataTable FieldTable ab.
        /// </summary>
        private int _lookupRow = 0;
        /// <summary>
        /// This field maps the column position of the relative cursor for the DataTable FieldTable.
        /// Dieses Feld bildet die Spaltenposition des relativen Cursors für die DataTable FieldTable ab.
        /// </summary>
        private int _lookupColumn = 0;
        /// <summary>
        /// This field represents the actual position of the relative cursor for the DataTable FieldTable.
        /// Dieses Feld stellt die tatsächliche Position des relativen Cursors für die DataTable FieldTable dar.
        /// </summary>
        private int _lookupField = 0;
        /// <summary>
        /// The path of the file to process. Set to proof that necessary data is available.
        /// Der Pfad der zu verarbeitenden Datei. Gesetzt um sicher zu stellen, dass erforderliche Daten verfügbar sind.
        /// </summary>
        private string _filePath = "";
        /// <summary>
        /// This field represents a dynamic object in which the supported file types are analyzed before extraction.
        /// Dieses Feld stellt ein dynamisches Objekt dar, in dem die unterstützten Dateitypen vor der Extraktion analysiert werden.
        /// </summary>
        private dynamic _fileData;
        /// <summary>
        /// Instantiation of the RegexUtils class.
        /// Instanziierung der RegexUtils-Klasse.
        /// </summary>
        private readonly RegexUtils _regexUtils = new();
        #endregion

        // ! PROPERTIES
        #region properties
        /// <summary>
        /// This DataTable represents a two-dimensional table in which separable fields of the input file are recorded depending on their position.
        /// Diese DataTable bildet eine zweidimensionale Tabelle ab, in der voneinander trennnbare Felder der Eingabedatei abhängig von ihrer Position erfasst werden.
        /// </summary>
        public DataTable FieldTable { get; private set; } = new();
        /// <summary>
        /// The results of the extraction of search keys are stored here.
        /// Hier werden die Ergebnisse der Extraktion von Suchschlüsseln erfasst.
        /// </summary>
        public GroupedKeyValuePairs KeyValuePairs { get; set; } = new();
        /// <summary>
        /// The results of the extraction of grouped search keys are stored here.
        /// Hier werden die Ergebnisse der Extraktion von gruppierten Suchschlüsseln erfasst.
        /// </summary>
        public Dictionary<string, Dictionary<int, GroupedKeyValuePairs>> KeyValuePairGroups { get; set; } = new();
        /// <summary>
        /// Instantiation of the Config.
        /// Instanziierung der Klasse Config.
        /// </summary>
        public Config Config { get; set; }
        /// <summary>
        /// Instantiation of the Separators class.
        /// Instanziierung der Klasse Separators.
        /// </summary>
        public SeparatorCollection Separators { get; set; }
        #endregion

        // ! CONSTRUCTORS
        #region constructors
        /// <summary>
        /// Initializes a new instance of the KeyValuePairReader class with default settings.
        /// Initialisiert eine neue Instanz der Klasse KeyValuePairReader mit Voreinstellungen.
        /// </summary>
        public KeyValuePairReader()
        {
            Config = new Config();
            Separators = new SeparatorCollection(Config.MaxWhiteSpaces, Config.MinSeparatorSpaces,
                Config.MaxSeparatorSpaces, Config.Separators);
            _fileData = new PdfFileData(Config);
        }
        /// <summary>
        /// Initializes a new instance of the KeyValuePairReader class and loads the configuration from the provided .cfg file.
        /// Initialisiert eine neue Instanz der Klasse KeyValuePairReader und lädt die Konfiguration aus der bereitgestellten .cfg-Datei.
        /// </summary>
        /// <param name="configPath">The path of the .cfg file. Der Pfad der .cfg-Datei.</param>
        public KeyValuePairReader(string configPath)
        {
            Config = new Config(configPath);
            Separators = new SeparatorCollection(Config.MaxWhiteSpaces, Config.MinSeparatorSpaces,
                Config.MaxSeparatorSpaces, Config.Separators);
            _fileData = new PdfFileData(Config);
        }
        /// <summary>
        /// Initializes a new instance of the KeyValuePairReader class, loads the configuration from the provided .cfg file in configPath, and uses it to extract the key-value pairs from the provided file in filePath.
        /// Initialisiert eine neue Instanz der Klasse KeyValuePairReader, lädt die Konfiguration aus der bereitgestellten .cfg-Datei in configPath und extrahiert anhand dieser die Schlüssel-Wert-Paare aus der bereitgestellten Datei in filePath.
        /// </summary>
        /// <param name="configPath">The path of the .cfg file. Der Pfad der .cfg-Datei.</param>
        /// <param name="filePath">The path of the file to process. Der Pfad der zu verarbeitenden Datei.</param>
        public KeyValuePairReader(string configPath, string filePath) : this(configPath) => ReadFile(filePath);
        #endregion

        // ! METHODS
        #region methods
        /// <summary>
        /// This method reads the configuration for extraction from the provided .cfg file in configPath.
        /// Diese Methode liest die Konfiguration zur Extraktion aus der bereitgestellten .cfg-Datei in configPath.
        /// </summary>
        /// <param name="configPath">The path of the .cfg file. Der Pfad der .cfg-Datei.</param>
        public void ReadConfig(string configPath)
        {
            Config = new Config(configPath);
            Separators = new SeparatorCollection(Config.MaxWhiteSpaces, Config.MinSeparatorSpaces,
                Config.MaxSeparatorSpaces, Config.Separators);
        }
        /// <summary>
        /// This method extracts the key-value pairs from the provided file in filePath based on the configuration.
        /// Diese Methode extrahiert anhand der Konfiguration die Schlüssel-Wert-Paare aus der bereitgestellten Datei in filePath.
        /// </summary>
        /// <param name="filePath">The path of the file to process. Der Pfad der zu verarbeitenden Datei.</param>
        public void ReadFile(string filePath)
        {
            // ! Reset result objects before reading new file
            ClearFile();
            // ! Set to proof that necessary data is available
            _filePath = filePath;
            // ! Get file extension
            string fileExtension = Path.GetExtension(filePath).TrimStart('.');
            // ! Declare fileType, initialize fileType, switch-case by fileType
            FileType fileType = fileExtension.ToLower() switch
            {
                "pdf" => FileType.Pdf,
                "csv" => FileType.Csv,
                "xlsx" => FileType.Xlsx,
                "xml" => FileType.Xml,
                _ => FileType.Unknown,
            };
            // ! Dynamic instantiation of _fileData
            switch (fileType)
            {
                case FileType.Pdf:
                    _fileData = new PdfFileData(Config, filePath);
                    FieldTable = _fileData.FieldTable;
                    GetKeyValuePairs();
                    GetKeyValuePairGroups();
                    break;
                case FileType.Csv:
                    _fileData = new CsvFileData();
                    FieldTable = _fileData.FieldTable;
                    throw new NotImplementedException("CSV files not supported yet. " +
                        "CSV-Dateien werden noch nicht unterstützt.");
                //GetKeyValuePairs();
                //GetKeyValuePairGroups();
                //break;
                case FileType.Xlsx:
                    _fileData = new XlsxFileData();
                    FieldTable = _fileData.FieldTable;
                    throw new NotImplementedException("XLSX files not supported yet. " +
                        "XLSX-Dateien werden noch nicht unterstützt.");
                //GetKeyValuePairs();
                //GetKeyValuePairGroups();
                //break;
                case FileType.Xml:
                    _fileData = new XmlFileData();
                    FieldTable = _fileData.FieldTable;
                    throw new NotImplementedException("XML files not supported yet. " +
                        "XML-Dateien werden noch nicht unterstützt.");
                //GetKeyValuePairs();
                //GetKeyValuePairGroups();
                //break;
                case FileType.Unknown:
                    throw new NotImplementedException("File type not supported. Dateityp nicht unterstützt.");
            }
        }
        /// <summary>
        /// Resets the result objects KeyValuePairs and KeyValuePairGroups.
        /// Setzt die Ergebnis-Objekte KeyValuePairs und KeyValuePairGroups zurück.
        /// </summary>
        private void ClearFile()
        {
            _filePath = "";
            KeyValuePairs = new();
            KeyValuePairGroups = new();
        }
        /// <summary>
        /// This method traverses the DataTable FieldTable with a cursor alternately column-wise and row-wise and performs an extraction attempt for each field using the search keys from SearchKeys.
        /// Diese Methode durchläuft mit einem Cursor abwechselnd Spalten- und Zeilen-weise die DataTable FieldTable und führt für jedes Feld einen Extraktionsversuch anhand der Suchschlüssel aus SearchKeys durch.
        /// </summary>
        private void GetKeyValuePairs()
        {
            GetFirstRow();
            do
            {
                ExtractSearchKeyValue(GetCurrentValue(), Config.SearchKeys, KeyValuePairs);
                while (GetNextColumn()) ExtractSearchKeyValue(GetCurrentValue(), Config.SearchKeys,
                    KeyValuePairs);
            } while (GetNextRow());
        }
        /// <summary>
        /// This method traverses the DataTable FieldTable with a cursor alternately column-wise and row-wise and performs an extraction attempt for each field using the search keys from SearchKeyGroups.
        /// Diese Methode durchläuft mit einem Cursor abwechselnd Spalten- und Zeilen-weise die DataTable FieldTable und führt für jedes Feld einen Extraktionsversuch anhand der Suchschlüssel aus SearchKeyGroups durch.
        /// </summary>
        private void GetKeyValuePairGroups()
        {
            foreach (KeyValuePair<string, GroupedSearchKeys> kvp in Config.SearchKeyGroups)
            {
                int gskSize = 0;
                bool headerFound = false;
                string gskName = kvp.Key;
                int gskCount = 0;
                int turn = 0;
                bool otherGskFound = false;
                foreach (SearchKey searchKey in kvp.Value)
                {
                    GetFirstField();
                    do
                    {
                        if (GetCurrentValue().Contains(searchKey.Key))
                        {
                            gskSize++;
                            break;
                        }
                    } while (GetNextField());
                }
                GetFirstField();
                do
                {
                    if (!headerFound) if (GetCurrentValue() == gskName) headerFound = true;
                    if (!headerFound) continue;
                    if (!KeyValuePairGroups.ContainsKey(gskName)) KeyValuePairGroups.Add(gskName, new());
                    if (!KeyValuePairGroups[gskName].ContainsKey(gskCount)) KeyValuePairGroups[gskName].Add(gskCount,
                        new());
                    if (ExtractSearchKeyValue(GetCurrentValue(), kvp.Value, KeyValuePairGroups[gskName]
                        [gskCount]) == true)
                    {
                        if (turn == gskSize - 1)
                        {
                            gskCount++;
                            turn = 0;
                        }
                        else turn++;
                    }
                    foreach (string otherGskName in Config.SearchKeyGroups.Keys)
                    {
                        if (otherGskName == gskName) continue;
                        if (GetCurrentValue() == otherGskName)
                        {
                            otherGskFound = true;
                            break;
                        }
                    }
                    if (otherGskFound) break;
                } while (GetNextField());
            }
            foreach (KeyValuePair<string, Dictionary<int, GroupedKeyValuePairs>> kvpName in KeyValuePairGroups)
            {
                foreach (KeyValuePair<int, GroupedKeyValuePairs> kvpGroup in kvpName.Value) if (kvpGroup.Value
                        .IsEmpty()) kvpName.Value.Remove(kvpGroup.Key);
            }
        }
        /// <summary>
        /// This method compares the provided search keys in searchKeys with the current position of the cursor in currentField and, if they match, calls an extraction method depending on the SearchDirection property of the respective search key.
        /// Diese Methode vergleicht die bereitgestellten Suchschlüssel in searchKeys mit der aktuellen Position des Cursors in currentField und ruft bei Übereinstimmung eine Extraktionsmethode abhängig von der Eigenschaft SearchDirection des jeweiligen Suchschlüssels auf.
        /// </summary>
        /// <param name="currentField">The current field value as string. Der aktuelle Feld-Wert als Zeichenkette.</param>
        /// <param name="searchKeys">The collection of search keys to be compared. Die Sammlung von Suchschlüsseln, die verglichen werden sollen.</param>
        /// <param name="keyValuePairs">The collection of key-value pairs to which the results are added. Die Sammlung von Schlüssel-Wert-Paaren, zu der die Ergebnisse hinzugefügt werden.</param>
        /// <returns>True if search key and field matched, otherwise false. True, wenn Suchschlüssel und Feld übereinstimmen, sonst false.</returns>
        public bool ExtractSearchKeyValue(string currentField, GroupedSearchKeys searchKeys, GroupedKeyValuePairs
            keyValuePairs)
        {
            foreach (SearchKey searchKey in searchKeys)
            {
                if (currentField == searchKey.Key && searchKey.Direction == SearchDirection.Horizontal)
                {
                    ExtractHorizontal(searchKey, keyValuePairs);
                    return true;
                }
                if (currentField == searchKey.Key && searchKey.Direction == SearchDirection.Vertical)
                {
                    ExtractVertical(searchKey, keyValuePairs);
                    return true;
                }
                if (currentField == searchKey.Key && searchKey.Direction == SearchDirection.Columns)
                {
                    ExtractColumns(searchKey, keyValuePairs);
                    return true;
                }
                if (currentField == searchKey.Key && searchKey.Direction == SearchDirection.Rows)
                {
                    ExtractRows(searchKey, keyValuePairs);
                    return true;
                }
                if (currentField == searchKey.Key && searchKey.Direction == SearchDirection.Table)
                {
                    ExtractTable(searchKey, keyValuePairs);
                    return true;
                }
                if (currentField.Contains(searchKey.Key) && searchKey.Direction == SearchDirection.CheckBoxes)
                {
                    ExtractCheckBoxes(searchKey, keyValuePairs);
                    return true;
                }
                if (currentField.Contains(searchKey.Key) && searchKey.Direction == SearchDirection.RadioButtons)
                {
                    ExtractRadioButtons(searchKey, keyValuePairs);
                    return true;
                }
                if (currentField.Contains(searchKey.Key) && searchKey.Direction == SearchDirection.Separators)
                {
                    ExtractSeparators(searchKey, keyValuePairs);
                    return true;
                }
                if (currentField.Contains(searchKey.Key) && searchKey.Direction == SearchDirection.Abstract)
                {
                    ExtractAbstract(searchKey, keyValuePairs);
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// This method performs an extraction attempt on the current position of the cursor based on the provided search key in searchKey and according to the searchKey property SearchDirection and stores the result in keyValuePairs.
        /// Diese Methode führt anhand des bereitgestellten Suchschlüssels in searchKey und entsprechend der searchKey-Eigenschaft SearchDirection einen Extraktionsversuch auf die aktuelle Position des Cursors durch und speichert das Ergebnis in keyValuePairs.
        /// </summary>
        /// <param name="searchKey">The search key to be used for extraction. Der Suchschlüssel welcher für die Extraktion verwendet werden soll.</param>
        /// <param name="keyValuePairs">The collection of key-value pairs to which the results are added. Die Sammlung von Schlüssel-Wert-Paaren, zu der die Ergebnisse hinzugefügt werden.</param>
        /// <returns>True if extraction was successful, otherwise false. True, wenn Extraktion erfolgreich war, sonst false.</returns>
        private bool ExtractHorizontal(SearchKey searchKey, GroupedKeyValuePairs keyValuePairs)
        {
            int offset = 0;
            IncreaseOffsetRight(ref offset, searchKey);
            if (GetRightValue(offset) != "")
            {
                bool extract = true;
                foreach (SearchKey key in Config.SearchKeys)
                {
                    if (key.Key == searchKey.Key) continue;
                    if (GetRightValue(offset) == key.Key)
                    {
                        extract = false;
                        break;
                    }
                }
                foreach (KeyValuePair<string, GroupedSearchKeys> kvpName in Config.SearchKeyGroups)
                {
                    foreach (SearchKey key in kvpName.Value)
                    {
                        if (key.Key == searchKey.Key) continue;
                        if (GetRightValue(offset) == key.Key)
                        {
                            extract = false;
                            break;
                        }
                    }
                }
                if (extract)
                {
                    foreach (string breakField in Config.BreakFields)
                        if (Regex.IsMatch(GetRightValue(offset), breakField))
                        {
                            extract = false;
                            break;
                        }
                }
                if (extract)
                {
                    keyValuePairs.AddProperty(_regexUtils.DemaskRegexChars(TrimSeparators(searchKey.Key)) + searchKey
                        .GetAppendixString(), _regexUtils.DemaskRegexChars(TrimSeparators(GetRightValue(offset))));
                    return true;
                }
                else if (Config.AddEmptyProperties) keyValuePairs.AddProperty(_regexUtils.DemaskRegexChars(
                    TrimSeparators(searchKey.Key)) + searchKey.GetAppendixString(), "");
                return false;
            }
            else if (Config.AddEmptyProperties) keyValuePairs.AddProperty(_regexUtils.DemaskRegexChars(TrimSeparators(
                searchKey.Key)) + searchKey.GetAppendixString(), "");
            return false;
        }
        /// <summary>
        /// This method performs an extraction attempt on the current position of the cursor based on the provided search key in searchKey and according to the searchKey property SearchDirection and stores the result in keyValuePairs.
        /// Diese Methode führt anhand des bereitgestellten Suchschlüssels in searchKey und entsprechend der searchKey-Eigenschaft SearchDirection einen Extraktionsversuch auf die aktuelle Position des Cursors durch und speichert das Ergebnis in keyValuePairs.
        /// </summary>
        /// <param name="searchKey">The search key to be used for extraction. Der Suchschlüssel welcher für die Extraktion verwendet werden soll.</param>
        /// <param name="keyValuePairs">The collection of key-value pairs to which the results are added. Die Sammlung von Schlüssel-Wert-Paaren, zu der die Ergebnisse hinzugefügt werden.</param>
        /// <returns>True if extraction was successful, otherwise false. True, wenn Extraktion erfolgreich war, sonst false.</returns>
        private bool ExtractVertical(SearchKey searchKey, GroupedKeyValuePairs keyValuePairs)
        {
            if (GetBelowValue() != "")
            {
                bool extract = true;
                int offset = 0;
                foreach (SearchKey key in Config.SearchKeys)
                {
                    if (key.Key == searchKey.Key) continue;
                    if (GetBelowValue(offset) == key.Key)
                    {
                        extract = false;
                        break;
                    }
                }
                foreach (KeyValuePair<string, GroupedSearchKeys> kvpName in Config.SearchKeyGroups)
                {
                    foreach (SearchKey key in kvpName.Value)
                    {
                        if (key.Key == searchKey.Key) continue;
                        if (GetBelowValue(offset) == key.Key)
                        {
                            extract = false;
                            break;
                        }
                    }
                }
                if (extract)
                {
                    foreach (string breakField in Config.BreakFields)
                        if (Regex.IsMatch(_fileData.Rows[GetCurrentRow() + 1 + offset], breakField))
                        {
                            extract = false;
                            break;
                        }
                }
                if (extract)
                {
                    foreach (string breakRow in Config.BreakRows)
                        if (Regex.IsMatch(_fileData.Rows[GetCurrentRow() + 1 + offset], breakRow))
                        {
                            extract = false;
                            break;
                        }
                }
                foreach (string append in searchKey.Appendix) if (GetBelowValue(offset) == append) offset++;
                if (extract)
                {
                    keyValuePairs.AddProperty(_regexUtils.DemaskRegexChars(TrimSeparators(searchKey.Key)) + searchKey
                        .GetAppendixString(), _regexUtils.DemaskRegexChars(TrimSeparators(GetBelowValue(offset))));
                    return true;
                }
                else if (Config.AddEmptyProperties) keyValuePairs.AddProperty(_regexUtils.DemaskRegexChars(
                    TrimSeparators(searchKey.Key)) + searchKey.GetAppendixString(), "");
                return false;
            }
            else if (Config.AddEmptyProperties) keyValuePairs.AddProperty(_regexUtils.DemaskRegexChars(TrimSeparators(
                searchKey.Key)) + searchKey.GetAppendixString(), "");
            return false;
        }
        /// <summary>
        /// This method performs an extraction attempt on the current position of the cursor based on the provided search key in searchKey and according to the searchKey property SearchDirection and stores the result in keyValuePairs.
        /// Diese Methode führt anhand des bereitgestellten Suchschlüssels in searchKey und entsprechend der searchKey-Eigenschaft SearchDirection einen Extraktionsversuch auf die aktuelle Position des Cursors durch und speichert das Ergebnis in keyValuePairs.
        /// </summary>
        /// <param name="searchKey">The search key to be used for extraction. Der Suchschlüssel welcher für die Extraktion verwendet werden soll.</param>
        /// <param name="keyValuePairs">The collection of key-value pairs to which the results are added. Die Sammlung von Schlüssel-Wert-Paaren, zu der die Ergebnisse hinzugefügt werden.</param>
        /// <returns>True if extraction was successful, otherwise false. True, wenn Extraktion erfolgreich war, sonst false.</returns>
        private bool ExtractColumns(SearchKey searchKey, GroupedKeyValuePairs keyValuePairs)
        {
            int offset = 0;
            bool extract = false;
            bool first = true;
            string matches = "";
            while (GetRightValue(offset) != "")
            {
                IncreaseOffsetRight(ref offset, searchKey);
                //if (GetRightValue(offset) == null) break;
                foreach (SearchKey key in Config.SearchKeys)
                {
                    if (key.Key == searchKey.Key) continue;
                    if (GetRightValue(offset) == key.Key)
                    {
                        extract = true;
                        break;
                    }
                }
                foreach (KeyValuePair<string, GroupedSearchKeys> kvpName in Config.SearchKeyGroups)
                {
                    foreach (SearchKey key in kvpName.Value)
                    {
                        if (key.Key == searchKey.Key) continue;
                        if (GetRightValue(offset) == key.Key)
                        {
                            extract = true;
                            break;
                        }
                    }
                }
                if (!extract)
                {
                    foreach (string breakField in Config.BreakFields)
                        if (Regex.IsMatch(GetRightValue(offset), breakField))
                        {
                            extract = true;
                            break;
                        }
                }
                if (extract) break;
                if (!first) matches += "\t";
                if (!extract) matches += TrimSeparators(GetRightValue(offset));
                if (first) first = false;
                offset++;
            }
            if (matches != "")
            {
                keyValuePairs.AddProperty(_regexUtils.DemaskRegexChars(TrimSeparators(searchKey.Key)) + searchKey
                    .GetAppendixString(), _regexUtils.DemaskRegexChars(matches.Trim()));
                return true;
            }
            else if (Config.AddEmptyProperties)
            {
                keyValuePairs.AddProperty(_regexUtils.DemaskRegexChars(TrimSeparators(searchKey.Key)) + searchKey
                    .GetAppendixString(), "");
                return false;
            }
            else return false;
        }
        /// <summary>
        /// This method performs an extraction attempt on the current position of the cursor based on the provided search key in searchKey and according to the searchKey property SearchDirection and stores the result in keyValuePairs.
        /// Diese Methode führt anhand des bereitgestellten Suchschlüssels in searchKey und entsprechend der searchKey-Eigenschaft SearchDirection einen Extraktionsversuch auf die aktuelle Position des Cursors durch und speichert das Ergebnis in keyValuePairs.
        /// </summary>
        /// <param name="searchKey">The search key to be used for extraction. Der Suchschlüssel welcher für die Extraktion verwendet werden soll.</param>
        /// <param name="keyValuePairs">The collection of key-value pairs to which the results are added. Die Sammlung von Schlüssel-Wert-Paaren, zu der die Ergebnisse hinzugefügt werden.</param>
        /// <returns>True if extraction was successful, otherwise false. True, wenn Extraktion erfolgreich war, sonst false.</returns>
        private bool ExtractRows(SearchKey searchKey, GroupedKeyValuePairs keyValuePairs)
        {
            int offset = 0;
            bool extract = false;
            bool first = true;
            string matches = "";
            while (GetBelowValue(offset) != "")
            {
                foreach (SearchKey key in Config.SearchKeys)
                {
                    if (key.Key == searchKey.Key) continue;
                    if (GetBelowValue(offset) == key.Key)
                    {
                        extract = true;
                        break;
                    }
                }
                foreach (KeyValuePair<string, GroupedSearchKeys> kvpName in Config.SearchKeyGroups)
                {
                    foreach (SearchKey key in kvpName.Value)
                    {
                        if (key.Key == searchKey.Key) continue;
                        if (GetBelowValue(offset) == key.Key)
                        {
                            extract = true;
                            break;
                        }
                    }
                }
                if (!extract)
                {
                    foreach (string breakField in Config.BreakFields)
                        if (Regex.IsMatch(_fileData.Rows[GetCurrentRow() + 1 + offset], breakField))
                        {
                            extract = true;
                            break;
                        }
                }
                if (!extract)
                {
                    foreach (string breakRow in Config.BreakRows)
                        if (Regex.IsMatch(_fileData.Rows[GetCurrentRow() + 1 + offset], breakRow))
                        {
                            extract = true;
                            break;
                        }
                }
                if (!extract)
                {
                    foreach (string append in searchKey.Appendix) if (GetBelowValue(offset) == append)
                            offset++;
                }
                //if (extract || GetBelowValue(offset) == null) break;
                if (!first) matches += "\r\n";
                if (!extract) matches += TrimSeparators(GetBelowValue(offset));
                if (first) first = false;
                offset++;
            }
            if (matches != "")
            {
                keyValuePairs.AddProperty(_regexUtils.DemaskRegexChars(TrimSeparators(searchKey.Key)) + searchKey
                    .GetAppendixString(), _regexUtils.DemaskRegexChars(matches.Trim()));
                return true;
            }
            else if (Config.AddEmptyProperties)
            {
                keyValuePairs.AddProperty(_regexUtils.DemaskRegexChars(TrimSeparators(searchKey.Key)) + searchKey
                    .GetAppendixString(), "");
                return false;
            }
            else return false;
        }
        /// <summary>
        /// This method performs an extraction attempt on the current position of the cursor based on the provided search key in searchKey and according to the searchKey property SearchDirection and stores the result in keyValuePairs.
        /// Diese Methode führt anhand des bereitgestellten Suchschlüssels in searchKey und entsprechend der searchKey-Eigenschaft SearchDirection einen Extraktionsversuch auf die aktuelle Position des Cursors durch und speichert das Ergebnis in keyValuePairs.
        /// </summary>
        /// <param name="searchKey">The search key to be used for extraction. Der Suchschlüssel welcher für die Extraktion verwendet werden soll.</param>
        /// <param name="keyValuePairs">The collection of key-value pairs to which the results are added. Die Sammlung von Schlüssel-Wert-Paaren, zu der die Ergebnisse hinzugefügt werden.</param>
        /// <returns>True if extraction was successful, otherwise false. True, wenn Extraktion erfolgreich war, sonst false.</returns>
        private bool ExtractTable(SearchKey searchKey, GroupedKeyValuePairs keyValuePairs)
        {
            bool extract = false;
            bool first = true;
            string matches = "";
            int startField = _currentField;
            if (!GetNextField())
            {
                if (Config.AddEmptyProperties) keyValuePairs.AddProperty(_regexUtils.DemaskRegexChars(TrimSeparators(
                    searchKey.Key)) + searchKey.GetAppendixString(), "");
                GetField(startField);
                return false;
            }
            do
            {
                bool skip = false;
                foreach (string ignoreField in Config.IgnoreFields)
                    if (Regex.IsMatch(GetCurrentValue(), ignoreField))
                    {
                        skip = true;
                        break;
                    }
                if (skip) continue;
                foreach (string ignoreRow in Config.IgnoreRows)
                    if (Regex.IsMatch(_fileData.Rows[GetCurrentRow()], ignoreRow))
                    {
                        skip = true;
                        break;
                    }
                if (skip) continue;
                foreach (string append in searchKey.Appendix)
                    if (GetCurrentValue() == append)
                    {
                        skip = true;
                        break;
                    }
                if (skip) continue;
                foreach (SearchKey key in Config.SearchKeys)
                {
                    if (key.Key == searchKey.Key) continue;
                    if (GetCurrentValue().ToString() == key.Key)
                    {
                        extract = true;
                        break;
                    }
                }
                foreach (KeyValuePair<string, GroupedSearchKeys> kvpName in Config.SearchKeyGroups)
                {
                    foreach (SearchKey key in kvpName.Value)
                    {
                        if (key.Key == searchKey.Key) continue;
                        if (GetCurrentValue() == key.Key)
                        {
                            extract = true;
                            break;
                        }
                    }
                }
                if (!extract)
                {
                    foreach (string breakField in Config.BreakFields)
                        if (Regex.IsMatch(GetCurrentValue(), breakField))
                        {
                            extract = true;
                            break;
                        }
                }
                if (!extract)
                {
                    foreach (string breakRow in Config.BreakRows)
                        if (Regex.IsMatch(_fileData.Rows[GetCurrentRow()], breakRow))
                        {
                            extract = true;
                            break;
                        }
                }
                if (extract) break;
                if (!first) matches += "\t";
                if (!extract) matches += TrimSeparators(GetCurrentValue());
                if (first) first = false;
            } while (GetNextField());
            if (matches != "")
            {
                keyValuePairs.AddProperty(_regexUtils.DemaskRegexChars(TrimSeparators(searchKey.Key)) + searchKey
                    .GetAppendixString(), _regexUtils.DemaskRegexChars(matches));
                GetField(startField);
                return true;
            }
            else if (Config.AddEmptyProperties) keyValuePairs.AddProperty(_regexUtils.DemaskRegexChars(TrimSeparators(
                searchKey.Key)) + searchKey.GetAppendixString(), "");
            GetField(startField);
            return false;
        }
        /// <summary>
        /// This method performs an extraction attempt on the current position of the cursor based on the provided search key in searchKey and according to the searchKey property SearchDirection and stores the result in keyValuePairs.
        /// Diese Methode führt anhand des bereitgestellten Suchschlüssels in searchKey und entsprechend der searchKey-Eigenschaft SearchDirection einen Extraktionsversuch auf die aktuelle Position des Cursors durch und speichert das Ergebnis in keyValuePairs.
        /// </summary>
        /// <param name="searchKey">The search key to be used for extraction. Der Suchschlüssel welcher für die Extraktion verwendet werden soll.</param>
        /// <param name="keyValuePairs">The collection of key-value pairs to which the results are added. Die Sammlung von Schlüssel-Wert-Paaren, zu der die Ergebnisse hinzugefügt werden.</param>
        /// <returns>True if extraction was successful, otherwise false. True, wenn Extraktion erfolgreich war, sonst false.</returns>
        private bool ExtractCheckBoxes(SearchKey searchKey, GroupedKeyValuePairs keyValuePairs)
        {
            int startField = _currentField;
            do
            {
                bool skip = false;
                bool extract = false;
                foreach (string ignoreField in Config.IgnoreFields)
                    if (Regex.IsMatch(GetCurrentValue(), ignoreField))
                    {
                        skip = true;
                        break;
                    }
                if (skip) continue;
                foreach (string ignoreRow in Config.IgnoreRows)
                    if (Regex.IsMatch(_fileData.Rows[GetCurrentRow()], ignoreRow))
                    {
                        skip = true;
                        break;
                    }
                if (skip) continue;
                foreach (string append in searchKey.Appendix)
                    if (GetCurrentValue() == append)
                    {
                        skip = true;
                        break;
                    }
                if (skip) continue;
                foreach (SearchKey key in Config.SearchKeys)
                {
                    if (key.Key == searchKey.Key) continue;
                    if (GetCurrentValue() == key.Key)
                    {
                        extract = false;
                        break;
                    }
                }
                foreach (KeyValuePair<string, GroupedSearchKeys> kvpName in Config.SearchKeyGroups)
                {
                    foreach (SearchKey key in kvpName.Value)
                    {
                        if (key.Key == searchKey.Key) continue;
                        if (GetCurrentValue() == key.Key)
                        {
                            extract = false;
                            break;
                        }
                    }
                }
                if (extract)
                {
                    foreach (string breakField in Config.BreakFields)
                        if (Regex.IsMatch(GetCurrentValue(), breakField))
                        {
                            extract = false;
                            break;
                        }
                }
                if (extract)
                {
                    foreach (string breakRow in Config.BreakRows)
                        if (Regex.IsMatch(_fileData.Rows[GetCurrentRow()], breakRow))
                        {
                            extract = false;
                            break;
                        }
                }
                if (extract)
                {
                    foreach (string checkBox in Config.CheckBoxes)
                    {
                        if (Regex.IsMatch(GetCurrentValue(), checkBox))
                        {
                            keyValuePairs.AddProperty(_regexUtils.DemaskRegexChars(TrimSeparators(searchKey.Key)) +
                                searchKey.GetAppendixString(), "CHECKED");
                            GetField(startField);
                            return true;
                        }
                    }
                    if (Config.AddEmptyProperties) keyValuePairs.AddProperty(_regexUtils.DemaskRegexChars(
                        TrimSeparators(searchKey.Key)) + searchKey.GetAppendixString(), "");
                    GetField(startField);
                    return false;
                }
                else
                {
                    if (Config.AddEmptyProperties) keyValuePairs.AddProperty(_regexUtils.DemaskRegexChars(
                        TrimSeparators(searchKey.Key)) + searchKey.GetAppendixString(), "");
                    GetField(startField);
                    return false;
                }
            } while (GetNextField());
            GetField(startField);
            return false;
        }
        /// <summary>
        /// This method performs an extraction attempt on the current position of the cursor based on the provided search key in searchKey and according to the searchKey property SearchDirection and stores the result in keyValuePairs.
        /// Diese Methode führt anhand des bereitgestellten Suchschlüssels in searchKey und entsprechend der searchKey-Eigenschaft SearchDirection einen Extraktionsversuch auf die aktuelle Position des Cursors durch und speichert das Ergebnis in keyValuePairs.
        /// </summary>
        /// <param name="searchKey">The search key to be used for extraction. Der Suchschlüssel welcher für die Extraktion verwendet werden soll.</param>
        /// <param name="keyValuePairs">The collection of key-value pairs to which the results are added. Die Sammlung von Schlüssel-Wert-Paaren, zu der die Ergebnisse hinzugefügt werden.</param>
        /// <returns>True if extraction was successful, otherwise false. True, wenn Extraktion erfolgreich war, sonst false.</returns>
        private bool ExtractRadioButtons(SearchKey searchKey, GroupedKeyValuePairs keyValuePairs)
        {
            bool extract = false;
            bool first = true;
            string matches = "";
            int startField = _currentField;
            do
            {
                bool skip = false;
                foreach (string ignoreField in Config.IgnoreFields)
                    if (Regex.IsMatch(GetCurrentValue(), ignoreField))
                    {
                        skip = true;
                        break;
                    }
                if (skip) continue;
                foreach (string ignoreRow in Config.IgnoreRows)
                    if (Regex.IsMatch(_fileData.Rows[GetCurrentRow()], ignoreRow))
                    {
                        skip = true;
                        break;
                    }
                if (skip) continue;
                foreach (string append in searchKey.Appendix)
                    if (GetCurrentValue() == append)
                    {
                        skip = true;
                        break;
                    }
                if (skip) continue;
                foreach (SearchKey key in Config.SearchKeys)
                {
                    if (key.Key == searchKey.Key) continue;
                    if (GetCurrentValue() == key.Key)
                    {
                        extract = true;
                        break;
                    }
                }
                foreach (KeyValuePair<string, GroupedSearchKeys> kvpName in Config.SearchKeyGroups)
                {
                    foreach (SearchKey key in kvpName.Value)
                    {
                        if (key.Key == searchKey.Key) continue;
                        if (GetCurrentValue() == key.Key)
                        {
                            extract = true;
                            break;
                        }
                    }
                }
                if (!extract)
                {
                    foreach (string breakField in Config.BreakFields)
                        if (Regex.IsMatch(GetCurrentValue(), breakField))
                        {
                            extract = true;
                            break;
                        }
                }
                if (!extract)
                {
                    foreach (string breakRow in Config.BreakRows)
                        if (Regex.IsMatch(_fileData.Rows[GetCurrentRow()], breakRow))
                        {
                            extract = true;
                            break;
                        }
                }
                if (extract) break;
                foreach (string radioButton in Config.RadioButtons)
                    if (Regex.IsMatch(GetCurrentValue(), radioButton))
                    {
                        if (!first) matches += "\t";
                        string match = TrimSeparators(GetCurrentValue());
                        match = TrimRadioButtons(match);
                        matches += TrimSeparators(match);
                        if (first) first = false;
                        break;
                    }
            } while (GetNextField());
            if (matches != "")
            {
                keyValuePairs.AddProperty(_regexUtils.DemaskRegexChars(TrimSeparators(searchKey.Key)) + searchKey
                    .GetAppendixString(), _regexUtils.DemaskRegexChars(matches));
                GetField(startField);
                return true;
            }
            else if (Config.AddEmptyProperties) keyValuePairs.AddProperty(_regexUtils.DemaskRegexChars(TrimSeparators(
                searchKey.Key)) + searchKey.GetAppendixString(), "");
            GetField(startField);
            return false;
        }
        /// <summary>
        /// This method performs an extraction attempt on the current position of the cursor based on the provided search key in searchKey and according to the searchKey property SearchDirection and stores the result in keyValuePairs.
        /// Diese Methode führt anhand des bereitgestellten Suchschlüssels in searchKey und entsprechend der searchKey-Eigenschaft SearchDirection einen Extraktionsversuch auf die aktuelle Position des Cursors durch und speichert das Ergebnis in keyValuePairs.
        /// </summary>
        /// <param name="searchKey">The search key to be used for extraction. Der Suchschlüssel welcher für die Extraktion verwendet werden soll.</param>
        /// <param name="keyValuePairs">The collection of key-value pairs to which the results are added. Die Sammlung von Schlüssel-Wert-Paaren, zu der die Ergebnisse hinzugefügt werden.</param>
        /// <returns>True if extraction was successful, otherwise false. True, wenn Extraktion erfolgreich war, sonst false.</returns>
        private bool ExtractSeparators(SearchKey searchKey, GroupedKeyValuePairs keyValuePairs)
        {
            bool extract = false;
            string separatorVariationMatch = "";
            foreach (string separatorVariation in Separators.SeparatorVariations)
                if (Regex.IsMatch(GetCurrentValue(), separatorVariation))
                {
                    extract = true;
                    if (separatorVariation.Length > separatorVariationMatch.Length) separatorVariationMatch =
                            separatorVariation;
                }
            string[] match = Regex.Split(GetCurrentValue(), separatorVariationMatch);
            if (extract && searchKey.Key.Contains(match[0]))
            {
                keyValuePairs.AddProperty(_regexUtils.DemaskRegexChars(TrimSeparators(searchKey.Key)) + searchKey
                    .GetAppendixString(), _regexUtils.DemaskRegexChars(TrimSeparators(string.Join("", match.Skip(1)))
                    .Trim()));
                return true;
            }
            else if (Config.AddEmptyProperties) keyValuePairs.AddProperty(_regexUtils.DemaskRegexChars(TrimSeparators(
                searchKey.Key)) + searchKey.GetAppendixString(), "");
            return false;
        }
        /// <summary>
        /// Reserved for abstract implementation in derived class for special cases that cannot be pictured with config rules.
        /// Reserviert für die abstrakte Implementierung von Sonderfällen in abgeleiteter Klasse, die nicht mit Konfigurationsregeln abgebildet werden können.
        /// </summary>
        /// <param name="searchKey">The search key to be used for extraction. Der Suchschlüssel welcher für die Extraktion verwendet werden soll.</param>
        /// <param name="keyValuePairs">The collection of key-value pairs to which the results are added. Die Sammlung von Schlüssel-Wert-Paaren, zu der die Ergebnisse hinzugefügt werden.</param>
        /// <returns>True if extraction was successful, otherwise false. True, wenn Extraktion erfolgreich war, sonst false.</returns>
        public virtual bool ExtractAbstract(SearchKey searchKey, GroupedKeyValuePairs keyValuePairs)
        {
            throw new NotImplementedException("Override in derived class for special cases that cannot be pictured " +
                "with config rules. Überschreibe in abgeleiteter Klasse für Sonderfälle, die nicht mit " +
                "Konfigurationsregeln abgebildet werden können.");
        }
        /// <summary>
        /// Method to trim radio button characters.
        /// Methode zum Trimmen von Radio-Button-Zeichen.
        /// </summary>
        /// <param name="input">The string to be trimmed. Die zu trimmende Zeichenkette.</param>
        /// <returns>The trimmed string. Die getrimmte Zeichenkette.</returns>
        private string TrimRadioButtons(string input)
        {
            foreach (string radioButton in Config.RadioButtons)
            {
                if (input.Contains(radioButton))
                {
                    char[] charArray = radioButton.ToCharArray();
                    input = input.Trim(charArray[0]).Trim();
                }
            }
            return input;
        }
        /// <summary>
        /// Method to trim separator characters.
        /// Methode zum Trimmen von Separator-Zeichen.
        /// </summary>
        /// <param name="input">The string to be trimmed. Die zu trimmende Zeichenkette.</param>
        /// <returns>The trimmed string. Die getrimmte Zeichenkette.</returns>
        private string TrimSeparators(string input)
        {
            foreach (string separator in Separators.Separators)
            {
                if (input.Contains(separator))
                {
                    char[] charArray = separator.ToCharArray();
                    input = input.Trim(charArray[0]).Trim();
                }
            }
            return input;
        }
        /// <summary>
        /// This method increases the offset for the extraction field by one to the right and calls itself recursively as long as a field is found to be skipped.
        /// Diese Methode erhöht den Versatz für das Extraktionsfeld um eins nach rechts und ruft sich selbst rekursiv auf, solange ein Feld gefunden wird, das übersprungen werden soll.
        /// </summary>
        /// <param name="offset">The offset to be increased. Der zu erhöhende Versatz.</param>
        /// <param name="searchKey">The search key to be used for extraction. Der Suchschlüssel welcher für die Extraktion verwendet werden soll.</param>
        private void IncreaseOffsetRight(ref int offset, SearchKey searchKey)
        {
            if (GetRightValue(offset) != "")
            {
                foreach (string append in searchKey.Appendix)
                {
                    if (Regex.IsMatch(GetRightValue(offset), append))
                    {
                        offset++;
                        IncreaseOffsetRight(ref offset, searchKey);
                    }
                }
                foreach (string ignoreField in Config.IgnoreFields)
                {
                    if (Regex.IsMatch(GetRightValue(offset), ignoreField))
                    {
                        offset++;
                        IncreaseOffsetRight(ref offset, searchKey);
                    }
                }
            }
            else return;
        }
        /// <summary>
        /// This method returns the number of fields from the DataTable FieldTable as an integer value.
        /// Diese Methode gibt die Anzahl der Felder aus der DataTable FieldTable als Integer-Wert zurück.
        /// </summary>
        /// <returns>The number of fields as an integer value. Die Anzahl der Felder als Integer-Wert.</returns>
        public int GetFieldCount() => FieldTable.Rows.Count;
        /// <summary>
        /// This method sets the cursor to the first field and returns a boolean value.
        /// Diese Methode setzt den Cursor auf das erste Feld und gibt einen booleschen Wert zurück.
        /// </summary>
        /// <returns>True if operation successfull, otherwise false. True, wenn der Vorgang erfolgreich war, sonst false.</returns>
        public bool GetFirstField() => GetField(0);
        /// <summary>
        /// This method sets the cursor to the field of the provided integer value for field and returns a boolean value.
        /// Diese Methode setzt den Cursor auf das Feld des bereitgestellten Integer-Wertes für field und gibt einen booleschen Wert zurück.
        /// </summary>
        /// <param name="field">The field to be set. Das zu setzende Feld.</param>
        /// <returns>True if operation successfull, otherwise false. True, wenn der Vorgang erfolgreich war, sonst false.</returns>
        public bool GetField(int field)
        {
            if (field >= 0 && field < FieldTable.Rows.Count)
            {
                _currentField = field;
                DataRow currentField = FieldTable.Rows[_currentField];
                _currentRow = (int)currentField["Row"];
                _currentColumn = (int)currentField["Column"];
                return true;
            }
            else return false;
        }
        /// <summary>
        /// This method sets the cursor to the downstream field and returns a boolean value.
        /// Diese Methode setzt den Cursor auf das nachgelagerte Feld und gibt einen booleschen Wert zurück.
        /// </summary>
        /// <returns>True if operation successfull, otherwise false. True, wenn der Vorgang erfolgreich war, sonst false.</returns>
        public bool GetNextField()
        {
            if (_currentField >= FieldTable.Rows.Count - 1) return false;
            _currentField++;
            DataRow currentField = FieldTable.Rows[_currentField];
            _currentRow = (int)currentField["Row"];
            _currentColumn = (int)currentField["Column"];
            return true;
        }
        /// <summary>
        /// This method returns the number of rows from the DataTable FieldTable as an integer value.
        /// Diese Methode gibt die Anzahl der Zeilen aus der DataTable FieldTable als Integer-Wert zurück.
        /// </summary>
        /// <returns>The number of rows as an integer value. Die Anzahl der Zeilen als Integer-Wert.</returns>
        public int GetRowCount()
        {
            DataRow determineField = FieldTable.Rows[GetFieldCount() - 1];
            return (int)determineField["Row"] + 1;
        }
        /// <summary>
        /// This method returns the integer value of the current row of the cursor as an integer value.
        /// Diese Methode gibt den Integer-Wert der aktuellen Zeile des Cursors als Integer-Wert zurück.
        /// </summary>
        /// <returns>The current row as an integer value. Die aktuelle Zeile als Integer-Wert.</returns>
        public int GetCurrentRow()
        {
            DataRow currentField = FieldTable.Rows[_currentField];
            return (int)currentField["Row"];
        }
        /// <summary>
        /// This method sets the cursor to the first field of the first row and returns a boolean value.
        /// Diese Methode setzt den Cursor auf das erste Feld der ersten Zeile und gibt einen booleschen Wert zurück.
        /// </summary>
        /// <returns>True if operation successfull, otherwise false. True, wenn der Vorgang erfolgreich war, sonst false.</returns>
        public bool GetFirstRow() => GetRow(0);
        /// <summary>
        /// This method sets the cursor to the first field of the row of the provided integer value for row and returns a boolean value.
        /// Diese Methode setzt den Cursor auf das erste Feld der Zeile des bereitgestellten Integer-Wertes für row und gibt einen booleschen Wert zurück.
        /// </summary>
        /// <param name="row">The row to be set. Die zu setzende Zeile.</param>
        /// <returns>True if operation successfull, otherwise false. True, wenn der Vorgang erfolgreich war, sonst false.</returns>
        public bool GetRow(int row)
        {
            if (row < 0) return false;
            _currentField = 0;
            DataRow currentField = FieldTable.Rows[_currentField];
            _currentRow = (int)currentField["Row"];
            _currentColumn = (int)currentField["Column"];
            if (row == 0) return true;
            while ((int)currentField["Row"] != row && _currentField < FieldTable.Rows.Count - 1)
            {
                _currentField++;
                currentField = FieldTable.Rows[_currentField];
                _currentRow = (int)currentField["Row"];
                _currentColumn = (int)currentField["Column"];
                if ((int)currentField["Row"] == row) return true;
            }
            return false;
        }
        /// <summary>
        /// This method sets the cursor to the first field of the downstream row and returns a boolean value.
        /// Diese Methode setzt den Cursor auf das erste Feld der nachgelagerten Zeile und gibt einen booleschen Wert zurück.
        /// </summary>
        /// <returns>True if operation successfull, otherwise false. True, wenn der Vorgang erfolgreich war, sonst false.</returns>
        public bool GetNextRow()
        {
            DataRow currentField = FieldTable.Rows[_currentField];
            _currentRow = (int)currentField["Row"];
            return GetRow(_currentRow + 1);
        }
        /// <summary>
        /// This method sets the cursor to the downstream column of the current row and returns a boolean value.
        /// Diese Methode setzt den Cursor auf die nachgelagerte Spalte der aktuellen Zeile und gibt einen booleschen Wert zurück.
        /// </summary>
        /// <returns>True if operation successfull, otherwise false. True, wenn der Vorgang erfolgreich war, sonst false.</returns>
        public bool GetNextColumn()
        {
            if (_currentField >= FieldTable.Rows.Count - 1) return false;
            DataRow currentField = FieldTable.Rows[_currentField];
            _currentRow = (int)currentField["Row"];
            DataRow nextField = FieldTable.Rows[_currentField + 1];
            int nextRow = (int)nextField["Row"];
            if (_currentRow == nextRow)
            {
                _currentField++;
                currentField = FieldTable.Rows[_currentField];
                _currentRow = (int)currentField["Row"];
                _currentColumn = (int)currentField["Column"];
                return true;
            }
            else return false;
        }
        /// <summary>
        /// This method extracts the value from the current field of the cursor as a string.
        /// Diese Methode extrahiert den Wert vom aktuellen Feld des Cursors als String.
        /// </summary>
        /// <returns>The extracted field value as a string. Der extrahierte Feld-Wert als Zeichenkette.</returns>
        public string GetCurrentValue()
        {
            if (_currentField < 0 || _currentField > GetFieldCount() - 1) return "";
            DataRow currentField = FieldTable.Rows[_currentField];
            _currentRow = (int)currentField["Row"];
            _currentColumn = (int)currentField["Column"];
            return currentField["Value"].ToString() ?? "";
        }
        /// <summary>
        /// This method extracts the value from the field below the cursor as a string. Rows are skipped based on the provided integer value for offset.
        /// Diese Methode extrahiert den Wert vom Feld unterhalb des Cursors als String. Anhand des bereitgestellten Integer-Wertes für offset werden Zeilen übersprungen.
        /// </summary>
        /// <param name="offset">The offset to be used. Der zu verwendende Versatz.</param>
        /// <returns>The extracted field value as a string. Der extrahierte Feld-Wert als Zeichenkette.</returns>
        public string GetBelowValue(int offset = 0)
        {
            int loop;
            do
            {
                loop = offset;
                foreach (string ignoreField in Config.IgnoreFields) if (GetCurrentRow() + 1 + offset < GetRowCount() -
                        1) if (Regex.IsMatch(_fileData.Rows[GetCurrentRow() + 1 + offset], ignoreField)) offset++;
                foreach (string ignoreRow in Config.IgnoreRows) if (GetCurrentRow() + 1 + offset < GetRowCount() - 1)
                        if (Regex.IsMatch(_fileData.Rows[GetCurrentRow() + 1 + offset], ignoreRow)) offset++;
            } while (loop != offset && GetCurrentRow() + 1 + offset < GetRowCount() - 1);
            _lookupField = _currentField;
            DataRow lookupField = FieldTable.Rows[_lookupField];
            _lookupRow = (int)lookupField["Row"];
            _lookupColumn = (int)lookupField["Column"] - Config.RangeBuffer;
            int _lookupEnd = (int)lookupField["End"];
            do
            {
                if (_lookupField >= GetFieldCount() - 1) return "";
                _lookupField++;
                lookupField = FieldTable.Rows[_lookupField];
                if ((int)lookupField["Row"] == _lookupRow + 1 + offset && (int)lookupField["Column"] >= _lookupColumn
                    && _lookupEnd >= (int)lookupField["Column"])
                {
                    _lookupRow = (int)lookupField["Row"];
                    _lookupColumn = (int)lookupField["Column"];
                    return lookupField["Value"].ToString() ?? "";
                }
            } while ((int)lookupField["Row"] <= _lookupRow + 1 + offset);
            return "";
        }
        /// <summary>
        /// This method extracts the value from the field to the right of the cursor as a string. Based on the provided integer value for offset, columns are skipped.
        /// Diese Methode extrahiert den Wert vom Feld rechtsseitig des Cursors als String. Anhand des bereitgestellten Integer-Wertes für offset werden Spalten übersprungen.
        /// </summary>
        /// <param name="offset">The offset to be used. Der zu verwendende Versatz.</param>
        /// <returns>The extracted field value as a string. Der extrahierte Feld-Wert als Zeichenkette.</returns>
        public string GetRightValue(int offset = 0)
        {
            if (_currentField >= FieldTable.Rows.Count - 1) return "";
            _lookupField = _currentField;
            DataRow lookupField = FieldTable.Rows[_lookupField];
            _lookupRow = (int)lookupField["Row"];
            DataRow nextField = FieldTable.Rows[_lookupField + 1 + offset];
            int nextRow = (int)nextField["Row"];
            if (_lookupRow == nextRow)
            {
                _lookupField += 1 + offset;
                lookupField = FieldTable.Rows[_lookupField];
                _lookupRow = (int)lookupField["Row"];
                _lookupColumn = (int)lookupField["Column"];
                return lookupField["Value"].ToString() ?? "";
            }
            else return "";
        }
        /// <summary>
        /// This method returns the results of the extraction as a tuple of KeyValuePairs and KeyValuePairGroups.
        /// Diese Methode gibt die Ergebnisse der Extraktion als Tuple aus KeyValuePairs und KeyValuePairGroups zurück.
        /// </summary>
        /// <returns>The tuple containing KeyValuePairs and KeyValuePairGroups. Das Tupel welches KeyValuePairs und KeyValuePairGroups enthält.</returns>
        /// <exception cref="FileNotFoundException">No file to process set. Keine Datei zur Verarbeitung gesetzt.</exception>"
        public (GroupedKeyValuePairs, Dictionary<string, Dictionary<int, GroupedKeyValuePairs>>) GetResultObject()
        {
            if (_filePath == "") throw new FileNotFoundException("No file to process set. " +
                "Keine Datei zur Verarbeitung gesetzt.");
            return (KeyValuePairs, KeyValuePairGroups);
        }
        /// <summary>
        /// This method returns the results of the extraction as a tuple of KeyValuePairs and KeyValuePairGroups based on the provided file in filePath.
        /// Diese Methode gibt die Ergebnisse der Extraktion als Tuple aus KeyValuePairs und KeyValuePairGroups anhand der bereitgestellten Datei in filePath zurück.
        /// </summary>
        /// <param name="filePath">The file to be processed. Die zu verarbeitende Datei.</param>
        /// <returns>The tuple containing KeyValuePairs and KeyValuePairGroups. Das Tupel welches KeyValuePairs und KeyValuePairGroups enthält.</returns>
        public (GroupedKeyValuePairs, Dictionary<string, Dictionary<int, GroupedKeyValuePairs>>) GetResultObject(
            string filePath)
        {
            ReadFile(filePath);
            return (KeyValuePairs, KeyValuePairGroups);
        }
        /// <summary>
        /// This method returns the results of the extraction as a tuple of KeyValuePairs and KeyValuePairGroups based on the provided file and configuration in filePath and configPath.
        /// Diese Methode gibt die Ergebnisse der Extraktion als Tuple aus KeyValuePairs und KeyValuePairGroups anhand der bereitgestellten Datei und Konfiguration in filePath und configPath zurück.
        /// </summary>
        /// <param name="filePath">The file to be processed. Die zu verarbeitende Datei.</param>
        /// <param name="configPath">The path of the .cfg file. Der Pfad der .cfg-Datei.</param>
        /// <returns>The tuple containing KeyValuePairs and KeyValuePairGroups. Das Tupel welches KeyValuePairs und KeyValuePairGroups enthält.</returns>
        public (GroupedKeyValuePairs, Dictionary<string, Dictionary<int, GroupedKeyValuePairs>>) GetResultObject(
            string filePath, string configPath)
        {
            ReadConfig(configPath);
            ReadFile(filePath);
            return (KeyValuePairs, KeyValuePairGroups);
        }
        /// <summary>
        /// Reports the results of the methods for the FieldTable object to the console for debugging purpose.
        /// Meldet die Ergebnisse der Methoden für das FieldTable-Objekt zu Debugging-Zwecken an die Konsole.
        /// </summary>
        public void CONSOLE_FieldTableMethods()
        {
            Console.WriteLine("CONSOLE_FieldTableMethods:");
            Console.WriteLine("##########################\r\n");
            // ! GetCount
            Console.WriteLine("GetFieldCount: \t°" + GetFieldCount() + "°");
            Console.WriteLine("  GetRowCount: \t°" + GetRowCount() + "°");
            // ! GetField(GetFieldCount - 2)
            Console.WriteLine("GetField(GetFieldCount - 2): \t°" + GetField(GetFieldCount() - 2) + "°");
            Console.WriteLine("              GetCurrentRow: \t°" + GetCurrentRow() + "°\r\n");
            // ! GetFirstField
            Console.WriteLine("GetFirstField: \t°" + GetFirstField() + "°");
            Console.WriteLine("GetCurrentRow: \t°" + GetCurrentRow() + "°\r\n");
            // ! GetRow(1)
            Console.WriteLine("    GetRow(1): \t°" + GetRow(1) + "°");
            Console.WriteLine("GetCurrentRow: \t°" + GetCurrentRow() + "°\r\n");
            // ! GetFirstRow
            Console.WriteLine("  GetFirstRow: \t°" + GetFirstRow() + "°");
            Console.WriteLine("GetCurrentRow: \t°" + GetCurrentRow() + "°\r\n");
            Console.ReadLine();
        }
        /// <summary>
        /// Reports the results of the methods for the FieldTable object to the console for debugging purpose.
        /// Meldet die Ergebnisse der Methoden für das FieldTable-Objekt zu Debugging-Zwecken an die Konsole.
        /// </summary>
        public void CONSOLE_FieldTableMethodsExpand()
        {
            int offsetRight = 0;
            SearchKey mockSearchKey = new("mock", SearchDirection.Horizontal, "mockAppendix");
            Console.WriteLine("CONSOLE_FieldTableMethodsExpand:");
            Console.WriteLine("################################\r\n");
            GetFirstField();
            if (GetCurrentValue() != "") Console.WriteLine("°x°" + GetCurrentValue() + "°x°");
            IncreaseOffsetRight(ref offsetRight, mockSearchKey);
            if (GetRightValue() != "") Console.WriteLine("°>  °" + GetRightValue() + "°>°");
            if (GetBelowValue() != "") Console.WriteLine("°v    °" + GetBelowValue() + "°v°");
            while (GetNextColumn())
            {
                offsetRight = 0;
                if (GetCurrentValue() != "") Console.WriteLine("°x°" + GetCurrentValue() + "°x°");
                IncreaseOffsetRight(ref offsetRight, mockSearchKey);
                if (GetRightValue() != "") Console.WriteLine("°>  °" + GetRightValue() + "°>°");
                if (GetBelowValue() != "") Console.WriteLine("°v    °" + GetBelowValue() + "°v°");
            }
            while (GetNextRow())
            {
                offsetRight = 0;
                if (GetCurrentValue() != "") Console.WriteLine("°x°" + GetCurrentValue() + "°x°");
                IncreaseOffsetRight(ref offsetRight, mockSearchKey);
                if (GetRightValue() != "") Console.WriteLine("°>  °" + GetRightValue() + "°>°");
                if (GetBelowValue() != "") Console.WriteLine("°v    °" + GetBelowValue() + "°v°");
                while (GetNextColumn())
                {
                    offsetRight = 0;
                    if (GetCurrentValue() != "") Console.WriteLine("°x°" + GetCurrentValue() + "°x°");
                    IncreaseOffsetRight(ref offsetRight, mockSearchKey);
                    if (GetRightValue() != "") Console.WriteLine("°>  °" + GetRightValue() + "°>°");
                    if (GetBelowValue() != "") Console.WriteLine("°v    °" + GetBelowValue() + "°v°");
                }
            }
            Console.ReadLine();
        }
        /// <summary>
        /// This method reports the results of the respective sub-methods in order of the processing steps to the console. Without parameters, all methods are output.
        /// Diese Methode gibt die Ergebnisse der jeweiligen Sub-Methoden in Reihenfolge der Verarbeitungsschritte in der Konsole aus. Ohne Parameter werden alle Methoden ausgegeben.
        /// </summary>
        /// <param name="methods">The method results to be reported. Die auszugebenden Ergebnisse der Methoden.</param>
        public void CONSOLE(params int[] methods)
        {
            if (methods.Length == 0 || methods.Contains(0)) Config.CONSOLE_Config();
            if (methods.Length == 0 || methods.Contains(1)) Separators.CONSOLE_SeparatorCollection();
            if (methods.Length == 0 || methods.Contains(2)) _fileData.CONSOLE_pdfDocString();
            if (methods.Length == 0 || methods.Contains(3)) _fileData.CONSOLE_pdfDocEmptyRowsRemoved();
            if (methods.Length == 0 || methods.Contains(4)) _fileData.CONSOLE_pdfDocFieldsDelimited();
            if (methods.Length == 0 || methods.Contains(5)) _fileData.CONSOLE_Rows();
            if (methods.Length == 0 || methods.Contains(6)) _fileData.CONSOLE_fields();
            if (methods.Length == 0 || methods.Contains(7)) _fileData.CONSOLE_indexedFields();
            if (methods.Length == 0 || methods.Contains(8)) _fileData.CONSOLE_FieldTable();
            if (methods.Length == 0 || methods.Contains(9)) CONSOLE_FieldTableMethods();
            if (methods.Length == 0 || methods.Contains(10)) CONSOLE_FieldTableMethodsExpand();
            if (methods.Length == 0 || methods.Contains(11)) KeyValuePairs.CONSOLE_GroupedKeyValuePairs();
            // ! KeyValuePairGroups => CONSOLE_GroupedKeyValuePairs()
            if (methods.Length == 0 || methods.Contains(12))
            {
                foreach (KeyValuePair<string, Dictionary<int, GroupedKeyValuePairs>> kvpName in KeyValuePairGroups)
                {
                    Console.WriteLine($"CONSOLE_{kvpName.Key}:");
                    foreach (KeyValuePair<int, GroupedKeyValuePairs> kvpGroup in kvpName.Value)
                    {
                        kvpGroup.Value.CONSOLE_GroupedKeyValuePairs();
                    }
                }
            }
            Console.WriteLine("### REPORT END ###");
            Console.WriteLine("##################");
            Console.ReadLine();
        }
        #endregion
    }
}