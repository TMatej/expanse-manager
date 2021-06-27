using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpanseManagerServiceLibrary.Services.Export
{
    public interface IDataExporter<T>
    {
        Task<bool> StoreData(List<T> data, string pathToFile);
    }
}
