using SFML.Graphics;
using System;

namespace Carbine.Graphics
{
    /// <summary>
    /// An interface for a texture within the Carbine Engine
    /// </summary>
    public interface ICarbineTexture : IDisposable
    {
        /// <summary>
		/// The texture
		/// </summary>
		Texture Image { get; }
    }
}
