using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bandit
{
    public class SoundPlayer
    {
        private AudioStreamPlayer _StreamPlayer;

        public AudioStream ProcessSound;
        public AudioStream LeverSound;
        public AudioStream WinnerSound;

        public SoundPlayer(AudioStream process, AudioStream lever, AudioStream winner)
        {
            ProcessSound = process;
            LeverSound = lever;
            WinnerSound = winner;

            _StreamPlayer = new AudioStreamPlayer();
            Main.Base.AddChild(_StreamPlayer);
        }

        public void ProcessPlay()
        {
            _StreamPlayer.Stream = ProcessSound;
            _StreamPlayer.Play();
        }
        public void LeverPlay()
        {
            _StreamPlayer.Stream = LeverSound;
            _StreamPlayer.Play();
        }
        public void WinnerPlay()
        {
            _StreamPlayer.Stream = WinnerSound;
            _StreamPlayer.Play();
        }
        public void Stop()
        {
            _StreamPlayer.Stop();
        }
    }
}
