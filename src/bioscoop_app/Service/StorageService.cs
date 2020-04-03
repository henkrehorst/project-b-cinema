using System;
using System.Collections.Generic;
using System.IO;
using bioscoop_app.Model;
using bioscoop_app.Repository;

namespace bioscoop_app.Service
{
    public sealed class StorageService
    {
        public static void SetupStorageFiles()
        {
            MovieRepository.SetupDataSource();
            ProductRepository.SetupDataSource();
        }

        public static string GetDataSourcePath()
        {
            string projectPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\..\\..\\"));

            return $"{projectPath}\\data\\";
        }
    }
}