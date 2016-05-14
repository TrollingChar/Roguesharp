using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RogueSharp {
    class Obj:IComparable {
        public bool remove = false; // if true, object does not exist.
        public bool active; // if true, object updates every turn, else it only can react on events.
        public int _x = -1, _y = -1;
        public int primkey;
        public long nextturn;
        public World world;
        public Dictionary<string, int> tags = new Dictionary<string,int>(); // "building destroyer: 2"

        public virtual void HandleInteraction (Obj obj, string s, int x = -1, int y = -1) { }
                
        public virtual bool PassableFor (Obj obj) {
            return true;
        }

        // if false, the object moves normally. if true, THIS makes the object move.
        public virtual bool Overrides (Obj obj) {
            return false;
        }

        public void Update () {
            HandleUpdate();
        }

        public virtual void HandleUpdate () { }

        public virtual bool CanMove (int x, int y) {
            return world[x, y].PassableFor(this);
        }

        public virtual bool CanMove (string direction) {
            switch (direction) {
                case "up":
                    return CanMove(_x, _y - 1);
                case "down":
                    return CanMove(_x, _y + 1);
                case "left":
                    return CanMove(_x - 1, _y);
                case "right":
                    return CanMove(_x + 1, _y);
                default:
                    return false;
            }
        }

        public virtual void Move (string direction) {
            switch (direction) {
                case "up":
                    Move(_x, _y - 1);
                    break;
                case "down":
                    Move(_x, _y + 1);
                    break;
                case "left":
                    Move(_x - 1, _y);
                    break;
                case "right":
                    Move(_x + 1, _y);
                    break;
                default:
                    break;
            }
        }

        public virtual void Move (int x, int y) {
            if (CanMove(x, y)) {
                world[_x, _y].objects.Remove(this);
                world[_x, _y].HandleInteraction(this, "leaving", x, y);
                if (remove) return;
                int z;
                z = _x; _x = x; x = z;
                z = _y; _y = y; y = z;
                world[_x, _y].objects.Add(this);
                world[_x, _y].HandleInteraction(this, "collision", x, y);
            } else Interact(x, y);
        }

        public virtual void Interact (int x, int y) { }

        public virtual void HandleRemoval () { }

        public virtual void Place (World w, int x = -1, int y = -1) {
            world = w;
            _x = x;
            _y = y;
            primkey = w.primkey++;
            w[x, y].HandleInteraction(this, "placement", x, y);
            w[x, y].objects.Add(this);
            if (active) w.objects.Add(new ObjListEntry(this, w.time));
        }

        public virtual void Remove () {
            remove = true;
            active = false;
            world[_x, _y].objects.Remove(this);
            world[_x, _y].HandleInteraction(this, "removal", _x, _y);
            HandleRemoval();
        }

        public void SetActive () {
            if (active) return;
            active = true;
            world.objects.Add(new ObjListEntry(this, world.time));
        }

        // clear links after removal 
        public virtual void ClearTiles () {
            world[_x, _y].objects.Remove(this);
        }

        public virtual MyChar Character {
            get {
                return new MyChar('&', MyColor.LRED, MyColor.BLACK);
            }
        }

        // for multi-tile objects
        public virtual MyChar CharAt (int x, int y) {
            return Character;
        }

        public virtual int Priority {
            get {
                return 0;
            }
        }

        public override string ToString () {
            return "Null object #" + primkey.ToString();
        }

        int IComparable.CompareTo (object obj) {
            Obj o = obj as Obj;
            return o.Priority == Priority ?
                o.primkey - primkey :
                o.Priority - Priority;
        }
    }
}
