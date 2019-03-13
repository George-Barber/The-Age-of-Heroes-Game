using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace The_Age_of_Heroes_Game
{
    class NPCAI
    {
        static Actor Actor;
        static Vector2 actorPos;
        static int xDistance;
        static int yDistance;

        public static void SetActorInput()
        {
            Pool.activeActor++; //increment the active actor
            if (Pool.activeActor >= Pool.actorCount) { Pool.activeActor = 1; } //skip 0th actor (HERO)
            //target the active actor from the actor's pool
            Actor = Pool.actorPool[Pool.activeActor];
            //if this actor isn't active, don't pass AI to it
            if (Actor.active == false) { return; }
            //if this actor is dead, don't pass AI to it
            if (Actor.state == ActorState.Dead) { return; }
            //reset the target actor's input
            Functions_Input.ResetInputData(Actor.compInput);

            //get the x & y distances between actor and hero hitbox
            xDistance = (int)Math.Abs(Pool.hero.compCollision.rec.Center.X - actorPos.X);
            yDistance = (int)Math.Abs(Pool.hero.compCollision.rec.Center.Y - actorPos.Y);


        }

        public static void ChaseHero()
        {

            //actor is close enough to chase hero, set actor's direction to hero
            if (yDistance < Actor.chaseRadius && xDistance < Actor.chaseRadius)
            {
                if (Actor.chaseDiagonally)
                {   //chasing diagonally lets actors slide around blocking objs, appearing smart
                    Actor.compInput.direction = Functions_Direction.GetDiagonalToHero(actorPos);
                }
                else
                {   //chasing cardinally lets actors easily get stuck on blocking objs, appearing dumb
                    Actor.compInput.direction = Functions_Direction.GetCardinalDirectionToHero(actorPos);
                }   //some actor sprites require cardinal movement, tho
            }
        }
    }
}
