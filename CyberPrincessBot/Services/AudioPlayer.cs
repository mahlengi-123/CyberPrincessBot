using System;
using System.Media;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSafeBot.Services
{
    internal class AudioPlayer
    {
        public void PlayGreeting(string filePath)
        {
            if (File.Exists(filePath))
            {
                SoundPlayer player = new SoundPlayer(filePath);
                player.Play();
            }
        }
    }
}
    

