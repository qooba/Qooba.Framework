using Qooba.Framework.Configuration.Abstractions;
using System.Configuration;
using System;

namespace Qooba.Framework.Configuration.AppConfiguration
{
    public class AppConfig : IConfig
    {
        public string ConnectionString
        {
            get
            {
                var connectionStrings = ConfigurationManager.ConnectionStrings;
                if(connectionStrings.Count > 0)
                {
                    return connectionStrings[0].ConnectionString;
                }

                return ConfigurationManager.AppSettings["Data:DefaultConnection:ConnectionString"];
            }
        }

        public string StorageConnectionString
        {
            get
            {
                return ConfigurationManager.AppSettings["Data:DefaultConnection:StorageConnectionString"];
            }
        }

        public string SearchApiKey
        {
            get
            {
                return ConfigurationManager.AppSettings["Data:DefaultConnection:SearchApiKey"];
            }
        }

        public string SearchServiceName
        {
            get
            {
                return ConfigurationManager.AppSettings["Data:DefaultConnection:SearchServiceName"];
            }
        }

        public string DocumentDbUri
        {
            get
            {
                return ConfigurationManager.AppSettings["Data:DefaultConnection:DocumentDbUri"];
            }
        }

        public string DocumentDbPrimaryKey
        {
            get
            {
                return ConfigurationManager.AppSettings["Data:DefaultConnection:DocumentDbPrimaryKey"];
            }
        }

        public string DocumentDbDatabaseName
        {
            get
            {
                return ConfigurationManager.AppSettings["Data:DefaultConnection:DocumentDbDatabaseName"];
            }
        }

        public string DocumentDbCollectionName
        {
            get
            {
                return ConfigurationManager.AppSettings["Data:DefaultConnection:DocumentDbCollectionName"];
            }
        }

        public string this[string key]
        {
            get
            {
                return ConfigurationManager.AppSettings[key];
            }
        }
    }
}
