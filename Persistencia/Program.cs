
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
                         c.Actor.Name,
                         c.Character,
                         c.Movie.Title
                     };
            foreach (var personagens in c1)
            {
                Console.WriteLine("\n {0} estrelando {1} como {2}.\n", personagens.Title,
                    personagens.Name, personagens.Character);
            }
            #endregion

            #region Consulta 2
            Console.WriteLine("\n2 - Mostrar o nome de todos atores que desempenharam um determinado " +
                "personagem (por exemplo, quais os atores que já atuaram como '007'?)");
            var a1 = from a in _context.Characters
                     where a.Character == "James Bond"
                     select new
                     {
                         a.Movie.Title,
                         a.Actor.Name,
                         a.Character
                     };

            foreach (var ator in a1)
            {
                Console.WriteLine("\n {0} estrelando {1} como {2}\n", ator.Title, ator.Name, ator.Character);
            }
            #endregion
        }
    }
}

