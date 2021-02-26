using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using The_Game.Models;

namespace The_Game.Services.Storage
{
    public abstract class Storage<T> : IStorage<T> where T:class 
    {
        protected  ConcurrentDictionary<int, T> DataBase = new ConcurrentDictionary<int, T>();

        
        public async Task<IEnumerable<ItemWithKey<T>>> GetAll()
        {
            return DataBase.Select(x => new ItemWithKey<T> {Id = x.Key, Item = x.Value}).ToArray();
        }

        public ConcurrentDictionary<int, T> GetDictionary()
        {
            return DataBase;
        }
        public async Task<IEnumerable<ItemWithKey<T>>> GetAllAsync()
        {
            return await GetAll();
        }

        public async Task<T> Get(int key)
        {
            return DataBase.TryGetValue(key, out var item) ? item : default;
        }

        public async Task<T> GetAsync(int key)
        {
            return await Get(key);
        }
        public int Add(T item)
        {
            var id = DataBase.Keys.Any() ? DataBase.Keys.Max() + 1 : 1;
            DataBase.TryAdd(id,item);
            return id;
        }

        public Task<int> AddAsync(T item)
        {
            return Task.FromResult(Add(item));
        }

        public void AddOrUpdate(int id, T item)
        {
            DataBase[id] = item;
        }

        public Task AddOrUpdateAsync(int key, T item)
        {
            AddOrUpdate(key, item);
            return Task.CompletedTask;
        }

        public bool Delete(int key)
        {
            return DataBase.TryRemove(key,out _);
        }

        public Task<bool> DeleteAsync(int key)
        {
            return Task.FromResult(Delete(key));
        }

    }
}