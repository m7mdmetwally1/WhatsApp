using Imagekit;
using Imagekit.Models;
using Imagekit.Models.Response;
using Imagekit.Sdk;
using Microsoft.AspNetCore.Http.HttpResults;
using static Imagekit.Models.CustomMetaDataFieldSchemaObject;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using Microsoft.CodeAnalysis.CSharp;
using Application.UserDto;

namespace infrastructure.Repositories;

public class ImageKitService : IImageKitService
{
    private readonly ILogger<ImageKitService> _logger;

    public  ImageKitService(ILogger<ImageKitService> _logger)
    {
        this._logger = _logger;
    }
    public async  Task<ImageKitResponse> UploadImage(IFormFile imageUrl){
        ImagekitClient imagekit = new ImagekitClient("public_w5izE1WwpvRFWesXz3v/g6w0FFs=", "private_431PL5FB4Rj/rrTsdwkMHLI/0AM=", "https://ik.imagekit.io/s1r03vuv9/whatsAppClone/");
       
        // var apiKey = "private_431PL5FB4Rj/rrTsdwkMHLI/0AM=";
        // var encodedApiKey = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(apiKey));
         string base64Image = await FileConverter.ConvertFormFileToBase64(imageUrl);   

                var transformation = new UploadTransformation
        {
            pre = "w-300,h-400",  
            post = new List<PostTransformation>  
            {
                new PostTransformation
                {
                    type = "transformation",   
                    value = "bg-red"           
                }
            }
        };
       
        FileCreateRequest ob2 = new FileCreateRequest
        {
            file = base64Image,
            fileName = Guid.NewGuid().ToString(),
           transformation =transformation,
        }  ;       
        
      
        try{ 
            Result resp = imagekit.Upload(ob2);
            _logger.LogInformation($"{resp.HttpStatusCode}");
          return  new ImageKitResponse{ImageUrl = resp.url,ImageId =resp.fileId};
        }catch(Exception err){
            _logger.LogInformation($"{err}");
            return null;
        }
} 

public async Task<bool>  DeleteImage(string imageId)
{
    ImagekitClient imagekit = new ImagekitClient("public_w5izE1WwpvRFWesXz3v/g6w0FFs=", "private_431PL5FB4Rj/rrTsdwkMHLI/0AM=", "https://ik.imagekit.io/s1r03vuv9/whatsAppClone/");
   
    if(string.IsNullOrWhiteSpace(imageId))
    {
    return false;
    }   

    ResultDelete res2 = imagekit.DeleteFile($"{imageId}");

    if(res2 == null )
    {
        return false;
    }

    return true;
}

}

public class FileConverter
{
    public static async Task<string> ConvertFormFileToBase64(IFormFile formFile)
    {
        if (formFile == null || formFile.Length == 0)
        {
            throw new ArgumentException("File is not valid");
        }

        using (var memoryStream = new MemoryStream())
        {
            
            await formFile.CopyToAsync(memoryStream);

            
            byte[] fileBytes = memoryStream.ToArray();

            
            string base64String = Convert.ToBase64String(fileBytes);

            
            return $"data:{formFile.ContentType};base64,{base64String}";
        }
    }
}



