using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Bots;
public interface ILogPrintable
{
    void PrintNormalLog(string header, string message);

    void PrintCommandSuccess(string header, string performer, string message);

    void PrintCommandError(string header, string performer, string message);

    void PrintSystemLog(string header, string message);

    void PrintError(string header, string errorMessage);
}
