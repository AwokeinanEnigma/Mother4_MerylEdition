using SFML.Graphics;
using System;

namespace Carbine.Graphics
{
    /// <summary>
    /// A texture that has a full range of colors
    /// </summary>
    public class FullColorTexture : ICarbineTexture, IDisposable
    {
        /// <summary>
        /// The image assosicated with the texture
        /// </summary>
        public Texture Image
        {
            get
            {
                return this.imageTex;
            }
        }

        /// <summary>
        /// Creates a new texture from an image
        /// </summary>
        /// <param name="image">The image to create the texture image from</param>
		public FullColorTexture(Image image)
        {
            this.imageTex = new Texture(image);
        }


        /// <summary>
        /// Creates a new texture from a texture
        /// </summary>
        /// <param name="image">The texture to create the texture image from.</param>
		public FullColorTexture(Texture tex)
        {
            this.imageTex = new Texture(tex);
        }

        ~FullColorTexture()
        {
            this.Dispose(false);
        }

        public virtual void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed && disposing)
            {
                this.imageTex.Dispose();
            }
            this.disposed = true;
        }

        private Texture imageTex;

        private bool disposed;
    }
}
