using Microsoft.EntityFrameworkCore;

namespace APICurso.Models
{
    public class CursoContext : DbContext
    {
        public CursoContext(DbContextOptions<CursoContext> options):  base(options) { }

        public DbSet<Curso> Cursos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Log> Logs { get; set; }
    }
}