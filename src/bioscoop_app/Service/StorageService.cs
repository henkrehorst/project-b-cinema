using System;
using System.Collections.Generic;
using System.IO;
using bioscoop_app.Model;
using bioscoop_app.Repository;

namespace bioscoop_app.Service
{
    public class StorageService
    {
        public static void SetupStorageFiles()
        {
            MovieRepository.SetupDataSource();
        }

        public static string GetDataSourcePath()
        {
            string projectPath =
                Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory()));

            return $"{projectPath}\\data\\";
        }
    }
}