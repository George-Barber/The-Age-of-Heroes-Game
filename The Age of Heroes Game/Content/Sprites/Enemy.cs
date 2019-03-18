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
        public Enemy(Dictionary<string, Animation> animations, bool health) : base(animations, health)
        {

        }
        protected virtual void SetAnimations()
        {
            if (Velocity.Y < 0 && Velocity.X!=0)
                _animationManager.Play(_animations["Player Forward"]);
            else if (Velocity.Y > 0 && Velocity.X != 0)
                _animationManager.Play(_animations["Player Backwards"]);
            else if (Velocity.Y < 0)
                _animationManager.Play(_animations["Player Forward"]);
            else if (Velocity.Y > 0)
                _animationManager.Play(_animations["Player Backwards"]);
            else if (Velocity.X > 0)
                _animationManager.Play(_animations["Player Right"]);
            else if (Velocity.X < 0)
                _animationManager.Play(_animations["Player Left"]);


        }
        public virtual void Update(GameTime gameTime, Vector2 pposition)
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

            SetAnimations();

            Position += Velocity;
            
            HealthBar.X = (int)Position.X - (int)pposition.X;
            HealthBar.Y = (int)Position.Y - (int)pposition.Y + 80;
            if(Velocity!=Vector2.Zero)
                _animationManager.Update(gameTime);
            Velocity = Vector2.Zero;
        }
    }
}
