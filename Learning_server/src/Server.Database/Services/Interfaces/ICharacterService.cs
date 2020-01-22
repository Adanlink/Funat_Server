using System.Threading.Tasks;
using Server.Database.Models;

namespace Server.Database.Services.Interfaces
{
    public interface ICharacterService : IMappedRepository<CharacterModel>
    {
        /// <summary>
        /// Will return the CharacterModel associated to the nickname given as parameter.
        /// </summary>
        /// <returns></returns>
        CharacterModel GetByNickname(string nickname);
        
        /// <summary>
        /// Will asynchronously return the CharacterModel associated to nickname given as parameter
        /// </summary>
        /// <returns></returns>
        Task<CharacterModel> GetByNicknameAsync(string nickname);
    }
}