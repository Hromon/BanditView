using Godot;
using System;
using Bandit.Utility;

namespace Bandit.Effects
{
    public class RollingController
    {
        private Spatial[] _Pivots;
        private MeshInstance[] _Rollings;
        private Material _Blur;
        private Material _Normal;

        private EGL.GodotBase.Utility.Tween _Tween;

        private int _NumberSegment;
        private float _RotationFromSegment;


        public float RollingTime = 2f;
        public float RollingTimeStep = 0.5f;
        public float RollingDelay = 0.1f;
        public float Speed = 7f;

        public int NumberSegment
            => _NumberSegment;
        public event Func<int, int, int[]> StartingRolling;
        public event Action EndingRolling;

        public RollingController(int numberSegment, Material blur, Material normal, Spatial[] pivots)
        {
            _Pivots = pivots;

            _Rollings = new MeshInstance[_Pivots.Length];
            for (int i = 0; i < _Pivots.Length; i++)
                _Rollings[i] = _Pivots[i].GetChild<MeshInstance>(0);

            _Blur = blur;
            _Normal = normal;

            _Tween = new EGL.GodotBase.Utility.Tween();
            _Tween.TweenAllComleted += EndRolling;
            _Tween.TweenCompleted += StopRoll;
            Main.Base.AddChild(_Tween.Base);

            _NumberSegment = numberSegment;
            _RotationFromSegment = 180f / numberSegment;
        }

        public void Start()
        {
            int[] result = StartingRolling.Invoke(_Rollings.Length, _NumberSegment);

            for (int i = 0; i < _Rollings.Length; i++)
            {
                _Rollings[i].MaterialOverride = _Blur;
                _Tween.Base.InterpolateProperty(_Pivots[i], "rotation_degrees:z", 180 * Speed,
                    _RotationFromSegment * result[i] + 90 + _RotationFromSegment / 2,
                    RollingTime + i * RollingTimeStep, Tween.TransitionType.Linear, Tween.EaseType.InOut, RollingDelay);
                _Tween.Base.Start();
            }
        }

        private void StopRoll(object roll, string path)
        {
            Spatial pivot = (Spatial)roll;
            pivot.GetChild<MeshInstance>(0).MaterialOverride = _Normal;
        }
        private void EndRolling()
            => EndingRolling?.Invoke();
    }
}
