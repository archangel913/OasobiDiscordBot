using Discord;
using Discord.WebSocket;
using Discord.Interactions;
using Domain.Interface;
using Application.Interface;
using Infrastructure.LocalFile;
using Application.Bots;

namespace Infrastructure.Loggings;
internal class DiscordLogger : IDiscordLogger
{
    public DiscordLogger(FileRepository fileRepository)
    {
        this.FileRepository = fileRepository;
    }

    private FileRepository FileRepository { get; }

    private ILogPrintable? Printable { get; set; }

    public void Register(DiscordSocketClient client, InteractionService interactionService, ILogPrintable printable)
    {
        this.Printable = printable;
        interactionService.SlashCommandExecuted += ShowCommandExecutedLog;
        client.Log += WriteBotSystemLog;
    }

    /// <summary>
    /// ボットのステータスをコンソールウインドウに出力する。クライアントのイベントに登録して使う
    /// </summary>
    /// <param name="message">ボットクライアントの情報(起動、接続完了などの情報)</param>
    /// <returns>Task</returns>
    public Task WriteBotSystemLog(LogMessage message)
    {
        if (this.Printable is not null)
        {
            this.Printable.PrintSystemLog("【BotSystem】", $"{message}");
        }
        WriteLogFile($"【BotSystem】 {message}");
        return Task.CompletedTask;
    }

    public void WriteBotSystemLog(string message)
    {
        var now = DateTime.Now.ToString("HH:mm:ss");
        if (this.Printable is not null)
        {
            this.Printable.PrintSystemLog($"【BotSystem】", $"{now}{message}");
        }
        WriteLogFile($"【BotSystem】 {now} {message}");
    }

    /// <summary>
    /// ディスコードのコマンドが実行された時、実行場所、実行者、実行結果をコンソールに出力する。
    /// </summary>
    /// <returns>Task</returns>
    public async Task ShowCommandExecutedLog(SlashCommandInfo commandInfo, IInteractionContext context, IResult result)
    {
        string performer = await GetCommandPerformer(context);
        if (result.IsSuccess)
        {
            WriteSuccessLog(commandInfo, performer);
            return;
        }
        WriteErrorLog(result, performer);
    }
    private async Task<string> GetCommandPerformer(IInteractionContext context)
    {
        IGuildUser user = await context.Guild.GetUserAsync(context.User.Id);
        string nickName = user.Nickname ?? "ニックネーム無し";
        return $"【<{context.Guild.Name}>{context.Channel.Name}】{DateTime.Now}<{user.Username}({nickName})>";
    }
    private void WriteErrorLog(IResult result, string performer)
    {
        string errorMessage = result.Error switch
        {
            InteractionCommandError.UnmetPrecondition => $"コマンドの前提条件が満たされていません。{result.ErrorReason}",
            InteractionCommandError.UnknownCommand => $"そのコマンドは存在していません。",
            InteractionCommandError.BadArgs => $"無効な引数が与えられました。",
            InteractionCommandError.Exception => $"コマンドで例外が発生しました。{result.ErrorReason}",
            InteractionCommandError.Unsuccessful => $"コマンドが実行できませんでした。",
            _ => ""
        };
        if (this.Printable is not null)
        {
            this.Printable.PrintCommandError("[コマンドの実行でエラーが発生しました]", performer,  errorMessage);
        }
        WriteLogFile(performer);
        WriteLogFile("[コマンドの実行でエラーが発生しました]" + errorMessage);
    }
    private void WriteSuccessLog(SlashCommandInfo commandInfo, string performer)
    {
        if (this.Printable is not null)
        {
            this.Printable.PrintCommandSuccess("[実行されました]", performer,  $"コマンド名：{commandInfo.Name}");
        }
        WriteLogFile(performer);
        WriteLogFile($"[実行されました]" + $"コマンド名：{commandInfo.Name}");
    }

    private void WriteLogFile(string text)
    {
        this.FileRepository.WriteLogFile(@"Log/currentLog.txt", text);
    }

    public void WriteErrorLog(string message)
    {
        var now = DateTime.Now.ToString("HH:mm:ss");
        if (this.Printable is not null)
        {
            this.Printable.PrintError($"【Error】", $"{now}{message}");
        }
        WriteLogFile($"【Error】{now} {message}");
    }
}