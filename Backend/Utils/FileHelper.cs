namespace Backend.Utils;

public static class FileHelper
{
    private const string Image = "http://localhost:5062/image/";

    public static async Task<string?> UploadImage(IFormFile? file)
    {
        if (file == null) return null;

        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Image", file.FileName);
        await using var stream = File.Create(path);
        await file.CopyToAsync(stream);
        return Image + file.FileName;
    }
}