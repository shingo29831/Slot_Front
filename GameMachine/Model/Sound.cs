using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;
using System.Numerics;

namespace GameMachine.Model
{
    public class Sound
    {

        public static SoundPlayer lever = new SoundPlayer(typeof(Program).Assembly.GetManifestResourceStream("GameMachine.SoundResources.Lever.wav"));
        public static SoundPlayer StopButton= new SoundPlayer(typeof(Program).Assembly.GetManifestResourceStream("GameMachine.SoundResources.StopButton.wav"));
        public static SoundPlayer reel = new SoundPlayer(typeof(Program).Assembly.GetManifestResourceStream("GameMachine.SoundResources.realon.wav"));
        public static SoundPlayer bet = new SoundPlayer(typeof(Program).Assembly.GetManifestResourceStream("GameMachine.SoundResources.bet2.wav"));


        public static SoundPlayer bell = new SoundPlayer(typeof(Program).Assembly.GetManifestResourceStream("GameMachine.SoundResources.bell.wav"));
        public static SoundPlayer replay = new SoundPlayer(typeof(Program).Assembly.GetManifestResourceStream("GameMachine.SoundResources.replay.wav"));
        public static SoundPlayer suika = new SoundPlayer(typeof(Program).Assembly.GetManifestResourceStream("GameMachine.SoundResources.suika.wav"));
        public static SoundPlayer cherry = new SoundPlayer(typeof(Program).Assembly.GetManifestResourceStream("GameMachine.SoundResources.cheri.wav"));
        public static SoundPlayer big = new SoundPlayer(typeof(Program).Assembly.GetManifestResourceStream("GameMachine.SoundResources.big.wav"));




        public static void Ring(string sound)
        {
            using (Stream? stream = typeof(Program).Assembly.GetManifestResourceStream("GameMachine.SoundResources." + sound))
            {
                if (stream != null)
                {
                    using (SoundPlayer player = new SoundPlayer(stream))
                    {
                        player.PlaySync(); // 再生が完了するまで待つ
                    }
                }
                else
                {
                    Console.WriteLine("音声ファイルが見つかりません。");
                }
            }
        }

        public async Task Task1()
        {
            MessageBox.Show("1");
        }

        public async Task Task2()
        {
            MessageBox.Show("2");
        }

        public static void AllStop()
        {
            lever.Stop();
            StopButton.Stop();
            reel.Stop();
            bet.Stop();

            bell.Stop();
            replay.Stop();
            suika.Stop();
            cherry.Stop();
            big.Stop();
        }

    }
}