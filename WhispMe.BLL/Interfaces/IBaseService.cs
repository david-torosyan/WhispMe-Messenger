namespace WhispMe.BLL.Interfaces;

public interface IBaseService<T>
    where T : class
{
    Task<T> CreateAsync(T entity);

    Task<T> UpdateAsync(T entity);
    
    Task<T> DeleteAsync(T entity);
    
    Task<T> GetByIdAsync(Guid id);

    Task<IEnumerable<T>> GetWithPaginationAsync(int pageNumber, int pageSize);
}
