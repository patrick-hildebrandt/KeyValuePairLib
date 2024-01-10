using Spire.Pdf;
using Spire.Pdf.Texts;
using System.Data;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;

namespace KeyValuePairLib
{
    /// <summary>
    /// Represents data extracted from a PDF file.
    /// Stellt Daten dar, die aus einer PDF-Datei extrahiert wurden.
    /// </summary>
    public class PdfFileData
    {
        // ! FIELDS
        #region fields
        /// <summary>
        /// The original content of the PDF document as a string.
        /// Der ursprüngliche Inhalt des PDF-Dokuments als Zeichenkette.
        /// </summary>
        private string _pdfDocString = "";
        /// <summary>
        /// The PDF document content with empty rows removed.
        /// Der Inhalt des PDF-Dokuments mit entfernten leeren Zeilen.
        /// </summary>
        private string _pdfDocEmptyRowsRemoved = "";
        /// <summary>
        /// The PDF document content with fields delimited.
        /// Der Inhalt des PDF-Dokuments mit abgegrenzten Feldern.
        /// </summary>
        private string _pdfDocFieldsDelimited = "";
        /// <summary>
        /// Stores the extracted fields from the PDF document as a dictionary where the key is the row number and the value is a list of field strings.
        /// Speichert die extrahierten Felder aus dem PDF-Dokument als Wörterbuch, wobei der Schlüssel die Zeilennummer und der Wert eine Liste von Feld-Zeichenketten ist.
        /// </summary>
        private Dictionary<int, List<string>> _fields = new();
        /// <summary>
        /// Stores the indexed fields from the PDF document as a dictionary where the key is the row number and the value is a list of tuples containing field value, starting index and ending index.
        /// Speichert die indizierten Felder aus dem PDF-Dokument als Wörterbuch, wobei der Schlüssel die Zeilennummer und der Wert eine Liste von Tupeln ist, die den Feld-Wert, den Startindex und den Endindex enthalten.
        /// </summary>
        private Dictionary<int, List<(string, int, int)>> _indexedFields = new();
        /// <summary>
        /// Instantiation of the Config class.
        /// Instanziierung der Klasse Config.
        /// </summary>
        private readonly Config _config;
        /// <summary>
        /// Instantiation of the RegexUtils class.
        /// Instanziierung der RegexUtils-Klasse.
        /// </summary>
        private readonly RegexUtils _regexUtils = new();
        #endregion

        // ! PROPERTIES
        #region properties
        /// <summary>
        /// Gets or sets the extracted rows from the PDF document, where the key is the row number and the value is the row content.
        /// Ruft die extrahierten Zeilen aus dem PDF-Dokument ab oder legt sie fest, wobei der Schlüssel die Zeilennummer und der Wert der Zeileninhalt ist.
        /// </summary>
        public Dictionary<int, string> Rows { get; private set; }
        /// <summary>
        /// Gets or sets the DataTable object containing detected fields in the PDF file. The PDF document is pictured as a two-dimensional table.
        /// Ruft das DataTable-Objekt ab, das die erkannten Felder in der PDF-Datei enthält, oder legt sie fest. Das PDF-Dokument wird hierbei als zweidimensionale Tabelle abgebildet.
        /// </summary>
        public DataTable FieldTable { get; private set; }
        #endregion

