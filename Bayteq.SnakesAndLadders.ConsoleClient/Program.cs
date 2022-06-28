using Bayteq.SnakesAndLadders.Application;
using Bayteq.SnakesAndLadders.ConsoleClient;
using Bayteq.SnakesAndLadders.Infrastructure;
using Serilog;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddInfrastructure();
        services.AddApplication(context.Configuration);
        services.AddHostedService<Worker>();
    })
    .UseSerilog((ctx, lc) =>
    {
        lc.WriteTo.Console();
    })
    .Build();

await host.RunAsync();
