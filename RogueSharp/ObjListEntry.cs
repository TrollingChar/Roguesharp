using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RogueSharp {
    struct ObjListEntry:IComparable<ObjListEntry> {
        public Obj obj;
        public long time;

        public ObjListEntry (Obj obj, long time) {
            this.obj = obj;
            this.time = time;
        }

        public int CompareTo (ObjListEntry other) {
            if (time == other.time) {
                return obj.primkey.CompareTo(other.obj.primkey);
            }
            return time.CompareTo(other.time);
        }
    }
}
