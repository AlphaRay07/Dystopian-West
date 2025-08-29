using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace DystopianWest
{
    internal class Enemy : Sprite
    {
        public bool isActive = true;
        int speed = 1300;
        int offsetX = 419;
        int offsetY = 294;
        public Rectangle CollBox
        {
            get
            {
                return new Rectangle((int)Position.X+offsetX, (int)Position.Y+offsetY, 162, 162);
            }
        }
        public Enemy(Texture2D texture, Vector2 position) : base(texture, position){}

        public new void Update(GameTime gameTime){
            float deltaTime= (float) gameTime.ElapsedGameTime.TotalSeconds;
            if (isActive) { 
                Position.Y+=speed*deltaTime;
            }
            if (Position.Y > (1080+Texture.Height))
            {
                isActive = false;
                //if (!isActive) {
                //    Log("Enemy deleted");
                //}
            }
        }
    }
}
