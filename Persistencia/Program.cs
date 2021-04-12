
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
            MovieContext mv_context = new MovieContext();

            //Query 1
            Console.WriteLine("\n1.Listar o nome de todos personagens desempenhados por um determinado ator, incluindo a informação de qual o filme");

            var consulta1 = mv_context.Characters
                             .Where(c => c.ActorId == 9)
                             .Select(c => new
                             {
                                 c.Actor.Name,
                                 c.Character,
                                 c.Movie.Title
                             });

            foreach (var elem in consulta1)
            {
                Console.WriteLine("O ator {0} desempenhou o personagem {1} do filme {2}\n",
                                    elem.Name, elem.Character, elem.Title);
            }


            //Query 2
            Console.WriteLine("\n 2 - Mostrar o nome de todos atores que desempenharam um determinado personagem");

           var query2 = from a in mv_context.Characters
                         where a.Character == "Darth Vader"
                         select new
                        {
                         a.Movie.Title,
                         a.Actor.Name,
                         a.Character
                        };

            foreach (var ator in query2)
            {
                Console.WriteLine(" O ator {0} interpretou o persongem {1}", ator.Name, ator.Character);
            }


            //Query 3
            Console.WriteLine("\n 3 - Informar qual o ator desempenhou mais vezes um determinado personagem");

            var query3 = (from a in mv_context.Characters
                     where a.Character == "Han Solo"
                          group a by a.Actor.Name into grupo
                     select new
                     {
                         Nome = grupo.Key,
                         QtdPersonagens = grupo.Count()
                     })
                     .OrderByDescending(a => a.QtdPersonagens)
                     .FirstOrDefault();

            Console.WriteLine("\n O ator {0} realizou {1} vezes o determinado personagem", query3.Nome, query3.QtdPersonagens);


            //Query 4
            Console.WriteLine("\n 4 - Mostrar o nome e a data de nascimento do ator mais idoso e o mais novo ");

            //Mais velho
            var dataMenor = (from a in mv_context.Actors
                             select a.DateBirth).Min();

            String atorMaisVelho = (from a in mv_context.Actors
                                 where a.DateBirth == dataMenor
                                 select a.Name).FirstOrDefault();

            //Mais novo
            var dataMaior = (from a in mv_context.Actors
                             select a.DateBirth).Max();

            String atorMaisNovo = (from a in mv_context.Actors
                                where a.DateBirth == dataMaior
                                select a.Name).FirstOrDefault();

            Console.WriteLine("\n O ator mais velho é o(a) {0} nascido(a) no dia {1}", atorMaisVelho, dataMenor.ToShortDateString());

            Console.WriteLine("\n O ator mais novo é o(a) {0} nascido(a) no dia {1}", atorMaisNovo, dataMaior.ToShortDateString());


            //Consulta 6
            Console.WriteLine("\n 6 - Mostrar o valor médio das avaliações dos filmes que um determinado ator participou.\n ");

            String actor = "Carrie Fisher";
            var averageAvaliation = (from aa in mv_context.Characters
                                     where aa.Actor.Name == actor
                                     select aa.Movie.Rating).Average();

            Console.WriteLine(" O ator {0} obteve uma media de {1} em relação aos filems que participou", actor, averageAvaliation.ToString("F"));


        }
    }
}

