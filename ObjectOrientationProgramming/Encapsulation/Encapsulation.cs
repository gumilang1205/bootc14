using System.Reflection;

namespace ObjectOrientationProgramming.Encapsulation
{
    public class BankAccount
    {
        private double saldo;
        public BankAccount(double saldoAwal)
        {
            saldo = saldoAwal;
        }

        public void Deposit(double amount)
        {
            if (amount > 0)
            {
                saldo += amount;
                Console.WriteLine($"Berhasil deposit : {amount} saldo sekarang : {saldo}");

            }
            else
            {
                Console.WriteLine("Nominal harus lebih dari 0");
            }

        }
        public double GetSaldo()
        {
            return saldo;
        }
    }
}
