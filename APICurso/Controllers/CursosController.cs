using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APICurso.Models;

namespace APICurso.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CursosController : ControllerBase
    {
        private readonly CursoContext _context;

        public CursosController(CursoContext context)
        {
            _context = context;
        }

        // GET: api/Cursos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Curso>>> GetCursos()
        {
            return await _context.Cursos.Include(x => x.Categoria).ToListAsync();
        }

        // GET: api/Cursos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Curso>> GetCurso(int id)
        {
            var curso = await _context.Cursos.FindAsync(id);

            if (curso == null)
            {
                return NotFound();
            }

            return curso;
        }

        // PUT: api/Cursos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCurso(int id, Curso curso)
        {

            if (id != curso.CursoId)
            {
                return BadRequest();
            }

            _context.Cursos.Update(curso);

            try
            {
                await _context.SaveChangesAsync();

                var log = await _context.Logs.FindAsync(curso.CursoId);
                log.DtAtualizacao = DateTime.Now;
                _context.Logs.Update(log);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CursoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Cursos
        [HttpPost]
        public async Task<ActionResult<Curso>> PostCurso(Curso curso)
        {

            Boolean AgendaCheia = (_context.Cursos.Any(c => c.DtInicial <= curso.DtFinal && c.DtFinal >= curso.DtInicial || c.DtInicial == curso.DtInicial && c.DtFinal == curso.DtFinal));

            Boolean NomeIgual = (_context.Cursos.Any(c => c.Descricao == curso.Descricao && c.Status == true));

            if (AgendaCheia)
            {
                return new BadRequestObjectResult(new {Errors = new[] {"Existe(m) curso(s) planejados(s) dentro do período informado."}});

            }else if (DateTime.Now > curso.DtInicial && DateTime.Now > curso.DtFinal) 
            {
                return new BadRequestObjectResult(new {Errors = new[] {"Data Inicial menor que a data Atual."}});

            }else if (curso.DtFinal < curso.DtInicial) 
            {
                return new BadRequestObjectResult(new {Errors = new[] {"Data Final menor que a data Inicial."}});

            }else if (NomeIgual)
            {
                return new BadRequestObjectResult(new {Errors = new[] {"Já existe um curso com este nome cadastrado."}});
            }
            else 
            {
                 _context.Cursos.Add(curso);
                await _context.SaveChangesAsync();

                var log = new Log() 
                {
                    CursoId = curso.CursoId,
                    DtInclusao = DateTime.Now,
                    Usuario = "Admin"
                };

                _context.Logs.Add(log);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetCurso", new { id = curso.CursoId }, curso);
            }
        }

        // DELETE: api/Cursos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCurso(int id)
        {
            var curso = await _context.Cursos.FindAsync(id);

            if (id != curso.CursoId)
            {
                return BadRequest();
            } 
            // else if ((curso.DtFinal > DateTime.Now && DateTime.Now > curso.DtInicial) || DateTime.Now > curso.DtFinal)
            // {
            //     return BadRequest(error:"Curso já finalizado.");
            // }
            else if (curso.Status == false)
            {
                return BadRequest(error:"Curso já finalizado.");
            }
            else
            {
                curso.Status = false;
                _context.Cursos.Update(curso);
                await _context.SaveChangesAsync();

                var log = await _context.Logs.FindAsync(curso.CursoId);
                log.DtAtualizacao = DateTime.Now;
                _context.Logs.Update(log);
                await _context.SaveChangesAsync();
            }

            return NoContent();
        }

        private bool CursoExists(int id)
        {
            return _context.Cursos.Any(e => e.CursoId == id);
        }

        [HttpGet("ativos/")]
        public async Task<ActionResult<IEnumerable<Curso>>> GetCursosAtivos()
        {
            return await _context.Cursos.Include(x => x.Categoria).Where(c => c.Status == true).ToListAsync();
        }

        [HttpGet("finalizados/")]
        public async Task<ActionResult<IEnumerable<Curso>>> GetCursosFinalizados()
        {
            return await _context.Cursos.Include(x => x.Categoria).Where(c => c.Status == false).ToListAsync();
        }
    }
}
