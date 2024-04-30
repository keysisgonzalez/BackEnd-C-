using Sabio.Models.Requests.SVIAddRequest;

namespace Sabio.Services
{
    public interface ISVIService
    {
        int Add(SVIAddRequest model);
    }
}