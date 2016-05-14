using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RogueSharp {
    struct MyChar {
        public int code;    // if 0, then draws symbol under this symbol
                            // if negative, colors are inverted
        public MyColor color;
        public MyColor background;

        //public MyChar () : this(0, MyColor.BLACK, MyColor.BLACK) { }

        public MyChar (int code, MyColor color = MyColor.LGRAY, MyColor background = MyColor.TRANSPARENT) {
            this.code = code;
            this.color = color;
            this.background = background;
        }
    }
}
