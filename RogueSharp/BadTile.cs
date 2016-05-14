using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RogueSharp {
    class BadTile:Tile {

        public BadTile (World w, int x, int y) : base(w, x, y) { }

        public override bool PassableFor (Obj obj) {
            return false;
        }

        public override ICollection<Tile> GetOrthogonal () {
            return new List<Tile>();
        }

        public override ICollection<Tile> GetDiagonal () {
            return new List<Tile>();
        }

        public override void HandleInteraction (Obj obj, string type, int x = -1, int y = -1) { }

        public override MyChar GetChar () {
            return new MyChar();
        }
    }
}
