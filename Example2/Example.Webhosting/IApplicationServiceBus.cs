using System.Threading.Tasks;

namespace Example.Webhosting
{
    public interface IApplicationServiceBus
    {
        Task<TResponse> GetAsync<TResponse>(object request);

        Task<TResponse> PutAsync<TResponse>(object request);

        Task<TResponse> PostAsync<TResponse>(object request);

        Task<TResponse> DeleteAsync<TResponse>(object request);
    }
}