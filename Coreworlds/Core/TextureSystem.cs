using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreworlds.Core
{
    public sealed class TextureSystem
    {
        private Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();
        private Texture2D defaultTexture;
        private Game game;

        public TextureSystem(Game game)
        {
            this.game = game;
            this.defaultTexture = new Texture2D(this.game.GraphicsDevice, 32, 32);
        }

        public void Load(string name, string contentId)
        {
            this.textures.Add(name, this.game.Content.Load<Texture2D>(contentId));
        }
        public Texture2D Get(string name)
        {
            if (!this.textures.ContainsKey(name)) return this.defaultTexture;
            return this.textures[name];
        }
    }
}
