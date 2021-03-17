using System;
using System.Collections.Generic;
using System.Text;

namespace Teste_Tecnico_Tapps
{
    class Propriedade
    {
        #region Proprieties
        private int Position { get; set; }
        private int Value { get; set; }
        private int Rent { get; set; }
        public Player Owner { get; set; }
        #endregion
        #region Constructors
        public Propriedade()
        {

        }
        public Propriedade(int value, int rent, int pos)
        {
            Value = value;
            Rent = rent;
            Position = pos;
        }
        #endregion
        #region Methods
        public int GetValue()
        {
            return this.Value;
        }
        public int GetRent()
        {
            return Rent;
        }
        public int GetPosition()
        {
            return Position;
        }

        #endregion
    }
}
