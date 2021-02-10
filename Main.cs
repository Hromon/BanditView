using Godot;
using System;
using Bandit;
using Bandit.Effects;

public class Main : Node
{
    public static Main Base { get; private set; }

    public static Action<float> RenderProcess;
    public static Action<float> PhysicsProcess;
    public static Action<InputEvent> InputProcess;

    private RollingMachine _Machine;
    private Lever _Lever;
    private Broker _Broker;

    private Random _Random = new Random();

    public override void _Ready()
    {
        Initialization();

        _Lever.Click += () =>
        {
            if (_Broker.SpendMoney(_Broker.Bet))
            {
                _Machine.SoundPlayer.LeverPlay();
                return true;
            }

            return false;
        };
        _Lever.StickDown += () =>
        {
            _Machine.Start();
            _Machine.SoundPlayer.ProcessPlay();
        };
        _Machine.RollingController.StartingRolling += (count, segments) =>
        {
            int[] result = new int[count];
            for (int i = 0; i < count; i++)
                result[i] = _Random.Next(0, segments - 1);

            _Broker.CheckBet(result);

            return result;
        };
        _Machine.RollingController.EndingRolling += () =>
        {
            _Machine.SoundPlayer.Stop();

            if (_Broker.PlayBet())
            {
                _Machine.SoundPlayer.WinnerPlay();
                _Machine.LightController.Execute("test");
            }
        };
    }

    private void Initialization()
    {
        Base = this;

        _Broker = new Broker(1000, new int[] { 5,4,3,2 }, GetNode<Label>("GUI/Label"),
            GetNode<Button>("GUI/5"),
            GetNode<Button>("GUI/10"),
            GetNode<Button>("GUI/100"));

        Node lightContainer = GetNode("LampLight");
        int count = lightContainer.GetChildCount();
        Light[] lights = new Light[count];
        for (int i = 0; i < count; i++)
            lights[i] = lightContainer.GetChild<Light>(i);

        Spatial[] rolls = new Spatial[]
        {
            GetNode<Spatial>("Rolls/PivotRoll1"),
            GetNode<Spatial>("Rolls/PivotRoll2"),
            GetNode<Spatial>("Rolls/PivotRoll3")
        };

        SoundPlayer sound = new SoundPlayer(
            ResourceLoader.Load<AudioStream>("res://proces_1.wav"),
            ResourceLoader.Load<AudioStream>("res://ruchka_1.wav"),
            ResourceLoader.Load<AudioStream>("res://eec95da626eda37_1.wav"));

        _Machine = new RollingMachine(sound,
        ResourceLoader.Load<Material>("res://NormalRollMat.tres"),
            ResourceLoader.Load<Material>("res://BlurRollMat.tres"),
            rolls,
            lights);

        _Machine.LightController.AddCommandArray("test", 3,
            new LightCommand(TypeCommand.Color, 0, 0.5f, 1, new Color(0, 0, 1),
            new Color(1, 0.57f, 0.03f), Tween.TransitionType.Linear, Tween.EaseType.In)
            //new LightCommand(TypeCommand.Energy, 2, 0.2f, 5, 0, 1.5f, Tween.TransitionType.Linear, Tween.EaseType.In)
            );

        _Lever = new Lever(new SpatialMaterial() { AlbedoColor = new Color(1, 0.61f, 0.15f) },
            new SpatialMaterial() { AlbedoColor = new Color(1, 0.12f, 0.12f) });
        _Lever.Position = new Vector3(0, -1.035f, -3.5f);
    }

    public override void _Process(float delta)
        => RenderProcess?.Invoke(delta);
    public override void _PhysicsProcess(float delta)
        => PhysicsProcess?.Invoke(delta);
    public override void _Input(InputEvent ev)
        => InputProcess?.Invoke(ev);
}
