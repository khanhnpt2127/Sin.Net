﻿using Sin.Net.Domain.IO;
using Sin.Net.Domain.IO.Adapter;
using Sin.Net.Domain.IO.Settings;
using Sin.Net.Domain.Logging;
using Sin.Net.Persistence.IO;
using Sin.Net.Persistence.Settings;
using System.IO;

namespace Sin.Net.Persistence.Exports
{
    // TODO: make english comments

    /// <summary>
    /// Die Klasse implementiert die IExportable Schnittstelle zum Exportieren von Objekten in das Json-Format.
    /// </summary>
    internal class JsonExporter : IExportable
    {

        // -- fields

        private FileSetting _setting;
        private object _exportData;

        // -- constructors

        public JsonExporter()
        {

        }

        // -- methods

        public IExportable Setup(SettingsBase setting)
        {
            if (setting is FileSetting)
            {
                _setting = setting as FileSetting;
            }
            else
            {
                Log.Error("The json export setting has the wrong type and was not accepted.");
            }
            return this;
        }

        public IExportable Build<T>(T data)
        {
            return Build<T>(data, null);
        }

        public IExportable Build<T>(T data, IAdaptable adapter)
        {
            if (adapter != null)
            {
                _exportData = adapter.Adapt<T, object>(data);
            }
            else
            {
                _exportData = data;
            }

            return this;
        }

        /// <summary>
        /// Führt die Kernfunktion des Exports aus und gibt bei erfolgreichen Export den kompletten Pfad aus. andernfalls null.
        /// </summary>
        /// <returns></returns>
        public string Export()
        {
            string file = _setting.FullPath;
            string result = null;

            if (!Directory.Exists(_setting.Location))
            {
                Directory.CreateDirectory(_setting.Location);
            }

            if (JsonIO.SaveToJson(_exportData, file))
            {
                result = file;
            }

            // done!
            Log.Info($"json export successfull for '{result}'");
            return result;
        }

        // -- properties

        public string Type => Persistence.Constants.Json.Key;
    }
}