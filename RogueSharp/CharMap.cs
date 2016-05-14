using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RogueSharp {
    class CharMap {
        MyChar[,] matrix;
        public readonly int w, h;

        public CharMap (int w = 100, int h = 60) {
            matrix = new MyChar[w, h];
            this.w = w;
            this.h = h;
        }

        public CharMap (MyChar ch, int w = 100, int h = 60) {
            matrix = new MyChar[w, h];
            for (int x = 0; x < w; x++) {
                for (int y = 0; y < h; y++) {
                    matrix[x, y] = ch;
                }
            }
            this.w = w;
            this.h = h;
        }

        public MyChar this[int x, int y] {
            get { return matrix[x, y]; }
            set { matrix[x, y] = value; }
        }

        public void Print (MyChar ch, int x, int y) {
            if (x < 0 || y < 0 || x >= w || y >= h) return;
            matrix[x, y].code = 0;
            if (ch.code != 0) matrix[x, y].code = ch.code;
            if (ch.color != MyColor.TRANSPARENT) matrix[x, y].color = ch.color;
            if (ch.background != MyColor.TRANSPARENT) matrix[x, y].background = ch.background;
        }

        public void Print (string s, int x, int y, MyColor color = MyColor.LGRAY, MyColor background = MyColor.TRANSPARENT) {
            if (y < 0 || y >= h) return;
            for (int i = 0; i < s.Length && x + i < w; i++) {
                Print(new MyChar((int)s[i], color, background), x + i, y);
            }
        }

        public void Print (CharMap map, int x, int y) {
            for (int i = 0; i < map.w && x + i < w; i++) {
                for (int j = 0; j < map.h && y + j < h; j++) {
                    Print(map[i, j], x + i, y + j);
                }
            }
        }
    }
}
