using System;
using System.Linq;
using System.Threading.Tasks;
using ChickenAPI.Core.Logging;
using Microsoft.EntityFrameworkCore;
using Server.Database.Models;
using Server.Database.Services.Bases;
using Server.Database.Services.Interfaces;

namespace Server.Database.Services
{
    public class CharacterService : MappedRepositoryBase<CharacterModel>, ICharacterService
    {
        public CharacterService(DbContext context, ILogger log) : base(context, log)
        {
        }

        public CharacterModel GetByNickname(string nickname)
        {
            try
            {
                return DbSet.SingleOrDefault(m => m.Nickname == nickname);
            }
            catch (Exception e)
            {
                Log.Error("[GET_BY_USERNAME]", e);
                return null;
            }
        }

        public async Task<CharacterModel> GetByNicknameAsync(string nickname)
        {
            try
            {
                return await DbSet.SingleOrDefaultAsync(m => m.Nickname == nickname).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Log.Error("[GET_BY_USERNAME]", e);
                return null;
            }
        }
    }
}