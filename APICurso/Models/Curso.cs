using System;

namespace APICurso.Models
{
    public class Curso
    {
        public int CursoId { get; set; }
        public string Descricao { get; set; }
        public DateTime DtInicial{ get; set; }
        public DateTime DtFinal { get; set; }
        public int QtdAlunos { get; set; }
        public bool Status { get; set; } = true;

        public Categoria Categoria { get; set; }
        public int CategoriaId { get; set; }

    }
}