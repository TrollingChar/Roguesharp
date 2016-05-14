using System;
//using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using C5;

namespace RogueSharp {
    class World {
        public readonly int w, h;
        public int primkey;

        Tile[,] tiles;
        public IntervalHeap<ObjListEntry> objects;
        public string lastError;
        public bool completed;
        public long time;

        public World (int w, int h) {
            tiles = new Tile[w, h];
            for (int x = 0; x < w; x++) {
                for (int y = 0; y < h; y++) {
                    tiles[x, y] = new Tile(this, x, y);
                }
            }
            this.w = w;
            this.h = h;

            lastError = "";
            primkey = 0;
            time = 0;
            objects = new IntervalHeap<ObjListEntry>();
        }

        public void Reset () {
            //lastError = "";
            primkey = 0;
            time = 0;
            while(objects.Count > 0) objects.DeleteMin();
            foreach (Tile tile in tiles) tile.objects.Clear();

            new Player().Place(this, 5, 5);
            for (int x = 0; x < w; x++) {
                for (int y = 10; y < 30; y++) {
                    if (RNG.Next(2) == 0) new Wall().Place(this, x, y);
                }
            }
        }

        public void Start () {
            completed = false;
            Reset();
            //this.level = level;
            //level.Start(this);
        }

        public void Restart () {
            Reset();
            //level.Start(this);
        }

        public void Update () {
            if(Global.keyboard.IsKeyDown(Keys.P)) return;
            
            MyKeyboard.Update(Keyboard.GetState());
            if (MyKeyboard.LastKey == Keys.R) {
                Restart();
                return;
            }

            // path
            if (MyKeyboard.LastKey == Keys.Space) {
                int x = Mouse.GetState().X / 10 - 5,
                    y = Mouse.GetState().Y / 10 - 5;
                if (x >= 0 && y >= 0 && x < w && y < h) {
                    Player p = objects.FindMin().obj as Player;
                    p.path = Algo.AStar(p, this[p._x, p._y], this[x, y]);
                }
            }

            //if (MyKeyboard.LastKey != Keys.Left && MyKeyboard.LastKey != Keys.Right && MyKeyboard.LastKey != Keys.Up && MyKeyboard.LastKey != Keys.Down && MyKeyboard.LastKey != Keys.OemPeriod) return;
            try {
                while (objects.Count > 0 && objects.FindMin().time <= time) {
                    ObjListEntry ole = objects.DeleteMin();
                    if (ole.obj.active && !ole.obj.remove) {
                        ole.obj.Update();
                        if (ole.obj.nextturn <= 0) {
                            //ole.obj.active = false;
                            throw new Exception("Obj.nextturn is " + ole.obj.nextturn.ToString() + ", but must be above zero!");
                        }
                        ole.time += ole.obj.nextturn;
                        ole.obj.nextturn = 0;
                    }
                    if (ole.obj.active && !ole.obj.remove) {
                        objects.Add(ole);
                    }
                }
                time++;
                if (completed) Restart();

            } catch(Exception e) {
                lastError = e/*.GetType()*/.ToString();// e.Message;
            }
        }

        public Tile this[int x, int y] {
            get {
                if (x < 0 || y < 0 || x >= w || y >= h) return new BadTile(this, x, y);
                return tiles[x, y];
            }
        }

        public CharMap GetView (int camX = 0, int camY = 0, int camW = 0, int camH = 0) {
            if (camW <= 0) camW = w;
            if (camH <= 0) camH = h;
            CharMap map = new CharMap(camW, camH);

            for (int x = 0; x < camW && x + camX < w; x++) {
                for (int y = 0; y < camH && y + camY < h; y++) {
                    map.Print(tiles[x + camX, y + camY].GetChar(), x, y);
                }
            }

            return map;
        }
    }
}
