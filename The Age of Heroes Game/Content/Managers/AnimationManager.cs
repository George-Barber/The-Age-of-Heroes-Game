using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using The_Age_of_Heroes_Game.Content.Models;
using Microsoft.Xna.Framework.Input;
using Squared.Tiled;
using System.IO;
using The_Age_of_Heroes_Game.Content.Sprites;

namespace The_Age_of_Heroes_Game.Content.Manager
{
    public class AnimationManager
    {

        public int CurrentFrame { get; set; }
        public int FrameCount { get; private set; }
        public int FrameHeight { get { return Texture.Height; } }
        public float FrameSpeed { get; set; }
        public int FrameWidth { get { return Texture.Width / FrameCount; } }
        public bool IsLooping { get; set; }
        public Texture2D Texture { get; private set; }
        private Animation _animation;
        private float _timer;
        public Vector2 Position { get; set; }
        public Rectangle sourceRect = new Rectangle();

        public AnimationManager(Animation animation)
        {
            _animation = animation;
        }
        public void Draw(SpriteBatch spriteBatch, Vector2 vp)
        {
            Console.WriteLine(Position + " " + vp);
            spriteBatch.Draw(_animation.Texture,
                            Position-vp,
                            new Rectangle(_animation.CurrentFrame * _animation.FrameWidth,
                                          0,
                                          _animation.FrameWidth,
                                          _animation.FrameHeight), 
                            Color.White);
        }
        public void Play(Animation animation)
        {
            if (_animation == animation)
                return;
            _animation = animation;
            _animation.CurrentFrame = 0;
            _timer = 0;
        }
        public void Stop()
        {
            _timer = 0f;
            _animation.CurrentFrame = 0;
        }
        public void Update(GameTime gameTime)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_timer > _animation.FrameSpeed)
            {
                _timer = 0f;
                _animation.CurrentFrame++;
                if (_animation.CurrentFrame >= _animation.FrameCount)
                    _animation.CurrentFrame = 0;
            }
        }
        //public Texture2D GetCurrentFrame()
        //{

        //    Texture2D frame = new Texture2D(new GraphicsDevice(),1,1);
        //    frame.SetData(test);
        //    map.ObjectGroups["Objects"].Objects["Player"].Texture = frame;
        //    currentplayerposition
        //    Position = new Vector2(map.ObjectGroups["Objects"].Objects["Player"].X, map.ObjectGroups["Objects"].Objects["Player"].Y);

        //    return frame;

        //}
    }
}
