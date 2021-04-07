
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

            #region Consulta 3
            Console.WriteLine("3 - Informar qual o ator desempenhou mais vezes um determinado " +
                "personagem (por exemplo: qual o ator que realizou mais filmes como o 'agente 007') ");

            var a2 = from a in _context.Characters
                     where a.Character == "James Bond"
                     group a by a.Actor.Name into grupo
                     select new
                     {
                         Nome = grupo.Key,
                         NroPersonagens = grupo.Count()
                     };
            
            //Primeira forma:
            String nomeAtor = a2.FirstOrDefault().Nome;
            int qtdVezesComoPersonagem = a2.FirstOrDefault().NroPersonagens;
            Console.WriteLine("\n Nome do ator que realizou mais vezes o personagem: {0}\n " +
                        "QtdFilmes: {1}", nomeAtor, qtdVezesComoPersonagem);

            //Segunda forma:
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

            //Mostrar todos os atores que desempenharam o personagem escolhido na ordem decrescente:
            /*foreach (var ator in a2.OrderByDescending(a => a.NroPersonagens))
            {
                    Console.WriteLine("\n Nome do ator: {0}\n QtdFilmes: {1}", ator.Nome, ator.NroPersonagens);
            };*/

            #endregion

            #region Consulta 4

            #endregion
        }
    }
}

