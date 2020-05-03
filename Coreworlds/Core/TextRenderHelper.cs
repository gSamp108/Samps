using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreworlds.Core
{
    public sealed class TextRenderHelper
    {
        private SpriteBatch spriteBatch;
        private Vector2 defaultRenderPosition;
        private Vector2 renderPosition;
        private SpriteFont font;
        private int lineSpace = 12;

        public TextRenderHelper(SpriteBatch spriteBatch,SpriteFont font, Vector2 renderStartPosition)
        {
            this.spriteBatch = spriteBatch;
            this.font = font;
            this.defaultRenderPosition = renderStartPosition;
            this.Reset();
        }

        public void Reset()
        {
            this.renderPosition = this.defaultRenderPosition;
        }

        public void RenderText(string text)
        {
            this.spriteBatch.DrawString(this.font, text, this.renderPosition, Color.White);
            this.renderPosition.Y += this.lineSpace;
        }


    }
}
