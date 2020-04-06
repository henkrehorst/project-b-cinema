﻿using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace bioscoop_app.Service
{
    public sealed class StorageService
    {
        public static void SetupStorageFiles()
        {
            // Get list of repository classes
            var repositoryList = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.Namespace == "bioscoop_app.Repository")
                .ToList();

            // Call dynamic the SetupDataSource method for each repository class
            foreach (Type repository in repositoryList)
            {
                if (!repository.GetTypeInfo().IsAbstract)
                {
                    var setupMethod = repository.GetMethod("SetupDataSource",
                        BindingFlags.FlattenHierarchy
                        | BindingFlags.Public
                        | BindingFlags.Static
                    );
                    
                    if (setupMethod != null) setupMethod.Invoke(repository, null);
                }
            }
        }

        public static string GetDataSourcePath()
        {
            string projectPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\..\\..\\"));

            return $"{projectPath}\\data\\";
        }
    }
}