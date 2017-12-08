using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QLearn {

    class MainApp {

        private const string GOAL = "G";
        
        public static void Output(string msg) { Debug.Write(msg); }
        public static void OutputLine(string msg) { Debug.WriteLine(msg); }

        public static void Main(string[] args) {
            string gridFile = "N:\\cs402\\QLearn\\cf-4th-floor.csv";

            GridWorld world;

            try {
                world = new QLearn.GridWorld(gridFile);
            } catch (System.IO.FileNotFoundException) {
                OutputLine("File not found: " + gridFile);
                return;
            }

            Actor actor = new Actor(world, 1, 1);
        }
    }
}
