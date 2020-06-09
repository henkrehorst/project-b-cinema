using System;
using Chromely.Core.Network;
using bioscoop_app.Model;
using bioscoop_app.Repository;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using bioscoop_app.Helper;
using System.Linq;
using FluentValidation;

namespace bioscoop_app.Controller
{
    /// <summary>
    /// Controller that handles the routes related to Product.
    /// </summary>
    public class ProductController : ChromelyController
    {
        /// <summary>
        /// Retrieves all products from the Data file. Excludes Tickets.
        /// </summary>
        /// <param name="req">http GET request</param>
        /// <returns>Products from the Data file, excluding Tickets.</returns>
        [HttpGet(Route = "/products")]
        public ChromelyResponse GetProducts(ChromelyRequest req)
        {
            Dictionary<int, Product> rawData;
            try
            {
                rawData = new Repository<Product>().Data;
            } catch (InvalidOperationException)
            {
                return Response.TransactionProtocolViolation(req.Id);
            }
            List<Product> data = new List<Product>();
            foreach (Product prod in rawData.Values)
            {
                if (prod.GetType() == typeof(Product)) data.Add(prod);
            }
            return new Response
            {
                status = 200,
                data = JsonConvert.SerializeObject(data)
            }.ChromelyWrapper(req.Id);
        }

        /// <summary>
        /// Get products by type
        /// </summary>
        /// <param name="req">http POST request containing the type</param>
        /// <returns>The products associated by the posted type</returns>
        [HttpPost(Route = "/product#type")]
        public ChromelyResponse GetProductByType(ChromelyRequest req)
        {
            JObject data = (JObject)JsonConvert.DeserializeObject(req.PostData.ToJson());

            string type;
            try
            {
                type = data.Value<string>("type");
            } catch (FormatException)
            {
                return Response.ParseError(req.Id);
            }
            return new Response
            {
                data = JsonConvert.SerializeObject(GetProductsByType(type)),
                status = 200
            }.ChromelyWrapper(req.Id);
        }

        /// <summary>
        /// Adds the specified Product to the data file.
        /// </summary>
        /// <param name="req">http POST request containing the Product data</param>
        /// <returns>Status 204</returns>
        [HttpPost(Route = "/products#add")]
        public ChromelyResponse AddProduct(ChromelyRequest req)
        {
            JObject data = (JObject)JsonConvert.DeserializeObject(req.PostData.ToJson());
            //Console.WriteLine(data);
            try
            {
                new Repository<Product>().AddThenWrite(new Product(
                    data["price"].Value<double>(),
                    data["name"].Value<string>(),
                    data["type"].Value<string>()
                ));
            } catch (FormatException)
            {
                return Response.ParseError(req.Id);
            } catch (InvalidOperationException)
            {
                return Response.TransactionProtocolViolation(req.Id);
            } catch (ValidationException exception)
            {
                return Response.ValidationError(req.Id, exception);
            }
            return new Response
            {
                status = 204
            }.ChromelyWrapper(req.Id);
        }

        /// <summary>
        /// Updates Product associated with the specified id in the data file.
        /// </summary>
        /// <param name="req">http POST request containing the key and value data.</param>
        /// <returns>Status 204 on succes, Status 400 on failure</returns>
        [HttpPost(Route = "/products#update")]
        public ChromelyResponse UpdateProduct(ChromelyRequest req)
        {
            JObject data = (JObject)JsonConvert.DeserializeObject(req.PostData.ToJson());
            int id;
            try
            {
                id = data.Value<int>("Id");
            } catch (FormatException)
            {
                return Response.ParseError(req.Id);
            }

            Repository<Product> repository = new Repository<Product>();
            try
            {
                repository.Update(id, ToProduct(data));
                repository.SaveChanges();
            } catch (InvalidOperationException exception)
            {
                if (exception.Message is object && exception.Message != "")
                {
                    return Response.IllegalUpdate(req.Id, exception.Message);
                } else
                {
                    return Response.TransactionProtocolViolation(req.Id);
                }
            } catch (ValidationException exception)
            {
                return Response.ValidationError(req.Id, exception);
            } catch (FormatException)
            {
                return Response.ParseError(req.Id);
            }
            return new Response
            {
                status = 204
            }.ChromelyWrapper(req.Id);
        }

        /// <param name="data">Product as a JObject</param>
        /// <returns>Product as a Product or Ticket</returns>
        /// <exception cref="FormatException">If the data is in incorrect format.</exception>
        public static Product ToProduct(JObject data)
        {
            if (data.ContainsKey("seatnr") && data.ContainsKey("row") && data.ContainsKey("screenTime") && data.ContainsKey("visitorAge"))
            {
                return new Ticket(
                    data["Id"].Value<int>(),
                    data["price"].Value<double>(),
                    data["name"].Value<string>(),
                    data["row"].Value<int>(),
                    data["seatnr"].Value<int>(),
                    data["screenTime"].Value<int>(),
                    data["visitorAge"].Value<int>()
                    );
            }
            else
            {
                return new Product(
                    data["Id"].Value<int>(),
                    data["price"].Value<double>(),
                    data["name"].Value<string>(),
                    data["type"].Value<string>()
                    );
            }
        }
        
        /// <param name="req">http POST request containing the desired Product id</param>
        /// <returns>The product associated with the id.</returns>
        [HttpPost(Route = "/product#id")]
        public ChromelyResponse GetMovieById(ChromelyRequest req)
        {
            int id;
            try
            {
               id = ((JObject)JsonConvert.DeserializeObject(req.PostData.ToJson())).Value<int>("id");
            } catch (FormatException)
            {
                return Response.ParseError(req.Id);
            }
            try
            {
                return new Response
                {
                    status = 200,
                    data = JsonConvert.SerializeObject(new Repository<Product>().Data[id])
                }.ChromelyWrapper(req.Id);
            } catch (InvalidOperationException)
            {
                return Response.TransactionProtocolViolation(req.Id);
            } catch (KeyNotFoundException)
            {
                return new Response
                {
                    status = 400,
                    statusText = $"No movie found for id {id}."
                }.ChromelyWrapper(req.Id);
            }
        }
        /// <param name="productType">The type of the product</param>
        /// <returns>A dictionary of Products associated with the product type</returns>
        private Dictionary<int, Product> GetProductsByType(string productType)
        {
            return new Repository<Product>().Data.Where(item =>
                    item.Value.type.Equals(productType))
                .ToDictionary(item => item.Key, item => item.Value);
        }

    }
}