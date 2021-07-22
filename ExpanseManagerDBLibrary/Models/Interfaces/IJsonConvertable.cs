using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpanseManagerDBLibrary.Models
{
    public interface IJsonConvertable
    {
        string ToJSON(Formatting formatting);
    }
}
