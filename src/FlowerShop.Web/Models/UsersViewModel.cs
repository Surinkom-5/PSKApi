using FlowerShop.Core.Entities;

namespace FlowerShop.Web.Models
{
    public class UserViewModel
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UserRole { get; private set; }

        public static UserViewModel ToModel(User user)
        {
            return new UserViewModel
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                UserRole = user.UserRole.ToString()
            };
        }
    }
}