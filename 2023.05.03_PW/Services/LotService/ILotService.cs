using _2023._05._03_PW.Models;
using Azure.Storage.Queues.Models;

namespace _2023._05._03_PW.Services.LotService
{
	public interface ILotService
	{
		Task<SendReceipt> AddLotAsync(CurrencyLot currencyLot);

		Task<PeekedMessage[]> GetLotsAsync(CurrencyType currencyType);

		(string messageId, string popReceipt)? GetLotData(string messageId, CurrencyType currencyType);

		Task BuyLotAsync((string messageId, string popReceipt) lotData, CurrencyType currencyType);
	}
}
