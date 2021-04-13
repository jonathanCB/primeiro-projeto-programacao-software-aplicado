
using Persistencia.Entidades;
using Persistencia.Repositorio;
using System.Linq;

using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Persistencia
{
    class Program
    {
        static void Main(string[] args)
        {
            MovieContext _context = new MovieContext();

            // CONSULTAS

            #region Consulta 1
            Console.WriteLine(" 1 - Listar o nome de todos personagens desempenhados por um determinado " +
                "ator (Carrie Fischer), incluindo a informação de qual o filme.\n");

            var c1 = from c in _context.Characters
                     where c.Actor.Name == "Carrie Fisher"
                     select new
                     {
                         c.Actor.Name,
                         c.Character,
                         c.Movie.Title
                     };
            int con1 = 1;
            foreach (var personagens in c1)
            {
                Console.WriteLine(" {0} - {1} estrelando {2} como {3}.", con1, personagens.Title,
                    personagens.Name, personagens.Character);
                con1++;
            }
            #endregion

            #region Consulta 2
            Console.WriteLine("\n 2 - Mostrar o nome de todos atores que desempenharam um determinado " +
                "personagem (por exemplo, quais os atores que já atuaram como '007'?)\n");

            var c2 = from c in _context.Characters
                     where c.Character == "James Bond"
                     select new
                     {
                         c.Actor.Name
                     };
                     

            foreach(var atores in c2)
            {
                Console.WriteLine("O ator {0} fez o papel de 007", atores.Name);
            }

                     #endregion

                     #region Consulta 3
            Console.WriteLine("\n 3 - Informar qual o ator desempenhou mais vezes um determinado " +
                "personagem (por exemplo: qual o ator que realizou mais filmes como o 'agente 007') ");


            #endregion

            #region Consulta 4
            Console.WriteLine("\n 4 - Mostrar o nome e a data de nascimento do ator mais idoso e o mais novo ");



            #endregion

            #region Consulta 5
            Console.WriteLine("\n 5 - Mostrar o nome e a data de nascimento do ator mais idoso " +
                "e o mais novo de um determinado gênero.\n ");


            #endregion
            #region Consulta 6
            Console.WriteLine("\n 6 - Mostrar o valor médio das avaliações dos filmes " +
                  "que um determinado ator participou.\n ");


            #endregion

            #region Consulta 7
            Console.WriteLine("\n 7 - Qual o elenco do filme PIOR avaliado?.\n ");


            #endregion

            #region Consulta 8
            Console.WriteLine("\n 8 - Qual o elenco do filme com o pior faturamento??\n ");


            #endregion

            #region Consulta 9
            Console.WriteLine("\n 9 - Quais os 3 filmes com maior faturamento, seu gênero e diretor?\n ");
            using (MovieContext context = new MovieContext())
            {
                var betterMovies = (from m in context.Movies
                              .Include(g => g.Genre)
                                    orderby m.Gross
                                    select new
                                    {
                                        m.Title,
                                        m.Gross,
                                        m.Genre.Name,
                                        m.Director
                                    }).OrderByDescending(m => m.Gross).Take(3);

                int cont = 1;
                foreach (var movie in betterMovies)
                {
                    Console.WriteLine(" {0}º lugar", cont);
                    Console.WriteLine(" Filme: {0}\n Faturamento: {1}\n Gênero: {2}\n Diretor: {3}\n",
                        movie.Title, movie.Gross, movie.Name, movie.Director);
                    cont++;
                }
            }


            #endregion

            #region Consulta 10
            Console.WriteLine(" 1 - Listar o nome de todos personagens desempenhados por um determinado " +
                "ator (Carrie Fischer), incluindo a informação de qual o filme.\n");


            #endregion
        }
    }
}