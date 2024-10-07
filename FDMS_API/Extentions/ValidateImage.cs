namespace FDMS_API.Extentions
{
    public static class ValidateImage
    {
        public static bool IsImage(this IFormFile formFile)
        {
            if(formFile == null && formFile?.Length == 0)
            {
                return false;
            }
            var allowedExtentions = new[] { ".jpg", ".jpeg", ".png", ".gif" }; 

            var fileExtention = Path.GetExtension(formFile.FileName).ToLowerInvariant();

            if (!allowedExtentions.Contains(fileExtention))
            {
                return false;
            }
            return true;
        }
    }
}
