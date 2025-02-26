using System;
using System.Collections.Generic;
using ArquiteturaDesafio.Core.Domain.Entities;
using ArquiteturaDesafio.Core.Domain.Enum;
using ArquiteturaDesafio.Core.Domain.Interfaces;
using ArquiteturaDesafio.Core.Domain.ValueObjects;
using NSubstitute;
using Xunit;
namespace ArquiteturaDesafio.Test.Domain
{
    public class OrderTests
    {
        [Fact]
        public void Update_ShouldSetOrderDateAndCustomerId()
        {
            // Arrange
            var order = new Order();
            var newDate = DateTime.UtcNow;
            var customer = new Customer { Id = Guid.NewGuid() };

            // Act
            order.Update(newDate, customer);

            // Assert
            Assert.Equal(newDate, order.OrderDate);
            Assert.Equal(customer.Id, order.CustomerId);
        }

        [Fact]
        public void Update_WithStatus_ShouldSetOrderDateCustomerIdAndStatus()
        {
            // Arrange
            var order = new Order();
            var newDate = DateTime.UtcNow;
            var customer = new Customer { Id = Guid.NewGuid() };
            var newStatus = OrderStatus.Canceled;

            // Act
            order.Update(newDate, customer, newStatus);

            // Assert
            Assert.Equal(newDate, order.OrderDate);
            Assert.Equal(customer.Id, order.CustomerId);
            Assert.Equal(newStatus, order.Status);
        }

        [Fact]
        public void ClearItems_ShouldEmptyTheItemsList()
        {
            // Arrange
            var order = new Order();
            order.Items = new List<OrderItem>
            {
                new OrderItem(Guid.NewGuid(), Guid.NewGuid(), "Product1", 2, 10),
                new OrderItem(Guid.NewGuid(), Guid.NewGuid(), "Product2", 1, 20)
            };

            // Act
            order.ClearItems();

            // Assert
            Assert.NotNull(order.Items);
            Assert.Empty(order.Items);
        }

        [Fact]
        public void RemoveItems_ShouldRemoveSpecifiedItems()
        {
            // Arrange
            var product = new Product("Nome", 1, Guid.NewGuid());
            var order = new Order();
            var item1 = new OrderItem(Guid.NewGuid(), product.Id, "Nome", 2, 1, Guid.NewGuid());
            var item2 = new OrderItem(Guid.NewGuid(), product.Id, "Nome", 1, 1, Guid.NewGuid());
            var item3 = new OrderItem(Guid.NewGuid(), product.Id, "Nome", 3, 1, Guid.NewGuid());
            var allItens = new List<OrderItem> { item1, item2, item3 };
            order.AddItems(allItens, new List<Product>() { product });
            // Act
            order.RemoveItems(new List<Guid> { item2.Id });

            // Assert
            Assert.DoesNotContain(order.Items, i => i.Id == item2.Id);
        }

        [Fact]
        public void UpdateItems_ShouldUpdateItemCorrectly()
        {
            // Arrange
            var order = new Order();
            var productId = Guid.NewGuid();
            var orderItem = new OrderItem(Guid.NewGuid(), productId, "OldProduct", 1, 10);
            order.Items = new List<OrderItem> { orderItem };

            // Cria um item "dto" para atualização, mantendo o mesmo Id
            var updatedOrderItem = new OrderItem(orderItem.OrderId, productId, "UpdatedProduct", 5, 0);
            updatedOrderItem.Id = orderItem.Id;

            var product = new Product("NewProductName", 15, productId);

            // Act
            order.UpdateItems(new List<OrderItem> { updatedOrderItem }, new List<Product> { product });

            // Assert
            var updatedItem = order.Items.First();
            Assert.Equal(productId, updatedItem.ProductId);
            Assert.Equal("NewProductName", updatedItem.Name);
            Assert.Equal(5, updatedItem.Quantity);
            Assert.Equal(15, updatedItem.UnitPrice);
        }

