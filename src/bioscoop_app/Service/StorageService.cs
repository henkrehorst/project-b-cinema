using System;
using System.Collections.Generic;
using System.IO;
using bioscoop_app.Model;

namespace bioscoop_app.Service
{
    public class StorageService
    {
        public void SetupStorageFiles()
        {
            
        }

        public static string GetDataSourcePath()
        {
            string projectPath =
                Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory()));

            return $"{projectPath}\\data\\";
        }
    }
}