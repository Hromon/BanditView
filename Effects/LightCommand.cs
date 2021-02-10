using Godot;


namespace Bandit.Effects
{

    public struct LightCommandArray
    {
        public string Name;
        public int CountRepeat;
        public LightCommand[] Commands;

        public LightCommandArray(string name, int countRepeat, LightCommand[] commands)
        {
            Name = name;
            CountRepeat = countRepeat;
            Commands = commands;
        }
    }

    public struct LightCommand
    {
        public TypeCommand TypeCommand;
        public float Delay;
        public float DelayStep;
        public float Duration;
        public object StartValue;
        public object EndValue;
        public Tween.TransitionType Transition;
        public Tween.EaseType Ease;

        public LightCommand(TypeCommand type, float delay, float delayStep, float duration, 
            object startValue, object endValue, Tween.TransitionType transition, Tween.EaseType ease)
        {
            TypeCommand = type;
            Delay = delay;
            DelayStep = delayStep;
            Duration = duration;
            StartValue = startValue;
            EndValue = endValue;
            Transition = transition;
            Ease = ease;
        }
    }

    public enum TypeCommand
    {
        Color,
        Energy
    }
}
