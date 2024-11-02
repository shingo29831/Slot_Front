using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;

namespace GameMachine.Model
{
    public static class Sound
    {
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
    }
}