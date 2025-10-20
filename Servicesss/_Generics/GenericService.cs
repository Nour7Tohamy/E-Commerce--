using E_Commerce.Repository._Generics;

namespace E_Commerce.Service.Classes
{
    public class GenericService<T> : IGenericService<T> where T : class
    {
        private readonly IGenericRepository<T> _repository;

        public GenericService(IGenericRepository<T> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task AddAsync(T entity)
        {
            await _repository.AddAsync(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            _repository.Update(entity);
            await _repository.SaveChangesAsync(); 
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);

            if (entity == null)
                return false; 

            _repository.Delete(entity);
            await _repository.SaveChangesAsync();
            return true; 
        }
    }
}