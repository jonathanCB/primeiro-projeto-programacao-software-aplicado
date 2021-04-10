
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

            var consulta3_1 = (from a in consulta3
                              group a by a.Actor.Name into grp
                              select new 
                              {
                                  actor = grp.Key,
                                  qtde = grp.Count()                                  
                              }).OrderByDescending (a => a.qtde).Take(1);              

            foreach (var elem in consulta3_1)
            {
                Console.WriteLine("Ator: {0}\nNumero de atuacoes: {1}\n", elem.actor, elem.qtde);
            }



            Console.WriteLine("\n4.Mostrar o nome e a data de nascimento do ator mais idoso e o mais novo\n");

            var consulta4 = (from m in context.Actors
                             select m.DateBirth).Max();

            var consulta4_1 = (from m in context.Actors
                               select m.DateBirth).Min();

            var consulta4_2 = (from m in context.Actors
                              where m.DateBirth == consulta4
                              select m.Name).FirstOrDefault();

            var consulta4_3 = (from m in context.Actors
                              where m.DateBirth == consulta4_1
                              select m.Name).FirstOrDefault();

            Console.WriteLine("Nome do Ator:{0}   Data de nascimento:{1}" +
                              "\nNome do Ator:{2}   Data de nascimento:{3}\n",
                              consulta4_3, consulta4_1, consulta4_2, consulta4);




            Console.WriteLine("\n5.Mostrar o nome e a data de nascimento do ator mais idoso " +
                                "e o mais novo de um determinado gênero");

            String genero = "Action";
            var consulta5 = (from cha in context.Characters
                            .Include(m => m.Movie)
                            .ThenInclude(g => g.Genre)
                            .Include(a => a.Actor)
                             where cha.Movie.Genre.Name == genero
                             select cha.Actor.DateBirth).Max();

            var consulta5_1 = (from cha in context.Characters
                            .Include(m => m.Movie)
                            .ThenInclude(g => g.Genre)
                            .Include(a => a.Actor)
                             where cha.Movie.Genre.Name == genero
                             select cha.Actor.DateBirth).Min();

            var consulta5_2 = (from a in context.Actors
                               where a.DateBirth == consulta5
                               select a.Name).FirstOrDefault();

            var consulta5_3 = (from a in context.Actors
                               where a.DateBirth == consulta5_1
                               select a.Name).FirstOrDefault();

            Console.WriteLine("\nMais jovem do genero de: {0}\nNome: {1}\nData de Nascimento: {2}\n" +
                              "\nMais idoso do genero de: {3}\nNome: {4}\nData de Nascimento: {5}\n", 
                                genero, consulta5_2, consulta5, genero, consulta5_3, consulta5_1);




            Console.WriteLine("\n6.Mostrar o valor médio das avaliações dos filmes que um " +
                                "determinado ator participou");

            String ator = "Harrison Ford";
            var consulta6 = (from m in context.Characters
                              .Include(a => a.Movie)
                              where m.Actor.Name == ator
                              select m.Movie.Rating).Average();

            
            Console.WriteLine("\nMedia de avaliacoes dos filmes em que o ator {0} participou: {1}\n",
                                ator, consulta6.ToString("#0.0"));




            Console.WriteLine("\n7.Qual o elenco do filme PIOR avaliado?\n");

            var consulta7 = (from m in context.Movies
                             select m.Rating).Min();

            var consulta7_1 = (from m in context.Movies
                              where m.Rating == consulta7
                              select m.MovieId).FirstOrDefault();

            var consulta7_2 = from m in context.Characters
                              .Include(a => a.Actor)
                              where m.MovieId == consulta7_1
                              select m.Actor.Name;

            Console.WriteLine("elenco do filme com ID: {0}\ncom uma avaliacao de: {1}",
                                consulta7_1, consulta7);
            foreach (var elem in consulta7_2)
            {
                Console.WriteLine(elem);
            }




            Console.WriteLine("\n\n8.Qual o elenco do filme com o pior faturamento?\n");

            var consulta8 = (from m in context.Movies
                             select m.Gross).Min();

            var consulta8_1 = (from m in context.Movies
                               where m.Gross == consulta8
                               select m.MovieId).FirstOrDefault();

            var consulta8_2 = from m in context.Characters
                              .Include(a => a.Actor)
                              where m.MovieId == consulta8_1
                              select m.Actor.Name;

            Console.WriteLine("elenco do filme com ID: {0}\ncom o pior faturamento sendo de: {1}",
                                consulta8_1, consulta8);
            foreach (var elem in consulta8_2)
            {
                Console.WriteLine(elem);
            }

            #endregion

        }

    }
}

