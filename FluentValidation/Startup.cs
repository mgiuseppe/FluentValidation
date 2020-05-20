using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.AspNetCore;
using FluentValidationTest;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FluentValidation
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
            // here I'm registering each validator in the assembly of PersonValidator (this assembly) as a transient service
            // I'm also adding the fluentvalidation middleware
            services.AddControllers().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<PersonValidator>());

            // The validator will return a response like this if the validation won't pass
            // {
            //     "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            //     "title": "One or more validation errors occurred.",
            //     "status": 400,
            //     "traceId": "|fc00762a-485dde8405d0b6f3.",
            //     "errors": {
            //         "Age": [
            //            "'Age' must be between 18 and 60. You entered 0."
            //         ]
            //     }
            // }

            services.AddSwaggerDocument();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
