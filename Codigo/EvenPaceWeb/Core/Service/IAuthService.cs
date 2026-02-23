using System.Security.Claims;

namespace Core.Service
{
    public interface IAuthService
    {
        /// <summary>
        /// Realiza o login do usuário com base no e-mail e senha informados.
        /// </summary>
        Task<bool> LoginAsync(string email, string senha);

        /// <summary>
        /// Realiza o encerramento da sessão do usuário atual.
        /// </summary>
        Task LogoutAsync();

        /// <summary>
        /// Obtém o e-mail do usuário autenticado a partir dos claims contextuais.
        /// </summary>
        Task<string> GetUsuarioLogadoEmailAsync(ClaimsPrincipal user);
    }
}