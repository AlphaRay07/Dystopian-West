using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Microsoft.Xna.Framework.Input;



namespace DystopianWest
{
    internal class Player : Sprite
    {
        public bool isAlive = true;
        public float speed = 700f;
        public Player(Texture2D texture, Vector2 position) : base(texture, position)
        {
            this.Texture=texture;
            this.Position = position;
        }

        int offsetX = 397;
        int offsetY = 252;

        public Rectangle CollBox
        {
            get
            {
                return new Rectangle((int)Position.X + offsetX, (int)Position.Y + offsetY, 184, 190);
            }
        }

        public new void Update(GameTime gameTime){
            KeyboardState kState= Keyboard.GetState();
            float deltaTime= (float) gameTime.ElapsedGameTime.TotalSeconds;
            if (kState.IsKeyDown(Keys.A))
                Position.X -= deltaTime*speed;
            if (kState.IsKeyDown(Keys.D))
                Position.X += deltaTime * speed;
            if (kState.IsKeyDown(Keys.W))
                Position.Y -= deltaTime * speed;
            if (kState.IsKeyDown(Keys.S))
                Position.Y += deltaTime * speed;
        }
    }
}
