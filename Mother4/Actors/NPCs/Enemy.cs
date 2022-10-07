using System;
using System.Collections.Generic;
using System.Diagnostics;
using Carbine.Actors;
using Carbine.Collision;
using Carbine.Graphics;
using Carbine.Utility;
using Mother4.Actors.Animation;
using Mother4.Actors.NPCs.Movement;
using Mother4.Data;
using Mother4.Data.Enemies;
using Mother4.Overworld;
using SFML.Graphics;
using SFML.System;
using Mother4.Scenes;

namespace Mother4.Actors.NPCs
{
	// Token: 0x02000052 RID: 82
	internal class EnemyNPC : SolidActor
	{
		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000207 RID: 519 RVA: 0x0000C675 File Offset: 0x0000A875
		public EnemyData Type
		{
			get
			{
				return this.enemyType;
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000208 RID: 520 RVA: 0x0000C67D File Offset: 0x0000A87D
		public Graphic Graphic
		{
			get
			{
				return this.npcGraphic;
			}
		}

		// Token: 0x06000209 RID: 521 RVA: 0x0000C688 File Offset: 0x0000A888x
		public EnemyNPC(RenderPipeline pipeline, CollisionManager colman, EnemyData enemyType, Vector2f position, FloatRect spawnArea) : base(colman)
		{
			//Console.WriteLine("enemy");

			this.pipeline = pipeline;
			this.position = position;
			this.enemyType = enemyType;
            this.mover = new LookForTroubleMover(100, 2.5f);//LookForTroubleMover(100, 2);//new ZigZagMover(colman, this, new FloatRect(new Vector2f(10, 10), new Vector2f(10, 10)), 10, 2,
													   //    100); //new MushroomMover(this, 100f, 2f);
			this.npcGraphic = new IndexedColorGraphic(Paths.GRAPHICS + enemyType.OverworldSprite + ".dat"/*"mushroom.dat"*/, "walk south", this.Position, (int)this.Position.Y);
			this.pipeline.Add(this.npcGraphic);
			this.hasDirection = new bool[8];
			this.hasDirection[0] = (this.npcGraphic.GetSpriteDefinition("walk east") != null);
			this.hasDirection[1] = (this.npcGraphic.GetSpriteDefinition("walk northeast") != null);
			this.hasDirection[2] = (this.npcGraphic.GetSpriteDefinition("walk north") != null);
			this.hasDirection[3] = (this.npcGraphic.GetSpriteDefinition("walk northwest") != null);
			this.hasDirection[4] = (this.npcGraphic.GetSpriteDefinition("walk west") != null);
			this.hasDirection[5] = (this.npcGraphic.GetSpriteDefinition("walk southwest") != null);
			this.hasDirection[6] = (this.npcGraphic.GetSpriteDefinition("walk south") != null);
			this.hasDirection[7] = (this.npcGraphic.GetSpriteDefinition("walk southeast") != null);
			this.shadowGraphic = new IndexedColorGraphic(Paths.GRAPHICS + "shadow.dat", ShadowSize.GetSubsprite(this.npcGraphic.Size), this.Position, (int)(this.Position.Y - 1f));
			this.pipeline.Add(this.shadowGraphic);
			int width = this.npcGraphic.TextureRect.Width;
			int height = this.npcGraphic.TextureRect.Height;
			this.mesh = new Mesh(new FloatRect((float)(-(float)(width / 2)), -3f, (float)width, 6f));
			this.aabb = this.mesh.AABB;
			this.animator = new AnimationControl(this.npcGraphic, this.direction);
			this.animator.UpdateSubsprite(this.GetAnimationContext());
            results = new ICollidable[1];
		}

        public void ChasePlayer()
		{
			mover = new GangUpOnPlayer(5);
        }

        private AnimationContext GetAnimationContext()
		{
			return new AnimationContext
			{
				Velocity = this.velocity,
				SuggestedDirection = this.direction,
				TerrainType = TerrainType.None,
				IsDead = false,
				IsCrouch = false,
				IsNauseous = false,
				IsTalk = false
			};
		}

		public void OverrideSubsprite(string subsprite)
		{
			this.animator.OverrideSubsprite(subsprite);
		}

		public void ClearOverrideSubsprite()
		{
			this.animator.ClearOverride();
		}

		public void FreezeSpriteForever()
		{
			this.npcGraphic.SpeedModifier = 0f;
		}

        public bool hasEnteredBattle = false;
        protected override void HandleCollision(ICollidable[] collisionObjects)
        {
            base.HandleCollision(collisionObjects);
            if (!Snipped)
            {
                if (OverworldScene.instance != null && collisionObjects.Length > 0)
                {
                    if (collisionObjects[0] is Player && hasEnteredBattle != true)
                    {
						//Console.WriteLine($"starting battle");
                        OverworldScene.instance.StartBattle(this);
                        hasEnteredBattle = true;

                    }
                }

            }
            /*if (collisionObjects[0] is EnemyNPC)
            {
                if (!collisionManager.PlaceFree(this, position))
                {
                    Console.WriteLine($"the phantom exterior like fish eggs interior like suicide wrist-red. I could exercise you, this could be your phys-ed. Cheat on your man homie AAGH I tried to sneak through the door man! Can't make it. Can't make it. Shit's stuck. Outta my way son! DOOR STUCK! DOOR STUCK! PLEASE! I BEG YOU! We're dead. You're a genuine dick sucker.");
                    this.position += new Vector2f(direction, direction);
                }
            }*/
		}

        public delegate void OnDestroyed(EnemyNPC npc);
        public event OnDestroyed onDestroy;
		
        public void Destroy()
		{
			onDestroy?.Invoke(this);
			Dispose(true);
        }

		public bool Snipped;

        public void Snip()
        {
            Snipped = true;
        }


		/*HandleCollision(CollisionContext context)
        {
            base.Collision(context);
            Console.WriteLine("collider");
            if (OverworldScene.instance != null)
            {
                if (context.Other is Player)
                {
                    OverworldScene.instance.StartBattle(new EnemyNPC[1]{this});
                }
            }
        }*/
        private static Type[] enemyofType =
            new Type[]
            {
                typeof(EnemyNPC)
            };


		public override void Update()
		{
			this.lastVelocity = this.velocity;
			if (!this.isMovementLocked)
			{
				this.changed = this.mover.GetNextMove(ref this.position, ref this.velocity, ref this.direction);
			}
			if (this.changed)
			{
				//Console.WriteLine("active");
				this.animator.UpdateSubsprite(this.GetAnimationContext());
				this.npcGraphic.Position = VectorMath.Truncate(this.position);
				this.npcGraphic.Depth = (int)this.position.Y;
				this.pipeline.Update(this.npcGraphic);
				this.shadowGraphic.Position = VectorMath.Truncate(this.position);
				this.shadowGraphic.Depth = (int)this.position.Y - 1;
				this.pipeline.Update(this.shadowGraphic);
				Vector2f v = new Vector2f(this.velocity.X, 0f);
				Vector2f v2 = new Vector2f(0f, this.velocity.Y);
				this.lastPosition = this.position;
				if (this.collisionManager.PlaceFree(this, this.position + v, results, enemyofType))
				{
					this.position += v;
				}
                else
                {
                    HandleCollision(results);
                }
                if (this.collisionManager.PlaceFree(this, this.position + v2, results, enemyofType))
				{
					this.position += v2;
				}
                else
                {
                    HandleCollision(results);
                }

				this.collisionManager.Update(this, this.lastPosition, this.position);
				this.changed = false;
                
                /*bool flag = collisionManager == null ||
                            collisionManager.PlaceFree(this, moveTemp, results, ignoreCollisionTypes);

                if (!flag)
                {
                    velocity.Y = 0f;
                    HandleCollision(results);
                }*/
			}
		}

        public ICollidable[] results;
        protected override void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				try
				{
					if (disposing)
					{
						this.pipeline.Remove(this.npcGraphic);
						this.pipeline.Remove(this.shadowGraphic);
						this.npcGraphic.Dispose();
                        this.shadowGraphic.Dispose();
					}
					this.disposed = true;
				}
				finally
				{
					base.Dispose(disposing);
				}
			}
		}

		// Token: 0x040002DE RID: 734
		private static readonly Vector2f HALO_OFFSET = new Vector2f(0f, -32f);

		// Token: 0x040002DF RID: 735
		private RenderPipeline pipeline;

		// Token: 0x040002E0 RID: 736
		private IndexedColorGraphic npcGraphic;

		// Token: 0x040002E1 RID: 737
		private IndexedColorGraphic haloGraphic;

		// Token: 0x040002E2 RID: 738
		private Graphic shadowGraphic;

		// Token: 0x040002E3 RID: 739
		private Mover mover;

		// Token: 0x040002E4 RID: 740
		private bool[] hasDirection;

		// Token: 0x040002E5 RID: 741
		private Vector2f lastVelocity;

		// Token: 0x040002E6 RID: 742
		private int direction;

		// Token: 0x040002E7 RID: 743
		private bool changed;

		// Token: 0x040002E8 RID: 744
		private EnemyData enemyType;

		// Token: 0x040002E9 RID: 745
		private AnimationControl animator;
	}
}
