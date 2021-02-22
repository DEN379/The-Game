using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using The_Game.Classes;

namespace The_Game.Services
{
    public class ItemWithKey<T> 
    {
        public int Id { get; set; }
        public T item { get; set; }

    }
    public abstract class Storage<T> where T:class
    {
        protected  ConcurrentDictionary<int, T> _dbPlayRooms = new ConcurrentDictionary<int, T>();

        
        public async Task<IEnumerable<ItemWithKey<T>>> GetAll()
        {
            return _dbPlayRooms.Select(x => new ItemWithKey<T> {Id = x.Key, item = x.Value}).ToArray();
        }

        public ConcurrentDictionary<int, T> GetDictionary()
        {
            return _dbPlayRooms;
        }
        public async Task<IEnumerable<ItemWithKey<T>>> GetAllAsync()
        {
            return await GetAll();
        }

        public async Task<T> Get(int key)
        {
            return _dbPlayRooms.TryGetValue(key, out var item) ? item : default;
        }

        public async Task<T> GetAsync(int key)
        {
            return await Get(key);
        }
        public int Add(T item)
        {
            var id = _dbPlayRooms.Keys.Any() ? _dbPlayRooms.Keys.Max() + 1 : 1;
            _dbPlayRooms.TryAdd(id,item);
            return id;
        }

        public Task<int> AddAsync(T item)
        {
            return Task.FromResult(Add(item));
        }

        public void AddOrUpdate(int id, T item)
        {
            _dbPlayRooms[id] = item;
        }

        public Task AddOrUpdateAsync(int key, T item)
        {
            AddOrUpdate(key, item);
            return Task.CompletedTask;
        }

        public bool Delete(int key)
        {
            return _dbPlayRooms.TryRemove(key,out _);
        }

        public Task<bool> DeleteAsync(int key)
        {
            return Task.FromResult(Delete(key));
        }
    }
}