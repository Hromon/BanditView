using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bandit.Effects
{
    public class Lever
    {
        private EGL.GodotBase.Nodes3D.Physics.StaticBody _Body;
        private EGL.GodotBase.Utility.Tween _Tween;
        private CollisionShape _CollisionShape;

        private bool IsDown = false;

        private MeshInstance _Basis;
        private MeshInstance _Stick;
        private MeshInstance _Sphere;
        private Spatial _Pivot;

        public event Func<bool> Click;
        public event Action StickDown;

        public float StartPosition = -18;
        public float EndPosition = -90;
        public float Duration = 1;


        public Material BasisMaterial
        {
            get => _Basis.MaterialOverride;
            set
            {
                _Basis.MaterialOverride = value;
                _Stick.MaterialOverride = value;
            }
        }
        public Material SphereMaterial
        {
            get => _Sphere.MaterialOverride;
            set => _Sphere.MaterialOverride = value;
        }
        public Vector3 Position
        {
            get => _Basis.Translation;
            set => _Basis.Translation = value;
        }

        public Lever(Material basisMaterial, Material sphereMaterial)
        {
            _Tween = new EGL.GodotBase.Utility.Tween();
            _Body = new EGL.GodotBase.Nodes3D.Physics.StaticBody();
            _CollisionShape = new CollisionShape() { Shape = new SphereShape() };

            _Body.Base.AddChild(_CollisionShape);
            _Body.Base.AddChild(_Tween.Base);

            _Basis = new MeshInstance() { Mesh = new CubeMesh(), Scale = new Vector3(0.3f, 0.3f, 0.3f) };
            _Stick = new MeshInstance() { Mesh = new CylinderMesh(), Translation = new Vector3(0, 1.808f, 0)
                , Scale = new Vector3(0.2f, 1.5f, 0.2f) };
            _Sphere = new MeshInstance() { Mesh = new SphereMesh(), Scale = new Vector3(0.5f, 0.5f, 0.5f)
            , Translation = new Vector3(0, 3f, 0)};

            _Pivot = new Spatial();

            _Basis.AddChild(_Pivot);
            _Pivot.AddChild(_Stick);
            _Pivot.AddChild(_Sphere);
            _Sphere.AddChild(_Body.Base);

            BasisMaterial = basisMaterial;
            SphereMaterial = sphereMaterial;

            _Pivot.Scale = new Vector3(2, 2, 2);
            _Pivot.RotationDegrees = new Vector3(0, 0, StartPosition);

            Main.Base.AddChild(_Basis);

            _Body.OnInputEvent += Input;
            _Tween.TweenCompleted += _Tween_TweenCompleted; ;
        }

        private void _Tween_TweenCompleted(object arg1, string arg2)
        {
            if (IsDown)
            {
                _Tween.Base.InterpolateProperty(_Pivot, "rotation_degrees:z", EndPosition,
                    StartPosition, Duration, Tween.TransitionType.Linear, Tween.EaseType.In);
                IsDown = false;
                StickDown?.Invoke();
                _Tween.Base.Start();
            }
        }

        private void Input(Node cam, InputEvent ev, Vector3 v1, Vector3 v2, int index)
        {
            if(ev is InputEventMouseButton button && button.Pressed && button.ButtonIndex == (int)ButtonList.Left)
            {
                if(!Click.Invoke()) return;
                _Tween.Base.InterpolateProperty(_Pivot, "rotation_degrees:z", StartPosition, 
                    EndPosition, Duration, Tween.TransitionType.Linear, Tween.EaseType.In);
                IsDown = true;
                _Tween.Base.Start();
            }
        }
    }
}
