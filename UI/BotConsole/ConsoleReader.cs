using Domain.Factory;
using Application.Interface;

namespace UI.BotConsole
{
    internal class ConsoleReader : IConsoleReader
    {
        public void ActiveConsole()
        {
            string? commandString;
            bool isExit = false;
            while (!isExit)
            {
                commandString = Console.ReadLine();
                if (commandString is null)
                {
                    Console.WriteLine("please input valid command.");
                }
                // isExitの値を変更したいから、exitコマンドだけ素直に分岐している。
                else if (commandString == "exit")
                {
                    isExit = true;
                }
                else if(commandString == "reconnect")
                {
                    Factory.GetService<IDiscordConnecter>().Reconnect();
                }
                else
                {
                    Console.WriteLine("not found command.");
                }
            }
        }
    }
}
