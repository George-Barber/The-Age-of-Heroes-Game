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
    public class Enemy : Sprite
    {


        public Enemy(Dictionary<string, Animation> animations2, bool health) : base(animations2, health)
        {

        }
        protected virtual void SetAnimations(bool check)
        {
            if (Velocity.Y < 0 && Velocity.X!=0)
                _animationManager.Play(_animations["Enemy Forward"]);
            else if (Velocity.Y > 0 && Velocity.X != 0)
                _animationManager.Play(_animations["Enemy Backwards"]);
            else if (Velocity.Y < 0)
                _animationManager.Play(_animations["Enemy Forward"]);
            else if (Velocity.Y > 0)
                _animationManager.Play(_animations["Enemy Backwards"]);
            else if (Velocity.X > 0)
                _animationManager.Play(_animations["Enemy Right"]);
            else if (Velocity.X < 0)
                _animationManager.Play(_animations["Enemy Left"]);


        }

        public virtual void Draw(SpriteBatch spriteBatch, Vector2 vp, bool check)
        {
            if (_texture != null)
            {
                spriteBatch.Draw(_texture, Position - vp + new Vector2(400,200), Color.White);



                Console.WriteLine(Position + " " + vp);
            }
            else if (_animationManager != null)
            {
                _animationManager.Draw(spriteBatch, Position - vp);
                Console.WriteLine(Position + " " + vp + " " + (Position - vp));
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
        public virtual void Update(GameTime gameTime, Vector2 pposition, Vector2 vp, bool check)
        { 
            if (Vector2.Distance(_position, pposition) < 20)
            {


            }
            else if (Vector2.Distance(_position, pposition) < 200)
            {
                Velocity = (pposition - _position )/ Vector2.Distance(_position, pposition);
                Console.WriteLine("move enmey: " + Velocity + " "+_position);
            }
            else
            {
                Velocity = Vector2.Zero;
            }

            SetAnimations(true);

            Position += Velocity;
            
            HealthBar.X = (int)Position.X - (int)pposition.X;
            HealthBar.Y = (int)Position.Y - (int)pposition.Y + 80;
            if(Velocity!=Vector2.Zero)
                _animationManager.Update(gameTime);
            Velocity = Vector2.Zero;
        }
    }
}
