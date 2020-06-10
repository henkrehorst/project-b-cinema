using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using bioscoop_app.Model;
using bioscoop_app.Repository;

namespace bioscoop_app.Service
{
    /// <summary>
    /// Service class that is used to configure data storage files.
    /// </summary>
    public static class StorageService
    {
        /// <summary>
        /// Creates data files with an empty JSON dictionary if the files don't exist yet.
        /// </summary>
        public static void SetupStorageFiles()
        {
            //Create directories
            CreateDataSourceDirectory();
            CreateUploadDirectory();

            //setup data sources
            Repository<Movie>.SetupDataSource();
            Repository<Product>.SetupDataSource();
            Repository<ScreenTime>.SetupDataSource();
            Repository<Order>.SetupDataSource();
            Repository<Kijkwijzer>.SetupDataSource();
        }

        /// <summary>
        /// Creates the folder that contains the data files, if it doesn't exist yet.
        /// </summary>
        private static void CreateDataSourceDirectory()
        {
            if (!Directory.Exists(GetDataSourcePath()))
            {
                Directory.CreateDirectory(GetDataSourcePath());
            }
        }

        /// <summary>
        /// Creates the folder that contains the uploaded images, if it doesn't exist yet.
        /// </summary>
        private static void CreateUploadDirectory()
        {
            if (!Directory.Exists(GetUploadPath()))
            {
                Directory.CreateDirectory(GetUploadPath());
            }
            
            
        }

        /// <summary>
        /// Retrieves the path of the data folder.
        /// </summary>
        /// <returns>path of the data folder</returns>
        public static string GetDataSourcePath()
        {
            string projectPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\"));

            return $"{projectPath}\\productionData\\";
        }

        /// <summary>
        /// Retrieves the path of the image upload folder.
        /// </summary>
        /// <returns>the path of the image upload folder</returns>
        public static string GetUploadPath()
        {
            return $"{AppDomain.CurrentDomain.BaseDirectory}\\frontend\\uploads\\";
        }
    }
}