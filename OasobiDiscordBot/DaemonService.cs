using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Application.Bots;

namespace OasobiDiscordBot
{
    class DaemonService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private readonly IOptions<DaemonConfig> _config;
        private readonly IAsyncBotClient _botClient;
        private readonly ILogPrintable? _logPrintable;
        public DaemonService(ILogger<DaemonService> logger, IOptions<DaemonConfig> config, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _config = config;
            _botClient = serviceProvider.GetRequiredService<IAsyncBotClient>();
            //実装が見つからないので保留
            //_logPrintable = serviceProvider.GetRequiredService<ILogPrintable>();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            //logPrintableの実装が終わり次第引数に渡す。
            //依存するメソッド全てのnullAbleを外す
            _botClient.StartAsync(null);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _botClient.StopAsync();
            _logger.LogInformation("Stopping daemon.");

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _logger.LogInformation("Disposing....");
        }
    }
}