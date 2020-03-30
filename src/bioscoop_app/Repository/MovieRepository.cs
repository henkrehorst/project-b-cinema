using System;
using System.Collections.Generic;
using System.IO;
using bioscoop_app.Model;
using bioscoop_app.Service;

namespace bioscoop_app.Repository
{
    public class MovieRepository
    {
        private const string FileName = "movies.json";
        
        public static void SetupDataSource()
        {
            if (!File.Exists(StorageService.GetDataSourcePath() + FileName))
            {
                File.Create(StorageService.GetDataSourcePath() + FileName);
            }
        }
    }
}