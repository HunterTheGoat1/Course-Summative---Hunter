using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course_Summative___Hunter
{
    internal class ReinforcedEnemy
    {
        private Rectangle _rectangle;
        private Texture2D _texture;
        private int _health;

        public ReinforcedEnemy(Rectangle rectangle, Texture2D texture, int health)
        {
            _rectangle = rectangle;
            _texture = texture;
            _health = health;
        }

        public Texture2D Texture
        {
            get { return _texture; }
        }

        public int Health
        {
            get { return _health; }
        }

        public Rectangle BoundRect
        {
            get { return _rectangle; }
            set { _rectangle = value; }
        }

        public double Move(GraphicsDeviceManager graphics, Rectangle castleRect, double castleHealth)
        {
            if (_rectangle.X > 340)
            {
                _rectangle.X--;
            }
            if (_rectangle.Y > 340)
            {
                _rectangle.Y--;
            }
            if (_rectangle.X < 340)
            {
                _rectangle.X++;
            }
            if (_rectangle.Y < 340)
            {
                _rectangle.Y++;
            }
            if (castleRect.Contains(_rectangle))
            {
                castleHealth -= 0.02;
            }

            return castleHealth;
        }

        public void MoveBack(GraphicsDeviceManager graphics, Rectangle castleRect)
        {
            if (_rectangle.X > 340)
            {
                _rectangle.X++;
            }
            if (_rectangle.Y > 340)
            {
                _rectangle.Y++;
            }
            if (_rectangle.X < 340)
            {
                _rectangle.X--;
            }
            if (_rectangle.Y < 340)
            {
                _rectangle.Y--;
            }
        }

        public void Damage(int ammount)
        {
            _health -= ammount;
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont healthFont)
        {
            spriteBatch.Draw(_texture, _rectangle, Color.White);
            spriteBatch.DrawString(healthFont, $"{_health}hp", new Vector2(_rectangle.X + 10, _rectangle.Y - 15), Color.DarkRed);
        }
    }
}
