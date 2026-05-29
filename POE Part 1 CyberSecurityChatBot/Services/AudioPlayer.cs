using System;
using System.IO;

namespace CybersecurityAwarenessBotApp.Services
{
    internal class AudioPlayer
    {
        public void PlayGreeting(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    // Use platform-specific audio playback to avoid System.Media.SoundPlayer dependency
                    // Windows-only: use System.Diagnostics.Process to play .wav files with default player
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = filePath,
                        UseShellExecute = true
                    });
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Audio file not found.");
                    Console.ResetColor();
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error playing audio: " + ex.Message);
                Console.ResetColor();
            }
        }
    }
}