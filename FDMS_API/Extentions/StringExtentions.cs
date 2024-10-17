namespace FDMS_API.Extentions
{
    public static class StringExtentions
    {
        public static string GetInitials(this string input)
        {
            // Tách chuỗi thành các từ
            string[] words = input.Split(' ');

            // Khởi tạo biến để lưu kết quả
            string initials = "";

            // Lặp qua từng từ và lấy chữ cái đầu tiên
            foreach (string word in words)
            {
                if (!string.IsNullOrEmpty(word))
                {
                    initials += word[0]; // Lấy chữ cái đầu
                }
            }

            return initials.ToUpper(); // Trả về chữ cái viết hoa
        }
    }
}