        [Fact]
        public void UpdateItems_ShouldThrowException_WhenItemNotFound()
        {
            // Arrange
            var order = new Order();
            order.Items = new List<OrderItem>(); // Nenhum item

            var updatedOrderItem = new OrderItem(Guid.NewGuid(), Guid.NewGuid(), "NonExistent", 1, 10);
            var product = new Product("Product", 10, updatedOrderItem.ProductId);

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() =>
                order.UpdateItems(new List<OrderItem> { updatedOrderItem }, new List<Product> { product })
            );
            Assert.Contains($"Item {updatedOrderItem.Id} not found", ex.Message);
        }

        [Fact]
        public void UpdateItems_ShouldThrowException_WhenProductNotFound()
        {
            // Arrange
            var order = new Order();
            var productId = Guid.NewGuid();
            var orderItem = new OrderItem(Guid.NewGuid(), productId, "Product", 1, 10);
            order.Items = new List<OrderItem> { orderItem };

            var updatedOrderItem = new OrderItem(orderItem.OrderId, productId, "Product", 2, 10);
            updatedOrderItem.Id = orderItem.Id;

            // Act & Assert: Não fornece o produto esperado
            var ex = Assert.Throws<ArgumentNullException>(() =>
                order.UpdateItems(new List<OrderItem> { updatedOrderItem }, new List<Product>())
            );
            Assert.Contains($"Product {productId} not found", ex.Message);
        }

        [Fact]
        public void AddItems_ShouldAddNewItemsCorrectly()
        {
            // Arrange
            var order = new Order();
            order.Items = new List<OrderItem>(); // Lista vazia

            var productId = Guid.NewGuid();
            var product = new Product("ProductName", 20, productId);

            var newOrderItem = new OrderItem(Guid.NewGuid(), productId, "Placeholder", 3, 0);

            // Act
            order.AddItems(new List<OrderItem> { newOrderItem }, new List<Product> { product });

            // Assert
            Assert.Single(order.Items);
            var addedItem = order.Items.First();
            Assert.Equal(productId, addedItem.ProductId);
            Assert.Equal("ProductName", addedItem.Name);
            Assert.Equal(3, addedItem.Quantity);
            Assert.Equal(20, addedItem.UnitPrice);
        }

        [Fact]
        public void AddItems_ShouldThrowException_WhenProductNotFound()
        {
            // Arrange
            var order = new Order();
            order.Items = new List<OrderItem>();

            var productId = Guid.NewGuid();
            var newOrderItem = new OrderItem(Guid.NewGuid(), productId, "Placeholder", 3, 0);

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() =>
                order.AddItems(new List<OrderItem> { newOrderItem }, new List<Product>())
            );
            Assert.Contains($"Product {productId} not found", ex.Message);
        }

        [Fact]
        public void VerifyIfAllItensAreMine_ShouldNotThrow_WhenAllItemsBelong()
        {
            // Arrange
            var order = new Order();
            var item1 = new OrderItem(Guid.NewGuid(), Guid.NewGuid(), "Product1", 2, 10);
            var item2 = new OrderItem(Guid.NewGuid(), Guid.NewGuid(), "Product2", 1, 20);
            order.Items = new List<OrderItem> { item1, item2 };

            var ids = new List<Guid> { item1.Id, item2.Id };

            // Act & Assert: Não deve lançar exceção
            order.VerifyIfAllItensAreMine(ids);
        }

        [Fact]
        public void VerifyIfAllItensAreMine_ShouldThrow_WhenNotAllItemsBelong()
        {
            // Arrange
            var order = new Order();
            var item1 = new OrderItem(Guid.NewGuid(), Guid.NewGuid(), "Product1", 2, 10);
            order.Items = new List<OrderItem> { item1 };

            var ids = new List<Guid> { item1.Id, Guid.NewGuid() };

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => order.VerifyIfAllItensAreMine(ids));
        }

        [Fact]
        public void TotalAmount_ShouldReturnSumOfItemsTotalPrice()
        {
            // Arrange
            var order = new Order();
            var item1 = new OrderItem(Guid.NewGuid(), Guid.NewGuid(), "Product1", 2, 10); // 2 x 10 = 20
            var item2 = new OrderItem(Guid.NewGuid(), Guid.NewGuid(), "Product2", 1, 15); // 1 x 15 = 15
            order.Items = new List<OrderItem> { item1, item2 };

            // Act
            var total = order.TotalAmount;

            // Assert
            Assert.Equal(35, total);
        }
    }
}
