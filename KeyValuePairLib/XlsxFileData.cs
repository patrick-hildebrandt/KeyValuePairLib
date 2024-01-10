using System.Data;

namespace KeyValuePairLib
{
    /// <summary>
    /// Represents data extracted from a XLSX file.
    /// Stellt Daten dar, die aus einer XLSX-Datei extrahiert wurden.
    /// </summary>
    internal class XlsxFileData
    {
        // ! FIELDS
        #region fields
        #endregion

        // ! PROPERTIES
        #region properties
        /// <summary>
        /// DataField object of the detected fields in the XLSX file.
        /// DataField-Objekt der erkannten Felder in der XLSX-Datei.
        /// </summary>
        public DataTable? FieldTable { get; private set; }
        #endregion

        // ! CONSTRUCTORS
        #region constructors
        #endregion

        // ! METHODS
        #region methods
        #endregion
    }
}