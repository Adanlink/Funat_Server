using Server.Database.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Server.Database.Models.Interfaces;

namespace Server.Database.Services.Interfaces
{
    public interface IMappedRepository<T> : ISynchronousRepository<T, long>, IAsyncRepository<T, long> where T : class, IMappedModel
    {
    }
}
