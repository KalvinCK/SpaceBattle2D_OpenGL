using SFML.Audio;

namespace MyGame
{
    public class GameMusic
    {
        private Music music;
        public GameMusic(string MusicPath, int volume = 100)
        {
            if(volume > 100 || volume < 0)
            {
                volume = 100;
            }
            music = new Music(MusicPath)
            {
                Loop = true,
                Volume = (float)volume

            };
            
        }
        public void Play() => music.Play();
        public void Stop()
        {
            music.Stop();
        }
        public void Dispose() => music.Dispose();
    }
    public class Sounds : IDisposable
    {
        private SoundBuffer Buffer;
        private Sound sound;
        public Sounds(string auidoPath, int volume = 100)
        {
            Buffer = new SoundBuffer(auidoPath);
            sound = new Sound(Buffer)
            {
                Volume = volume,
            };
        }
        public void Play() => sound.Play();
        public void Dispose()
        {
            sound.Dispose();
            Buffer.Dispose();
        }
    }
}