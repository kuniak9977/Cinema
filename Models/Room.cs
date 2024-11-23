using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Models
{
    public class Room
    {
        private string name;
        private int chairsQuantity;
        private State[] states;

        public string Name { get { return name; } set { name = value; } }
        public int ChairsQuantity { get { return chairsQuantity; } set { chairsQuantity = value; } }
        public State[] States { get { return states; } set { states = value; } }

        public Room() { }

        public Room(string _name, int _quantity)
        {
            this.name = _name;
            this.chairsQuantity = _quantity;
            this.states = new State[this.chairsQuantity];

            for (int i = 0; i < this.chairsQuantity; i++)
            {
                this.states[i] = State.Free;
            }
        }

        public enum State
        {
            Free = 0,
            Taken = 1,
            Broken = 2
        };

        public void SetChairState(int _chairNumber, int state)
        {
            if (state < 0 || state > 2)
                throw new Exception("");
            this.States[_chairNumber] = (State)state;
        }

        public State GetChairState(int _chairNumber)
        {
            State state = states[_chairNumber];
            return state;
        }

        public override string ToString()
        {
            return $"Nazwa: {name}, Ilość_miejsc: {chairsQuantity}, Zajętość: {states}";
        }

    }
}
