using ArquiteturaDesafio.Core.Domain.Entities;
using ArquiteturaDesafio.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArquiteturaDesafio.Core.Domain.Enum;

namespace ArquiteturaDesafio.Core.Domain.Entities
{
    public class Order : BaseEntity
    {
        public Guid CustomerId { get; set; }  // Identidade Externa do Cliente
        public DateTime OrderDate { get; set; } // Data da venda
        public List<OrderItem> Items { get; set; } // Relacionamento com itens da venda

        public OrderStatus Status { get; set; } = OrderStatus.Active;// Status da venda

        public decimal TotalAmount
        {
            get
            {
                return Items?.Sum(item => item.TotalPrice) ?? 0;
            }
            private set { /* Necessário para o Entity Framework */ }
        }

        public void Update(Order request, Customer customer)
        {
            OrderDate = request.OrderDate;
            CustomerId = customer.Id;
        }
       
        public void ClearItems()
        {
            Items = new List<OrderItem>();
        }

        public void UpdateItems(IEnumerable<OrderItem> items, IEnumerable<Product> productsUsed)
        {
            foreach (var itemDto in items)
            {
                var itemToUpdate = Items.FirstOrDefault(i => i.Id == itemDto.Id);
                if (itemToUpdate is null)
                {
                    throw new ArgumentNullException($"Item {itemDto.Id} not found");
                }

                var product = productsUsed.FirstOrDefault(p => p.Id == itemDto.ProductId);

                if(product is null)
                {
                    throw new ArgumentNullException($"Product {itemDto.ProductId} not found");
                }

                itemToUpdate.ProductId = itemDto.ProductId;
                itemToUpdate.Name = product.Name;
                itemToUpdate.Quantity = itemDto.Quantity;
                itemToUpdate.UnitPrice = product.Price;
            }
        }

        public void AddItems(IEnumerable<OrderItem> items, IEnumerable<Product> productsUsed)
        {
            Items = Items ?? new List<OrderItem>();
            foreach (var item in items)
            {
                var product = productsUsed.FirstOrDefault(p => p.Id == item.ProductId);
                if (product is null)
                {
                    throw new ArgumentNullException($"Product {item.ProductId} not found");
                }

                // Se o item não existir, pode ser adicionado, se necessário
                Items.Add(new OrderItem(item.OrderId, item.ProductId, product.Name, item.Quantity, product.Price));
            }
        }


        public void VerifyIfAllItensAreMine(List<Guid> idsItensToVerify)
        {
            idsItensToVerify = idsItensToVerify ?? new List<Guid>();
            
            var itensNotMine = Items.Where(x => !idsItensToVerify.Contains(x.Id)).ToList();

            if(itensNotMine.Any())
            {
                throw new InvalidOperationException("Some itens are not from this sale");
            }
        }
    }
}
