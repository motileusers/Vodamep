namespace Vodamep.Summaries
{
    public interface ISummaryFactory<T>
    {
        Task<Summary> Create(T model);
    }
}