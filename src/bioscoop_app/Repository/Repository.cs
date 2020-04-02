﻿using System;
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
        protected const string FileExtension = ".json";
        protected Dictionary<int, T> data;
        protected bool isOpen = false;

        public Dictionary<int, T> Data
        {
            get
            {
                return IsOpen ? this.data : throw new InvalidOperationException();
            }
            set
            {
                throw new InvalidOperationException();
            }
        }
        public bool IsOpen { get { return this.isOpen; } set { throw new InvalidOperationException(); } }

        protected Repository()
        {
            data = JsonConvert.DeserializeObject<Dictionary<int, T>>(
                File.ReadAllText(StorageService.GetDataSourcePath() + typeof(T).Name + FileExtension)
            );
            isOpen = true;
        }
        public static void SetupDataSource()
        {
            if (!File.Exists(StorageService.GetDataSourcePath() + typeof(T).Name + FileExtension ))
            {
                File.WriteAllText(
                    StorageService.GetDataSourcePath() + typeof(T).Name + FileExtension,
                    JsonConvert.SerializeObject(new Dictionary<int, T>())
                );
            }
        }

        public void Add(T entry)
        {
            if (!isOpen)
            {
                throw new System.InvalidOperationException();
            }
            if (!data.Any())
            {
                entry.id = 0;
            }
            else
            {
                entry.id = data.Keys.Max() + 1;
            }
            data.Add(entry.id, entry);
        }
        
        public void SaveChanges()
        {
            File.WriteAllText(
                StorageService.GetDataSourcePath() + typeof(T).Name + FileExtension,
                JsonConvert.SerializeObject(Data)
            );
        }

        public void Discard()
        {
            isOpen = false;
        }

        public void SaveChangesAndDiscard()
        {
            SaveChanges();
            Discard();
        }

        public void AddAndWrite(T entry)
        {
            Add(entry);
            SaveChanges();
        }
    }
}