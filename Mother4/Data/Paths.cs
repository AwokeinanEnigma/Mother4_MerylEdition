using System;
using System.IO;

namespace Mother4.Data
{
	internal static class Paths
	{
        public static readonly string RESOURCES = "Resources" + Path.DirectorySeparatorChar;

        public static readonly string SFX = Path.Combine(Paths.RESOURCES, "Audio", "SFX") + Path.DirectorySeparatorChar;

		public static readonly string BGM = Path.Combine(Paths.RESOURCES, "Audio", "BGM") + Path.DirectorySeparatorChar;

        public static readonly string AUDIO = Path.Combine(Paths.RESOURCES, "Audio", "") + Path.DirectorySeparatorChar;

        public static readonly string DATA = Path.Combine(Paths.RESOURCES, "Data", "") + Path.DirectorySeparatorChar;

        public static readonly string GRAPHICS = Path.Combine(Paths.RESOURCES, "Graphics", "") + Path.DirectorySeparatorChar;

        public static readonly string DATAENEMIES = DATA + "Enemies" + Path.DirectorySeparatorChar;

        #region  Generic SFX
        /// <summary>
        /// Corresponds to Resources/Audio/SFX/Menu/
        /// </summary>
        public static readonly string SFXMENU = SFX + "Menu" + Path.DirectorySeparatorChar;

        /// <summary>
        /// Corresponds to Resources/Audio/SFX/Text/
        /// </summary>
        public static readonly string SFXTEXT = SFX + "Text" + Path.DirectorySeparatorChar;

        /// <summary>
        /// Corresponds to Resources/Audio/SFX/Menu/
        /// </summary>
        public static readonly string SFXTILES = SFX + "Tiles" + Path.DirectorySeparatorChar;
        #endregion

        #region Battle SFX

        /// <summary>
        /// Corresponds to Resources/Audio/SFX/Battle/
        /// </summary>
        public static readonly string SFXBATTLE = SFX + "Battle" + Path.DirectorySeparatorChar;

        /// <summary>
        /// Corresponds to Resources/Audio/SFX/Battle/Combos/
        /// </summary>
        public static readonly string SFXBATTLECOMBO = SFXBATTLE + "Combos" + Path.DirectorySeparatorChar;

        /// <summary>
        /// Corresponds to Resources/Audio/SFX/Battle/PSI/
        /// </summary>
        public static readonly string SFXBATTLEPSI = SFXBATTLE + "PSI" + Path.DirectorySeparatorChar;

        /// <summary>
        /// Corresponds to Resources/Audio/SFX/Battle/Jingles/
        /// </summary>
        public static readonly string SFXBATTLEJINGLES = SFXBATTLE + "Jingles" + Path.DirectorySeparatorChar;
        #endregion

        #region  BGM

        /// <summary>
        /// Corresponds to Resources/Audio/BGM/BattleMusic/
        /// </summary>
        public static readonly string BGMBATTLE = BGM + "Battle" + Path.DirectorySeparatorChar;

        /// <summary>
        /// Corresponds to Resources/Audio/BGM/Overworld/
        /// </summary>
        public static readonly string BGMOVERWORLD = BGM + "Overworld" + Path.DirectorySeparatorChar;

        #endregion

        #region Graphics

        /// <summary>
        /// Corresponds to Resources/Graphics/PartyMembers/
        /// </summary>
        public static readonly string GRAPHICSPARTYMEMBERS = GRAPHICS + "PartyMembers" + Path.DirectorySeparatorChar;

        /// <summary>
        /// Corresponds to Resources/Graphics/Enemies/
        /// </summary>
        public static readonly string GRAPHICSENEMIES = GRAPHICS + "Enemies" + Path.DirectorySeparatorChar;

        #endregion


        public static readonly string PSI_GRAPHICS = Path.Combine(Paths.GRAPHICS, "PSI", "") + Path.DirectorySeparatorChar;

		public static readonly string MAPS = Path.Combine(Paths.RESOURCES, "Maps", "") + Path.DirectorySeparatorChar;

		public static readonly string PSI = Path.Combine(Paths.RESOURCES, "Psi", "") + Path.DirectorySeparatorChar;

		public static readonly string TEXT = Path.Combine(Paths.RESOURCES, "Text", "") + Path.DirectorySeparatorChar;

		public static readonly string BATTLE_SWIRL = Path.Combine(Paths.GRAPHICS, "swirl", "") + Path.DirectorySeparatorChar;
	}
}
