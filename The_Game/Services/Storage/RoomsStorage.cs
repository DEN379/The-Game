using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace The_Game.Services.Storage
{
   
    public abstract class RoomsStorage<T> : Storage<T> where T : class
    {
        public new ConcurrentDictionary<Guid, T> DataBase = new ConcurrentDictionary<Guid, T>();

        public async Task<T> Get(Guid key)
        {
            return DataBase.TryGetValue(key, out var item) ? item : default;
        }

        public async Task<T> GetAsync(Guid key)
        {
            return await Get(key);
        }
        public Guid Add(T item)
        {
            var id = Guid.NewGuid();
            DataBase.TryAdd(id, item);
            return id;
        }

        public Task<Guid> AddAsync(T item)
        {
            return Task.FromResult(Add(item));
        }

        public void AddOrUpdate(Guid id, T item)
        {
            DataBase[id] = item;
        }

        public Task AddOrUpdateAsync(Guid key, T item)
        {
            AddOrUpdate(key, item);
            return Task.CompletedTask;
        }

        public bool Delete(Guid key)
        {
            return DataBase.TryRemove(key, out _);
        }

        public Task<bool> DeleteAsync(Guid key)
        {
            return Task.FromResult(Delete(key));
        }

        public void AddWithGuid(Guid key, T item)
        {
            DataBase.TryAdd(key, item);
        }
        public Task AddWithGuidAsync(Guid key, T item)
        {
            AddWithGuid(key, item);
            return Task.CompletedTask;
        }
    }
}