using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RogueSharp {
    class Tile {
        public World world;
        public SortedSet<Obj> objects;
        public readonly int x, y;

        public Tile (World w, int x, int y) {
            world = w;
            this.x = x;
            this.y = y;
            objects = new SortedSet<Obj>();
        }

        public virtual ICollection<Tile> GetOrthogonal () {
            List<Tile> result = new List<Tile>();
            if (!(world[x - 1, y] is BadTile)) result.Add(world[x - 1, y]);
            if (!(world[x + 1, y] is BadTile)) result.Add(world[x + 1, y]);
            if (!(world[x, y - 1] is BadTile)) result.Add(world[x, y - 1]);
            if (!(world[x, y + 1] is BadTile)) result.Add(world[x, y + 1]);
            return result;
        }

        public virtual ICollection<Tile> GetDiagonal () {
            List<Tile> result = new List<Tile>();
            if (!(world[x - 1, y - 1] is BadTile)) result.Add(world[x - 1, y - 1]);
            if (!(world[x - 1, y + 1] is BadTile)) result.Add(world[x - 1, y + 1]);
            if (!(world[x + 1, y - 1] is BadTile)) result.Add(world[x + 1, y - 1]);
            if (!(world[x + 1, y + 1] is BadTile)) result.Add(world[x + 1, y + 1]);
            return result;
        }

        public virtual void HandleInteraction (Obj obj, string type, int x = -1, int y = -1) {
            foreach (Obj o in objects.ToList()) // prevents exception
                if (o != obj && !o.remove)
                    o.HandleInteraction(obj, type, x, y);
        }

        virtual public bool PassableFor (Obj obj) {
            foreach (Obj o in objects)
                if (!o.PassableFor(obj) && !o.remove)
                    return false;
            return true;
        }
        /*
        virtual public void OnCollision (Obj obj, int x = -1, int y = -1) {
            foreach (Obj o in objects.ToList())
                if (o != obj && !o.Deleting)
                    o.OnCollision(obj, x, y);
        }

        virtual public void OnInteraction (Obj obj, string interaction) {
            foreach (Obj o in objects.ToList())
                if (o != obj && !o.Deleting) 
                    o.OnInteraction(obj, interaction);            
        }
        */
        virtual public MyChar GetChar () {
            MyChar ch = new MyChar(0, MyColor.TRANSPARENT, MyColor.TRANSPARENT);
            foreach (Obj obj in objects) {
                if (obj.remove) continue;
                MyChar temp = obj.CharAt(x, y);
                if (ch.code == 0) ch.code = temp.code;
                if (ch.color == MyColor.TRANSPARENT) ch.color = temp.color;
                if (ch.background == MyColor.TRANSPARENT) ch.background = temp.background;
                if (ch.code != 0 && ch.color != 0 && ch.background != 0) break;
            }
            return ch;
        }
        /*
        virtual public void OnLeaving (Obj obj, int x = -1, int y = -1) {
            foreach (Obj o in objects.ToList())
                if (o != obj && !o.Deleting)
                    o.OnLeaving(obj, x, y);
        }*/
    }
}
