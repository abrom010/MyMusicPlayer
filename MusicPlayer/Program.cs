using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Windows.Media;

namespace MusicPlayer {
    class Program {
        static string directory;
        static string currentPlayingSongName;
        static Random random;
        static Queue<string> queue;
        static FileInfo[] files;
        static MediaPlayer mediaPlayer;

        static void Main(string[] args) {
            queue = new Queue<string>();
            random = new Random();
            directory = Environment.CurrentDirectory;
            files = new DirectoryInfo(directory).GetFiles("*.wav");
            mediaPlayer = new MediaPlayer();
            mediaPlayer.Volume = 0.1;

            Console.WriteLine("Now playing music from this directory!");
            Next();

            while (true) {
                Console.Write(">>> ");
                if (!HandleInput(Console.ReadLine())) break;
            }
        }

        static void PopulateQueue() {
            int n = files.Length;
            int[] indices = new int[n];
            int i = 0;

            for (i = 0; i < n; i++) {
                indices[i] = i;
            }

            i = n;
            while (i > 1) {
                int r = random.Next(i--);
                int temp = indices[i];
                indices[i] = indices[r];
                indices[r] = temp;
            }

            foreach (int index in indices) {
                queue.Enqueue(files[indices[index]].Name);
            }
        }

        static void Play() {
            Console.WriteLine("Playing: '" + currentPlayingSongName + "'");
            mediaPlayer.Play();
        }

        static void Next() {
            if (queue.Count == 0) {
                PopulateQueue();
            }
            Console.WriteLine("Queue length: " + queue.Count);
            currentPlayingSongName = queue.Dequeue();
            Console.WriteLine("Playing: '" + currentPlayingSongName + "'");

            string uri = directory + "/" + currentPlayingSongName;
            mediaPlayer.Open(new Uri(uri));
            mediaPlayer.Play();
        }

        static void Pause() {
            Console.WriteLine("Paused: " + "'" + currentPlayingSongName + "'!");
            mediaPlayer.Pause();
        }

        static void Stop() {
            Console.WriteLine("Stopped: " + "'" + currentPlayingSongName + "'!");
            mediaPlayer.Stop();
        }

        static void Volume(int volume) {
            mediaPlayer.Volume = (float)volume / 100;
        }

        static bool HandleInput(string input) {
            if (input == "exit") {
                return false;
            } else if (input == "play") {
                Play();
            } else if (input == "next") {
                Next();
            } else if (input == "pause") {
                Pause();
            } else if (input == "stop") {
                Stop();
            } else if(input.Substring(0, 6) == "volume") {
                string[] strs = input.Split(' ');
                Volume(Int32.Parse(strs[1]));
            } else {
                Console.WriteLine("'" + input + "'" + " is not a recognized command!");
            }
            return true;
        }
    }
}