        // ! CONSTRUCTORS
        #region constructors
        /// <summary>
        /// Initializes a new instance of the PdfFileData class with default values.
        /// Initialisiert eine neue Instanz der PdfFileData-Klasse mit Standardwerten.
        /// </summary>
        public PdfFileData()
        {
            Rows = new();
            FieldTable = new();
            _config = new();
        }
        /// <summary>
        /// Initializes a new instance of the PdfFileData class with the provided configuration.
        /// Initialisiert eine neue Instanz der PdfFileData-Klasse mit der bereitgestellten Konfiguration.
        /// </summary>
        /// <param name="config">The configuration object. Das Konfigurations-Objekt.</param>
        public PdfFileData(Config config)
        {
            Rows = new();
            FieldTable = new();
            _config = config;
        }
        /// <summary>
        /// Initializes a new instance of the PdfFileData class with the provided configuration file path.
        /// Initialisiert eine neue Instanz der PdfFileData-Klasse mit dem bereitgestellten Konfigurationsdateipfad.
        /// </summary>
        /// <param name="configPath">The file path to the configuration file. Der Dateipfad zur Konfigurationsdatei.</param>
        public PdfFileData(string configPath)
        {
            Rows = new();
            FieldTable = new();
            _config = new(configPath);
        }
        /// <summary>
        /// Initializes a new instance of the PdfFileData class with the provided configuration and PDF file path.
        /// Initialisiert eine neue Instanz der PdfFileData-Klasse mit der bereitgestellten Konfiguration und dem PDF-Dateipfad.
        /// </summary>
        /// <param name="config">The configuration object. Das Konfigurations-Objekt.</param>
        /// <param name="filePath">The file path to the PDF file. Der Dateipfad zur PDF-Datei.</param>
        public PdfFileData(Config config, string filePath) : this(config) => ReadFile(filePath);
        /// <summary>
        /// Initializes a new instance of the PdfFileData class with the provided configuration file and PDF file path.
        /// Initialisiert eine neue Instanz der PdfFileData-Klasse mit dem bereitgestellten Konfigurationsdateipfad und dem PDF-Dateipfad.
        /// </summary>
        /// <param name="configPath">The file path to the configuration file. Der Dateipfad zur Konfigurationsdatei.</param>
        /// <param name="filePath">The file path to the PDF file. Der Dateipfad zur PDF-Datei.</param>
        public PdfFileData(string configPath, string filePath) : this(configPath) => ReadFile(filePath);
        #endregion

