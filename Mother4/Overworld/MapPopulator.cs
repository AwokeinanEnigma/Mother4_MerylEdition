using System;
using System.Collections.Generic;
using System.IO;
using Carbine.Collision;
using Carbine.Flags;
using Carbine.Graphics;
using Carbine.Maps;
using Mother4.Actors.NPCs;
using Mother4.Battle.Background;
using Mother4.Data;
using SFML.Graphics;

namespace Mother4.Overworld
{
	internal class MapPopulator
	{
		public static List<NPC> GenerateNPCs(RenderPipeline pipeline, CollisionManager collisions, Map map)
		{
			List<NPC> list = new List<NPC>();
			foreach (Map.NPC npcData in map.NPCs)
			{
				if (npcData.Enabled == FlagManager.Instance[(int)npcData.Flag])
				{
					object obj = null;
					if (npcData.Constraint != null && npcData.Constraint.Length > 0)
					{
						foreach (Map.Path path in map.Paths)
						{
							if (path.Name == npcData.Constraint)
							{
								obj = path;
								break;
							}
						}
						if (obj == null)
						{
							foreach (Map.Area area in map.Areas)
							{
								if (area.Name == npcData.Constraint)
								{
									obj = area;
									break;
								}
							}
						}
					}
					NPC item = new NPC(pipeline, collisions, npcData, obj);
					list.Add(item);
				}
			}
			return list;
		}

		public static IList<ICollidable> GeneratePortals(Map map)
		{
			IList<ICollidable> list = new List<ICollidable>();
			foreach (Map.Portal portal in map.Portals)
			{
				Console.WriteLine("Made a door: {0},{1} {2}x{3} {4} {5},{6}", new object[]
				{
					portal.X,
					portal.Y,
					portal.Width,
					portal.Height,
					portal.Map,
					portal.Xto,
					portal.Yto
				});
				Portal item = new Portal(portal.X, portal.Y, portal.Width, portal.Height, portal.Xto, portal.Yto, portal.DirectionTo, portal.Map);
				list.Add(item);
			}
			return list;
		}

		public static List<EnemySpawner> GenerateSpawners(Map map)
		{
			List<EnemySpawner> list = new List<EnemySpawner>();
			foreach (Map.EnemySpawn enemySpawn in map.Spawns)
			{
				//Console.WriteLine("Spawning enemy");
				EnemySpawner item = new EnemySpawner(new FloatRect((float)enemySpawn.X, (float)enemySpawn.Y, (float)enemySpawn.Width, (float)enemySpawn.Height), enemySpawn.Enemies);
				list.Add(item);
			}
			return list;
		}

		public static List<ParallaxBackground> GenerateParallax(Map map)
		{
			List<ParallaxBackground> list = new List<ParallaxBackground>();
			foreach (Map.Parallax parallax in map.Parallaxes)
			{
				string sprite = Paths.GRAPHICS + parallax.Sprite + ".dat";
				ParallaxBackground item = new ParallaxBackground(sprite, parallax.Vector, parallax.Area, parallax.Depth);
				list.Add(item);
			}
			return list;
		}

		public static BattleBackgroundRenderable GenerateBBGOverlay(Map map)
		{
			BattleBackgroundRenderable result = null;
			if (!string.IsNullOrWhiteSpace(map.Head.BBG))
			{
				Console.WriteLine(map.Head.BBG);
				string text = Paths.GRAPHICS + "BBG/xml/" + map.Head.BBG + ".xml";
				if (File.Exists(text))
				{
					Console.WriteLine("Using BBG file \"{0}\"", text);
					int depth = (map.Head.BBG == "SolongSnow") ? 32767 : 0;
					result = new BattleBackgroundRenderable(text, depth);
				}
				else
				{
					Console.WriteLine("BBG file \"{0}\" does not exist", text);
				}
			}
			else
			{
				Console.WriteLine("This map does not use a BBG");
			}
			return result;
		}

		public static IList<ICollidable> GenerateTriggerAreas(Map map)
		{
			IList<ICollidable> list = new List<ICollidable>();
			foreach (Map.Trigger trigger in map.Triggers)
			{
				list.Add(new TriggerArea(trigger.Position, trigger.Points, trigger.Flag, trigger.Script));
			}
			return list;
		}
	}
}
