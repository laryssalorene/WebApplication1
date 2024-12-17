using Microsoft.EntityFrameworkCore;
using SistemaDeTarefas.Data.Map;
using SistemaDeTarefas.Models;

namespace SistemaDeTarefas.Data
{
    public class SistemaTarefasDBContext : DbContext
    {
              //(DbContextOptions<NomedoContexto> varnomeadacomooptions) 
        public SistemaTarefasDBContext(DbContextOptions<SistemaTarefasDBContext> options)
            :base(options)
        {    
        }

        //UsuarioModel representa uma tabela chamada
        //Usuarios no banco de dados
        public DbSet<UsuarioModel> Usuarios { get; set; }

        public DbSet<TarefaModel> Tarefas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {   //adiciona o mapeamento de usuario e tarefa p/ o dbcontext
            modelBuilder.ApplyConfiguration(new UsuarioMap());
            modelBuilder.ApplyConfiguration(new TarefaMap());
            //
            base.OnModelCreating(modelBuilder);
        }
    }
}
