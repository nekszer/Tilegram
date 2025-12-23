using System;
using System.Threading.Tasks;

namespace Tilegram.Feature.Authentication
{
    public interface IAuthenticationService
    {
        Task<Either<Exception, AuthenticationResponse>> LogIn(AuthenticationRequest request);
    }
}