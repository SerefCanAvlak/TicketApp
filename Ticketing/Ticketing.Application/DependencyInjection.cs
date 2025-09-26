using FluentValidation;
using GenericRepository;
using Microsoft.Extensions.DependencyInjection;
using Ticketing.Application.Behaviors;
using Ticketing.Domain.Entities;

namespace Ticketing.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddFluentEmail("info@ticketingapp.com").AddSmtpSender("localhost", 2525);
            services.AddAutoMapper(typeof(DependencyInjection).Assembly);

            services.AddMediatR(conf =>
            {
                conf.RegisterServicesFromAssemblies(typeof(DependencyInjection).Assembly);
                conf.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });

            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

            return services;
        }
    }
}
