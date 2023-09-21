using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace _2023._05._09_PW
{
	public static class Functions
	{
		public static void ProcessQueueMessage([QueueTrigger("webjobs-test")] string message, ILogger logger)
		{
			logger.LogInformation($"File to process: {message}");
		}

		public static void ProcessBlobData([BlobTrigger("big-photos/{filename}")] Stream inputFile, string filename,
			[Blob("small-photos/copy-{filename}", FileAccess.Write)] Stream outputFile, ILogger logger)
		{
			inputFile.CopyTo(outputFile);
			logger.LogInformation($"File to process: {filename}");
			logger.LogInformation($"{inputFile.Length} bytes");
		}
	}
}
