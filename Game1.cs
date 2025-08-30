using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks.Sources;
//using System;

namespace DystopianWest
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private bool isPaused=false;
        private bool isPressed = false; //used for pause and play
        SpriteFont font;
        Texture2D bg;
        Texture2D shooter;
        Vector2 Position = new Vector2(10, 150);

        Rectangle Rect;
        Player _player;
        Texture2D enemy;
        Enemy enemy1;
        int score = 0;


        private Texture2D bullets;
        private List<Projectile> projectiles;
        private List<Enemy> enemies;
        private int WinWidthLimit = 1800;
        private int WinHeightLimit = 1080;
        private float _timer;
        private KeyboardState prevKeyboardState;
        private KeyboardState currentKeyboardState;

        MouseState mState;
        bool isClick = false;

        Random rnd = new Random(); //for enemy spawning
        int enemyX;
        //ScaledSprite 
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.ApplyChanges();
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            //sprite = new ScaledSprite(bg, Vector2.Zero);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            bg = Content.Load<Texture2D>("SpaceShooterAssetPack_BackGrounds");
            shooter = Content.Load<Texture2D>("shooter");
            _player = new Player(shooter, Position);
            Rect = new Rectangle(0, 0, 128 * 4 * 4, 256 * 3 * 3);

            projectiles = new List<Projectile>();
            bullets = Content.Load<Texture2D>("boolet");

            enemy = Content.Load<Texture2D>("asteroid");

            enemy1 = new Enemy(enemy, new Vector2(1, -enemy.Height));
            enemies = new List<Enemy>();
            enemyX = rnd.Next(200, 1800);  //first spawn of enemy class
            _timer = 1f;

            font = Content.Load<SpriteFont>("galleryFont");
        }
        

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            _player.Update(gameTime);
            base.Update(gameTime);

            currentKeyboardState = Keyboard.GetState();
            if (_player.isAlive == false)
            {
                if (currentKeyboardState.IsKeyDown(Keys.Space))
                {
                    _player.isAlive = true;
                }
                return;
            }

            if (currentKeyboardState.IsKeyDown(Keys.P) && prevKeyboardState.IsKeyUp(Keys.P))
            {
                isPaused = !isPaused;
            }

            if (isPaused)
            {
                prevKeyboardState = currentKeyboardState;
                return;
            }
            prevKeyboardState = currentKeyboardState;

            foreach (Projectile p in projectiles) { 
                p.Update(gameTime);
            }

            mState = Mouse.GetState();
            if (mState.LeftButton == ButtonState.Pressed && isClick == false)
            {
                Fire();
                isClick = true;
            }

            if (mState.LeftButton == ButtonState.Released)
            {
                isClick = false;
            }

            //enemy collision and updates
            _timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_timer <= 0.5)
            {
                EnemySpawner();
                if (enemies.Count>0)
                {
                    Log("Spawned");
                }

                _timer = 1f;
            }

            foreach (var e in enemies) { 
                e.Update(gameTime);
                if (_player.CollBox.Intersects(e.CollBox))
                {
                    Log("Collision!");
                }
                foreach (Projectile p in projectiles)
                {
                    if (p.CollBox.Intersects(e.CollBox)) {
                        p.IsActive = false;
                        e.isActive = false;
                        score++;
                        break;
                    }
                }
                if (_player.CollBox.Intersects(e.CollBox))
                {
                    _player.isAlive = false;
                    e.isActive = false;
                }
            }


            enemies.RemoveAll(e => !e.isActive);
            projectiles.RemoveAll(p => !p.IsActive);

            //projectiles collision and updates
            

        }

        protected override void Draw(GameTime gameTime)
        {
            #region FIX: game over screen
            if (_player.isAlive == false)
            {
                GraphicsDevice.Clear(Color.Black);

                _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
                Log("Ded");
                _spriteBatch.DrawString(font, "Game over :(", new Vector2(150, 150), Color.White);
                _spriteBatch.End();
            }
            #endregion
            else if (isPaused)
            {
                GraphicsDevice.Clear(Color.Black);

                _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
                Log("Paused");
                _spriteBatch.DrawString(font, "PAUSED!", new Vector2(1920/2, 1080/2), Color.White);
                _spriteBatch.End();
            }
            else{
                GraphicsDevice.Clear(Color.CornflowerBlue);

                _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
                _spriteBatch.Draw(bg, Rect, Color.White);
                _player.Draw(_spriteBatch);

                foreach (Projectile p in projectiles) {
                    p.Draw(_spriteBatch);
                }

                foreach (var e in enemies)
                {
                    e.Draw(_spriteBatch);
                }

                _spriteBatch.DrawString(font, score.ToString(), new Vector2(5, 15), Color.White);

                _spriteBatch.End();
                base.Draw(gameTime);
            }
        }

        private void Fire()
        {
            if (projectiles.Count > 10)
            {
                return;
            }

            Projectile newBullet = new Projectile(bullets, _player.Position,shooter);
            projectiles.Add(newBullet);
            Log("Added bullet");
        }

        protected void EnemySpawner()
        {
            if (enemies.Count > 10)
            {
                return;
            }
            enemies.Add(new Enemy(enemy, new Vector2(enemyX, -enemy.Height))); 
            enemyX= rnd.Next(200,1800);
        }


        protected void Log(string msg)
        {
            System.Diagnostics.Debug.WriteLine(msg);
        }
    }
}

