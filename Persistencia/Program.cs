
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

            var a2 = (from a in _context.Characters
                      where a.Character == "James Bond"
                      group a by a.Actor.Name into grupo
                      select new
                      {
                          Nome = grupo.Key,
                          NroPersonagens = grupo.Count()
                      })
                     .OrderByDescending(a => a.NroPersonagens)
                     .FirstOrDefault();

            /*Primeira forma: Colocar o método .OrderByDescending e o .FirstOrDefault()
            diretamente na consulta e mostrar.)*/
            Console.WriteLine("\n Nome do ator que realizou mais vezes o personagem: {0}\n " +
                        "QtdFilmes: {1}", a2.Nome, a2.NroPersonagens);

            //------------------------------------------------------------------------------------------

            /*Segunda forma: Tirar o o método .OrderByDescending e o .FirstOrDefault()
            da consulta e trabalhar com variáveis.*/

            /*String nomeAtor = a2.FirstOrDefault().Nome;
            int qtdVezesComoPersonagem = a2.FirstOrDefault().NroPersonagens;
            Console.WriteLine("\n Nome do ator que realizou mais vezes o personagem: {0}\n " +
            "QtdFilmes: {1}", nomeAtor, qtdVezesComoPersonagem);*/

            //------------------------------------------------------------------------------------------

            //Terceira forma:
            /*int maisVezes = 0;
            foreach (var ator in a2.OrderByDescending(a => a.NroPersonagens))
            {
                if(ator.NroPersonagens > maisVezes)
                {
                    maisVezes = ator.NroPersonagens;
                    Console.WriteLine("\n Nome do ator que realizou mais vezes o personagem: {0}\n " +
                        "QtdFilmes: {1}", ator.Nome, ator.NroPersonagens);
                }
            };*/

            //------------------------------------------------------------------------------------------

            //Quarta forma: Mostrar todos os atores que desempenharam o personagem escolhido na ordem decrescente:
            /*foreach (var ator in a2.OrderByDescending(a => a.NroPersonagens))
            {
                    Console.WriteLine("\n Nome do ator: {0}\n QtdFilmes: {1}", ator.Nome, ator.NroPersonagens);
            };*/

            #endregion

            #region Consulta 4
            Console.WriteLine("\n 4 - Mostrar o nome e a data de nascimento do ator mais idoso e o mais novo ");

            //Ator mais idoso
            var dataMenor = (from a in _context.Actors
                             select a.DateBirth).Min();

            String atorMaisVelho = (from a in _context.Actors
                                    where a.DateBirth == dataMenor
                                    select a.Name).FirstOrDefault();

            Console.WriteLine("\n Nome do ator mais velho: {0}\n Data de nascimento: {1}",
                atorMaisVelho, dataMenor.ToShortDateString());

            //Ator mais novo
            var dataMaior = (from a in _context.Actors
                             select a.DateBirth).Max();

            String atorMaisNovo = (from a in _context.Actors
                                   where a.DateBirth == dataMaior
                                   select a.Name).FirstOrDefault();

            Console.WriteLine("\n Nome do ator mais novo: {0}\n Data de nascimento: {1}",
                atorMaisNovo, dataMaior.ToShortDateString());

            /*
            //Ator mais velho
            var a3 = (from a in _context.Actors
                     group a by a.Name into grupo
                     select new
                     {
                         Nome = grupo.Key,
                         DataNascimentoAtorMaisVelho = grupo.Min(t => t.DateBirth)
                     })
                     .OrderByDescending(a => a.DataNascimentoAtorMaisVelho)
                     .FirstOrDefault();

            Console.WriteLine("\n Nome do ator mais novo: {0}\n Data de nascimento: {1}", 
                a3.Nome, a3.DataNascimentoAtorMaisVelho);

            //Ator mais novo
            var a4 = (from a in _context.Actors
                      group a by a.Name into grupo
                      select new
                      {
                          Nome = grupo.Key,
                          DataNascimentoAtorMaisNovo = grupo.Max(t => t.DateBirth)
                      })
                     .OrderByDescending(a => a.DataNascimentoAtorMaisNovo)
                     .FirstOrDefault();

            Console.WriteLine("\n Nome do ator mais novo: {0}\n Data de nascimento: {1}",
                a4.Nome, a4.DataNascimentoAtorMaisNovo);
            */
            #endregion

            #region Consulta 5
            Console.WriteLine("\n 5 - Mostrar o nome e a data de nascimento do ator mais idoso " +
                "e o mais novo de um determinado gênero.\n ");

            String genero = "Action";
            var youngActorBirthday = (from c in _context.Characters
                             .Include(m => m.Movie)
                             .ThenInclude(g => g.Genre)
                             .Include(ac => ac.Actor)
                                      where c.Movie.Genre.Name == genero
                                      select c.Actor.DateBirth).Max();

            String youngActorName = (from a in _context.Actors
                                     where a.DateBirth == youngActorBirthday
                                     select a.Name).FirstOrDefault();

            Console.WriteLine(" Nome do ator mais jovem do gênero '{0}': {1}" +
                "\n Data de nascimento: {2}", genero, youngActorName,
                    youngActorBirthday.ToShortDateString());

            var oldActorBirthday = (from c in _context.Characters
                                    .Include(m => m.Movie)
                                    .ThenInclude(g => g.Genre)
                                    .Include(a => a.Actor)
                                    where c.Movie.Genre.Name == genero
                                    select c.Actor.DateBirth).Min();

            String oldActorName = (from a in _context.Actors
                                   where a.DateBirth == oldActorBirthday
                                   select a.Name).FirstOrDefault();

            Console.WriteLine("\n Nome do ator mais idoso do gênero '{0}': {1}" +
                "\n Data de nascimento: {2}", genero, oldActorName,
                    oldActorBirthday.ToShortDateString());

            #endregion

            #region Consulta 6
            Console.WriteLine("\n 6 - Mostrar o valor médio das avaliações dos filmes " +
                "que um determinado ator participou.\n ");

            String actor = "Carrie Fisher";
            var averageAvaliation = (from aa in _context.Characters
                                     where aa.Actor.Name == actor
                                     select aa.Movie.Rating).Average();

            Console.WriteLine(" Nome do ator: {0}\n Média de avaliações dos filmes que " +
                "participou: {1}", actor, averageAvaliation.ToString("F"));

            #endregion

            #region Consulta 7
            Console.WriteLine("\n 7 - Qual o elenco do filme PIOR avaliado?.\n ");
            var worstMovie = (from m in _context.Movies
                              select m.Rating).Min();

            var mName = (from m in _context.Movies
                         where m.Rating == worstMovie
                         select m.Title).FirstOrDefault();

            var actorsWorstMovie = from m in _context.Characters
                                   where m.Movie.Rating == worstMovie
                                   select m.Actor.Name;

            /*Segunda opção de fazer, utilizando Include:
            var teste = from m in _context.Characters
                              .Include(a => a.Actor)
                              where m.Movie.Rating == worstMovie
                              select m.Actor.Name;*/

            Console.WriteLine(" Filme: {0}\n", mName);
            Console.WriteLine(" Avaliação: {0}\n", worstMovie);
            Console.WriteLine(" Elenco:\n");
            foreach (var actorMovie in actorsWorstMovie)
            {
                Console.WriteLine(" " + actorMovie);
            }
            #endregion

            #region Consulta 8
            Console.WriteLine("\n 8 - Qual o elenco do filme com o pior faturamento??\n ");
            var gross = (from m in _context.Movies
                         select m.Gross).Min();

            var movieName = (from m in _context.Movies
                             where m.Gross == gross
                             select m.Title).FirstOrDefault();

            var actorsMovie = from m in _context.Characters
                              where m.Movie.Gross == gross
                              select m.Actor.Name;

            Console.WriteLine(" Filme: {0}\n", movieName);
            Console.WriteLine(" Faturamento: {0}\n", gross);
            Console.WriteLine(" Elenco:\n");
            foreach (var actorMovie in actorsMovie)
            {
                Console.WriteLine(" " + actorMovie);
            }

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
            Console.WriteLine("\n 10 - Qual o faturamento total de todos os filmes juntos?\n ");

            var c10 = (from m in _context.Movies
                      where m.Director == "Martin Campbell"  
                      select m.Gross).Sum();
            
            

            
            Console.WriteLine("Os filmes do diretor Martin Campbell renderam um montante de {0}",c10); 
            
            

            #endregion
        }
    }
}