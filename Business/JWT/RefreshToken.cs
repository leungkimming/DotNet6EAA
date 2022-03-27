using System.ComponentModel.DataAnnotations.Schema;
using Business.Users;
using Business.Base;

namespace Business.JWT
{
    public class RefreshToken : BaseEntity<int>
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Token { get; set; }
        public string JwtId { get; set; }
        public bool IsUsed { get; set; }
        public bool IsRevorked { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime ExpiryDate { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User {get;set;}
    }
}