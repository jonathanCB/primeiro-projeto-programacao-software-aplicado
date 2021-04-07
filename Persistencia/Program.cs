
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
            #region "codigo inserts e manipulacao no DB (comentado)"
            /*
            MovieContext _context = new MovieContext();

            Genre g1 = new Genre()
            {
                Name = "Comedia",
                Description = "Filmes de comedia"
            };

            Genre g2 = new Genre()
            {
                Name = "Ficcao",
                Description = "Filmes de ficcao"
            };

            _context.Genres.Add(g1);
            _context.Genres.Add(g2);

            Console.WriteLine("g1.genreId: {0}\n", g1.GenreId);

            _context.SaveChanges();
            
            Console.WriteLine("g1.genreId: {0}\n", g1.GenreId);

            Console.WriteLine("g1: {0}\n", g1.Name);
            List<Genre> genres = _context.Genres.ToList();

            foreach (Genre g in genres)
            {
                Console.WriteLine(String.Format("{0,2} {1,-10} {2}",
                                    g.GenreId, g.Name, g.Description));
            }

            genres[0].Description += "/modificado";

            _context.SaveChanges();

            Console.WriteLine(String.Format("{0,2} {1,-10} {2}",
                       genres[0].GenreId, genres[0].Name, genres[0].Description));

            Movie m1 = new Movie() {

                Title = "Back to the Future",
                Director = "Robert Zemeckis",
                ReleaseDate = new DateTime(1989, 01, 22),
                Gross = 210609762M,
                Rating = 8.5,
                GenreID = 1
        };
            Movie m2 = new Movie()
            {

                Title = "Back to the Future II",
                Director = "Robert Zemeckis",
                ReleaseDate = new DateTime(1989, 01, 22),
                Gross = 210609762M,
                Rating = 8.5,
                GenreID = 1
            };

            _context.Movies.Add(m1);
            _context.Movies.Add(m2);
        _context.SaveChanges();

         Console.WriteLine("m1 id: {0} genero: {1}\n", 
                                m1.MovieId,
                                m1.Genre.Name);
        
        Console.WriteLine(String.Format("Genero: {0} NroFilmes: {1} Filmes: {2}\n",
                      g1.GenreId, g1.Movies.Count, g1.Movies ));

            foreach (Movie m in g1.Movies)
            {
                Console.WriteLine(String.Format("Titulo: {0} Diretor: {1} \n",
                     m.Title, m.Director));
            }
            */
            #endregion

            #region "Consultas (em uso)"
            MovieContext context = new MovieContext();

            Console.WriteLine("\n1.Listar o nome de todos personagens desempenhados" +
                              " por um determinado ator,\nincluindo a informação de qual o filme\n");

            var consulta1 = context.Characters
                             .Where(c => c.ActorId == 9)
                             .Select(c => new
                             {
                                 c.Actor.Name,
                                 c.Character,
                                 c.Movie.Title
                             });

            foreach (var elem in consulta1)
            {
                Console.WriteLine("Nome do ator: {0}\nPersonagem: {1}\nTitulo do filme: {2}\n",
                                    elem.Name, elem.Character, elem.Title);
            }

            Console.WriteLine("\n2.Mostrar o nome de todos os atores que desempenharam um " +
                "determinado personagem\n(por exemplo, quais os atores que já atuaram como \"007\" ?)\n");

            var consulta2 = context.Characters
                            .Where(c => c.Character == "James Bond")
                            .Select(c => new
                            {
                                c.Actor.Name,
                                c.Character
                            });

            foreach(var elem in consulta2)
            {
                Console.WriteLine("Nome do Ator: {0}\nPersonagem: {1}\n", elem.Name, elem.Character);
            }

            Console.WriteLine("\n3.Informar qual o ator que desempenhou mais vezes um determinado personagem" +
                "(por exemplo: qual o ator que realizou mais filmes como o “agente 007”)\n");

            var consulta3 = context.Characters
                           .Where(c => c.Character == "James Bond");

            var consulta3_1 = from a in consulta3
                              group a by a.Actor.Name into grp
                              select new
                              {
                                  actor = grp.Key,
                                  qtde = grp.Count()
                              };              

            foreach (var elem in consulta3_1)
            {
                Console.WriteLine("Ator: {0}  \t Numero de atuacoes: {1}\n", elem.actor, elem.qtde);
            }

            Console.WriteLine("\n4.Mostrar o nome e a data de nascimento do ator mais idoso e o mais novo\n");

            var consulta4 = (from m in context.Actors
                             select m.DateBirth).Max();

            var consulta4_1 = (from m in context.Actors
                              where m.DateBirth == consulta4
                              select m.Name).FirstOrDefault();

            var consulta4_2 = (from m in context.Actors
                             select m.DateBirth).Min();

            var consulta4_3 = (from m in context.Actors
                              where m.DateBirth == consulta4_2
                              select m.Name).FirstOrDefault();

            Console.WriteLine("Nome do Ator:{0}   Data de nascimento:{1}" +
                              "\nNome do Ator:{2}   Data de nascimento:{3}",
                              consulta4_1, consulta4, consulta4_3, consulta4_2);

            #endregion

        }

    }
}

