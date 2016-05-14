using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RogueSharp {
    class Wall:Obj {
        public override MyChar Character {
            get {
                return new MyChar('#', MyColor.GRAY);
            }
        }

        public override string ToString () {
            return "Wall #" + primkey.ToString();
        }

        public override bool PassableFor (Obj obj) {
            return false;
        }
    }
}
