using Microsoft.AspNetCore.Identity;

namespace APICurso.Models.Identidade
{
    public class Usuario : IdentityUser
    {  
        public string NomeExibicao { get; set; }
    }
}