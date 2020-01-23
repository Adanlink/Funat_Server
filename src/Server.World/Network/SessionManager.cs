using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Server.Database.Models;
using Server.World.Configuration;
using Server.World.Network.Interfaces;

namespace Server.World.Network
{
    public class SessionManager : ISessionManager
    {
        private readonly Dictionary<IPAddress, ushort> _connectionsPerIp = new Dictionary<IPAddress, ushort>();
        
        private readonly Dictionary<long, bool> _charactersLoggedOn = new Dictionary<long, bool>();

        private readonly WorldConfiguration _worldConfiguration;

        public SessionManager(WorldConfiguration worldConfiguration)
        {
            _worldConfiguration = worldConfiguration;
        }

        public Task<bool> RegisterAsync(ISession session)
        {
            Monitor.Enter(_connectionsPerIp);
            try
            {
                if (!_connectionsPerIp.TryGetValue(session.EndPoint.Address, out var connections))
                {
                    _connectionsPerIp.Add(session.EndPoint.Address, 1);
                    return Task.FromResult(true);
                }
                
                if (connections >= _worldConfiguration.MaxConnectionsPerIp)
                {
                    return Task.FromResult(false);
                }

                _connectionsPerIp[session.EndPoint.Address]++;
            }
            finally
            {
                Monitor.Exit(_connectionsPerIp);
            }

            return Task.FromResult(true);
        }

        public Task UnregisterAsync(ISession session)
        {
            Monitor.Enter(_connectionsPerIp);
            try
            {
                if (!_connectionsPerIp.TryGetValue(session.EndPoint.Address, out var connections))
                {
                    return Task.CompletedTask;
                }

                if (connections > 0)
                {
                    _connectionsPerIp[session.EndPoint.Address]--;
                }
                
                if (_connectionsPerIp[session.EndPoint.Address] == 0)
                {
                    _connectionsPerIp.Remove(session.EndPoint.Address);
                    return Task.CompletedTask;
                }
            }
            finally
            {
                Monitor.Exit(_connectionsPerIp);
            }

            return Task.CompletedTask;
        }

        public Task<bool> RegisterCharacterAsync(CharacterModel characterModel)
        {
            Monitor.Enter(_charactersLoggedOn);
            
            try
            {
                if (!_charactersLoggedOn.TryGetValue(characterModel.Id, out var alreadyPlaying))
                {
                    _charactersLoggedOn.Add(characterModel.Id, true);
                    return Task.FromResult(true);
                }
                
                if (alreadyPlaying)
                {
                    return Task.FromResult(false);
                }

                _charactersLoggedOn[characterModel.Id] = true;
            }
            finally
            {
                Monitor.Exit(_charactersLoggedOn);
            }
            
            return Task.FromResult(true);
        }

        public Task UnregisterCharacterAsync(CharacterModel characterModel)
        {
            Monitor.Enter(_charactersLoggedOn);

            try
            {
                _charactersLoggedOn.Remove(characterModel.Id);
            }
            finally
            {
                Monitor.Exit(_charactersLoggedOn);
            }
            
            return Task.CompletedTask;
        }
    }
}