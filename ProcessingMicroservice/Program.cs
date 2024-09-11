using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProcessingMicroservice.QueueProcessors;
using ProcessingMicroservice.QueueProcessors.Interface;
using ProcessingMicroservice.Repositories;
using ProcessingMicroservice.Repositories.Interface;

class Program
{
    static async Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;

        var assetQueueProcessor = services.GetRequiredService<AssetQueueProcessor>();
        var dividendInterestQueueProcessor = services.GetRequiredService<DividendInterestQueueProcessor>();
        var marketValueQueueProcessor = services.GetRequiredService<MarketValueQueueProcessor>();
        var portfolioQueueProcessor = services.GetRequiredService<PortfolioQueueProcessor>();
        var priceHistoryQueueProcessor = services.GetRequiredService<PriceHistoryQueueProcessor>();
        var transactionQueueProcessor = services.GetRequiredService<TransactionQueueProcessor>();
        var userQueueProcessor = services.GetRequiredService<UserQueueProcessor>();

        var processors = new IQueueProcessor[]
        {
                assetQueueProcessor,
                dividendInterestQueueProcessor,
                marketValueQueueProcessor,
                portfolioQueueProcessor,
                priceHistoryQueueProcessor,
                transactionQueueProcessor,
                userQueueProcessor
        };

        foreach (var processor in processors)
        {
            processor.ProcessSaveQueue();
            processor.ProcessUpdateQueue();
            processor.ProcessDeleteQueue();
        }

        Console.WriteLine("Queue processors are running. Press [enter] to exit.");
        Console.ReadLine();
    }

    static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((_, services) =>
            {
                // Add repositories
                services.AddSingleton<IAssetRepository, AssetRepository>();
                services.AddSingleton<IDividendInterestRepository, DividendInterestRepository>();
                services.AddSingleton<IMarketValueRepository, MarketValueRepository>();
                services.AddSingleton<IPortfolioRepository, PortfolioRepository>();
                services.AddSingleton<IPriceHistoryRepository, PriceHistoryRepository>();
                services.AddSingleton<ITransactionRepository, TransactionRepository>();
                services.AddSingleton<IUserRepository, UserRepository>();

                // Add queue processors
                services.AddSingleton<AssetQueueProcessor>();
                services.AddSingleton<DividendInterestQueueProcessor>();
                services.AddSingleton<MarketValueQueueProcessor>();
                services.AddSingleton<PortfolioQueueProcessor>();
                services.AddSingleton<PriceHistoryQueueProcessor>();
                services.AddSingleton<TransactionQueueProcessor>();
                services.AddSingleton<UserQueueProcessor>();
            });
}
