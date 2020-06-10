using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using bioscoop_app.Model;
using bioscoop_app.Service;
using System.Reflection;
using FluentValidation;

namespace bioscoop_app.Repository
{
    /// <summary>
    /// Abstract class that contains base functionality for Repositories.
    /// </summary>
    /// <typeparam name="T">The Type this repository acts on. Must satisfy (T is DataType).</typeparam>
    public sealed class Repository<T> where T : DataType
    {
        private const string FileExtension = ".json";
        private readonly Dictionary<int, T> _data;
        private bool _isOpen = false;

        /// <summary>
        /// The data this repository operates on.
        /// </summary>
        /// <exception cref="InvalidOperationException">If the repository is closed.</exception>
        public Dictionary<int, T> Data
        {
            get
            {
                return IsOpen ? _data : throw new InvalidOperationException();
            }
        }
        /// <summary>
        /// States whether the data is up to date and this repository can be acted on.
        /// Can't be modified from public context.
        /// </summary>
        public bool IsOpen { get { return _isOpen; } set { throw new InvalidOperationException(); } }

        /// <summary>
        /// Initializes a repository with the data read from the corresponding data file.
        /// </summary>
        public Repository()
        {
            _data = JsonConvert.DeserializeObject<Dictionary<int, T>>(
                File.ReadAllText(StorageService.GetDataSourcePath() + typeof(T).Name + FileExtension)
            );
            _isOpen = true;
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
        /// <exception cref="ValidationException">Thrown when the new data does not match validator rules.</exception>
        public void Add(T entry)
        {
            if (!_isOpen)
            {
                throw new InvalidOperationException();
            }
            ValidateT(entry);
            if (!_data.Any())
            {
                entry.Id = 1;
            }
            else
            {
                entry.Id = _data.Keys.Max() + 1;
            }
            _data.Add(entry.Id, entry);
        }

        /// <summary>
        /// Updates the entry on the specified id with the specified data.
        /// </summary>
        /// <param name="id">The key to the value</param>
        /// <param name="value">The data to replace the value with.</param>
        /// <exception cref="InvalidOperationException">Thrown when the id does not exist in the data sequence, or the repository is closed.</exception>
        /// <exception cref="ValidationException">Thrown when the new data does not match validator rules.</exception>
        public void Update(int id, T value)
        {
            if (!_isOpen) throw new InvalidOperationException();
            if (!_data.Keys.Contains(id)) {
                string msg = "Update should not be used on entries that are not yet stored in the data sequence";
                throw new InvalidOperationException(msg);
            }
            ValidateT(value);
            value.Id = id;
            _data[id] = value;
        }

        /// <summary>
        /// Removes the key and it's value from the data sequence.
        /// </summary>
        /// <param name="id">The key to the key-value pair that is to be removed.</param>
        /// <returns>True iff the id was removed succesfully.
        /// False if the id did not exist or was not removed.</returns>
        public bool Remove(int id)
        {
            if (!_isOpen) throw new InvalidOperationException();
            if (!_data.ContainsKey(id)) return false;
            _data.Remove(id);
            return _data.ContainsKey(id);
        }
        
        /// <summary>
        /// Writes the current state of the data to the storage file.
        /// </summary>
        /// <exception cref="InvalidOperationException">When the Repository is closed.</exception>
        public void SaveChanges()
        {
            if (!_isOpen) throw new InvalidOperationException();
            File.WriteAllText(
                StorageService.GetDataSourcePath() + typeof(T).Name + FileExtension,
                JsonConvert.SerializeObject(_data)
            );
        }

        /// <summary>
        /// Disables the repository.
        /// </summary>
        public void Discard()
        {
            _isOpen = false;
        }

        /// <summary>
        /// Writes the current state of the data to the storage file, then disables the repository.
        /// </summary>
        /// <exception cref="InvalidOperationException">When the Repository is closed.</exception>
        public void SaveChangesThenDiscard()
        {
            SaveChanges();
            Discard();
        }

        /// <summary>
        /// Adds an entry to the data sequence, then writes the current state of the data to the storage file.
        /// </summary>
        /// <exception cref="InvalidOperationException">When the Repository is closed.</exception>
        public void AddThenWrite(T entry)
        {
            Add(entry);
            SaveChanges();
        }

        /// <summary>
        /// Dynamically initializes the type specific Validator for T, and validates obj.
        /// </summary>
        /// <param name="obj">The object to validate.</param>
        /// <exception cref="ValidationException">Thrown when the new data does not match validator rules.</exception>
        private void ValidateT(T obj)
        {
            string identifier = obj.GetType().Name + "Validator";
            Type type = Type.GetType(obj.GetType().Name + "Validator");
            ((AbstractValidator<T>)Type.GetType(obj.GetType().Name + "Validator")
                .GetConstructors()[0].Invoke(new object[0])).ValidateAndThrow(obj);
        }
    }
}
