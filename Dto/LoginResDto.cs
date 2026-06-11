namespace MOFU.Dto
{
    public class LoginResDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = "";
        public string UserEmail { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public string Token { get; set; } = "";
    }
}
