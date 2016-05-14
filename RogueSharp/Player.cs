using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RogueSharp {
    class Player:Obj {
        public LinkedList<Tile> path;

        public Player () {
            active = true;
            path = new LinkedList<Tile>();
        }

        public override void HandleUpdate () {
            while (path.Count > 0 && path.First.Value.x == _x && path.First.Value.y == _y) path.RemoveFirst();
            if (path.Count == 0) {
                nextturn = 1;
                return;
            }

            Tile t = path.First.Value;
            int d = Math.Abs(t.x - _x) + Math.Abs(t.y - _y);
            Move(t.x, t.y);
            nextturn = 3 + d * 2;
            return;
            /*
            int x = _x,
                y = _y;
            bool
                up = MyKeyboard.state.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Up),
                down = MyKeyboard.state.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Down),
                left = MyKeyboard.state.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Left),
                right = MyKeyboard.state.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Right);

            if (up) y--;
            if (down) y++;
            if (left) x--;
            if (right) x++;

            Move(x, y);
            nextturn += up || down || left || right ? 5 : 1;
             */
        }

        public override MyChar Character {
            get {
                return new MyChar('@', MyColor.LGREEN);
            }
        }

        public override string ToString () {
            return "Player #" + primkey.ToString();
        }

        public override int Priority {
            get {
                return 200;
            }
        }
    }
}