        // ! METHODS
        #region methods
        /// <summary>
        /// Reads a PDF file and extracts its content.
        /// Liest eine PDF-Datei und extrahiert deren Inhalt.
        /// </summary>
        /// <param name="filePath">The file path to the PDF document. Der Dateipfad zur PDF-Datei.</param>
        public void ReadFile(string filePath)
        {
            using (PdfDocument pdfDoc = new(filePath)) _pdfDocString = GetPdfDocString(pdfDoc);
            _pdfDocEmptyRowsRemoved = GetPdfDocEmptyRowsRemoved(_pdfDocString);
            _pdfDocFieldsDelimited = GetPdfDocFieldsDelimited(_pdfDocEmptyRowsRemoved);
            Rows = GetRows(_pdfDocEmptyRowsRemoved);
            _fields = GetFields(_pdfDocFieldsDelimited);
            _indexedFields = GetIndexedFields(Rows, _fields);
            FieldTable = GetFieldTable(_indexedFields);
        }
        /// <summary>
        /// Method for uniformly extracting a PdfPageBase object as a string.
        /// Methode zum einheitlichen Extrahieren eines PdfPageBase-Objekts als Zeichenkette.
        /// </summary>
        /// <param name="pdfPage">The PDF page that should be extracted. Die PDF-Seite, die extrahiert werden soll.</param>
        /// <returns>The extracted string of the PDF page. Die extrahierte Zeichenkette der PDF-Seite.</returns>
        private string GetPdfPageString(PdfPageBase pdfPage)
        {
            // ! Determine the size of the PDF page
            int pdfWidth = (int)pdfPage.ActualSize.Width;
            int pdfHeight = (int)pdfPage.ActualSize.Height;
            // ! Instantiation of PdfTextExtractor class for extraction
            PdfTextExtractor pdfTextExtractor = new(pdfPage);
            // ! Instantiation of PdfTextExtractOptions class for configuring the extraction
            PdfTextExtractOptions extractionOptions = new();
            // ! Define the area for extraction
            extractionOptions.ExtractArea = new RectangleF(_config.CutLeft, _config.CutTop, pdfWidth -
                _config.CutRight, pdfHeight - _config.CutBottom);
            // ? Effect unknown / default = false
            //extractionOptions.IsExtractAllText = false;
            // ! Extract from PDF page as string
            string pdfPageStr = pdfTextExtractor.ExtractText(extractionOptions);
            // ! Encode string to UTF8
            pdfPageStr = Encoding.UTF8.GetString(Encoding.Convert(Encoding.Default, Encoding.UTF8,
                Encoding.Default.GetBytes(pdfPageStr)));
            // ! Mask characters used in regex
            pdfPageStr = _regexUtils.MaskRegexChars(pdfPageStr);
            // ! Remove Spire.PDF watermark
            pdfPageStr = pdfPageStr.Replace(
                "Evaluation Warning : The document was created with Spire.PDF for .NET.", "");
            return pdfPageStr;
        }
        /// <summary>
        /// Method for extracting a PdfDocument object as a string.
        /// Methode zum Extrahieren eines PdfDocument-Objekts als String.
        /// </summary>
        /// <param name="pdfDoc">The PDF document that should be extracted. Das PDF-Dokument, das extrahiert werden soll.</param>
        /// <returns>The extracted string from the PDF document. Die extrahierte Zeichenkette aus dem PDF-Dokument.</returns>
        private string GetPdfDocString(PdfDocument pdfDoc)
        {
            // ! Return string
            string pdfDocStr = "";
            // ! Processing PDF document page by page
            foreach (PdfPageBase pdfPage in pdfDoc.Pages) pdfDocStr += GetPdfPageString(pdfPage);
            // ! Return
            return pdfDocStr;
        }
        /// <summary>
        /// Method for removing empty rows.
        /// Methode zum Entfernen von leeren Zeilen.
        /// </summary>
        /// <param name="pdfDocStr">The string from which empty rows are to be removed. Die Zeichenkette, aus der leere Zeilen entfernt werden sollen.</param>
        /// <returns>The string with blank rows removed. Die Zeichenkette mit entfernten leeren Zeilen.</returns>
        private static string GetPdfDocEmptyRowsRemoved(string pdfDocStr)
        {
            // ! Return string
            string pdfDocEmptyRowsRemoved = pdfDocStr;
            // ! STRINGSTART-WHITESPACE zero or more-RETURN optional-NEWLINE OR 
            // ! RETURN optional-NEWLINE-WHITESPACE zero or more-STRINGEND
            string startEndEmptyRows = @"^\s*\r?\n|\r?\n\s*$";
            // ! Remove empty rows at start and end
            pdfDocEmptyRowsRemoved = Regex.Replace(pdfDocEmptyRowsRemoved, startEndEmptyRows, "");
            // ! (RETURN optional-NEWLINE){QUANTIFIER 2 or more}
            string doubleRowBreaks = @"(\r?\n){2,}";
            // ! Remove double row breaks
            pdfDocEmptyRowsRemoved = Regex.Replace(pdfDocEmptyRowsRemoved, doubleRowBreaks, "\r\n");
            // ! (RETURN optional-NEWLINE-WHITESPACE zero or more-RETURN optional-NEWLINE)
            string innerEmptyRows = @"(\r?\n\s*\r?\n)";
            // ! Remove remaining inner empty rows
            pdfDocEmptyRowsRemoved = Regex.Replace(pdfDocEmptyRowsRemoved, innerEmptyRows, "\r\n");
            // ! Return
            return pdfDocEmptyRowsRemoved;
        }
        /// <summary>
        /// Method for separating fields by delimiters.
        /// Methode zum Separieren von Feldern durch Delimiter.
        /// </summary>
        /// <param name="pdfDocEmptyRowsRemoved">The string from which fields are to be separated by delimiters. Die Zeichenkette, aus der Felder durch Delimiter separiert werden sollen.</param>
        /// <returns>The string with separated fields. Die Zeichenkette mit separierten Feldern.</returns>
        private string GetPdfDocFieldsDelimited(string pdfDocEmptyRowsRemoved)
        {
            // ! Return string
            string pdfDocFieldsDelimited = pdfDocEmptyRowsRemoved;
            // ! Empty "rooms" are replaced with "\u0000\t\t\u0000"
            pdfDocFieldsDelimited = Regex.Replace(pdfDocFieldsDelimited,
                // ! (NON-WHITESPACE)(SPACE{QUANTIFIER MaxWhiteSpaces}), backreference capture group 1 => \u0000\t\t\u0000
                @"(\S)( {" + _config.MaxWhiteSpaces + @"})", "$1\u0000\t\t\u0000");
            pdfDocFieldsDelimited = Regex.Replace(pdfDocFieldsDelimited, @"^", "\u0000\t\t\u0000");
            pdfDocFieldsDelimited = Regex.Replace(pdfDocFieldsDelimited, @"$", "\u0000\t\t\u0000");
            pdfDocFieldsDelimited = Regex.Replace(pdfDocFieldsDelimited, @"\r\n",
                "\u0000\t\t\u0000\r\n\u0000\t\t\u0000");
            while (pdfDocFieldsDelimited.Contains("\u0000\t\t\u0000 ")) pdfDocFieldsDelimited = pdfDocFieldsDelimited
                    .Replace("\u0000\t\t\u0000 ", "\u0000\t\t\u0000");
            while (pdfDocFieldsDelimited.Contains(" \u0000\t\t\u0000")) pdfDocFieldsDelimited = pdfDocFieldsDelimited
                    .Replace(" \u0000\t\t\u0000", "\u0000\t\t\u0000");
            pdfDocFieldsDelimited = pdfDocFieldsDelimited.Replace("\u0000\t\t\u0000\u0000\t\t\u0000",
                "\u0000\t\t\u0000");
            // ! Return
            return pdfDocFieldsDelimited;
        }
        /// <summary>
        /// Method to capture rows in a Dictionary(int, string) for position dependent comparisons.
        /// Methode um Zeilen in einem Dictionary(int, string) für positionsabhängige Vergleiche zu erfassen.
        /// </summary>
        /// <param name="pdfDocEmptyRowsRemoved">The string from which rows are to be captured. Die Zeichenkette, aus der Zeilen erfasst werden sollen.</param>
        /// <returns>The dictionary with the captured rows and their indexes. Das Wörterbuch mit den erfassten Zeilen und ihren Indexen.</returns>
        private Dictionary<int, string> GetRows(string pdfDocEmptyRowsRemoved)
        {
            // ! Return Dictionary
            Dictionary<int, string> rows = new();
            // ! Split rows to List
            List<string> splitRows = pdfDocEmptyRowsRemoved.Split("\r\n").ToList();
            // ! Remove row breaks
            for (int i = 0; i < splitRows.Count; i++) splitRows[i] = splitRows[i].Replace("\r\n", "");
            // ! Add rows to Dictionary with an index
            for (int i = 0; i < splitRows.Count; i++)
            {
                // ! Buffer corresponding to RangeBuffer is inserted at STRINGSTART
                rows[i] = Regex.Replace(splitRows[i], @"^", new string(' ', _config.RangeBuffer));
            }
            return rows;
        }
        /// <summary>
        /// Method to capture separated fields per row in a Dictionary(int, List(string)).
        /// Methode um separierte Felder je Zeile in einem Dictionary(int, List(string)) zu erfassen.
        /// </summary>
        /// <param name="pdfDocFieldsDelimited">The string from which separated fields are to be captured. Die Zeichenkette, aus der separierte Felder erfasst werden sollen.</param>
        /// <returns>The dictionary with the captured fields per row. Das Dictionary mit den erfassten Feldern pro Zeile.</returns>
        private static Dictionary<int, List<string>> GetFields(string pdfDocFieldsDelimited)
        {
            // ! Return Dictionary
            Dictionary<int, List<string>> fields = new();
            // ! Split rows to List
            List<string> rows = pdfDocFieldsDelimited.Split("\r\n").ToList();
            // ! Remove row breaks
            for (int i = 0; i < rows.Count; i++) rows[i] = rows[i].Replace("\r\n", "");
            // ! Provide rows with an index and split fields
            for (int i = 0; i < rows.Count; i++)
            {
                if (!fields.ContainsKey(i)) fields.Add(i, new List<string>());
                fields[i] = rows[i].Split("\u0000\t\t\u0000").ToList();
                fields[i].RemoveAll(string.IsNullOrEmpty);
            }
            return fields;
        }
        /// <summary>
        /// Method to index start and end of fields.
        /// Methode um Start und Ende von Feldern zu indexieren.
        /// </summary>
        /// <param name="rows">The dictionary with captured lines and their indexes. Das Dictionary mit erfassten Zeilen und ihren Indexen.</param>
        /// <param name="fields">The dictionary with recorded fields per line. Das Dictionary mit erfassten Feldern je Zeile.</param>
        /// <returns>The dictionary with indexed fields. Das Dictionary mit indexierten Feldern.</returns>
        private static Dictionary<int, List<(string, int, int)>> GetIndexedFields(Dictionary<int, string> rows,
            Dictionary<int, List<string>> fields)
        {
            // ! Return Dictionary
            Dictionary<int, List<(string, int, int)>> indexedFields = new();
            // ! Iterate through rows
            for (int i = 0; i < fields.Count; i++)
            {
                // ! Needed to move search area
                int nextIndex = 0;
                // ! Iterate through fields
                foreach (string field in fields[i])
                {
                    // ! Create key if not exists
                    if (!indexedFields.ContainsKey(i)) indexedFields[i] = new List<(string, int, int)>();
                    // ! Field (with start and end index) is stored or searched for after index of last field
                    indexedFields[i].Add((field, rows[i].IndexOf(field, nextIndex), rows[i].IndexOf(field, nextIndex) +
                        field.Length));
                    // ! Move search area
                    nextIndex = rows[i].IndexOf(field) + field.Length + 1;
                }
            }
            return indexedFields;
        }
        /// <summary>
        /// Transforms the indexedFields Dictionary into a DataTable object.
        /// Wandelt das indexedFields-Dictionary in ein DataTable-Objekt um.
        /// </summary>
        /// <param name="indexedFields">The dictionary with indexed fields. Das Dictionary mit indexierten Feldern.</param>
        /// <returns>The DataTable object that contains the indexed fields. Das DataTable-Objekt welches die indexierten Felder enthält.</returns>
        private static DataTable GetFieldTable(Dictionary<int, List<(string, int, int)>> indexedFields)
        {
            // ! Return DataTable
            DataTable dataTable = new();
            // ! Rowindex
            dataTable.Columns.Add("Row", typeof(int));
            // ! Startindex
            dataTable.Columns.Add("Column", typeof(int));
            // ! Value
            dataTable.Columns.Add("Value", typeof(string));
            // ! Endindex
            dataTable.Columns.Add("End", typeof(int));
            // ! Iterate through rows
            for (int i = 0; i < indexedFields.Count; i++)
            {
                // ! Iterate through columns
                foreach ((string, int, int) entry in indexedFields[i]) dataTable.Rows.Add(i, entry.Item2, entry.Item1,
                    entry.Item3);
            }
            // ! Convert DataTable to DataView
            DataView dataView = dataTable.DefaultView;
            // ! Sort in DataView
            dataView.Sort = "Row ASC, Column ASC";
            // ! Convert back to DataTable
            return dataView.ToTable();
        }
        /// <summary>
        /// Reports the result of the method to the console for debugging purpose.
        /// Meldet das Ergebnis der Methode zu Debugging-Zwecken an die Konsole.
        /// </summary>
        public void CONSOLE_pdfDocString()
        {
            Console.WriteLine("CONSOLE_pdfDocString:");
            Console.WriteLine("#####################\r\n");
            Console.WriteLine(_pdfDocString);
            Console.ReadLine();
        }
        /// <summary>
        /// Reports the result of the method to the console for debugging purpose.
        /// Meldet das Ergebnis der Methode zu Debugging-Zwecken an die Konsole.
        /// </summary>
        public void CONSOLE_pdfDocEmptyRowsRemoved()
        {
            Console.WriteLine("CONSOLE_pdfDocEmptyRowsRemoved:");
            Console.WriteLine("###############################\r\n");
            Console.WriteLine(_pdfDocEmptyRowsRemoved);
            Console.ReadLine();
        }
        /// <summary>
        /// Reports the result of the method to the console for debugging purpose.
        /// Meldet das Ergebnis der Methode zu Debugging-Zwecken an die Konsole.
        /// </summary>
        public void CONSOLE_pdfDocFieldsDelimited()
        {
            Console.WriteLine("CONSOLE_pdfDocFieldsDelimited:");
            Console.WriteLine("##############################\r\n");
            Console.WriteLine(_pdfDocFieldsDelimited);
            Console.ReadLine();
        }
        /// <summary>
        /// Reports the result of the method to the console for debugging purpose.
        /// Meldet das Ergebnis der Methode zu Debugging-Zwecken an die Konsole.
        /// </summary>
        public void CONSOLE_Rows()
        {
            Console.WriteLine("CONSOLE_Rows:");
            Console.WriteLine("############\r\n");
            foreach (KeyValuePair<int, string> row in Rows) Console.WriteLine("ROW " + row.Key + "° \t^" + row.Value
                + "°");
            Console.ReadLine();
        }
        /// <summary>
        /// Reports the result of the method to the console for debugging purpose.
        /// Meldet das Ergebnis der Methode zu Debugging-Zwecken an die Konsole.
        /// </summary>
        public void CONSOLE_fields()
        {
            Console.WriteLine("CONSOLE_fields:");
            Console.WriteLine("###############\r\n");
            foreach (KeyValuePair<int, List<string>> row in _fields)
            {
                Console.Write("ROW " + row.Key);
                foreach (string field in row.Value) Console.Write("° \t^" + field);
                Console.WriteLine("°");
            }
            Console.ReadLine();
        }
        /// <summary>
        /// Reports the result of the method to the console for debugging purpose.
        /// Meldet das Ergebnis der Methode zu Debugging-Zwecken an die Konsole.
        /// </summary>
        public void CONSOLE_indexedFields()
        {
            Console.WriteLine("CONSOLE_indexedFields:");
            Console.WriteLine("######################\r\n");
            foreach (KeyValuePair<int, List<(string, int, int)>> row in _indexedFields)
            {
                Console.Write("ROW " + row.Key + "° \t");
                foreach ((string, int, int) field in row.Value) Console.Write("^" + field.Item1 + "° <" + field.Item2 +
                    "-" + field.Item3 + "> \t");
                Console.WriteLine();
            }
            Console.ReadLine();
        }
        /// <summary>
        /// Reports the result of the method to the console for debugging purpose.
        /// Meldet das Ergebnis der Methode zu Debugging-Zwecken an die Konsole.
        /// </summary>
        public void CONSOLE_FieldTable()
        {
            Console.WriteLine("CONSOLE_FieldTable:");
            Console.WriteLine("###################\r\n");
            foreach (DataRow row in FieldTable.Rows)
            {
                int count = 0;
                foreach (var item in row.ItemArray)
                {
                    if (count == 3) Console.Write("\r\n\t°" + item + "°");
                    else if (count == 2) Console.Write("°" + item + "°");
                    else Console.Write("°" + item + "° \t");
                    count++;
                }
                Console.WriteLine();
            }
            Console.ReadLine();
        }
        #endregion
    }
}