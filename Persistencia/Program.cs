
using Persistencia.Entidades;
using Persistencia.Repositorio;
using System.Linq;

using System;
using System.Collections.Generic;

namespace Persistencia
{
    class Program
    {
        static void Main(string[] args)
        {
            MovieContext _context = new MovieContext();

            #region Consulta 1
            Console.WriteLine("1 - Listar o nome de todos personagens desempenhados por um determinado " +
                "ator (Carrie Fischer), incluindo a informação de qual o filme.");

            var c1 = from c in _context.Characters
                     where c.Actor.Name == "Carrie Fisher"
                     select new
                     {
                         c.Character,
                         c.Movie.Title

                     };
            
            foreach (var personagens in c1)
            {
                Console.WriteLine("{0}", personagens);
            }
            #endregion

            #region Consulta 2
            #endregion
        }
    }
}

