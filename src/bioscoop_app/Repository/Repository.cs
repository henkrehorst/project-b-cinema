using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using bioscoop_app.Model;
using bioscoop_app.Service;

namespace bioscoop_app.Repository
{
    /// <summary>
    /// Abstract class that contains base functionality for Repositories.
    /// </summary>
    /// <typeparam name="T">The Type this repository acts on. Must satisfy (T is DataType).</typeparam>
    public class Repository<T> where T : DataType
    {
        protected const string FileExtension = ".json";
        protected Dictionary<int, T> data;
        protected bool isOpen = false;

        /// <summary>
        /// The data this repository operates on.
        /// </summary>
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
        /// <summary>
        /// States whether the data is up to date and this repository can be acted on.
        /// Can't be modified from public context.
        /// </summary>
        public bool IsOpen { get { return this.isOpen; } set { throw new InvalidOperationException(); } }

        /// <summary>
        /// Initializes a repository with the data read from the corresponding data file.
        /// </summary>
        public Repository()
        {
            data = JsonConvert.DeserializeObject<Dictionary<int, T>>(
                File.ReadAllText(StorageService.GetDataSourcePath() + typeof(T).Name + FileExtension)
            );
            isOpen = true;
        }

        /// <summary>
        /// Static method to set up a storage file for the DataType this Repository acts on, if it does not exist yet.
        /// </summary>
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

        /// <summary>
        /// Adds the entry to the data sequence.
        /// </summary>
        /// <param name="entry">Instance to add to the data sequence</param>
        /// <exception cref="InvalidOperationException">Thrown when the repository is not open.</exception>
        public void Add(T entry)
        {
            if (!isOpen)
            {
                throw new InvalidOperationException();
            }
            if (!data.Any())
            {
                entry.Id = 1;
            }
            else
            {
                entry.Id = data.Keys.Max() + 1;
            }
            data.Add(entry.Id, entry);
        }

        /// <summary>
        /// Updates the entry on the specified id with the specified data.
        /// </summary>
        /// <param name="id">The key to the value</param>
        /// <param name="value">The data to replace the value with.</param>
        /// <exception cref="InvalidOperationException">Thrown when the id does not exist in the data sequence.</exception>
        public void Update(int id, T value)
        {
            if(!data.Keys.Contains(id)) {
                string msg = "Update should not be used on entries that are not yet stored in the data sequence";
                throw new InvalidOperationException(msg);
            }
            value.Id = id;
            data[id] = value;
        }

        /// <summary>
        /// Removes the key and it's value from the data sequence.
        /// </summary>
        /// <param name="id">The key to the key-value pair that is to be removed.</param>
        /// <returns>True iff the id was removed succesfully.
        /// False if the id did not exist or was not removed.</returns>
        public bool Remove(int id)
        {
            if (!data.ContainsKey(id)) return false;
            data.Remove(id);
            return data.ContainsKey(id);
        }
        
        /// <summary>
        /// Writes the current state of the data to the storage file.
        /// </summary>
        public void SaveChanges()
        {
            File.WriteAllText(
                StorageService.GetDataSourcePath() + typeof(T).Name + FileExtension,
                JsonConvert.SerializeObject(Data)
            );
        }

        /// <summary>
        /// Disables the repository.
        /// </summary>
        public void Discard()
        {
            isOpen = false;
        }

        /// <summary>
        /// Writes the current state of the data to the storage file, then disables the repository.
        /// </summary>
        public void SaveChangesThenDiscard()
        {
            SaveChanges();
            Discard();
        }

        /// <summary>
        /// Adds an entry to the data sequence, then writes the current state of the data to the storage file.
        /// </summary>
        public void AddThenWrite(T entry)
        {
            Add(entry);
            SaveChanges();
        }
    }
}
