using System;
using Microsoft.Extensions.Configuration;
using Qooba.Framework.Configuration.Abstractions;

namespace Qooba.Framework.Configuration
{
    public class Config : IConfig
    {
        private IConfigurationRoot configuration;

        public Config(IConfigurationRoot configuration)
        {
            this.configuration = configuration;
        }

        public string ConnectionString
        {
            get
            {
                return this.configuration["Data:DefaultConnection:ConnectionString"];
            }
        }

        public string StorageConnectionString
        {
            get
            {
                return this.configuration["Data:DefaultConnection:StorageConnectionString"];
            }
        }

        public string SearchApiKey
        {
            get
            {
                return this.configuration["Data:DefaultConnection:SearchApiKey"];
            }
        }

        public string SearchServiceName
        {
            get
            {
                return this.configuration["Data:DefaultConnection:SearchServiceName"];
            }
        }

        public string DocumentDbUri
        {
            get
            {
                return this.configuration["Data:DefaultConnection:DocumentDbUri"];
            }
        }

        public string DocumentDbPrimaryKey
        {
            get
            {
                return this.configuration["Data:DefaultConnection:DocumentDbPrimaryKey"];
            }
        }

        public string DocumentDbDatabaseName
        {
            get
            {
                return this.configuration["Data:DefaultConnection:DocumentDbDatabaseName"];
            }
        }

        public string DocumentDbCollectionName
        {
            get
            {
                return this.configuration["Data:DefaultConnection:DocumentDbCollectionName"];
            }
        }

        public string this[string key]
        {
            get
            {
                return this.configuration[key];
            }
        }
    }
}
