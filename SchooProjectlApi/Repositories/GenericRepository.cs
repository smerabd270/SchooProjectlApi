using Microsoft.EntityFrameworkCore;
using SchooProjectlApi.Data;

namespace SchooProjectlApi.Repositories;
public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly SchoolContext _db;
    protected readonly DbSet<T> _set;
    public GenericRepository(SchoolContext db) { _db = db; _set = db.Set<T>(); }
    public IQueryable<T> Query() => _set.AsQueryable();
    public async Task<T?> GetByIdAsync(int id) => await _set.FindAsync(id);
    public async Task AddAsync(T entity) => await _set.AddAsync(entity);
    public void Update(T entity) => _set.Update(entity);
    public void Remove(T entity) => _set.Remove(entity);
    public Task SaveChangesAsync() => _db.SaveChangesAsync();
}
