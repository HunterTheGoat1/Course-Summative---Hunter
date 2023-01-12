using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course_Summative___Hunter
{
    internal class Bullet
    {
        private Rectangle _rectangle;
        private Texture2D _texture;
        private int _defenderWhoShot;

        public Bullet(Rectangle rectangle, Texture2D texture)
        {
            _rectangle = rectangle;
            _texture = texture;
        }

        public Texture2D Texture
        {
            get { return _texture; }
        }
        public Rectangle BoundRect
        {
            get { return _rectangle; }
            set { _rectangle = value; }
        }

        public void Move(GraphicsDeviceManager graphics, Rectangle enemyRect)
        {
            if(_rectangle.X < (enemyRect.X + 15))
            {
                _rectangle.X += 3;
            }
            if (_rectangle.X > (enemyRect.X + 15))
            {
                _rectangle.X -= 3;
            }
            if (_rectangle.Y < (enemyRect.Y + 15))
            {
                _rectangle.Y += 3;
            }
            if (_rectangle.Y > (enemyRect.Y + 15))
            {
                _rectangle.Y -= 3;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _rectangle, Color.White);
        }
    }
}
