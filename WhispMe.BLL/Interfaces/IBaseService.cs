namespace WhispMe.BLL.Interfaces;

public interface IBaseService<T>
    where T : class
{
    Task<T> CreateAsync(T entity);

    Task<T> UpdateAsync(string id, T entity);
    
    Task<T> DeleteByIdAsync(string id);
    
    Task<T> GetByIdAsync(string id);

    Task<IEnumerable<T>> GetWithPaginationAsync(int pageNumber, int pageSize);
}
