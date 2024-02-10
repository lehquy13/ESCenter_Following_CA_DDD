using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using ESCenter.Application.Interfaces.Cloudinarys;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ESCenter.Infrastructure.ServiceImpls.Cloudinary;

internal class CloudinaryServices(IOptions<CloudinarySetting> cloudinarySetting, ILogger<CloudinaryServices> logger)
    : ICloudinaryServices
{
    private CloudinaryDotNet.Cloudinary Cloudinary { get; set; } = new(
        new Account(
            cloudinarySetting.Value.CloudName,
            cloudinarySetting.Value.ApiKey,
            cloudinarySetting.Value.ApiSecret
        ))
    {
        Api =
        {
            Secure = true
        }
    };

    public string GetImage(string fileName)
    {
        try
        {
            var getResourceParams = new GetResourceParams("fileName")
            {
                QualityAnalysis = true
            };
            var getResourceResult = Cloudinary.GetResource(getResourceParams);
            var resultJson = getResourceResult.JsonObj;

            // Log quality analysis score to the console
            logger.LogInformation("{Message}", resultJson["quality_analysis"]);

            return resultJson["url"]?.ToString()??"https://res.cloudinary.com/dhehywasc/image/upload/v1686121404/default_avatar2_ws3vc5.png";
        }
        catch (Exception ex)
        {
            logger.LogError("{ExMessage}", ex.Message);
            return @"https://res.cloudinary.com/dhehywasc/image/upload/v1686121404/default_avatar2_ws3vc5.png";
        }
    }

    public string UploadImage(string fileName)
    {
        try
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(fileName),
                UseFilename = true,
                UniqueFilename = false,
                Overwrite = true
            };
            
            var uploadResult = Cloudinary.Upload(uploadParams);
            logger.LogInformation("{ResultValue}",uploadResult.JsonObj.ToString());

            return uploadResult.Url.ToString();
        }
        catch (Exception ex)
        {
            logger.LogError("{ExMessage}", ex.Message);
            return @"https://res.cloudinary.com/dhehywasc/image/upload/v1686121404/default_avatar2_ws3vc5.png";
        }
    }
    /// <summary>
    /// Recommended to use this method
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="stream"></param>
    /// <returns></returns>
    public string UploadImage(string fileName, Stream stream)
    {
        try
        {
            var paramss = new ImageUploadParams()
            {
                File = new FileDescription(fileName, stream),
                Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
            };
            var result = Cloudinary.Upload(paramss);
            return result.Url.ToString();
        }
        catch (Exception ex)
        {
            logger.LogError("{ExMessage}", ex.Message);
            return string.Empty;
        }
    }
}