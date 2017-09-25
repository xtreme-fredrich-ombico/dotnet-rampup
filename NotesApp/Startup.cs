﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NotesApp.Models;
using NotesApp.Services;

namespace NotesApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<NoteContext>(opt =>
            {
                if (Environment.IsEnvironment("Integration Test"))
                {
                    opt.UseInMemoryDatabase("notes");
                }
                else
                {
                    var hostname = "localhost";
                    var name = "notes";
                    var username = "root";
                    var password = "password";
                    opt.UseMySql($@"Server={hostname};database={name};uid={username};pwd={password};");    
                }
            });

            services.AddTransient(typeof(NoteService));

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            Environment = env;
            app.UseMvc();
        }
    }
}