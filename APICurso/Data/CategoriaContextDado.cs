using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using APICurso.Models;
using Microsoft.Extensions.Logging;

namespace APICurso.Data
{
    public class CategoriaContextDado
    {
        public static async Task SeedAsync(CursoContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                if (!context.Categorias.Any())
                {
                    var CategoriasData =
                        File.ReadAllText("../Data/Dado/Categoria.json");
                    var categorias = JsonSerializer.Deserialize<List<Categoria>>(CategoriasData);

                    foreach (var item in categorias)
                    {
                        context.Categorias.Add(item);
                    }

                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<CategoriaContextDado>();
                logger.LogError(ex.Message);
            }
        }
    }
}