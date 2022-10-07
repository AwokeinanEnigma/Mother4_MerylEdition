using System;
using System.Collections.Generic;
using Carbine;
using Carbine.Collision;
using Carbine.Graphics;
using Carbine.Maps;
using Mother4.Actors.NPCs;
using Mother4.Data;
using Mother4.Data.Enemies;
using SFML.Graphics;
using SFML.System;

namespace Mother4.Overworld
{
	internal class EnemySpawner
	{
		public FloatRect Bounds
		{
			get
			{
				return this.rectangle;
			}
		}

		public bool SpawnFlag
		{
			get
			{
				return this.spawnFlag;
			}
			set
			{
				this.spawnFlag = value;
			}
		}

		public EnemySpawner(FloatRect rectangle, List<Map.Enemy> enemies)
		{
			this.rectangle = rectangle;
			this.chances = enemies;
			this.spawnFlag = true;
			this.spawnedOnce = false;
		}

		public List<EnemyNPC> GenerateEnemies(RenderPipeline pipeline, CollisionManager collision)
		{
			List<EnemyNPC> list = list = new List<EnemyNPC>();

			if (this.spawnFlag && !this.spawnedOnce)
			{
				foreach (Map.Enemy enemy in this.chances)
				{

                    Vector2f position = new Vector2f(this.rectangle.Left + (float)Engine.Random.Next((int)this.rectangle.Width), this.rectangle.Top + (float)Engine.Random.Next((int)this.rectangle.Height));
                    EnemyNPC item = new EnemyNPC(pipeline, collision, EnemyFile.Instance.GetEnemyData($"{enemy.EnemyName}"), position, this.rectangle);
                    list.Add(item);

				}
				this.spawnFlag = false;
			}
			return list;
		}

		private const int MAX_CHANCE = 100;

		private FloatRect rectangle;

		private List<Map.Enemy> chances;

		private bool spawnFlag;

		private bool spawnedOnce;
	}
}
