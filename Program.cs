using Pastel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Numerics;
using System.Text.Json;
using System.Threading.Channels;
using System.Xml.Linq;

namespace BankAccount.cs
{
    public class Program
    {
        private readonly static string Path = @"C:\Users\DELL\Desktop\back-end\BankAccountManagementSystem\BankAccountManagementSystem\BankAccount.Json";
        private static readonly Publisher publisher = new();
        public enum BankOperations
        {
            CreateAccount = 1,
            DisposeMoney,
            TransferMoney,
            ShowAccount,
            ShowTransaction,
            DeleteAccount,
            Exit,
        }

        internal static Dictionary<int, BankAccount> BankAccounts = new();
        static void Main(string[] args)
        {
            publisher.OnMakingAccount += publisher.RaiseOnMakingAccount;
            Console.WriteLine("Welcome to Bank Account Management System\n".Pastel(ConsoleColor.DarkCyan));
            Console.WriteLine("***** Warning You Should Leave By Exit Option In Order To save The Data *****\n".Pastel(ConsoleColor.Red));
            TransferData();
            while (true)
            {
                Console.WriteLine("Please Choose Operation:- ");
                Console.WriteLine(
                    string.Concat("1. CreateAccount\n"+
                    "2. DisposeMoney\n" +
                    "3. TransferMoney\n"+
                    "4. ShowAccount\n"+
                    "5. ShowTransaction\n"+
                    "6. DeleteAccount\n" +
                    "7. Exit"
                    ).Pastel(ConsoleColor.Yellow));
                Console.Write("Type Here: ");
                if(BankOperations.TryParse<BankOperations>(Console.ReadLine(), true, out BankOperations result))
                {
                    switch (result)
                    {
                        case BankOperations.CreateAccount:
                            CreateBankAccount(); break;
                     
                        case BankOperations.DisposeMoney:
                            DisposeMoney(); break;
                        
                        case BankOperations.TransferMoney:
                            TransferMoney(); break;
                        
                        case BankOperations.ShowAccount:
                            ShowAccount(); break;
                        
                        case BankOperations.ShowTransaction:
                            ShowTransaction(); break;

                        case BankOperations.DeleteAccount:
                            DeleteAccount(); break;

                        case BankOperations.Exit:
                            SaveData();
                            Environment.Exit(0); break;
                    }
                }
                else
                    Console.WriteLine("Sorry Operation Not Available At This Moment!".Pastel(ConsoleColor.Red));
            }
        }

        private static void ShowTransaction()
        {
            Console.Write("Please, Enter Your ID: ");
            if (int.TryParse(Console.ReadLine(),out int ID))
            {
                if (BankAccounts.ContainsKey(ID))
                {
                    Console.WriteLine("\n**** Transaction History ****\n".Pastel(ConsoleColor.Blue));
                    foreach (var transactionvalues in BankAccounts[ID].TransactionHistory)
                    {
                        Console.WriteLine($"{transactionvalues}".Pastel(ConsoleColor.Cyan));
                        Console.WriteLine("_________________");
                    }
                }
                else
                    Console.WriteLine("Sorry, Wrong ID".Pastel(ConsoleColor.DarkRed));
            }
            else
            {
                Console.WriteLine("failed, Try Again Later!".Pastel(ConsoleColor.DarkRed));
            }
        }
        private static void TransferData()
        {
            if (File.Exists(Path))
            {
                string jsonString = File.ReadAllText(Path);
                if (!string.IsNullOrWhiteSpace(jsonString))
                {
                    BankAccounts = JsonSerializer.Deserialize<Dictionary<int, BankAccount>>(jsonString)
                                   ?? new Dictionary<int, BankAccount>();
                }
            }
        }

        private static void DeleteAccount()
        {
            Console.WriteLine("Please, Enter Your ID");
            if (int.TryParse(Console.ReadLine()?.Trim(), out int ID))
            {
                BankAccounts.Remove(ID);
                publisher?.invokingMsg("Account Has Been Deleted Successfully", ConsoleColor.Green);
            }
            else
            {
                Console.WriteLine("Invalid ID".Pastel(ConsoleColor.Red));
            }
        }

