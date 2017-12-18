using System;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Magento
{
    public interface IMagentoApi
    {
        Task<User> RegisterUser(User user);
        Task<User> AuthenticateUser(User user);
    }
}
