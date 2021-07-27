using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace MusicPlayer {
    class Program {
        static string directory;
        static string currentPlayingSongName;
        static System.Media.SoundPlayer player;
        static Random random;
        static Queue<string> queue;
        static FileInfo[] files;

        static void Main(string[] args) {
            queue = new Queue<string>();
            random = new Random();
            player = new System.Media.SoundPlayer();
            directory = Environment.CurrentDirectory;
            files = new DirectoryInfo(directory).GetFiles("*.wav");

            Console.WriteLine("Now playing music from this directory!");
            PlayNext();

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

        static void PlayNext() {
            if (queue.Count == 0) {
                PopulateQueue();
            }
            Console.WriteLine("Queue length: " + queue.Count);
            currentPlayingSongName = queue.Dequeue();
            Console.WriteLine("Playing: '" + currentPlayingSongName + "'");
            player.SoundLocation = directory + "/" + currentPlayingSongName;
            player.Load();
            player.Play();
        }

        static void Stop() {
            Console.WriteLine("Stopped playing: " + "'" + currentPlayingSongName + "'!");
            player.Stop();
        }

        static bool HandleInput(string input) {
            switch(input) {
                case "exit":
                    return false;
                case "play":
                case "next":
                    PlayNext();
                    return true;
                case "stop":
                    Stop();
                    return true;
                default:
                    Console.WriteLine("'" + input + "'" + " is not a recognized command!");
                    return true;
            }
        }
    }
}
