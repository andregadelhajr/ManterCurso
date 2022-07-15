using APICurso.Models.Identidade;

namespace APICurso.Interface
{
    public interface ITokenService
    {
        string CreateToken(Usuario user);
    }
}