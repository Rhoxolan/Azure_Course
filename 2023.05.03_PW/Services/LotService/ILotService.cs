using _2023._05._03_PW.Data.Contexts;
using _2023._05._03_PW.Data.Models;
using _2023._05._03_PW.Models;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using System.Text.Json;

namespace _2023._05._03_PW.Services.LotService
{
	public interface ILotService
	{
		Task<SendReceipt> AddLotAsync(CurrencyLot currencyLot);

		Task<PeekedMessage[]> GetLotsAsync(CurrencyType currencyType);

		(string messageId, string popReceipt)? GetLotData(string messageId, CurrencyType currencyType);

		Task BuyLotAsync((string messageId, string popReceipt) lotData, CurrencyType currencyType);
	}

	public class LotService : ILotService
	{
		private readonly QueueServiceClient _queueServiceClient;
		private readonly MessagesDataContext _messagesDataContext;

		public LotService(QueueServiceClient queueServiceClient, MessagesDataContext messagesDataContext)
		{
			_queueServiceClient = queueServiceClient;
			_messagesDataContext = messagesDataContext;
		}

		public async Task<SendReceipt> AddLotAsync(CurrencyLot currencyLot)
		{
			QueueClient queueClient = _queueServiceClient.GetQueueClient($"lotes-{currencyLot.CurrencyType.ToString().ToLower()}");
			await queueClient.CreateIfNotExistsAsync();
			var receipt = await queueClient.SendMessageAsync(JsonSerializer.Serialize(currencyLot), timeToLive: TimeSpan.FromDays(1));
			var messageDataEntity = new MessageDataEntity
			{
				MessageId = receipt.Value.MessageId,
				PopReceipt = receipt.Value.PopReceipt
			};
			await _messagesDataContext.MessageDataEntities.AddAsync(messageDataEntity);
			await _messagesDataContext.SaveChangesAsync();
			return receipt;
		}

		public async Task BuyLotAsync((string messageId, string popReceipt) lotData, CurrencyType currencyType)
		{
			QueueClient queueClient = _queueServiceClient.GetQueueClient($"lotes-{currencyType.ToString().ToLower()}");
			await queueClient.CreateIfNotExistsAsync();
			await queueClient.DeleteMessageAsync(lotData.messageId, lotData.popReceipt);
			_messagesDataContext.MessageDataEntities.Remove(_messagesDataContext.MessageDataEntities.FirstOrDefault(m => m.MessageId == lotData.messageId)!);
			await _messagesDataContext.SaveChangesAsync();
		}

		public (string messageId, string popReceipt)? GetLotData(string messageId, CurrencyType currencyType)
		{
			QueueClient queueClient = _queueServiceClient.GetQueueClient($"lotes-{currencyType.ToString().ToLower()}");
			var messageDataEntity = _messagesDataContext.MessageDataEntities.FirstOrDefault(m => m.MessageId == messageId);
			if(messageDataEntity == null)
			{
				return null;
			}
			return (messageDataEntity.MessageId, messageDataEntity.PopReceipt);
		}

		public async Task<PeekedMessage[]> GetLotsAsync(CurrencyType currencyType)
		{
			QueueClient queueClient = _queueServiceClient.GetQueueClient($"lotes-{currencyType.ToString().ToLower()}");
			await queueClient.CreateIfNotExistsAsync();
			var azureResponse = await queueClient.PeekMessagesAsync(maxMessages: 10);
			return azureResponse.Value;
		}
	}
}
