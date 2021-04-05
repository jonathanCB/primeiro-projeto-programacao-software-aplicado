
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
            //TESTE
            
            /*1 - Listar o nome de todos personagens desempenhados por um
            determinado ator, incluindo a informação de qual o filme.*/
            Console.WriteLine("1 - Listar o nome de todos personagens desempenhados por um determinado ator, incluindo a informação de qual o filme.");
            var characters = _context.Characters.Where(n => n.ActorId == 12);
            foreach (ActorMovie personagensTomHanks in characters)
            {
                Console.WriteLine("\t{0}", personagensTomHanks.Character);
            }
        }
    }
}

