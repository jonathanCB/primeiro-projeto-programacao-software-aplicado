using System;
using System.Text;

namespace Persistencia.Entidades
{ 
    public class ActorMovie
    {
        public int ActorMovieId { get; set; }
        public String Character { get; set; }
        public int MovieId { get; set; }
        public virtual Movie Movie { get; set; }
        public int ActorId { get; set; }
        public virtual Actor Actor { get; set; }
    }
}