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

        public Worker(ILogger<Worker> logger, IConsumerMessage consumer)
        {
            _logger = logger;
            _consumer = consumer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _consumer.ConsumeQueue("transaction.created", ProcessMessage, stoppingToken);
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

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao salvar o relatório de saldo diário");
            }
        }
    }
}
