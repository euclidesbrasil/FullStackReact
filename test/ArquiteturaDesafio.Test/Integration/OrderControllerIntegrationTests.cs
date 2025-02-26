using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using ArquiteturaDesafio.Core.Application.UseCases.Commands.AuthenticateUser;
using System.Net.Http.Headers;
using ArquiteturaDesafio.Application.UseCases.Commands.User.CreateUser;
using ArquiteturaDesafio.Application.UseCases.Commands.User.UpdateUser;
using ArquiteturaDesafio.Core.Domain.Enum;
using ArquiteturaDesafio.Core.Application.UseCases.Queries.GetUsersById;
using ArquiteturaDesafio.Application.UseCases.Commands.Order.CreateSale;
using ArquiteturaDesafio.Application.UseCases.Queries.GetOrderById;
using ArquiteturaDesafio.Core.Application.UseCases.Queries.GetOrdersQuery;
using System.Net.Http.Json;
using System.Net;
using ArquiteturaDesafio.Application.UseCases.Commands.Customer.CreateCustomer;
using ArquiteturaDesafio.Application.UseCases.Commands.Product.CreateProduct;
using ArquiteturaDesafio.Core.Domain.Entities;
using ArquiteturaDesafio.Application.UseCases.Commands.Order.UpdateSale;
namespace ArquiteturaDesafio.Test.Integration
{
    public class OrderControllerIntegrationTests
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _options;
        private Guid _idCustomerCreated;
        private Guid _idProductCreated;
        private Guid _idOrderCreated;

        public OrderControllerIntegrationTests()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5000")
            };
            _options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        private async Task<string> GetAuthTokenAsync()
        {
            var command = new AuthenticateUserRequest
            {
                Username = "admin",
                Password = "s3nh@"
            };

            var content = new StringContent(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/auth/login", content);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var responseObject = JsonSerializer.Deserialize<AuthenticateUserResult>(responseString, _options);

            return responseObject?.Token;
        }

        private async Task createData()
        {
            var token = await GetAuthTokenAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var baseUser = $"{Guid.NewGuid()}";
            var request = new CreateCustomerRequest
            {
                Identification = new Core.Application.UseCases.DTOs.InfoContactDTO() { Email="email@email.com",Phone = "999999999"},
                Name = $"Clientes teste {Guid.NewGuid()}"
            };

            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/Customers", content);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<CreateCustomerResponse>(responseString, _options);
            _idCustomerCreated = result.id;

            var request2 = new CreateProductRequest() { Name = $"Prod {Guid.NewGuid()}", Price = 1 };
            var content2 = new StringContent(JsonSerializer.Serialize(request2), Encoding.UTF8, "application/json");
            var response2 = await _client.PostAsync("/Products", content2);
            response2.EnsureSuccessStatusCode();
            var responseString2 = await response2.Content.ReadAsStringAsync();
            var result2 = JsonSerializer.Deserialize<CreateProductResponse>(responseString2, _options);
            _idProductCreated = result2.id;

            var orderRequest = new
            {
                orderDate = DateTime.UtcNow,
                customerId = _idCustomerCreated,
                items = new[]
                {
                    new
                    {
                        productId = _idProductCreated,
                        quantity = 2
                    }
                }
            };

            // Act
            var responseOrder = await _client.PostAsJsonAsync("/Orders", orderRequest);

            
            var createdOrder = await responseOrder.Content.ReadFromJsonAsync<CreateOrderResponse>();
            _idOrderCreated = createdOrder.id;

        }


        [Fact]
        public async Task CreateOrder_ReturnsCreatedOrder()
        {
            await createData();
            var customerId = _idCustomerCreated;
            var productId = _idProductCreated;

            var orderRequest = new
            {
                orderDate = DateTime.UtcNow,
                customerId = _idCustomerCreated,
                items = new[]
                {
                    new
                    {
                        productId = _idProductCreated,
                        quantity = 2
                    }
                }
            };

            // Act
            var responseOrder = await _client.PostAsJsonAsync("/Orders", orderRequest);

            // Assert
            Assert.Equal(HttpStatusCode.Created, responseOrder.StatusCode);
            var createdOrder = await responseOrder.Content.ReadFromJsonAsync<CreateOrderResponse>();
            Assert.NotNull(createdOrder);
            Assert.NotEqual(Guid.Empty, createdOrder.id);
        }

        [Fact]
        public async Task UpdateOrder_ReturnsNoContent()
        {
            await createData();
            var customerId = _idCustomerCreated;
            var productId = _idProductCreated;
            var orderId = _idOrderCreated;

            var orderRequest = new UpdateOrderRequest()
            {
                OrderDate = DateTime.UtcNow,
                CustomerId = _idCustomerCreated,
                Items = new List<UpdateSaleItemRequest>()
                 {
                    new UpdateSaleItemRequest()
                    {
                        ProductId = _idProductCreated,
                        Quantity = 2
                    }
                }
            };

            var token = await GetAuthTokenAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            // A rota do PUT espera o id via query string (ex.: /Orders?id={orderId})
            var response = await _client.PutAsJsonAsync($"/Orders?id={orderId}", orderRequest);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task DeleteOrder_ReturnsNoContent()
        {
            // Arrange
            await createData();
            var customerId = _idCustomerCreated;
            var productId = _idProductCreated;
            var orderId = _idOrderCreated;

            var token = await GetAuthTokenAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // O DELETE espera o id como parâmetro na query string
            var request = new HttpRequestMessage(HttpMethod.Delete, $"/Orders?id={orderId}");
            var response = await _client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task GetOrderById_ReturnsOrderDetails()
        {
            await createData();
            var customerId = _idCustomerCreated;
            var productId = _idProductCreated;
            var orderId = _idOrderCreated.ToString();

            var token = await GetAuthTokenAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var response = await _client.GetAsync($"/Orders/{orderId}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var orderDetails = await response.Content.ReadFromJsonAsync<GetOrderByIdResponse>();
            Assert.NotNull(orderDetails);
            Assert.Equal(orderId, orderDetails.OrderId);
            // Adicione outras asserções conforme os dados retornados
        }

        [Fact]
        public async Task GetOrders_ReturnsOrdersQueryResponse()
        {
            await createData();
            var customerId = _idCustomerCreated;
            var productId = _idProductCreated;
            var orderId = _idOrderCreated.ToString();

            var token = await GetAuthTokenAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var response = await _client.GetAsync("/Orders?_page=1&_size=10&_order=id asc");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var ordersQueryResponse = await response.Content.ReadFromJsonAsync<GetOrdersQueryResponse>();
            Assert.NotNull(ordersQueryResponse);
            Assert.True(ordersQueryResponse.TotalItems > 0);
        }
    }
}
