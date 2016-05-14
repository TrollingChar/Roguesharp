using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RogueSharp {
    /// <summary>
    /// This is the main s for your game.
    /// </summary>
    public class MyGame : Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Color[] colors;
        Texture2D font, cursor;

        CharMap map;

        World world;

        public MyGame () {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize () {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferWidth = 1000;
            graphics.PreferredBackBufferHeight = 600;
            graphics.ApplyChanges();
            colors = new Color[] {
                new Color(0, 0, 0),
                new Color(0, 0, 160),
                new Color(0, 120, 0),
                new Color(0, 120, 120),
                new Color(140, 0, 0),
                new Color(160, 0, 160),
                new Color(160, 120, 20),
                new Color(160, 160, 160),
                new Color(80, 80, 80),
                new Color(40, 80, 240),
                new Color(0, 220, 0),
                new Color(80, 220, 220),
                new Color(240, 0, 0),
                new Color(240, 80, 240),
                new Color(220, 220, 40),
                new Color(240, 240, 240),
            };
            //game = this;
            TargetElapsedTime = System.TimeSpan.FromMilliseconds(10);

            (world = new World(50, 50)).Start();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent () {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Content.Load<Texture2D>("10x10");
            cursor = Content.Load<Texture2D>("cursor");
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent () {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the w,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update (GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            Global.mouse = Mouse.GetState();
            Global.keyboard = Keyboard.GetState();

            // TODO: Add your update logic here
            world.Update();

            map = new CharMap(new MyChar(0xb0, MyColor.BROWN, MyColor.GRAY), 100, 60);
            
            map.Print(world.lastError, 0, 0, MyColor.LMAGENTA, MyColor.BLACK);

            CharMap _map = new CharMap(50, 50);
            _map.Print(world.GetView(0, 0, 50, 50), 0, 0);
            map.Print(_map, 5, 5);

            _map = new CharMap(35, 50);
            int x = Global.mouse.X / 10 - 5,
                y = Global.mouse.Y / 10 - 5,
                i = 0;
            foreach (Obj obj in world[x,y].objects) {
                _map.Print(obj.ToString(), 1, i++, obj.remove ? MyColor.GRAY : obj.active ? MyColor.WHITE : MyColor.LGRAY);
            }
            i = 10;
            foreach (ObjListEntry ole in world.objects) {
                Obj obj = ole.obj;
                _map.Print(obj.ToString(), 1, i++, obj.remove ? MyColor.GRAY : obj.active ? MyColor.WHITE : MyColor.LGRAY);
            }
            string s = (world.time / 100).ToString() + "." + (world.time % 100).ToString("00");
            _map.Print(s, _map.w - s.Length, 0, MyColor.LGREEN);
            map.Print(_map, 60, 5);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw (GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            ShowMap(map);
            spriteBatch.Draw(cursor, new Rectangle(Global.mouse.X, Global.mouse.Y, 16, 16), Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        void ShowMap (CharMap map) {
            for (int x = 0; x < map.w; x++) {
                for (int y = 0; y < map.h; y++) {
                    MyChar ch = map[x, y];
                    int code = ch.code;
                    Color color, background;
                    if (code < 0) {
                        color = colors[(int)ch.background];
                        background = colors[(int)ch.color];
                        code = -code;
                    } else {
                        color = colors[(int)ch.color];
                        background = colors[(int)ch.background];
                    }
                    spriteBatch.Draw(font, new Rectangle(x * 10, y * 10, 10, 10), new Rectangle(0, 0, 10, 10), background);
                    spriteBatch.Draw(font, new Rectangle(x * 10, y * 10, 10, 10), new Rectangle(code % 16 * 10, code / 16 * 10, 10, 10), color);
                }
            }
        }
    }
}
