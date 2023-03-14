using System.Text;

namespace ToDo.Helper;

public static class UploadFileHelper
{
    public static async Task<bool> UploadFileAsync(IFormFile file, string folderPath)
    {

        if (file.Length <= 0)
        {
            return false;
        }

        if (!System.IO.File.Exists(folderPath))
        {
            // create dictionary if doesn't exist
            System.IO.Directory.CreateDirectory(folderPath);
        }

        var filePath = Path.Combine(folderPath, file.FileName);
        await using var stream = System.IO.File.Create(filePath);
        await file.CopyToAsync(stream);
        return true;
    }

    public static string GetFileSrc(IFormFile file, Guid id)
    {
        var sb = new StringBuilder();
        sb.Append("/UploadFiles/");
        sb.Append(id.ToString());
        sb.Append("/");
        sb.Append(file.FileName);
        return sb.ToString();
    }

}