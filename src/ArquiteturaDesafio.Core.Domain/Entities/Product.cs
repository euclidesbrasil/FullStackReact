using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArquiteturaDesafio.Core.Domain.Common;

namespace ArquiteturaDesafio.Core.Domain.Entities
{
    public sealed class Product : BaseEntity
    {
        public string Name { get; private set; }
        public decimal Price { get; private set; }
       
        private Product() { } // Construtor privado para ORM

        public Product(string title, decimal price)
        {
            Name = title;
            Price = price;
           
        }

        public void Update(string title, decimal price)
        {
            Name = title;
            Price = price;
        }
    }
}