using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using Carbine.Collision;
using Carbine.Tiles;

namespace Carbine.Maps
{
    public class Map
    {
        public Map()
        {
            this.Head = default(Map.Header);
            this.Music = new List<Map.BGM>();
            this.SoundEffects = new List<Map.SFX>();
            this.Portals = new List<Map.Portal>();
            this.Triggers = new List<Map.Trigger>();
            this.NPCs = new List<Map.NPC>();
            this.Paths = new List<Map.Path>();
            this.Areas = new List<Map.Area>();
            this.Crowds = new List<Map.Crowd>();
            this.Spawns = new List<Map.EnemySpawn>();
            this.Mesh = new List<Mesh>();
            this.Groups = new List<Map.Group>();
            this.TileAnimationProperties = new Dictionary<int, Map.TileAnimation>();
            this.Parallaxes = new List<Map.Parallax>();
        }

        public IList<TileGroup> MakeTileGroups(string graphicDirectory, uint palette)
        {
            string arg = "default";
            if (this.Head.Tilesets.Count > 0)
            {
                arg = this.Head.Tilesets[0].Name;
            }

            string resource = string.Format("{0}{1}.dat", graphicDirectory, arg);
            IList<TileGroup> list = new List<TileGroup>(this.Groups.Count);
            long ticks = DateTime.Now.Ticks;
            for (int i = 0; i < this.Groups.Count; i++)
            {
                Map.Group group = this.Groups[i];
                IList<Tile> list2 = new List<Tile>(group.Tiles.Length / 2);
                int j = 0;
                int num = 0;
                while (j < group.Tiles.Length)
                {
                    int num2 = group.Tiles[j] - 1;
                    if (num2 >= 0)
                    {
                        ushort num3;
                        if (j + 1 < group.Tiles.Length)
                        {
                            num3 = group.Tiles[j + 1];
                        }
                        else
                        {
                            num3 = 0;
                        }
                        int num4 = group.Width * 8;
                        Vector2f position = new Vector2f(num * 8L % num4, num * 8L / num4 * 8L);
                        bool flipHoriz = (num3 & 1) > 0;
                        bool flipVert = (num3 & 2) > 0;
                        bool flipDiag = (num3 & 4) > 0;
                        ushort animId = (ushort)(num3 >> 3);
                        Tile item = new Tile((uint)num2, position, flipHoriz, flipVert, flipDiag, animId);
                        list2.Add(item);
                    }
                    j += 2;
                    num++;
                }
                TileGroup item2 = new TileGroup(list2, resource, group.Depth, new Vector2f(group.X, group.Y), palette);
                list.Add(item2);
            }
            Console.WriteLine("Created tile groups in {0}ms", (DateTime.Now.Ticks - ticks) / 10000L);
            return list;
        }

        public Map.Header Head;

        public IList<Map.BGM> Music;

        public IList<Map.SFX> SoundEffects;

        public IList<Map.Portal> Portals;

        public IList<Map.Trigger> Triggers;

        public IList<Map.NPC> NPCs;

        public IList<Map.Path> Paths;

        public IList<Map.Area> Areas;

        public IList<Map.Crowd> Crowds;

        public IList<Map.EnemySpawn> Spawns;

        public IList<Mesh> Mesh;

        public IList<Map.Group> Groups;

        public IList<Map.Parallax> Parallaxes;

        public IDictionary<int, Map.TileAnimation> TileAnimationProperties;

        public struct Header
        {
            public Color PrimaryColor;

            public Color SecondaryColor;

            public string Title;

            public string Subtitle;

            public int Width;

            public int Height;

            public List<Map.Tileset> Tilesets;

            public string Script;

            public string BBG;

            public bool Shadows;

            public bool Ocean;
        }

        public struct Tileset
        {
            public string Name;

            public int FirstId;
        }

        public struct BGM
        {
            public int X;

            public int Y;

            public int Width;

            public int Height;

            public string Name;

            public short Flag;

            public bool Loop;
        }

        public struct SFX
        {
            public int X;

            public int Y;

            public int Width;

            public int Height;

            public string Name;

            public short Flag;

            public short Interval;

            public bool Loop;
        }

        public struct Portal
        {
            public int X;

            public int Y;

            public int Width;

            public int Height;

            public int Xto;

            public int Yto;

            public int DirectionTo;

            public string Map;

            public int Flag;

            public int SFX;
        }

        public struct Trigger
        {
            public Vector2f Position;

            public List<Vector2f> Points;

            public string Script;

            public int Flag;
        }

        public struct NPCtext
        {
            public string ID;

            public int Flag;
        }

        public struct NPC
        {
            public int X;

            public int Y;

            public int Width;

            public int Height;

            public string Name;

            public string Sprite;

            public short Direction;

            public short Mode;

            public float Speed;

            public short Delay;

            public short Distance;

            public string Constraint;

            public short Flag;

            public bool Shadow;

            public bool Solid;

            public bool Sticky;

            public int DepthOverride;

            public bool Enabled;

            public List<Map.NPCtext> Text;

            public List<Map.NPCtext> TeleText;
        }

        public struct Path
        {
            public string Name;

            public List<Vector2f> Points;
        }

        public struct Area
        {
            public string Name;

            public IntRect Rectangle;
        }

        public struct Crowd
        {
            public bool Loop;

            public List<int> Sprites;

            public List<Vector2f> Points;
        }

        public struct Enemy
        {
            public string EnemyName;

            public int Chance;
        }

        public struct EnemySpawn
        {
            public int X;

            public int Y;

            public int Width;

            public int Height;

            public List<Map.Enemy> Enemies;
        }

        public struct TileModifier
        {
            public bool FlipHorizontal;

            public bool FlipVertical;

            public bool FlipDiagonal;
        }

        public struct Group
        {
            public ushort[] Tiles;

            public int Depth;

            public int X;

            public int Y;

            public int Width;

            public int Height;

            public bool Rainaway;
        }

        public struct TileAnimation
        {
            public int Id;

            public int Frames;

            public int SkipVert;

            public int SkipHoriz;

            public float Speed;
        }

        public struct Parallax
        {
            public string Sprite;

            public Vector2f Vector;

            public IntRect Area;

            public int Depth;
        }
    }
}
