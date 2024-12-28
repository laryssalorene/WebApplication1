
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Refit;
using SistemaDeTarefas.Data;
using SistemaDeTarefas.Integracao;
using SistemaDeTarefas.Integracao.Interfaces;
using SistemaDeTarefas.Integracao.Refit;
using SistemaDeTarefas.Repositorios;
using SistemaDeTarefas.Repositorios.Interfaces;

namespace SistemaDeTarefas
{
    public class Program
    {
 
        public static void Main(string[] args)
        {
            string chaveSecreta = "e029127a-ca2d-4f13-9509-1a99fe699751";

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            
            //congiguração do swagger p/ ler o token
            builder.Services.AddSwaggerGen( c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Sistema de Tarefas - API", Version = "v1" });

                var securitySchema = new OpenApiSecurityScheme
                {
                    Name = "JWT Autenticação",
                    Description = "Entre com o JWT Bearer token",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securitySchema);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {securitySchema, new string[] {} }
                });

            });


            //Alteracao Importantissima para adicionar o contexto e os repositorios
            //informa qual db, DBContext(nome do contexto) e string de conexao vai ser utilizada
            builder.Services.AddEntityFrameworkSqlServer()
                .AddDbContext<SistemaTarefasDBContext>(
                    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DataBase"))
                );
            //configura as dependencias do repositorio:

            //IusuarioRepositorio vai implementar o UsuarioRepositorio. 
            //Estou configurando injeção de dependencia.
            //É informado q toda vez em q for chamada esta interface,deverá ser
            //instanciada a classe UsuarioRepositorio
            builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
            builder.Services.AddScoped<ITarefaRepositorio, TarefaRepositorio>();
            builder.Services.AddScoped<IViaCepIntegracao, ViaCepIntegracao>();


            builder.Services.AddRefitClient<IViaCepIntegracaoRefit>().ConfigureHttpClient(c =>
            {
                c.BaseAddress = new Uri("https://viacep.com.br");
            });

            //verificações para acessar a aplicação.if true -> acesso ; else -> error 401
            builder.Services.AddAuthentication(options => 
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true, //token tem tempo de expiração
                    ValidateIssuerSigningKey = true, // irá usar uma signing key (informada abaixo)
                    ValidIssuer = "sua_empresa", //configurações do ValidateIssuer
                    ValidAudience = "sua_aplicacao",//configurações do ValidateAudience
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(chaveSecreta))

                };
            });
            //////////////
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            //é necessário seguir a ordem UseAuthentication e UseAuthorization
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
