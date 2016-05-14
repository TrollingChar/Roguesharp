using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace RogueSharp {
    class Bigobj:Obj {
        public BitArray mask;

        public override void Place (World w, int x = -1, int y = -1) {
            world = w;
            _x = x;
            _y = y;
            primkey = w.primkey++;
            for (int i = x - 1; i <= x + 1; i++) {
                for (int j = y - 1; j <= y + 1; j++) {
                    w[i, j].HandleInteraction(this, "placement", x-1, y);
                }
            }
            for (int i = x - 1; i <= x + 1; i++) {
                for (int j = y - 1; j <= y + 1; j++) {
                    w[i, j].objects.Add(this);
                }
            }
            if (active) w.objects.Add(new ObjListEntry(this, w.time));
        }

        public override bool CanMove (int x, int y) {
            for (int i = x - 1; i <= x + 1; i++) {
                for (int j = y - 1; j <= y + 1; j++) {
                    if (!world[i, j].PassableFor(this))
                        return false;
                }
            }
            return true;
        }

        public override void Move (int x, int y) {
            if (CanMove(x, y)) {
                for (int i = _x - 1; i <= _x + 1; i++) {
                    for (int j = _y - 1; j <= _y + 1; j++) {
                        world[i, j].objects.Remove(this);
                    }
                }
                for (int i = _x - 1; i <= _x + 1; i++) {
                    for (int j = _y - 1; j <= _y + 1; j++) {
                        world[i, j].HandleInteraction(this, "leaving", x, y);
                    }
                }
                if (remove) return;
                int z;
                z = _x; _x = x; x = z;
                z = _y; _y = y; y = z;
                for (int i = _x - 1; i <= _x + 1; i++) {
                    for (int j = _y - 1; j <= _y + 1; j++) {
                        world[i, j].objects.Add(this);
                    }
                }
                for (int i = _x - 1; i <= _x + 1; i++) {
                    for (int j = _y - 1; j <= _y + 1; j++) {
                        world[i, j].HandleInteraction(this, "collision", x, y);
                    }
                }
            } else {

            }
        }

        public override void Remove () {
            remove = true;
            active = false;
            //world[_x, _y].objects.Remove(this);
            //world[_x, _y].HandleInteraction(this, "removal", _x, _y);
            for (int i = _x - 1; i <= _x + 1; i++) {
                for (int j = _y - 1; j <= _y + 1; j++) {
                    world[i, j].objects.Remove(this);
                }
            } 
            for (int i = _x - 1; i <= _x + 1; i++) {
                for (int j = _y - 1; j <= _y + 1; j++) {
                    world[i, j].HandleInteraction(this, "leaving", _x, _y);
                }
            }
            HandleRemoval();
        }

        public override bool PassableFor (Obj obj) {
            return obj == this;
        }

        public override string ToString () {
            return "Null multitile object #" + primkey.ToString();
        }
    }
}
