using ChickenAPI.Core.Logging;
using Microsoft.EntityFrameworkCore;
using Server.Database.Models;
using Server.Database.Services.Bases;
using Server.Database.Services.Interfaces;

namespace Server.Database.Services
{
    public class MapService : MappedRepositoryBase<MapModel>, IMapService
    {
        public MapService(DbContext context, ILogger log) : base(context, log)
        {
        }
    }
}