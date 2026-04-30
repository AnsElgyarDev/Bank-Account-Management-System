using Pastel;
using System.Globalization;

namespace BankAccount.cs
{
    internal class BankAccount
    {
        private readonly string msg = "New Account Made!";
        public string? Username { get; set; }
        public int ID{ get; set; }
        public string? email{ get; set; }
        public DateTime? CreatedAt{ get; set; }
        public decimal? Balance{ get; set; }
        public List<string> TransactionHistory { get; set; } = new();
        public BankAccount() { }
        public BankAccount(string? Username, int iD, string? email, decimal? balance)
        {
            Publisher publisher = new Publisher();
            publisher.OnMakingAccount += publisher.RaiseOnMakingAccount;
            publisher?.invokingMsg(msg, ConsoleColor.DarkGreen);
            this.Username = Username;
            this.ID = iD;
            this.email = email;
            this.Balance = balance;
            this.CreatedAt = DateTime.UtcNow;
        }
        public override string ToString() => this?.email ?? "Email Not Found!".Pastel(ConsoleColor.DarkRed);
    }
}
