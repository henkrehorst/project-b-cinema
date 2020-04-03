﻿using System;
using System.Collections.Generic;
using System.Text;
using bioscoop_app.Model;
using System.Linq;

namespace bioscoop_app.Repository
{
    class ProductRepository : Repository<Product>
    {
        public List<Product> Query(Product signature, int limit)
        {
            List<Product> resultSet = new List<Product>();
            if(signature.GetType() == typeof(Ticket))
            {
                List<Ticket> subset = data.Values.OfType<Ticket>().ToList();
                Ticket ticketSignature = (Ticket)signature;
                foreach (Ticket entry in subset)
                {
                    if(ticketSignature.name is null || ticketSignature.name.Equals(entry.name))
                    {
                        if(ticketSignature.price is null || ticketSignature.price == entry.price)
                        {
                            if(ticketSignature.screenTime is null || ticketSignature.screenTime.Equals(entry.screenTime))
                            {
                                if(ticketSignature.seat is null || ticketSignature.seat.Equals(entry.seat))
                                {
                                    if(ticketSignature.visitorAge is null || ticketSignature.visitorAge == entry.visitorAge)
                                    {
                                        resultSet.Add(entry);
                                        if(resultSet.Count >= limit)
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }else if(signature.GetType() == typeof(Product))
            {
                foreach (Product entry in data.Values)
                {
                    if(signature.name is null || signature.name.Equals(entry.name))
                    {
                        if(signature.price is null || signature.price == entry.price)
                        {
                            resultSet.Add(entry);
                            if(resultSet.Count() >= limit)
                            {
                                break;
                            }
                        }
                    }
                }
            }
            return resultSet;
        }

        public List<Product> QueryFirst(Product signature)
        {
            return Query(signature, 1);
        }

        public List<Product> UnlimitedQuery(Product signature)
        {
            return Query(signature, -1);
        }
    }
}
