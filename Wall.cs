using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course_Summative___Hunter
{
    internal class Wall
    {
        private Rectangle _rectangle;
        private Texture2D _texture;
        private double _health;

        public Wall(Rectangle rectangle, Texture2D texture)
        {
            _rectangle = rectangle;
            _texture = texture;
            _health = 19.99;
        }

        public Texture2D Texture
        {
            get { return _texture; }
        }

        public double Health
        {
            get { return _health; }
        }

        public Rectangle BoundRect
        {
            get { return _rectangle; }
            set { _rectangle = value; }
        }
        public void Damage(double ammount)
        {
            _health -= ammount;
        }
        public void Draw(SpriteBatch spriteBatch, SpriteFont healthFont)
        {
            spriteBatch.Draw(_texture, _rectangle, Color.White);
            spriteBatch.DrawString(healthFont, $"{Math.Round(_health, 2)}hp", new Vector2(_rectangle.X + 1, _rectangle.Y + 15), Color.White);
        }
    }
}
