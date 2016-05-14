using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace RogueSharp {
    class MyKeyboard {
        public static KeyboardState state;
        public static Keys lastkey = Keys.None;
        public static int delay = 0;
        public static int iteration = 0;

        public static void Update (KeyboardState state) {
            // hold key
            if (lastkey != Keys.None) {
                if (state.IsKeyDown(lastkey)) {
                    if (delay == 0) {
                        delay = 25 / ++iteration;
                        if (delay < 2) delay = 2;
                    } else {
                        delay--;
                    }
                } else/* if (MyKeyboard.state.IsKeyDown(lastkey))*/ {
                    //throw new Exception();
                    delay = iteration = 0;
                    lastkey = Keys.None;
                }
            }

            // press new key
            foreach (Keys key in Enum.GetValues(typeof(Keys))) {
                if (MyKeyboard.state.IsKeyUp(key) && state.IsKeyDown(key)) {
                    lastkey = key;
                    delay = iteration = 0;
                    break;
                }
            }
            MyKeyboard.state = state;
        }

        public static Keys LastKey {
            get {
                return delay == 0 ? lastkey : Keys.None;
            }
        }
    }
}
