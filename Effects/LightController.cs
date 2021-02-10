using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bandit.Utility;

namespace Bandit.Effects
{
    public class LightController
    {
        private LightCommandArray _LastCommand;
        private List<LightCommandArray> _CommandArrays;
        private Light[] _Lights;
        private EGL.GodotBase.Utility.Tween _Tween;
        private int _BlinkCounter = 0;

        public LightController(Light[] lights)
        {
            _Lights = lights;
            _CommandArrays = new List<LightCommandArray>();

            _Tween = new EGL.GodotBase.Utility.Tween();
            Main.Base.AddChild(_Tween.Base);

            _Tween.TweenAllComleted += _Tween_TweenAllComleted;
        }

        public void Execute(string nameArrayCommand)
        {
            int target = _CommandArrays.FindIndex(x => x.Name == nameArrayCommand);
            if (target == -1) return;

            _LastCommand = _CommandArrays[target];

            for (int i = 0; i < _CommandArrays[target].Commands.Length; i++)
            {
                switch(_CommandArrays[target].Commands[i].TypeCommand)
                {
                    case TypeCommand.Color:
                        for (int j = 0; j < _Lights.Length; j++)
                            _Tween.Base.InterpolateProperty(_Lights[j], "light_color",
                                _CommandArrays[target].Commands[i].StartValue, _CommandArrays[target].Commands[i].EndValue,
                                _CommandArrays[target].Commands[i].Duration, _CommandArrays[target].Commands[i].Transition,
                                _CommandArrays[target].Commands[i].Ease,
                                _CommandArrays[target].Commands[i].Delay + i * _CommandArrays[target].Commands[i].DelayStep);
                        break;

                    case TypeCommand.Energy:
                        for (int j = 0; j < _Lights.Length; j++)
                            _Tween.Base.InterpolateProperty(_Lights[j], "light_energy",
                                _CommandArrays[target].Commands[i].StartValue, _CommandArrays[target].Commands[i].EndValue,
                                _CommandArrays[target].Commands[i].Duration, _CommandArrays[target].Commands[i].Transition,
                                _CommandArrays[target].Commands[i].Ease,
                                _CommandArrays[target].Commands[i].Delay + i * _CommandArrays[target].Commands[i].DelayStep);
                        break;
                }
            }

            _Tween.Base.Start();
        }

        public void AddCommandArray(LightCommandArray array)
        {
            _CommandArrays.Add(array);
        }
        public void AddCommandArray(string name, int countRepeat, params LightCommand[] commands)
        {
            _CommandArrays.Add(new LightCommandArray(name, countRepeat, commands));
        }
        

        private void _Tween_TweenAllComleted()
        {
            ++_BlinkCounter;
            Console.WriteLine("repeat");

            if (_LastCommand.CountRepeat > _BlinkCounter)
            {
                Execute(_LastCommand.Name);
                return;
            }

            _BlinkCounter = 0;
        }
    }
}
