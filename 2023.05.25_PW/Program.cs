﻿using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using static System.Environment;

namespace _2023._05._25_PW
{
	internal class Program
	{
		static void Main(string[] args)
		{
			var endpoint = "https://servicecvisionazurestudy.cognitiveservices.azure.com/";
			Console.WriteLine("Please enter your key:" + NewLine);
			var key = Console.ReadLine()!;
			Console.WriteLine();
			ComputerVisionClient visionClient = Authenticate(key, endpoint);
			AnalyzeImageAsync(visionClient).Wait();
		}

		static async Task AnalyzeImageAsync(ComputerVisionClient client)
		{
			Console.WriteLine("Please enter url:" + NewLine);
			string url = Console.ReadLine()!;
			Console.WriteLine();

			var visualFeatureTypes = new List<VisualFeatureTypes?>
			{
				VisualFeatureTypes.Categories,
				VisualFeatureTypes.Faces,
				VisualFeatureTypes.Description,
				VisualFeatureTypes.Objects,
				VisualFeatureTypes.Color,
				VisualFeatureTypes.Brands,
				VisualFeatureTypes.Tags,
				VisualFeatureTypes.ImageType
			};

			var imageAnalysis = await client.AnalyzeImageAsync(url, visualFeatureTypes);

			Console.WriteLine($"Image Analize with filename: {Path.GetFileName(url)}{NewLine}");

			Console.WriteLine("Objects:" + NewLine);
			GetObjects(imageAnalysis);
			Console.WriteLine();

			Console.WriteLine("Tags:" + NewLine);
			GetTags(imageAnalysis);
			Console.WriteLine();

			Console.WriteLine("Brands:" + NewLine);
			GetBrands(imageAnalysis);
			Console.WriteLine();

			Console.WriteLine("Faces:" + NewLine);
			GetFaces(imageAnalysis);
			Console.WriteLine();

			Console.WriteLine("Categories:" + NewLine);
			GetCategories(imageAnalysis);
			Console.WriteLine();

			Console.WriteLine("Captions:" + NewLine);
			GetCaptions(imageAnalysis);
			Console.WriteLine();

			Console.WriteLine("Color scheme:" + NewLine);
			GetColorScheme(imageAnalysis);
			Console.WriteLine();

			Console.WriteLine("Celebrities:" + NewLine);
			GetCelebrities(imageAnalysis);
			Console.WriteLine();

			Console.WriteLine("LandMarks:" + NewLine);
			GetLandmarks(imageAnalysis);
			Console.WriteLine();
		}

		static void GetObjects(ImageAnalysis imageAnalysis)
		{
			foreach (var @object in imageAnalysis.Objects)
				Console.WriteLine($"Object {@object.ObjectProperty} " +
					$"with confidence {@object.Confidence} is in region " +
					$"({@object.Rectangle.X},{@object.Rectangle.Y}) - " +
					$"({@object.Rectangle.X + @object.Rectangle.W}," +
					$" {@object.Rectangle.Y + @object.Rectangle.H})");
		}

		static void GetTags(ImageAnalysis imageAnalysis)
		{
			foreach (var tag in imageAnalysis.Tags)
				Console.WriteLine($"{tag.Name} with {tag.Confidence}");
		}

		static void GetBrands(ImageAnalysis imageAnalysis)
		{
			foreach (var brand in imageAnalysis.Brands)
			{
				Console.WriteLine($"Brand {brand.Name} " +
					$"with confidence {brand.Confidence} is in region " +
					$"({brand.Rectangle.X},{brand.Rectangle.Y}) - " +
					$"({brand.Rectangle.X + brand.Rectangle.W}," +
					$" {brand.Rectangle.Y + brand.Rectangle.H})");
			}
		}

		static void GetFaces(ImageAnalysis imageAnalysis)
		{
			foreach (var face in imageAnalysis.Faces)
			{
				Console.WriteLine($"Detected face, gender : {face.Gender}, age: {face.Age} ");
				Console.WriteLine($"It is in region " +
					$"({face.FaceRectangle.Left}, {face.FaceRectangle.Top})" +
					$"({face.FaceRectangle.Left + face.FaceRectangle.Width}," +
					$" {face.FaceRectangle.Top + face.FaceRectangle.Height})");
			}
		}

		static void GetCategories(ImageAnalysis imageAnalysis)
		{
			foreach (var category in imageAnalysis.Categories)
				Console.WriteLine($"Category: {category.Name}, score: {category.Score}");
		}

		static void GetCaptions(ImageAnalysis imageAnalysis)
		{
			foreach (var caption in imageAnalysis.Description.Captions)
				Console.WriteLine($"Caption: {caption.Text}, confidence: {caption.Confidence}");
		}

		static void GetColorScheme(ImageAnalysis imageAnalysis)
		{
			Console.WriteLine("\n=> Colors:");
			Console.WriteLine($"Accent color: {imageAnalysis.Color.AccentColor}");
			Console.WriteLine($"Prevailing background colour:" +
				$" {imageAnalysis.Color.DominantColorBackground}");
			Console.WriteLine($"Prevailing primary colour: " +
				$"{imageAnalysis.Color.DominantColorForeground}");
			Console.WriteLine("Primary Colors: " +
				string.Join(", ", imageAnalysis.Color.DominantColors));
			Console.WriteLine("The image is " +
				(imageAnalysis.Color.IsBWImg ? "black-white" : "colorful"));
		}

		static void GetCelebrities(ImageAnalysis imageAnalysis)
		{
			foreach (Category category in imageAnalysis.Categories)
			{
				if (category.Detail?.Celebrities != null)
				{
					foreach (var celeb in category.Detail.Celebrities)
					{
						Console.WriteLine($"Celebrity {celeb.Name} " +
							$"with confidence {celeb.Confidence} is in region " +
							$"({celeb.FaceRectangle.Left}, {celeb.FaceRectangle.Top})" +
							$"({celeb.FaceRectangle.Left + celeb.FaceRectangle.Width}, " +
							$"{celeb.FaceRectangle.Top + celeb.FaceRectangle.Height})");
					}
				}
			}
		}

		static void GetLandmarks(ImageAnalysis imageAnalysis)
		{
			foreach (var category in imageAnalysis.Categories)
			{
				if (category.Detail?.Landmarks != null)
				{
					foreach (var landMark in category.Detail.Landmarks)
					{
						Console.WriteLine($"LandMark {landMark.Name} " +
							$"with confidence {landMark.Confidence}");
					}
				}
			}
		}

		static ComputerVisionClient Authenticate(string key, string endpoint)
		{
			var credentials = new ApiKeyServiceClientCredentials(key);
			return new ComputerVisionClient(credentials) { Endpoint = endpoint };
		}
	}
}
