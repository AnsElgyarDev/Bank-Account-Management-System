using Pastel;

namespace BankAccount.cs
{
    public static class BankingOperations
    {
        internal static void Disposing(BankAccount bk, decimal? Balance)
        {
            bk.Balance -= Balance;
        }
        internal static void Adding(BankAccount bk, decimal? Balance)
        {
            bk.Balance += Balance;
        }
        internal static void ShowAccount(BankAccount bk)
        {
            Console.WriteLine("\n****** User Account ******\n" + string.Concat($"Name: {bk.Username}\n",
                   $"Email: {bk.email}\n",
                   $"Balance {bk.Balance}\n",
                   $"CreatedAt: {bk.CreatedAt}\n").Pastel(ConsoleColor.DarkBlue) +
                   "\n=======================\n");
        }
    }
}
