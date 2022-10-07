using SFML.System;

namespace Carbine.Graphics
{
    /// <summary>
    /// A class containing 
    /// </summary>
    public class SpriteDefinition
    {
        /// <summary>
        /// The name of the sprite
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// The coords of the sprite
        /// </summary>
        public Vector2i Coords { get; private set; }
        /// <summary>
        /// The bounds of the sprite
        /// </summary>
        public Vector2i Bounds { get; private set; }
        /// <summary>
        /// The origin of the sprite
        /// </summary>
        public Vector2f Origin { get; private set; }
        /// <summary>
        /// How many frames this sprite definition has
        /// </summary>
        public int Frames { get; private set; }
        /// <summary>
        /// The speeds of the animations within the sprite definitions
        /// </summary>
        public float[] Speeds { get; private set; }
        /// <summary>
        /// Should we flip this sprite horizontally?
        /// </summary>
        public bool FlipX { get; private set; }
        /// <summary>
        /// Should we flip this sprite vertically
        /// </summary>
        public bool FlipY { get; private set; }
        /// <summary>
        /// What mode this sprite definition is in
        /// </summary>
        public int Mode { get; private set; }
        /// <summary>
        /// Additional data
        /// </summary>
        public int[] Data { get; private set; }
        /// <summary>
        /// Creates a new sprite definition
        /// </summary>
        /// <param name="name"></param>
        /// <param name="coords"></param>
        /// <param name="bounds"></param>
        /// <param name="origin"></param>
        /// <param name="frames"></param>
        /// <param name="speeds"></param>
        /// <param name="flipX"></param>
        /// <param name="flipY"></param>
        /// <param name="mode"></param>
        /// <param name="data"></param>
        public SpriteDefinition(string name, Vector2i coords, Vector2i bounds, Vector2f origin, int frames, float[] speeds, bool flipX, bool flipY, int mode, int[] data)
        {
            this.Name = name;
            this.Coords = coords;
            this.Bounds = bounds;
            this.Origin = origin;
            this.Frames = frames;
            this.Speeds = speeds;
            this.FlipX = flipX;
            this.FlipY = flipY;
            this.Mode = mode;
            this.Data = data;
        }
    }
}
