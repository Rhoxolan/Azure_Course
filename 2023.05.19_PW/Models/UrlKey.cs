using Azure;
using Azure.Data.Tables;
using System;

namespace _2023._05._19_PW.Models
{
	public class UrlKey : ITableEntity
	{
		public int Id { get; set; }

		public string PartitionKey { get; set; }

		public string RowKey { get; set; }

		public DateTimeOffset? Timestamp { get; set; }

		public ETag ETag { get; set; }
	}
}
