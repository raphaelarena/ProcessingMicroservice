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

        var alunoQueueProcessor = services.GetRequiredService<AlunoQueueProcessor>();
        var cursoQueueProcessor = services.GetRequiredService<CursoQueueProcessor>();
        var matriculaQueueProcessor = services.GetRequiredService<MatriculaQueueProcessor>();
        var professorQueueProcessor = services.GetRequiredService<ProfessorQueueProcessor>();
        var turmaQueueProcessor = services.GetRequiredService<TurmaQueueProcessor>();
        var usuarioQueueProcessor = services.GetRequiredService<UsuarioQueueProcessor>();

        var processors = new IQueueProcessor[]
        {
                alunoQueueProcessor,
                cursoQueueProcessor,
                matriculaQueueProcessor,
                professorQueueProcessor,
                turmaQueueProcessor,
                usuarioQueueProcessor
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
                services.AddSingleton<IAlunoRepository, AlunoRepository>();
                services.AddSingleton<ICursoRepository, CursoRepository>();
                services.AddSingleton<IMatriculaRepository, MatriculaRepository>();
                services.AddSingleton<IProfessorRepository, ProfessorRepository>();
                services.AddSingleton<ITurmaRepository, TurmaRepository>();
                services.AddSingleton<IUsuarioRepository, UsuarioRepository>();

                // Add queue processors
                services.AddSingleton<AlunoQueueProcessor>();
                services.AddSingleton<CursoQueueProcessor>();
                services.AddSingleton<MatriculaQueueProcessor>();
                services.AddSingleton<ProfessorQueueProcessor>();
                services.AddSingleton<TurmaQueueProcessor>();
                services.AddSingleton<UsuarioQueueProcessor>();
            });
}
