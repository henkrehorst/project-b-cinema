using System;
using System.Collections.Generic;
using System.Text;
using bioscoop_app.Model;
using System.Linq;

namespace bioscoop_app.Repository
{
    /// <summary>
    /// Repository that acts on Product.
    /// </summary>
    class ProductRepository : Repository<Product>
    {
        /// <param name="productType">The type of the product</param>
        /// <returns>A dictionary of Products associated with the product type</returns>
        public Dictionary<int, Product> GetProductsByType(string productType)
        {
            return Data.Where(item =>
                    item.Value.type.Equals(productType))
                .ToDictionary(item => item.Key, item => item.Value);
        }
    }
}