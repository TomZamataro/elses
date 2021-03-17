using System;
using System.Collections.Generic;
using System.Text;

namespace Teste_Tecnico_Tapps
{
    class Player
    {
        #region Proprieties
        public int PlayerCoins { get; set; }
        public int PlayerPosition { get; set; }
        #endregion
        public List<Propriedade> Propriedades = new List<Propriedade>();
        #region Constructors
        public Player()
        {
            this.PlayerCoins = 300;
            this.PlayerPosition = 1;
        }
        #endregion
        #region Methods
        public void Move()
        {
            // Simulating a 1d6
            PlayerPosition += new Random().Next(1, 7);
            // Checking if completed a Lap
            if (PlayerPosition > 20)
            {
                // Paying the Lap
                this.PlayerCoins += 100;
                this.PlayerPosition -= 20;
            }
        }
        public virtual bool BuyOrPay(Propriedade prop)
        {
            // Buy
            // Checking for Owners & if has enough Coins
            if (prop.Owner == null)
            {
                if (prop.GetValue() <= this.PlayerCoins)
                {
                    this.PlayerCoins -= prop.GetValue();
                    Propriedades.Add(prop);
                    prop.Owner = this;
                }
            }
            // Pay
            else
            {
                // Checking if Eliminated
                if (this.PlayerCoins - prop.GetRent() < 0)
                {
                    // Optional Output - Console.WriteLine($"\n       O Jogador do tipo Impulsivo foi Eliminado pois ficaria com {this.PlayerCoins - prop.GetRent()} de saldo para pagar o aluguel");
                    prop.Owner.PlayerCoins += this.PlayerCoins;
                    // Puting the Proprieties to Sale
                    foreach (Propriedade p in Propriedades)
                    {
                        p.Owner = null;
                    }
                    // Clearing the List
                    Propriedades.Clear();
                    return true;
                }
                // Paying the Owner
                else
                {
                    this.PlayerCoins -= prop.GetRent();
                    prop.Owner.PlayerCoins += prop.GetRent();
                }
            }
            return false;
        }
        #endregion
    }
}
