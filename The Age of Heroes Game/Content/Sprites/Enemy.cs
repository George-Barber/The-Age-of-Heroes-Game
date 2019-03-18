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
    class Enemy : Sprite
    {
        public Enemy(Dictionary<string, Animation> animations, bool health) : base(animations, health)
        {

        }

        public virtual void Update(GameTime gameTime, Vector2 pposition)
        { 
            if (Vector2.Distance(_position, pposition) < 20)
            {


            }
            else if (Vector2.Distance(_position, pposition) < 200)
            {
                Vector2 tempos = pposition - _position;
                tempos = tempos / Vector2.Distance(_position, pposition);
                Console.WriteLine("move enmey: " + tempos);
                Vector2 newpos = _position + tempos;
                _position = newpos;
            }

            SetAnimations();
        }
    }
}
