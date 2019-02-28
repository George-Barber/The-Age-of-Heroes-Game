using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using The_Age_of_Heroes_Game.Content.Manager;
using The_Age_of_Heroes_Game.Content.Models;

namespace The_Age_of_Heroes_Game.Content.Sprites
{
    public class Sprite
    {
        #region Fields
        protected AnimationManager _animationManager;
        protected Dictionary<string, Animation> _animations;
        protected Vector2 _position;
        protected Texture2D _texture, healthtexture;
        public Rectangle HealthBar;
        public int Health;
        public int HealthMax;
        #endregion
        #region Properties
        public Input Input;
        public Vector2 Position
        {
            get { return _position; }
            set
            {
                _position = value;

                if (_animationManager != null)
                    _animationManager.Position = _position;
            }
        }
        public float Speed = 1f;
        public Vector2 Velocity;
        #endregion
        #region Method

        public virtual void Draw(SpriteBatch spriteBatch, Vector2 vp)
        {
            if (_texture != null)
            {
                spriteBatch.Draw(_texture, Position - vp, Color.White);


                
                Console.WriteLine(Position + " " + vp);
            }
            else if (_animationManager != null)
            {
                _animationManager.Draw(spriteBatch, vp);
                if (HealthBar != null)
                {

                    healthtexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);

                    // Create a 1D array of color data to fill the pixel texture with.  
                    Color[] colorData = {
                        Color.White,
                    };

                    // Set the texture data with our color information.  
                    healthtexture.SetData<Color>(colorData);

                    spriteBatch.Draw(healthtexture, HealthBar, Color.Red);

                }
            }
            else throw new Exception("DRAW ERROR!!!");

        }

        public virtual void Move()
        {
            if (Keyboard.GetState().IsKeyDown(Input.Up))
                Velocity.Y = -Speed;
            else if (Keyboard.GetState().IsKeyDown(Input.Down))
                Velocity.Y = Speed;
            else if (Keyboard.GetState().IsKeyDown(Input.Left))
                Velocity.X = -Speed;
            else if (Keyboard.GetState().IsKeyDown(Input.Right))
                Velocity.X = Speed;
        }

        public Sprite(Dictionary<string, Animation> animations, bool health)
        {
            if (health)
            {
                Health = 10;
                HealthMax = 10;
                HealthBar = new Rectangle(0, 0, 27/HealthMax*Health, 5);

                // Make a 1x1 texture named pixel.  

            }
            _animations = animations;
            _animationManager = new AnimationManager(_animations.First().Value);
             
        }

        public Sprite(Texture2D texture)
        {
            _texture = texture;
        }

        public virtual void Update(GameTime gameTime, List<Sprite> sprites, Vector2 vp)
        {
            Move();

            SetAnimations();
            Position += Velocity;
            Velocity = Vector2.Zero;
            HealthBar.X = (int)Position.X - (int)vp.X;
            HealthBar.Y = (int)Position.Y - (int)vp.Y+80;
            _animationManager.Update(gameTime);
        }

        protected virtual void SetAnimations()
        {
            if (Velocity.Y < 0)
                _animationManager.Play(_animations["Player Forward"]);
            if (Velocity.Y > 0)
                _animationManager.Play(_animations["Player Backwards"]);
            if (Velocity.X > 0)
                _animationManager.Play(_animations["Player Right"]);
            if (Velocity.X < 0)
                _animationManager.Play(_animations["Player Left"]);
            if (Velocity.Y == 0)
            {

            }
            if (Velocity.X == 0)
            {

            }

        }




        #endregion
    }
}