using Dapper;
using ExpanseManagerDBLibrary.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;

namespace ExpanseManagerDBLibrary
{
    public static class SqliteDataAccess
    {
        public static string LoadConnectionString(string id = "Database")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }
    }
}
