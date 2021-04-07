
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

            #region Consulta 1
            Console.WriteLine(" 1 - Listar o nome de todos personagens desempenhados por um determinado " +
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
            Console.WriteLine("\n 2 - Mostrar o nome de todos atores que desempenharam um determinado " +
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
                Console.WriteLine("\n {0} estrelando {1} como {2}.\n", ator.Title, ator.Name, ator.Character);
            }
            #endregion

            #region Consulta 3
            Console.WriteLine(" 3 - Informar qual o ator desempenhou mais vezes um determinado " +
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
        }
    }
}

