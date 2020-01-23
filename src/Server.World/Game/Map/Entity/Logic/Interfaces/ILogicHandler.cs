using System.Threading;
using System.Threading.Tasks;
using Server.Network;

namespace Server.World.Game.Map.Entity.Logic.Interfaces
{
    public interface ILogicHandler
    {
        Task Handle(ILogicEvent logicEvent, CancellationToken cancellationToken = default);
    }
}