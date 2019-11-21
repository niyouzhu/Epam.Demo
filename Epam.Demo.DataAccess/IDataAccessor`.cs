namespace Epam.Demo.DataAccess
{
    public interface IDataAccessor<TEntity>
    {
        TEntity Add(TEntity entity);
    }
}