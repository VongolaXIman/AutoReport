using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Model;
using Serilog.Context;
using Service.AllLog;

namespace ApiTemplate
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOpenTracing();
            services.AddControllers();

            //Repository

            //Service
            services.AddScoped<IAllLogService, AllLogService>();

            services.AddScoped<ITemplateDbContext, TemplateDbContext>();
            services.AddDbContext<TemplateDbContext>(
                  options =>
                  {
                      options.UseMySql(Configuration.GetConnectionString("ServerConnection"),
                  mySqlOptions =>
                  {
                      mySqlOptions.MigrationsAssembly(typeof(TemplateDbContext).Assembly.FullName);
                      mySqlOptions.EnableRetryOnFailure(5); //最多重試5次
                  });
                  }
            );

            services.AddHttpClient();
            services.AddHttpContextAccessor();
            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var pathBase = Configuration["PathBase"];
            if (!string.IsNullOrEmpty(pathBase))
            {
                app.UsePathBase(pathBase);
            }

            // 讓Serilog自動抓IP
            app.Use(async (ctx, next) =>
            {
                using (LogContext.PushProperty("IPAddress", ctx.Connection.RemoteIpAddress))
                {
                    await next().ConfigureAwait(false);
                }
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
