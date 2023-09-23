using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;

namespace _2023._05._09_PW
{
	public static class Functions
	{
		public static void ProcessQueueMessage([QueueTrigger("webjobs-test")] string message, ILogger logger)
		{
			logger.LogInformation($"File to process: {message}");
		}

		//Создание уменьшенной копии изображения фиксированного размера 200/100 при первоначальной загрзуке изображения
		public static void ProcessBlobData([BlobTrigger("big-photos/{filename}")] Stream inputFile, string filename,
			[Blob("small-photos/copy-{filename}", FileAccess.Write)] Stream outputFile, ILogger logger)
		{
			using Image image = Image.Load(inputFile);
			using Image imageThumbnail = image.Clone(i => i.Resize(200, 100));
			imageThumbnail.Save(outputFile, imageThumbnail.Metadata.DecodedImageFormat!);
			logger.LogInformation($"File to process: {filename}");
			logger.LogInformation($"{inputFile.Length} bytes");
		}
	}
}
