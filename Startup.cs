using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using ECommerceApp.Data;
using Stripe;
using ECommerceApp.Models;

namespace ECommerceApp
{
    
    public class StripeSettings
    {
        public string SecretKey { get; set; }
        public string PublishableKey { get; set; }
    }
    
    public class Startup
    {
		public IConfiguration Configuration {get;}
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.Configure<StripeSettings>(Configuration.GetSection("Stripe"));
            services.AddMvc();
            services.AddSession();
            services.AddDbContext<DataContext>(options => options.UseMySql(Configuration["DBInfo:ConnectionString"]));
            services.AddScoped<IRepository, Repository>();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            
			if(env.IsDevelopment())
			{
            	app.UseDeveloperExceptionPage();
			}
            app.UseStaticFiles();
            app.UseSession();
            app.UseMvc();

            StripeConfiguration.SetApiKey(Configuration.GetSection("Stripe")["SecretKey"]);
        }
    }
}
