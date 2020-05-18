using System;
using Newtonsoft.Json;

namespace bioscoop_app.Model
{
    public sealed class Reservering : DataType
    {
        public string naam;
        //TODO: make properties later available
        // private Kijkwijzer[] kijkwijzers { get; set; }
        // public Enum dimension { get; set; }
        public string email;
        public string producten;

        public Reservering(string naam, string email, string producten) {
            this.naam = naam;
            this.email = email;
            this.producten = producten;
        }
        
        [JsonConstructor]
        public Reservering(int id, string naam, string email, string producten) {
            this.Id = id;
            this.naam = naam;
            this.email = email;
            this.producten = producten;
           
        }
    }
}