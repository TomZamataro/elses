using System;
using System.Collections.Generic;
using System.Text;

namespace Teste_Tecnico_Tapps
{
    class PickyPlayer : Player
    {
        public override bool BuyOrPay(Propriedade prop)
        {
            // Buy
            // Checking for Owners
            if (prop.Owner == null)
            {
                // Checking if has enough Coins & if it's satisfies the pickiness of the Player
                if (prop.GetValue() <= this.PlayerCoins && prop.GetRent() > 50)
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
                    // Optional Output - Console.WriteLine($"\n       O Jogador do tipo Exigente foi Eliminado pois ficaria com {this.PlayerCoins - prop.GetRent()} de saldo para pagar o aluguel");
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
    }
}
