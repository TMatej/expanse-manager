using ExpanseManagerDBLibrary.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpanseManagerServiceLibrary.Services.Export
{
    public class DataExporterImpl<IJsonConvertable> : IDataExporter<IJsonConvertable>
    {
        public async Task<bool> StoreData(List<IJsonConvertable> data, string pathToFile)
        {
            var dataInJson = await Task.Run(() => JsonConvert.SerializeObject(data, Formatting.Indented));

            using var fs = new FileStream(pathToFile, FileMode.OpenOrCreate, FileAccess.Write);
            using var jsonWriter = new StreamWriter(fs);
            await Task.Run(() => jsonWriter.Write(dataInJson));
            return true;
        }
    }
}
