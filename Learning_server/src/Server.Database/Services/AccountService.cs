using System;
using System.Collections.Generic;
using System.Text;
using Server.Database.Services.Bases;
using Server.Database.Models;
using ChickenAPI.Core.Logging;
using Microsoft.EntityFrameworkCore;
using Server.Database.Services.Interfaces;
using System.Threading.Tasks;
using System.Linq;
using Server.Database.Context;

namespace Server.Database.Services
{
    public class AccountService : MappedRepositoryBase<AccountModel>, IAccountService
    {
        public AccountService(DbContext context, ILogger log) : base(context, log)
        {
        }

        public AccountModel GetByEmail(string Email)
        {
            try
            {
                return DbSet.SingleOrDefault(m => m.Email == Email);
            }
            catch (Exception e)
            {
                Log.Error("[GET_BY_EMAIL]", e);
                return null;
            }
        }

        public async Task<AccountModel> GetByEmailAsync(string Email)
        {
            try
            {
                return await DbSet.SingleOrDefaultAsync(m => m.Email == Email).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Log.Error("[GET_BY_EMAIL]", e);
                return null;
            }
        }

        public AccountModel GetByUsername(string username)
        {
            try
            {
                return DbSet.SingleOrDefault(m => m.Username == username);
            }
            catch (Exception e)
            {
                Log.Error("[GET_BY_USERNAME]", e);
                return null;
            }
        }
        
        public AccountModel GetByUsernameFresh(string username)
        {
            try
            {
                var asd = DbSet.SingleOrDefault(m => m.Username == username);
                Refresh(asd);
                return asd;

            }
            catch (Exception e)
            {
                Log.Error("[GET_BY_USERNAME]", e);
                return null;
            }
        }

        public async Task<AccountModel> GetByUsernameAsync(string Username)
        {
            try
            {
                return await DbSet.SingleOrDefaultAsync(m => m.Username == Username).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Log.Error("[GET_BY_USERNAME_ASYNC]", e);
                return null;
            }
        }
    }
}
