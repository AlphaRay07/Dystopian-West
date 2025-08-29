using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Microsoft.Xna.Framework.Input;
//using System.Numerics;
//using System.Runtime.CompilerServices;

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

namespace DystopianWest
{
    internal class Projectile
    {

        public Texture2D Texture;
        public Vector2 Position;
        public float Speed=1000;
        public bool IsActive;

        public Projectile(Texture2D texture, Vector2 position,Texture2D shooter)
        {
            IsActive = true;
            this.Texture = texture;
            this.Position = position;
            this.Position.X+=  shooter.Width/2 - Texture.Width/2;
        }

        Rectangle Scaler
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, 200, 200);
            }
        }

        int offsetX = 134;
        int offsetY = 126;

        public Rectangle CollBox
        {
            get
            {
                return new Rectangle((int)Position.X + offsetX, (int)Position.Y + offsetY, 156, 156);
            }
        }

        public void Update(GameTime gameTime)
        {

            float deltaTime= (float) gameTime.ElapsedGameTime.TotalSeconds;
            Position.Y -= deltaTime*Speed;

            if(Position.Y < -Texture.Height)
            {
                IsActive = false;
                System.Diagnostics.Debug.WriteLine("Exited window.");
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Scaler, Color.White);
        }
    }
}
