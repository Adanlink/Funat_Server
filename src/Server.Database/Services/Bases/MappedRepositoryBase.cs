using System;
using System.Collections.Generic;
using System.Text;
using Server.Database.Services.Interfaces;
using Server.Database.Models.Interfaces;
using System.Threading.Tasks;
using ChickenAPI.Core.Logging;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Z.EntityFramework.Plus;
using Microsoft.EntityFrameworkCore.Storage;
using EFCore.BulkExtensions;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;

namespace Server.Database.Services.Bases
{
    public class MappedRepositoryBase<TModel> : IMappedRepository<TModel> where TModel : class, IMappedModel, new()
    {
        protected readonly ILogger Log;
        private readonly DbContext _context;
        protected readonly DbSet<TModel> DbSet;

        protected MappedRepositoryBase(DbContext context, ILogger log)
        {
            _context = context;
            Log = log;
            DbSet = context.Set<TModel>();
        }

        //TODO, should remove, only little bypass for now, future will use some cache.
        protected void Refresh(TModel mappedModel)
        {
            _context.Entry(mappedModel).Reload();
        }
        
        public virtual void DeleteByModel(TModel model)
        {
            try
            {
                DbSet.Attach(model);
                DbSet.Remove(model);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                Log.Error("[DELETE_BY_MODEL]", e);
            }
        }

        public virtual void DeleteById(long id)
        {
            try
            {
                var model = new TModel { Id = id };
                DbSet.Attach(model);
                DbSet.Remove(model);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                Log.Error("[DELETE_BY_ID]", e);
            }
        }

        public virtual async Task DeleteByIdAsync(long id)
        {
            try
            {
                var model = new TModel { Id = id };
                DbSet.Attach(model);
                DbSet.Remove(model);
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Log.Error("[DELETE_BY_ID_ASYNC]", e);
            }
        }

        public virtual void DeleteByIds(IEnumerable<long> ids)
        {
            try
            {
                DbSet.Where(f => ids.Contains(f.Id)).Delete();
            }
            catch (Exception e)
            {
                Log.Error("[DELETE_BY_IDS]", e);
            }
        }

        public virtual Task DeleteByIdsAsync(IEnumerable<long> ids)
        {
            try
            {
                return DbSet.Where(f => ids.Contains(f.Id)).DeleteAsync();
            }
            catch (Exception e)
            {
                Log.Error("[DELETE_BY_IDS_ASYNC]", e);
                return Task.CompletedTask;
            }
        }

        public virtual IEnumerable<TModel> Get()
        {
            try
            {
                return DbSet.ToArray();
            }
            catch (Exception e)
            {
                Log.Error("[GET]", e);
                return null;
            }
        }

        public virtual async Task<IEnumerable<TModel>> GetAsync()
        {
            try
            {
                return await DbSet.ToArrayAsync().ConfigureAwait(false);
            }
            catch(Exception e)
            {
                Log.Error("[GET_ASYNC]", e);
                return null;
            }
        }

        public virtual TModel GetById(long id)
        {
            try
            {
                return DbSet.Find(id);
            }
            catch (Exception e)
            {
                Log.Error("[GET_BY_ID]", e);
                return null;
            }
        }

        public virtual async Task<TModel> GetByIdAsync(long id)
        {
            try
            {
                return await DbSet.FindAsync(id).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Log.Error("[GET_BY_ID_ASYNC]", e);
                return null;
            }
        }

        public virtual IEnumerable<TModel> GetByIds(IEnumerable<long> ids)
        {
            try
            {
                return DbSet.Where(s => ids.Contains(s.Id)).ToList();
            }
            catch (Exception e)
            {
                Log.Error("[GET_BY_IDS]", e);
                return null;
            }
        }

        public virtual async Task<IEnumerable<TModel>> GetByIdsAsync(IEnumerable<long> ids)
        {
            try
            {
                return await DbSet.Where(s => ids.Contains(s.Id)).ToListAsync().ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Log.Error("[GET_BY_IDS_ASYNC]", e);
                return null;
            }
        }

        public virtual TModel Save(TModel obj)
        {
            try
            {
                TModel model = DbSet.Find(obj.Id);
                if (model == null)
                {
                    Log.Debug($"[SAVE] Saving a model that couldn't be find with -> 'Type: {typeof(TModel).Name}' 'Id: {obj.Id}'");
                    model = DbSet.Add(obj).Entity;
                }
                else
                {
                    Log.Debug($"[SAVE] Updating model with -> 'Type: {typeof(TModel).Name}' 'Id: {obj.Id}'");
                    _context.Entry(model).CurrentValues.SetValues(obj);
                }

                _context.SaveChanges();
                return model;
            }
            catch (Exception e)
            {
                Log.Error("[SAVE]", e);
                return null;
            }
        }

        public virtual void Save(IEnumerable<TModel> objs)
        {
            try
            {
                if (objs.All(s => s == null))
                {
                    return;
                }

                var tmp = objs.Where(s => s != null).ToList();
                using (var transaction = _context.Database.BeginTransaction())
                {
                    _context.BulkInsertOrUpdate(tmp, new BulkConfig
                    {
                        PreserveInsertOrder = true,
                        SqlBulkCopyOptions = SqlBulkCopyOptions.KeepIdentity
                    });
                    transaction.Commit();
                }

                _context.SaveChanges();
                Log.Debug($"[SAVE_ENUMERABLE] Saved -> {tmp.Count} {typeof(TModel).Name}");
            }
            catch (Exception e)
            {
                Log.Error("[SAVE_ENUMERABLE]", e);
            }
        }

        public virtual async Task<TModel> SaveAsync(TModel obj)
        {
            try
            {
                var model = DbSet.Find(obj.Id);
                if (model == null)
                {
                    Log.Debug($"[SAVE_ASYNC] Saving a model that couldn't be find with -> 'Type: {typeof(TModel).Name}' 'Id: {obj.Id}'");
                    model = DbSet.Add(obj).Entity;
                }
                else
                {
                    Log.Debug($"[SAVE_ASYNC] Updating model with -> 'Type: {typeof(TModel).Name}' 'Id: {obj.Id}'");
                    _context.Entry(model).CurrentValues.SetValues(obj);
                }

                await _context.SaveChangesAsync().ConfigureAwait(false);
                return model;
            }
            catch (Exception e)
            {
                Log.Error("[SAVE_ASYNC]", e);
                return null;
            }
        }

        public virtual async Task SaveAsync(IEnumerable<TModel> objs)
        {
            try
            {
                if (objs.All(s => s == null))
                {
                    return;
                }

                var tmp = objs.Where(s => s != null).ToList();
                await using (var transaction = _context.Database.BeginTransaction())
                {
                    await _context.BulkInsertOrUpdateAsync(tmp, new BulkConfig
                    {
                        PreserveInsertOrder = true
                    });
                    transaction.Commit();
                }

                await _context.SaveChangesAsync().ConfigureAwait(false);
                Log.Debug($"[SAVE_ASYNC_ENUMERABLE] Saved -> {tmp.Count} {typeof(TModel).Name}");
            }
            catch (Exception e)
            {
                Log.Error("[SAVE_ASYNC_ENUMERABLE]", e);
            }
        }
    }
}
