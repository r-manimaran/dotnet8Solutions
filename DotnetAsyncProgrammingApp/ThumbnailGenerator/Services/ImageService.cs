﻿using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace ThumbnailGenerator.Services
{
    public class ImageService
    {
        public static readonly int[] ThumbnailWidths = [32,64,128,256,512,1024];
        private static readonly string[] AllowedExtensions = [".jpg", ".png", ".gif", ".jpeg"];
        private static readonly string[] AllowedMimeTypes = ["image/jpeg", "image/png", "image/gif"];

        public bool IsValidImage(IFormFile file)
        {
            if(file.Length == 0)
            {
                return false;
            }
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            return AllowedExtensions.Contains(extension) && AllowedMimeTypes.Contains(file.ContentType);
            
        }

        public async Task<string> SaveOriginalImageAsync(IFormFile file, string folderPath, string fileName)
        {
            if (!IsValidImage(file))
            {
                throw new ArgumentException("Invalid Image file", nameof(file));
            }
            var originalFilePath = Path.Combine(folderPath, fileName);
            Directory.CreateDirectory(folderPath);

            using var stream = new FileStream(originalFilePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return originalFilePath;
        }
         
        public async Task<IEnumerable<string>> GenerateThumbnailsAsync(
                                                    string originalFilePath, 
                                                    string folderPath, 
                                                    string fileNameWithoutExtension,
                                                    int[]? widths=null)
        {
            var thumbnailPaths = new List<string>();
            var extension = Path.GetExtension(originalFilePath);
            widths ??= ThumbnailWidths;

            using var image = await Image.LoadAsync(originalFilePath);
            foreach(var width in widths)
            {
                var thumbnailFileName = $"{fileNameWithoutExtension}_w{width}{extension}";
                var thumbnailPath = Path.Combine(folderPath, thumbnailFileName);

                var resizedImage = image.Clone(x => x.Resize(width, 0));
                await resizedImage.SaveAsync(thumbnailPath);

                thumbnailPaths.Add(thumbnailPath);

                //Introudce a delay to replicate a realtime delay
                await Task.Delay(5_000);
            }
            return thumbnailPaths;
        }
    }
}
