using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLearn {

    public class Actor {
        private int[][] Q;
        private int[][] R;
        private GridWorld world;
        private GridCell currentState;

        public Actor(GridWorld world, int x, int y) {
            this.world = world;
            Q = world.InitialQMatrix();
            R = world.InitialRMatrix();

            Move(x, y);
            MainApp.OutputLine(String.Join(" ", world.GetNeighbors(currentState).Select(a => a.ToString())));
        }

        public void SetGoalState(int x, int y) {

        }
        
        public void Move(int x, int y) {
            if (currentState != null)
                currentState.Occupant = null;
            currentState = world.Cell(x, y);
            currentState.Occupant = this;

            MainApp.Output(world.ToString());
        }
    }
}
