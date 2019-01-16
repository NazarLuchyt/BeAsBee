using System.Security.Claims;
using System.Threading.Tasks;

namespace BeAsBee.Domain.Interfaces.Services {
    public interface IJwtService {
        Task<string> GenerateEncodedToken ( string userName, ClaimsIdentity identity, string role );
        ClaimsIdentity GenerateClaimsIdentity ( string userName, string role );
    }
}