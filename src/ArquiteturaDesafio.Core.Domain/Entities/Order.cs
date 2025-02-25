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

        public void Update(DateTime orderDate, Customer customer)
        {
            OrderDate = orderDate;
            CustomerId = customer.Id;
        }

        public void Update(DateTime orderDate, Customer customer, OrderStatus status)
        {
            OrderDate = orderDate;
            CustomerId = customer.Id;
            Status = status;
        }

        public void ClearItems()
        {
            Items = new List<OrderItem>();
        }

        public void RemoveItems(List<Guid> idsToRemove)
        {
            Items = Items ?? new List<OrderItem>();
            Items = Items.Where(x => !idsToRemove.Contains(x.Id)).ToList();
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
            idsItensToVerify = idsItensToVerify.Where(x => x != Guid.Empty).ToList();// Somente os com ID associado
            var itensFromAtualOrder = Items.Select(x => x.Id).ToList();
            var itensNotMine = idsItensToVerify.Where(x => !itensFromAtualOrder.Contains(x)).ToList();

            if(itensNotMine.Any())
            {
                throw new InvalidOperationException("Some itens are not from this sale");
            }
        }
    }
}
