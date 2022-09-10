﻿using CloudinaryDotNet.Actions;
using RodizioSmartKernel.Interfaces.BaseInterfaces;
using System.Threading.Tasks;

namespace API.Application.Interfaces
{
    public interface IPhotoService: IBaseService
    {
        Task<ImageUploadResult> AddPhotoAsync(string path);
        Task<DeletionResult> DeletePhotoAsync(string publicId);
    }
}
