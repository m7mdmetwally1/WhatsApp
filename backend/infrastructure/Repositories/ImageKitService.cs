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
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System.IO;

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

        //  string base64Image = await FileConverter.ConvertFormFileToBase64(imageUrl);
         using var stream = new MemoryStream();
         using var image = Image.Load(imageUrl.OpenReadStream());
        image.Mutate(x => x.Resize(150, 150)); 
        image.Save(stream, new JpegEncoder { Quality = 75 }); 
        var base64Image = Convert.ToBase64String(stream.ToArray());   
        
        var transformation = new UploadTransformation
        {
            pre = "q-75",  
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
           
        } ;

        Result resp;
        try
        {             
         resp =  imagekit.Upload(ob2);
         _logger.LogInformation($"{resp.HttpStatusCode}"); 

        if(resp.HttpStatusCode != 200 && resp.HttpStatusCode != 202)
        {
            return  new ImageKitResponse{Message="Failed to upload image ,connection",Success=false};
        }
                     
        }catch(Exception err){           
            _logger.LogInformation($"{err}");
            _logger.LogError(err.Message);
            return new ImageKitResponse{Message=$" internal server error",Success=false};
        }

        return  new ImageKitResponse{ImageUrl = resp.url,ImageId =resp.fileId,Success=true};
} 

public async Task<bool>  DeleteImage(string imageId)
{
    ImagekitClient imagekit = new ImagekitClient("public_w5izE1WwpvRFWesXz3v/g6w0FFs=", "private_431PL5FB4Rj/rrTsdwkMHLI/0AM=", "https://ik.imagekit.io/s1r03vuv9/whatsAppClone/");
   
    if(string.IsNullOrWhiteSpace(imageId))
    {
    return false;
    }   

    ResultDelete res2 = await Task.Run(() => imagekit.DeleteFile(imageId));

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



