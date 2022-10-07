using SFML.System;

namespace Carbine.Tiles
{
    public struct Tile
    {
        /// <summary>
        /// Creates a new tile.
        /// </summary>
        /// <param name="tileID">The ID of the tile</param>
        /// <param name="position">The position of the tile</param>
        /// <param name="flipHoriz">Should this tile be flipped horizontally?</param>
        /// <param name="flipVert">Should this tile be flipped vertically?</param>
        /// <param name="flipDiag">Should this tile be flipped diagonally?</param>
        /// <param name="animId">The animation ID of this tile</param>
        public Tile(uint tileID, Vector2f position, bool flipHoriz, bool flipVert, bool flipDiag, ushort animId)
        {
            this.ID = tileID;
            this.Position = position;
            this.FlipHorizontal = flipHoriz;
            this.FlipVertical = flipVert;
            this.FlipDiagonal = flipDiag;
            this.AnimationId = animId;
        }

        /// <summary>
        /// The constant size of the tile. Don't change this.
        /// </summary>
        public const uint SIZE = 8U;

        /// <summary>
        /// The tile's ID
        /// </summary>
        public readonly uint ID;

        /// <summary>
        /// The position of this tile
        /// </summary>
        public readonly Vector2f Position;

        /// <summary>
        /// Is this tile flipped horizontally?
        /// </summary>
        public readonly bool FlipHorizontal;

        /// <summary>
        /// Is this tile flipped vertically?
        /// </summary>
        public readonly bool FlipVertical;

        /// <summary>
        /// ...Is this tile flipped diagonally?
        /// </summary>
        public readonly bool FlipDiagonal;

        /// <summary>
        /// The animation ID of this tile
        /// </summary>
        public readonly ushort AnimationId;
    }
}
