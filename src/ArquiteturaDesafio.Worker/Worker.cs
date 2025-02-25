using ArquiteturaDesafio.Core.Domain.Entities;
using ArquiteturaDesafio.Core.Domain.Enum;
using ArquiteturaDesafio.Core.Domain.Interfaces;
using ArquiteturaDesafio.Core.Domain.ValueObjects;
using ArquiteturaDesafio.Infrastructure.Messaging.RabbitMQ.Consumer;
using Newtonsoft.Json.Linq;

namespace ArquiteturaDesafio.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConsumerMessage _consumer;
        private readonly IOrderReadRepository _repository;

        public Worker(ILogger<Worker> logger, IConsumerMessage consumer, IOrderReadRepository repository)
        {
            _logger = logger;
            _consumer = consumer;
            _repository = repository;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _consumer.ConsumeQueue("order.created", ProcessMessage, stoppingToken);
        }

        private async Task ProcessMessage(string message)
        {
            try
            {
                if (message is null)
                {
                    return;
                }

                // Parse da mensagem para um JObject
                JObject jsonObject = JObject.Parse(message);

                // Extrair a parte "Data" do JSON e converter para a classe TransactionXYZ
                OrderRead _transaction = jsonObject.ToObject<OrderRead>();

                // Recupera o saldo diário da data da transação
                var filterOrders= await _repository.Filter(x => x.OrderId == _transaction.OrderId, CancellationToken.None);

                // Verifica se é um novo realmente
                var isNew = filterOrders.Count == 0;

                // Se novo inclui
                if (isNew)
                {
                    _transaction.Id = Guid.NewGuid().ToString();
                    await _repository.Create(_transaction);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao salvar o order read.");
            }
        }
    }
}
