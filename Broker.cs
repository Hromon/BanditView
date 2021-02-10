using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bandit
{
    public class Broker
    {
        private int _Chips { get; set; }
        private Label _Display;

        private int _WinCoefficient;
        private bool _IsWin = false;

        private int _Bet = 10;

        private _Broker _Base;

        public int Bet
        {
            get => _Bet;
            set
            {
                _Bet = value > 5 ? value : 5;
                UpdateDisplay();
            }
        }
        public int Chips
        {
            get => _Chips;
            set
            {
                _Chips = value;
                UpdateDisplay();
            }
        }
        public int[] Coefficient;

        public Broker(int startChips, int[] coefficient, Label display, Button button5, Button button10, Button button100)
        {
            _Chips = startChips;
            _Display = display;

            Coefficient = coefficient;

            _Base = new _Broker(button5, button10, button100);
            _Base.ChangeBet += (size) => Bet += size;

            UpdateDisplay();
        }

        public bool SpendMoney(int count)
        {
            if(_Chips >= count)
            {
                _Chips -= count;
                UpdateDisplay();
                return true;
            }

            return false;
        }

        public void CheckBet(int[] result)
        {
            bool comparator = true;
            for (int i = 0; i < result.Length - 1; i++)
            {
                if (result[i] != result[i + 1])
                {
                    comparator = false;
                    break;
                }
            }

            _WinCoefficient = Coefficient[result[0]];

            _IsWin = comparator;
        }
        public bool PlayBet()
        {
            if (_IsWin)
                Chips += Bet * _WinCoefficient;
            
            return _IsWin;
        }

        private void UpdateDisplay()
            => _Display.Text = $"Chips:{_Chips}\nBet: {_Bet}";

        private class _Broker : Node
        {
            public event Action<int> ChangeBet;

            public _Broker(Button button5, Button button10, Button button100)
            {
                button5.Connect("gui_input", this, nameof(Bet5));
                button10.Connect("gui_input", this, nameof(Bet10));
                button100.Connect("gui_input", this, nameof(Bet100));
            }

            private void Bet5(InputEvent ev)
            {
                if(ev is InputEventMouseButton button && button.Pressed)
                {
                    if (button.ButtonIndex == (int)ButtonList.Left)
                        ChangeBet?.Invoke(5);
                    else if (button.ButtonIndex == (int)ButtonList.Right)
                        ChangeBet?.Invoke(-5);
                }
            }
            private void Bet10(InputEvent ev)
            {
                if (ev is InputEventMouseButton button && button.Pressed)
                {
                    if (button.ButtonIndex == (int)ButtonList.Left)
                        ChangeBet?.Invoke(10);
                    else if (button.ButtonIndex == (int)ButtonList.Right)
                        ChangeBet?.Invoke(-10);
                }
            }
            private void Bet100(InputEvent ev)
            {
                if (ev is InputEventMouseButton button && button.Pressed)
                {
                    if (button.ButtonIndex == (int)ButtonList.Left)
                        ChangeBet?.Invoke(100);
                    else if (button.ButtonIndex == (int)ButtonList.Right)
                        ChangeBet?.Invoke(-100);
                }
            }
        }
    }
}
