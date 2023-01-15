using Discord;
using Discord.WebSocket;
using Discord.Interactions;
using Domain.Interface;
using Application.Interface;
using Domain.Factory;

namespace UI.BotConsole;
internal class DiscordLogger : IDiscordLogger
{
    public void Register(DiscordSocketClient client, InteractionService interactionService)
    {
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
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("【BotSystem】");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"{message}");
        WriteLogFile("【BotSystem】" + $"{message}");
        return Task.CompletedTask;
    }

    public void WriteBotSystemLog(string message,ConsoleColor color = ConsoleColor.Cyan)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("【BotSystem】");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write($"{DateTime.Now.ToString("HH:mm:ss")} ");
        Console.ForegroundColor = color;
        Console.WriteLine($"{message}");
        WriteLogFile("【BotSystem】" + $"{DateTime.Now.ToString("HH:mm:ss")} " + $"{message}");
    }

    /// <summary>
    /// ディスコードのコマンドが実行された時、実行場所、実行者、実行結果をコンソールに出力する。
    /// </summary>
    /// <returns>Task</returns>
    public async Task ShowCommandExecutedLog(SlashCommandInfo commandInfo, IInteractionContext context, IResult result)
    {
        await WriteCommandPerformer(context);
        if (result.IsSuccess)
        {
            WriteSuccessLog(commandInfo);
            return;
        }
        WriteErrorLog(result);
    }
    private async Task WriteCommandPerformer(IInteractionContext context)
    {
        IGuildUser user = await context.Guild.GetUserAsync(context.User.Id);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write($"【<{context.Guild.Name}>{context.Channel.Name}】");
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine($"{DateTime.Now}");
        Console.ForegroundColor = ConsoleColor.Yellow;
        string nickName = user.Nickname ?? "ニックネーム無し";
        Console.Write($"<{user.Username}({nickName})>");
        WriteLogFile($"【<{context.Guild.Name}>{context.Channel.Name}】" + $"{DateTime.Now}" + $"<{user.Username}({nickName})>");
    }
    private void WriteErrorLog(IResult result)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("[コマンドの実行でエラーが発生しました]");
        Console.ForegroundColor = ConsoleColor.White;
        string errorMessage = result.Error switch
        {
            InteractionCommandError.UnmetPrecondition => $"コマンドの前提条件が満たされていません。{result.ErrorReason}",
            InteractionCommandError.UnknownCommand => $"そのコマンドは存在していません。",
            InteractionCommandError.BadArgs => $"無効な引数が与えられました。",
            InteractionCommandError.Exception => $"コマンドで例外が発生しました。{result.ErrorReason}",
            InteractionCommandError.Unsuccessful => $"コマンドが実行できませんでした。",
            _ => ""
        };
        Console.WriteLine(errorMessage);
        WriteLogFile("[コマンドの実行でエラーが発生しました]" + errorMessage);
    }
    private void WriteSuccessLog(SlashCommandInfo commandInfo)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write($"[実行されました]");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"コマンド名：{commandInfo.Name}");
        WriteLogFile($"[実行されました]" + $"コマンド名：{commandInfo.Name}");
    }

    private void WriteLogFile(string text)
    {
        Factory.GetService<IFileWriter>().WriteLogFile(@"Log/currentLog.txt", text);
    }
}