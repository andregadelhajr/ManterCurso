using System.Collections.Generic;

namespace APICurso.Models
{
    public class Categoria
    {
        public int CategoriaId { get; set; }
        public string Nome { get; set; }

        public ICollection<Curso> Cursos { get; set; }
    }
}