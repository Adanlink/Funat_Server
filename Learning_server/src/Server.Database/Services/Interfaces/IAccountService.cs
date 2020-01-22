using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Server.Database.Models;

namespace Server.Database.Services.Interfaces
{
    public interface IAccountService : IMappedRepository<AccountModel>
    {
        /// <summary>
        /// Will return the UserAccountModel associated to the username given as parameter.
        /// </summary>
        /// <param name="Username"></param>
        /// <returns></returns>
        AccountModel GetByUsername(string Username);
        
        AccountModel GetByUsernameFresh(string Username);

        /// <summary>
        /// Will asynchronously return the AccountDto associated to name given as parameter
        /// </summary>
        /// <param name="Username"></param>
        /// <returns></returns>
        Task<AccountModel> GetByUsernameAsync(string Username);

        /// <summary>
        /// Will return the UserAccountModel associated to the Email given as parameter.
        /// </summary>
        /// <param name="Email"></param>
        /// <returns></returns>
        AccountModel GetByEmail(string Email);

        /// <summary>
        /// Will asynchronously return the UserAccountModel associated to the Email given as parameter.
        /// </summary>
        /// <param name="Email"></param>
        /// <returns></returns>
        Task<AccountModel> GetByEmailAsync(string Email);
    }
}
