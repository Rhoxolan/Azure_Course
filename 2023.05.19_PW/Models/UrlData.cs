using Azure;
using Azure.Data.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2023._05._19_PW.Models
{
	internal class UrlData : ITableEntity
	{
		public string Id { get; set; }

		public string Url { get; set; }

		public int Count { get; set; }

		public string PartitionKey { get; set; }

		public string RowKey { get; set; }

		public DateTimeOffset? Timestamp { get; set; }

		public ETag ETag { get; set; }
	}
}
