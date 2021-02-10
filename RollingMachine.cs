using Godot;
using System;
using System.Collections.Generic;
using Bandit.Effects;

namespace Bandit
{
    public class RollingMachine
    {
        private LightController _LightController;
        private RollingController _RollingController;
        private SoundPlayer _SoundPlayer;

        public RollingController RollingController 
            => _RollingController;
        public LightController LightController
            => _LightController;
        public SoundPlayer SoundPlayer 
            => _SoundPlayer;

        public RollingMachine(SoundPlayer soundPlayer, Material normal, Material blur, Spatial[] pivots, Light[] lights)
        {
            _LightController = new LightController(lights);
            _RollingController = new RollingController(4, blur, normal, pivots);
            _SoundPlayer = soundPlayer;
        }

        public void Start()
        {
            _RollingController.Start();
        }
    }
}
