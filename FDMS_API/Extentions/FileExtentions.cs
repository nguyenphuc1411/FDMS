namespace FDMS_API.Extentions
{
    public static class FileExtentions
    {
        // Danh sách các định dạng file hợp lệ
        private static readonly string[] ValidFileExtensions =
        {
        ".doc", ".docx", ".odt", ".txt", ".rtf",
        ".pdf", ".xls", ".xlsx", ".ods",
        ".ppt", ".pptx", ".odp",
        ".jpg", ".jpeg", ".png", ".gif",
        ".xml", ".html", ".htm",
        ".csv", ".json"
        };
        public static bool IsImage(this IFormFile file)
        {
            if(file == null && file?.Length == 0)
            {
                return false;
            }
            var allowedExtentions = new[] { ".jpg", ".jpeg", ".png", ".gif" }; 

            var fileExtention = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!allowedExtentions.Contains(fileExtention))
            {
                return false;
            }
            return true;
        }

        public static bool IsValidFileFormat(this IFormFile file)
        {
            if (file == null && file?.Length == 0)
                return false;

            // Lấy phần mở rộng của file
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            // Kiểm tra xem phần mở rộng có trong danh sách hợp lệ hay không
            return Array.Exists(ValidFileExtensions, ext => ext.Equals(extension, StringComparison.OrdinalIgnoreCase));
        }
    }
}