        private static void ShowAccount()
        {
            Console.Write("Please Enter Your Bank ID: ");
            int.TryParse(Console.ReadLine(), out int BankId);
            if (BankAccounts.ContainsKey(BankId))
            {
                BankingOperations.ShowAccount(BankAccounts[BankId]);
            }

            else
            {
                Console.WriteLine("Sorry, There is no Account With This ID!".Pastel(ConsoleColor.DarkRed));
            }
        }

        private static void TransferMoney()
        {
            Console.Write("Please Enter Your Bank ID: ");
            int.TryParse(Console.ReadLine(), out int BankId);
            if (BankAccounts.ContainsKey(BankId))
            {
                Console.Write("Please Enter Money To Transfer: ");
                if (decimal.TryParse(Console.ReadLine(), out decimal TransferMoney))
                {
                    if (TransferMoney > 0)
                    {
                        BankAccounts[BankId].Balance += TransferMoney;
                        publisher?.invokingMsg($"Money Transfered Succesfully on {DateTime.Now}", ConsoleColor.DarkGreen);
                            BankAccounts[BankId].TransactionHistory.Add(
                            $"Operation: Transfering\n" +
                            $"Money:{TransferMoney:c}\n" +
                            $"TransferedAt: {DateTime.Now:G}");
                    }
                    else
                    {
                        publisher?.invokingMsg("Sorry, Something Went Wrong!", ConsoleColor.DarkRed);
                    }
                }
                else
                    Console.WriteLine("Something Went Wrong!".Pastel(ConsoleColor.Red));
            }
            else
            {
                Console.WriteLine("Sorry, There is no Account With This ID!".Pastel(ConsoleColor.DarkRed));
            }
        }

        private static void DisposeMoney()
        {
            Console.Write("Please Enter Your Bank ID: ");
            int.TryParse(Console.ReadLine(), out int BankId);
            if (BankAccounts.ContainsKey(BankId))
            {
                Console.Write("Please Enter Money To Dispose: ");
                if (decimal.TryParse(Console.ReadLine(), out decimal DisposeMoney))
                {
                    if (DisposeMoney > 0 && DisposeMoney < BankAccounts[BankId].Balance)
                    {
                        BankAccounts[BankId].Balance -= DisposeMoney;
                        publisher.invokingMsg($"Money Dsiposed Succesfully on {DateTime.Now}", ConsoleColor.DarkGreen);
                        BankAccounts[BankId].TransactionHistory.Add($"Operation: Disposing\n" +
                            $"Money:{DisposeMoney:c}\n" +
                            $"DisposedAt: {DateTime.Now:G}");
                    }
                    else
                    {
                        publisher?.invokingMsg("Sorry, You have not that money in the Account", ConsoleColor.DarkRed);
                    }
                }
                else
                    Console.WriteLine("Something Went Wrong!".Pastel(ConsoleColor.Red));
            }
            else
            {
                Console.WriteLine("Sorry, There is no Account With This ID!".Pastel(ConsoleColor.DarkRed));
            }
            string? Transaction = $"The Operation: Dispose\nThe Money Disposed: {DisposeMoney}\nDate: {DateTime.Now.ToString("G")}"; 
        }
        private static void CreateBankAccount()
        {
            Console.Write("Please, Enter Your ID: ");
            int.TryParse(Console.ReadLine()?.Trim(), out int ID);
            if(BankAccounts.ContainsKey(ID))
            {
                Console.WriteLine("Used Id, Try Again Later".Pastel(ConsoleColor.Red));
                return;
            }
            Console.Write("Please, Enter Your Name: ");   
            string? Name = Console.ReadLine()?.Trim();

            Console.Write("Please, Enter Your Email: ");
            string? email = Console.ReadLine()?.Trim() ?? null;
            int emailtrial = 3;
            while(!email.Contains('@') && emailtrial > 0)
            {
                    Console.WriteLine("Please Enter Right Email!".Pastel(ConsoleColor.Red));
                    email = Console.ReadLine()?.Trim();
                    emailtrial--;
            }
            if (!email.Contains('@'))
                email = null;
            
            Console.Write("Please, Enter Your Balance: ");
            decimal.TryParse(Console.ReadLine()?.Trim(), out decimal balance);
            
            var bankAccount = new BankAccount(Name, ID, email, balance);
            BankAccounts.Add(bankAccount.ID, bankAccount);
        }
        private static void SaveData()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(BankAccounts, options);
            File.WriteAllText(Path, jsonString);
        }
    }
}