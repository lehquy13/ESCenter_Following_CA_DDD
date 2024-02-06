namespace ESCenter.Application.Interfaces.Cloudinarys;

public interface ICloudinaryServices
{
    string GetImage(string fileName);
    string UploadImage(string fileName);
    string UploadImage(string fileName, Stream stream);
}