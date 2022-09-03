using API.Helpers;
using API.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace API.Services
{
    public class PhotoService : _BaseService, IPhotoService
    {
        private readonly Cloudinary _cloudinary;

        //I removed the parameter and use the config, but like, i left it in the comments cause i am not 100% sure it will work, but im pretty confident it will
        // UPDATE: It indeed works but I'm leaving it here so as to track where we are coming from
        // REFACTOR: We might need to change this Configuration to use environment variables
        public PhotoService(/*IOptions<CloudinarySettings> config*/)
        {
            var acc = new Account
                (
                Configuration["CloudinarySettings:CloudName"],
                Configuration["CloudinarySettings:ApiKey"],
                Configuration["CloudinarySettings:ApiSecret"]
                );

            _cloudinary = new Cloudinary(acc);
        }
        public async Task<ImageUploadResult> AddPhotoAsync(string path)
        {
            var uploadResult = new ImageUploadResult();

            int n = path.IndexOf("base64,");

            path = path.Remove(0, n + 7);
            var bytes = Convert.FromBase64String(path);

            FormFile file;

            var myStream = new MemoryStream(bytes);
            file = new FormFile(myStream, 0, myStream.Length, null, "imageName");

            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription("imageOfFood", stream),
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face")
                };

                uploadResult = await _cloudinary.UploadAsync(uploadParams);     
            }

            return uploadResult;
        }

        public async Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);

            var result = await _cloudinary.DestroyAsync(deleteParams);

            return result;
        }
    }
}
