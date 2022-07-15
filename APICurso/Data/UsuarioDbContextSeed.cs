using System.Linq;
using System.Threading.Tasks;
using APICurso.Models.Identidade;
using Microsoft.AspNetCore.Identity;

namespace APICurso.Data
{
    public class UsuarioDbContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<Usuario> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new Usuario
                {
                    NomeExibicao = "André",
                    Email = "andre@test.com",
                    UserName = "andre@test.com",
                };

                await userManager.CreateAsync(user, "12345678");
            }
        }
    }
}