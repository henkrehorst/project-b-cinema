using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Linq;
using System.IO;

namespace bioscoop_app.Model
{
    /// <summary>
    /// Abstract class that is used for polymorphism and to store the id of Types that are in the database.
    /// </summary>
    public abstract class DataType : CustomObject
    {
        protected int id;

        /// <summary>
        /// The id by which you can fetch the instance from the database.
        /// </summary>
        public int Id { 
            get 
            {
                return id; 
            } 
            set 
            {
                if (value <= 0)
                {
                    throw new InvalidDataException("DataType.Id can't be zero or negative");
                }
                id = value;
            } 
        }
	}
}
