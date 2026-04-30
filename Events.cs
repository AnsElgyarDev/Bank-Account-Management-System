using Microsoft.VisualBasic;
using Pastel;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
namespace BankAccount.cs
{
    public class Publisher
    {
        public delegate void EventHandler(string msg, ConsoleColor cc);
        public  event EventHandler? OnMakingAccount;
        public void invokingMsg(string msg, ConsoleColor cc)
        {
            OnMakingAccount?.Invoke(msg, cc);
        }
        public void RaiseOnMakingAccount(string msg, ConsoleColor cc) => Console.WriteLine((msg + "\n").Pastel(cc));
    }
}
