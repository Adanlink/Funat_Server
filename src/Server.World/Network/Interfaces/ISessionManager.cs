using System;
using System.Net;
using System.Threading.Tasks;
using Server.Database.Models;

namespace Server.World.Network.Interfaces
{
    /// <summary>
    /// The interface for the manager of sessions, helps with: find the desired session or
    /// know if that session shouldn't log on.
    /// </summary>
    public interface ISessionManager
    {
        Task<bool> RegisterAsync(ISession session);

        Task UnregisterAsync(ISession session);

        Task<bool> RegisterCharacterAsync(CharacterModel characterModel);

        Task UnregisterCharacterAsync(CharacterModel characterModel);
    }
}