using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RogueSharp {
    static class RNG {
        static Random r = new Random();

        public static int Next (int n) {
            return r.Next(n);
        }

        public static bool[] GenArray (int len, int items) {
            bool[] arr = new bool[len];
            for (int i = len; i > 0; i--) {
                if (Next(i) < items) {
                    items--;
                    arr[len - i] = true;
                }
            }
            return arr;
        }

        public static int Round (double n) {
            int result = (int)Math.Floor(n);
            if (r.NextDouble() < n - result) result++;
            return result;
        }
    }
}
