using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Squared.Tiled;
using System.IO;
using System.Collections.Generic;
using System;
using The_Age_of_Heroes_Game.Content.Sprites;
using The_Age_of_Heroes_Game.Content.Models;
using A1r.SimpleTextUI;
using System.Timers;

namespace The_Age_of_Heroes_Game
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Map map;
        Layer collision;
        private Vector2 viewportPosition;
        int tilepixel;
        public Texture2D PlayerAnimation;
        private List<Sprite> _sprites;
        private Texture2D blankTexture;
        private Texture2D coinTexture;
        private readonly Texture2D keyTexture;
        List<Squared.Tiled.Object> Inventory;
        int coin_collected = 0;
        SimpleTextUI menu;
        SimpleTextUI options;
        SimpleTextUI current;
        SpriteFont big;
        SpriteFont small;
        Timer keytimer;
        // Get the width of the player ship
        // The time since we last updated the frame
        int elapsedTime;

        // The number of frames that the animation contains
        int frameCount;

        // The index of the current frame we are displaying
        int currentFrame;

        // The color of the frame we will be displaying
        Color color;

        // The area of the image strip we want to display
        public Rectangle sourceRect = new Rectangle();

        // The area where we want to display the image strip in the game
        public Rectangle destinationRect = new Rectangle();

        // Width of a given frame
        public int FrameWidth;

        // Height of a given frame
        public int FrameHeight;
        private int scale;

        public object Position { get; private set; }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            FrameWidth = 115;
            FrameHeight = 69;
            frameCount = 8;

            // Set the time to zero
            elapsedTime = 0;
            currentFrame = 0;
            base.Initialize();
            graphics.IsFullScreen = true;
            graphics.ApplyChanges();
        }
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            map = Map.Load(Path.Combine(Content.RootDirectory, "SimpleRPG.tmx"), Content);
            collision = map.Layers["Collision"];
            tilepixel = map.TileWidth;
            var animations = new Dictionary<string, Animation>()
            {
                { "Player Forward", new Animation(Content.Load<Texture2D>("Player/Player Forward"), 3) },
                { "Player Backwards", new Animation(Content.Load<Texture2D>("Player/Player Backwards"), 3) },
                { "Player Left", new Animation(Content.Load<Texture2D>("Player/Player Left"), 3) },
                { "Player Right", new Animation(Content.Load<Texture2D>("Player/Player Right"), 3) },
            };
            _sprites = new List<Sprite>()
            {
                new Sprite(new Dictionary<string, Animation>()
                {
                    { "Player Forward", new Animation(Content.Load<Texture2D>("Player/Player Forward"), 3) },
                    { "Player Backwards", new Animation(Content.Load<Texture2D>("Player/Player Backwards"), 3) },
                    { "Player Left", new Animation(Content.Load<Texture2D>("Player/Player Left"), 3) },
                    { "Player Right", new Animation(Content.Load<Texture2D>("Player/Player Right"), 3) },
                })
                {
                    Position = new Vector2(100, 100),
                    Input = new Input()
                    {
                        Up = Keys.Up,
                        Down = Keys.Down,
                        Left = Keys.Left,
                        Right = Keys.Right,
                    }
                },
            };

            big = Content.Load<SpriteFont>("Big");
            small = Content.Load<SpriteFont>("Small");
            // Set menus and screens
            menu = new SimpleTextUI(this, big, new[] { "Play", "Options", "Credits", "Exit" })
            {
                TextColor = Color.Purple,
                SelectedElement = new TextElement(">", Color.Green),
                Align = Alignment.Left
            };

            options = new SimpleTextUI(this, big, new TextElement[]
            {
                new SelectElement("Video", new[]{"FullScreen","Windowed"}),
                new NumericElement("Music",1,3,0f,10f,1f),
                new TextElement("Back")
            });
            current = menu;
            keytimer = new Timer();

            coinTexture = Content.Load<Texture2D>("coinTexture");
            blankTexture = Content.Load<Texture2D>("Transparent");
            int coinCount = Convert.ToInt32(map.ObjectGroups["Objects"].Properties["Coin_Count"]);
            for (int i = 1; i <= coinCount; i++)
            {
                map.ObjectGroups["Objects"].Objects["Coin" + i].Texture = coinTexture;
            }


            Inventory = new List<Squared.Tiled.Object>();
            // TODO: use this.Content to load your game content here
        }
        


        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {

            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            KeyboardState keys = Keyboard.GetState();
            bool change = true;

            if (!keytimer.Enabled)
            {
                if (keys.IsKeyDown(Keys.Up))
                {
                    current.Move(Direction.Up);
                }

                else if (keys.IsKeyDown(Keys.Down))
                {
                    current.Move(Direction.Down);
                }
                else
                    change = false;

                if (change)
                {
                    keytimer = new Timer();
                    keytimer.Interval = 200;
                    keytimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
                    keytimer.Enabled = true;
                }
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
            
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyState = Keyboard.GetState();
            //ProcessMovement(keyState, gamePadState);
            //store current position to move back too if collision
            int tempx = map.ObjectGroups["Objects"].Objects["Player"].X;
            int tempy = map.ObjectGroups["Objects"].Objects["Player"].Y;
            Position = new Vector2(map.ObjectGroups["Objects"].Objects["Player"].X, map.ObjectGroups["Objects"].Objects["Player"].Y);
            ProcessMovement(keyState, gamePadState);

            //now we have moved checkbounds
            if (CheckBounds())
            {
                map.ObjectGroups["Objects"].Objects["Player"].X = tempx;
                map.ObjectGroups["Objects"].Objects["Player"].Y = tempy;
            }
            var p = map.ObjectGroups["Objects"].Objects["Player"];
            Rectangle playerRec = new Rectangle(p.X, p.Y, p.Width, p.Height);
            CheckCoins(playerRec);
            foreach (var sprite in _sprites)
                sprite.Update(gameTime, _sprites);
            elapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            // If the elapsed time is larger than the frame time
            // we need to switch frames
            if (elapsedTime > 100)
            {
                // Move to the next frame
                currentFrame++;

                // If the currentFrame is equal to frameCount reset currentFrame to zero 
                if (currentFrame == frameCount) 
                {
                    currentFrame = 0;
                }

                // Reset the elapsed time to zero
                elapsedTime = 0;
            }

            // Grab the correct frame in the image strip by multiplying the currentFrame index by the Frame width

            sourceRect = new Rectangle(currentFrame * FrameWidth, 0, FrameWidth, FrameHeight);
            _sprites[0].Position = new Vector2(map.ObjectGroups["Objects"].Objects["Player"].X, map.ObjectGroups["Objects"].Objects["Player"].Y);
            // Grab the correct frame in the image strip by multiplying the currentFrame index by the frame width
            viewportPosition = new Vector2(map.ObjectGroups["Objects"].Objects["Player"].X - (graphics.PreferredBackBufferWidth / 2), map.ObjectGroups["Objects"].Objects["Player"].Y - (graphics.PreferredBackBufferHeight / 2));
            base.Update(gameTime);
        }
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            keytimer.Enabled = false;
        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// 
        protected override void Draw(GameTime gameTime)
        {
            current.Draw(gameTime);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            map.Draw(spriteBatch, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), viewportPosition);
            foreach (var sprite in _sprites)
            {
                sprite.Draw(spriteBatch, viewportPosition +new Vector2(0,100));
            }

            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);

        }


        
        
        public void ProcessMovement(KeyboardState keyState, GamePadState gamePadState)
        {
            //detect key press and xy scroll values
            int scrollx = 0, scrolly = 0, moveSpeed = 2;
            if (keyState.IsKeyDown(Keys.Left))
                scrollx = -1;
            if (keyState.IsKeyDown(Keys.Right))
                scrollx = 1;
            if (keyState.IsKeyDown(Keys.Up))
                scrolly = 1;
            if (keyState.IsKeyDown(Keys.Down))
                scrolly = -1;

            scrollx += (int)gamePadState.ThumbSticks.Left.X;
            scrolly += (int)gamePadState.ThumbSticks.Left.Y;

            map.ObjectGroups["Objects"].Objects["Player"].X += (scrollx * moveSpeed);
            map.ObjectGroups["Objects"].Objects["Player"].Y -= (scrolly * moveSpeed);
            
        }
        
        public bool CheckBounds()
        {
            bool check = false;

            Rectangle playrec = new Rectangle(
                map.ObjectGroups["Objects"].Objects["Player"].X,
                map.ObjectGroups["Objects"].Objects["Player"].Y,
                map.ObjectGroups["Objects"].Objects["Player"].Width,
                map.ObjectGroups["Objects"].Objects["Player"].Height
                );

            for (int x = 0; x < map.Width; x++)
            {
                for (int y = 0; y < map.Height; y++)
                {
                    if (collision.GetTile(x, y) != 0)
                    {
                        Rectangle tile = new Rectangle(
                            (int)x * tilepixel,
                            (int)y * tilepixel,
                            tilepixel,
                            tilepixel
                            );

                        if (playrec.Intersects(tile))
                            check = true;
                    }
                }
            }

            return check;
        }
        
        public void CheckCoins(Rectangle player)
        {
            int coinCount = Convert.ToInt32(map.ObjectGroups["Objects"].Properties["Coin_Count"]);
            int collectCount = 0;

            for (int i = 1; i <= coinCount; i++)
            {
                var coin = map.ObjectGroups["Objects"].Objects["Coin" + i];
                if (coin.Texture != blankTexture)
                {
                    Rectangle coinRec = new Rectangle(coin.X, coin.Y, coin.Width, coin.Height);
                    if (player.Intersects(coinRec))
                    {
                        Inventory.Add(coin);
                        coin_collected++;
                        Console.WriteLine("collision - " + i);
                        coin.Texture = blankTexture;
                    }
                }
                else
                {
                    collectCount++;
                }
            }

        }
    }
}

