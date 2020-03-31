using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using bioscoop_app.Model;
using bioscoop_app.Service;

namespace bioscoop_app.Repository
{
    public abstract class Repository<T> where T : DataType
    {
        protected Dictionary<int, T> Data { get { return this.Data; } set { throw new InvalidOperationException(); } }
        protected bool IsOpen { get { return this.IsOpen; } set { throw new InvalidOperationException(); } }
        protected const string FileName = "";
        protected Repository()
        {
            Data = JsonConvert.DeserializeObject<Dictionary<int, T>>(
                File.ReadAllText(StorageService.GetDataSourcePath() + FileName)
            );
            IsOpen = true;
        }
        public static void SetupDataSource()
        {
            if (!File.Exists(StorageService.GetDataSourcePath() + FileName))
            {
                File.WriteAllText(
                    StorageService.GetDataSourcePath() + FileName,
                    JsonConvert.SerializeObject(new Dictionary<int, T>())
                );
            }
        }

        public void Add(T entry)
        {
            if (!IsOpen)
            {
                throw new System.InvalidOperationException();
            }
            if (!Data.Any())
            {
                entry.id = 0;
            }
            else
            {
                entry.id = Data.Keys.Max() + 1;
            }
            this.Data.Add(entry.id, entry);
        }
    }
}
