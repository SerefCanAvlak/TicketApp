using GenericRepository;
using log4net;
using log4net.Config;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Ticketing.Application.Features.Events.Commands.FinishExpiredEvents;
using Ticketing.Application.Mapping;
using Ticketing.Application.Services;
using Ticketing.Domain.Entities;
using Ticketing.Domain.Interfaces;
using Ticketing.Infrastructure.Context;
using Ticketing.Infrastructure.Repositories;
using Ticketing.Infrastructure.Services;
using Ticketing.Worker.Configuration;
using Ticketing.Worker.Jobs;
using Ticketing.Worker.Services;

var builder = Host.CreateApplicationBuilder(args);

// Configuration
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);


var entryAssembly = Assembly.GetEntryAssembly() ?? throw new InvalidOperationException("Entry assembly could not be determined.");

var logRepository = LogManager.GetRepository(entryAssembly);
XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

// Services
builder.Services.Configure<WorkerSettings>(builder.Configuration.GetSection("WorkerSettings"));
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(FinishExpiredEventsCommandHandler).Assembly);
});

builder.Services.AddScoped<INotificationRepository, NotificationRepository>();

builder.Services.AddScoped<ITicketRepository, TicketRepository>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<ISeatRepository, SeatRepository>();
builder.Services.AddScoped<ISeatLockRepository, SeatLockRepository>();
builder.Services.AddScoped<IRepository<Event>, Repository<Event, ApplicationDbContext>>();
builder.Services.AddScoped<ILogService, LogService>();
builder.Services.AddScoped<ILogReadService, LogReadService>();
builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.AddScoped<IWorkerJob, EventExpirationJob>();
builder.Services.AddHostedService<WorkerService>();
builder.Services.AddIdentity<AppUser, IdentityRole<Guid>>(options =>
{
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

//builder.Services.AddScoped<IWorkerJob, NotificationJob_inMemory>();
builder.Services.AddScoped<IWorkerJob, NotificationJob_EventDriven>();
//builder.Services.AddScoped<IWorkerJob, NotificationJob_RabbitMQ>();

builder.Services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());
builder.Services.AddScoped<IRepository<Notification>, Repository<Notification, ApplicationDbContext>>();
builder.Services.AddScoped<EventExpirationJob>();
builder.Services.AddFluentEmail("info@ticketingapp.com").AddSmtpSender("localhost", 2525);

builder.Services.AddScoped<IMailService, MailService>();

var host = builder.Build();
await host.RunAsync();