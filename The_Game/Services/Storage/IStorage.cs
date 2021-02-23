using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace The_Game.Models
{
    public interface IStorage<T>
    {
        public  Task<IEnumerable<ItemWithKey<T>>> GetAll();

        public ConcurrentDictionary<int, T> GetDictionary();

        public  Task<IEnumerable<ItemWithKey<T>>> GetAllAsync();


        public  Task<T> Get(int key);


        public  Task<T> GetAsync(int key);

        public int Add(T item);

        public Task<int> AddAsync(T item);


        public void AddOrUpdate(int id, T item);



        public Task AddOrUpdateAsync(int key, T item);


        public bool Delete(int key);


        public Task<bool> DeleteAsync(int key);
    }

}