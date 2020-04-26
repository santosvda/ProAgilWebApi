using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProAgil.Domain.Identity;
using ProAgil.Repository;

namespace ProAgil.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //configuração de conexão com o banco
            services.AddDbContext<ProAgilContext>(
                x => x.UseSqlite(Configuration.GetConnectionString("DefaultConnection"))
            );

            /*
                Todas as tabelas serão injetadas nas controllers
                Todas as controllers terão que passar por uma autenticação(quem consumir precisa estar autenticado e autorizado)
            */
            IdentityBuilder builder = services.AddIdentityCore<User>(options => 
                {
                    options.Password.RequireDigit =  false;
                    options.Password.RequireNonAlphanumeric = false; 
                    options.Password.RequireLowercase = false; 
                    options.Password.RequireUppercase = false; 
                    options.Password.RequiredLength = 4; 
                }
                //Configuração padrão para que determinados requisições do campo Passwrod não sejam obnrigatorias
            );

            //Passa a mesma configuração do User pro Role - macete
            builder = new IdentityBuilder(builder.UserType, typeof(Role), builder.Services);
            
            builder.AddEntityFrameworkStores<ProAgilContext>();
            builder.AddRoleValidator<RoleValidator<Role>>();
            builder.AddRoleManager<RoleManager<Role>>();
            builder.AddSignInManager<SignInManager<User>>();

            //configurando jwt
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => 
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true, //Valida o emissor da chave (a propia API)
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII//Uma chave que será descriptografada perante o Token
                            .GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                    };
                }
            );

            //cria uma politica para que sempre que uma controller for chamada automaticamente respeitar a configuração criada (politica de autenticação)
            services.AddMvc(options => {
                    var policy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                     .Build();
                    options.Filters.Add(new AuthorizeFilter(policy));

            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
            .AddJsonOptions(opt => opt.SerializerSettings.ReferenceLoopHandling = 
            Newtonsoft.Json.ReferenceLoopHandling.Ignore);//controla redundancia em relação ao retorno da serialização dos itens

            //sempre que precisar do IProAgilRepository, impletamenta o ProAgilRepository
            services.AddScoped<IProAgilRepository, ProAgilRepository>();

            //Informar a aplicação que a mesma trabalha com AutoMapper
            /*
                    *Domain*    *API*  
                Ex: Evento <--> EventoDto

                DTO = Data transfer object
            */
            services.AddAutoMapper();
            //Configuração de permisão - CORS
            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //se der ruim, gera uma pag de exceção padrão enquanto estiver ambiente de dev
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseAuthentication();//informa que a aplicação deve ser autenticada

            //app.UseHttpsRedirection();
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseStaticFiles();//permite que o usuario encontre as imagens dentro do serv
            //informa onde a API permite salvar arquivos e onde puxar as imagens
            app.UseStaticFiles(new StaticFileOptions(){
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
                RequestPath = new PathString("/Resources")
            });
            app.UseMvc();
        }
    }
}
