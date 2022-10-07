using System;
using Carbine;
using Carbine.Actors;
using Carbine.Collision;
using Carbine.Graphics;
using Carbine.Input;
using Carbine.Utility;
using Mother4.Actors.Animation;
using Mother4.Actors.NPCs;
using Mother4.Actors.NPCs.Movement;
using Mother4.Battle;
using Mother4.Data;
using Mother4.Overworld;
using SFML.Graphics;
using SFML.System;

namespace Mother4.Actors
{
	internal class Player : SolidActor
	{
		public Vector2f CheckVector
		{
			get
			{
				return this.checkVector;
			}
		}

		public int Direction
		{
			get
			{
				return this.direction;
			}
		}

		public bool Running
		{
			get
			{
				return this.isRunning;
			}
		}

		public float HopFactor
		{
			get
			{
				return this.hopFactor;
			}
			set
			{
				this.hopFactor = value;
				this.hopFrame = Engine.Frame;
			}
		}

		public int Depth
		{
			get
			{
				return this.playerGraphic.Depth;
			}
		}

		public Vector2f EmoticonPoint
		{
			get
			{
				return new Vector2f(this.position.X, this.position.Y - this.playerGraphic.Origin.Y);
			}
		}

		public bool InputLocked
		{
			get
			{
				return this.isInputLocked;
			}
			set
			{
				this.isInputLocked = value;
			}
		}

		public override bool MovementLocked
		{
			get
			{
				return this.isMovementLocked;
			}
			set
			{
				this.isMovementLocked = value;
				this.playerGraphic.SpeedModifier = (float)(this.isMovementLocked ? 0 : 1);
				this.recorder.MovementLocked = this.isMovementLocked;
			}
		}

		public event Player.OnCollisionHanlder OnCollision;

		public event Player.OnTelepathyAnimationCompleteHanlder OnTelepathyAnimationComplete;

		public event Player.OnRunningChangeHandler OnRunningChange;

		public Player(RenderPipeline pipeline, CollisionManager colman, PartyTrain recorder, Vector2f position, int direction, CharacterType character, bool useShadow, bool isOcean, bool isRunning) : base(colman)
		{
			this.position = position;
			this.moveVector = new Vector2f(0f, 0f);
			this.pipeline = pipeline;
			this.mesh = new Mesh(new FloatRect(-8f, -3f, 15f, 6f));
			this.aabb = base.Mesh.AABB;
			this.ignoreCollisionTypes = new Type[]
			{
				typeof(PartyFollower)
			};
			this.checkVector = new Vector2f(0f, 10f);
			this.direction = direction;
			this.character = character;
			this.terrainType = (isOcean ? TerrainType.Ocean : TerrainType.Tile);
			this.recorder = recorder;
			this.recorder.Reset(this.position, this.direction, this.terrainType);
			this.speed = 0f;
			this.isRunning = isRunning;
			this.runVector = VectorMath.DirectionToVector(this.direction);
			this.UpdateStatusEffects();
			string file = CharacterGraphics.GetFile(character);
			this.ChangeSprite(file, "walk south");
			this.isShadowEnabled = (useShadow && !isOcean);
			this.shadowGraphic = new IndexedColorGraphic(Paths.GRAPHICS + "shadow.dat", ShadowSize.GetSubsprite(this.playerGraphic.Size), position, (int)position.Y - 1);
			this.shadowGraphic.Visible = this.isShadowEnabled;
			pipeline.Add(this.shadowGraphic);
			InputManager.Instance.ButtonPressed += this.ButtonPressed;
			InputManager.Instance.ButtonReleased += this.ButtonReleased;
			TimerManager.Instance.OnTimerEnd += this.CrouchTimerEnd;
		}

		public void UpdateStatusEffects()
		{
			this.isDead = (CharacterStatusEffects.HasStatusEffect(this.character, StatusEffect.Unconscious) || CharacterStats.GetStats(this.character).HP <= 0);
			this.isNauseous = CharacterStatusEffects.HasStatusEffect(this.character, StatusEffect.Nausea);
		}

		public void Telepathize()
		{
			this.effectGraphic = new IndexedColorGraphic(Paths.GRAPHICS + "telepathy.dat", "telepathy", VectorMath.Truncate(this.position - new Vector2f(0f, this.playerGraphic.Origin.Y - this.playerGraphic.Size.Y / 4f)), 2147450881);
			this.effectGraphic.OnAnimationComplete += this.effectGraphic_OnAnimationComplete;
			this.pipeline.Add(this.effectGraphic);
		}

		private void effectGraphic_OnAnimationComplete(AnimatedRenderable graphic)
		{
			this.effectGraphic.OnAnimationComplete -= this.effectGraphic_OnAnimationComplete;
			this.pipeline.Remove(this.effectGraphic);
			this.effectGraphic.Dispose();
			this.effectGraphic = null;
			if (this.OnTelepathyAnimationComplete != null)
			{
				this.OnTelepathyAnimationComplete(this);
			}
		}

		private void ButtonPressed(InputManager sender, Button b)
		{
			if (!this.isMovementLocked && !this.isInputLocked && b == Button.B)
			{
				this.isRunButtonPressed = true;
			}
		}

		private void ButtonReleased(InputManager sender, Button b)
		{
			if (!this.isMovementLocked && !this.isInputLocked && b == Button.B)
			{
				this.isRunButtonReleased = true;
			}
		}

		private void CrouchTimerEnd(int timerIndex)
		{
			if (this.crouchTimerIndex == timerIndex)
			{
				this.isRunTimerComplete = true;
			}
		}

		public void SetPosition(Vector2f position)
		{
			this.SetPosition(position, false);
		}

		public void SetPosition(Vector2f position, bool extend)
		{
			this.position = position;
			this.lastPosition = position;
			this.recorder.Reset(this.position, this.direction, this.terrainType, extend);
			this.UpdateGraphics();
		}

		public void SetDirection(int dir)
		{
			while (dir < 0)
			{
				dir += 8;
			}
			this.direction = dir % 8;
			this.animator.UpdateSubsprite(this.GetAnimationContext());
		}

		public void SetShadow(bool isVisible)
		{
			this.isShadowEnabled = isVisible;
			this.shadowGraphic.Visible = this.isShadowEnabled;
		}

		public void SetMover(Mover mover)
		{
			this.mover = mover;
		}

		public void ClearMover()
		{
			this.mover = null;
		}

		public void OverrideSubsprite(string subsprite)
		{
			this.animator.OverrideSubsprite(subsprite);
			this.animator.UpdateSubsprite(this.GetAnimationContext());
		}

		public void ClearOverrideSubsprite()
		{
			this.animator.ClearOverride();
			this.animator.UpdateSubsprite(this.GetAnimationContext());
			if (this.animationLoopCountTarget > 0)
			{
				this.animationLoopCount = 0;
				this.animationLoopCountTarget = 0;
				this.playerGraphic.OnAnimationComplete -= this.playerGraphic_OnAnimationComplete;
			}
		}

		public void SetAnimationLoopCount(int loopCount)
		{
			if (this.animator.Overriden)
			{
				this.animationLoopCount = 0;
				this.animationLoopCountTarget = Math.Max(1, loopCount);
				this.playerGraphic.OnAnimationComplete += this.playerGraphic_OnAnimationComplete;
			}
		}

		private void playerGraphic_OnAnimationComplete(AnimatedRenderable renderable)
		{
			this.animationLoopCount++;
			if (this.animationLoopCount >= this.animationLoopCountTarget)
			{
				this.playerGraphic.SpeedModifier = 0f;
				this.playerGraphic.OnAnimationComplete -= this.playerGraphic_OnAnimationComplete;
			}
		}

		private void SetRunning(bool isRunning)
		{
			this.isRunning = isRunning;
			if (this.OnRunningChange != null)
			{
				this.OnRunningChange(this);
			}
		}

		private AnimationContext GetAnimationContext()
		{
			return new AnimationContext
			{
				Velocity = this.velocity * this.speed,
				SuggestedDirection = this.direction,
				TerrainType = this.terrainType,
				IsDead = this.isDead,
				IsCrouch = this.isCrouch,
				IsNauseous = this.isNauseous,
				IsTalk = false
			};
		}

		public void ChangeSprite(string resource, string subsprite)
		{
			if (this.playerGraphic != null)
			{
				this.pipeline.Remove(this.playerGraphic);
			}
			this.playerGraphic = new IndexedColorGraphic(resource, subsprite, this.position, (int)this.position.Y);
			this.pipeline.Add(this.playerGraphic);
			if (this.animator == null)
			{
				this.animator = new AnimationControl(this.playerGraphic, this.direction);
			}
			this.animator.ChangeGraphic(this.playerGraphic);
			this.animator.UpdateSubsprite(this.GetAnimationContext());
		}

		private void HandleRunFlags()
		{
			if (this.isNauseous)
			{
				return;
			}
			if (this.isRunButtonPressed)
			{
				if (!this.isRunning)
				{
					this.isCrouch = true;
					this.crouchTimerIndex = TimerManager.Instance.StartTimer(10);
					this.runVector = VectorMath.DirectionToVector(this.direction);
					this.moveVector = VectorMath.ZERO_VECTOR;
				}
				else
				{
					this.SetRunning(false);
				}
				this.isRunButtonPressed = false;
			}
			if (this.isRunButtonReleased)
			{
				if (this.isRunReady)
				{
					this.isRunReady = false;
					this.SetRunning(true);
				}
				else
				{
					TimerManager.Instance.Cancel(this.crouchTimerIndex);
				}
				this.animator.ClearOverride();
				this.isCrouch = false;
				this.isRunButtonReleased = false;
			}
			if (this.isRunTimerComplete)
			{
				this.isRunReady = true;
				this.isRunTimerComplete = false;
			}
		}

		public override void Input()
		{
			if (this.mover == null)
			{
				this.HandleRunFlags();
				if (!this.isInputLocked)
				{
					if (this.moveVector.X != 0f || this.moveVector.Y != 0f)
					{
						this.lastMoveVector = this.moveVector;
						this.lastSpeed = this.speed;
					}
					Vector2f v = VectorMath.Truncate(InputManager.Instance.Axis);
					this.speed = (this.isRunning ? 3f : 1f);
					this.recorder.Running = this.isRunning;
					this.recorder.Crouching = this.isCrouch;
					if (!this.isCrouch && !this.isRunning)
					{
						this.moveVector = v;
						if (v.X != 0f || v.Y != 0f)
						{
							this.direction = VectorMath.VectorToDirection(this.moveVector);
						}
					}
					else if (this.isCrouch)
					{
						if (v.X != 0f || v.Y != 0f)
						{
							this.runVector = v;
							this.direction = VectorMath.VectorToDirection(v);
							this.animator.UpdateSubsprite(this.GetAnimationContext());
						}
					}
					else if (this.isRunning)
					{
						if (v.X != 0f || v.Y != 0f)
						{
							this.runVector = v;
						}
						this.moveVector = this.runVector;
						this.direction = VectorMath.VectorToDirection(this.moveVector);
					}
				}
				else
				{
					this.moveVector.X = 0f;
					this.moveVector.Y = 0f;
				}
				if (((this.lastMoveVector.X != this.moveVector.X || this.lastMoveVector.Y != this.moveVector.Y) && (this.moveVector.X != 0f || this.moveVector.Y != 0f)) || this.speed != this.lastSpeed)
				{
					this.checkVector = VectorMath.Truncate(VectorMath.Normalize(this.moveVector) * 11f);
				}
				this.velocity = this.moveVector;
				this.animator.UpdateSubsprite(this.GetAnimationContext());
				this.isSolid = !InputManager.Instance.State[Button.L];
			}
		}

		private void HandleCornerSliding()
		{
			if (this.direction % 2 == 0)
			{
				Vector2f vector2f = VectorMath.DirectionToVector(this.direction);
				Vector2f vector2f2 = VectorMath.LeftNormal(vector2f);
				int num = (this.direction == 0 || this.direction == 4) ? 8 : 10;
				int num2 = -1;
				for (int i = num; i > 0; i--)
				{
					bool flag = this.collisionManager.PlaceFree(this, this.position + vector2f + vector2f2 * (float)i, null, this.ignoreCollisionTypes);
					if (flag)
					{
						num2 = i;
						break;
					}
				}
				int num3 = -1;
				for (int j = num; j > 0; j--)
				{
					bool flag2 = this.collisionManager.PlaceFree(this, this.position + vector2f - vector2f2 * (float)j, null, this.ignoreCollisionTypes);
					if (flag2)
					{
						num3 = j;
						break;
					}
				}
				if (num2 >= 0 || num3 >= 0)
				{
					Vector2f position = this.position + ((num2 > num3) ? vector2f2 : (-vector2f2));
					bool flag3 = this.collisionManager.PlaceFree(this, position, null, this.ignoreCollisionTypes);
					if (flag3)
					{
						this.lastPosition = this.position;
						this.position = position;
						this.collisionManager.Update(this, this.lastPosition, this.position);
						position = this.position + vector2f;
						flag3 = this.collisionManager.PlaceFree(this, position, null, this.ignoreCollisionTypes);
						if (flag3)
						{
							this.lastPosition = this.position;
							this.position = position;
							this.collisionManager.Update(this, this.lastPosition, this.position);
						}
					}
				}
			}
		}

		protected override void HandleCollision(ICollidable[] collisionObjects)
		{
			if (this.OnCollision != null)
			{
				this.OnCollision(this, collisionObjects);
			}
			for (int i = 0; i < collisionObjects.Length; i++)
			{
				if (collisionObjects[i] != null && !(collisionObjects[i] is Portal) && !(collisionObjects[i] is TriggerArea))
				{
					if (this.isRunning)
					{
						this.SetRunning(false);
						return;
					}
					if (collisionObjects[i] is SolidStatic)
					{
						this.HandleCornerSliding();
						return;
					}
					if (collisionObjects[i] is NPC && ((collisionObjects[i] as NPC).MovementLocked = false))
					{
						if (!collisionManager.PlaceFree(this, position))
						{
							Console.WriteLine($"the phantom exterior like fish eggs interior like suicide wrist-red. I could exercise you, this could be your phys-ed. Cheat on your man homie AAGH I tried to sneak through the door man! Can't make it. Can't make it. Shit's stuck. Outta my way son! DOOR STUCK! DOOR STUCK! PLEASE! I BEG YOU! We're dead. You're a genuine dick sucker.");
							this.position += new Vector2f(direction, direction);
							HandleCornerSliding();
						}
					}
				}
			}
		}

		private void UpdateGraphics()
		{
			this.graphicOffset.Y = -this.zOffset;
			this.playerGraphic.Position = VectorMath.Truncate(this.position + this.graphicOffset);
			this.playerGraphic.Depth = (int)this.Position.Y;
			this.pipeline.Update(this.playerGraphic);
			if (this.isShadowEnabled)
			{
				this.shadowGraphic.Position = VectorMath.Truncate(this.position);
				this.shadowGraphic.Depth = this.playerGraphic.Depth - 1;
				this.pipeline.Update(this.shadowGraphic);
			}
		}

		public override void Update()
		{
			if (this.mover != null)
			{
				this.mover.GetNextMove(ref this.position, ref this.velocity, ref this.direction);
				this.speed = (float)((int)VectorMath.Magnitude(this.velocity));
				this.animator.UpdateSubsprite(this.GetAnimationContext());
				this.velocity.X = Math.Max(-1f, Math.Min(1f, this.velocity.X));
				this.velocity.Y = Math.Max(-1f, Math.Min(1f, this.velocity.Y));
				this.moveVector = this.velocity;
			}
			int num = 0;
			while ((float)num < this.speed)
			{
				base.Update();
				if ((int)this.lastPosition.X != (int)this.position.X || (int)this.lastPosition.Y != (int)this.position.Y)
				{
					this.recorder.Record(this.position, this.moveVector, this.terrainType);
				}
				num++;
			}
			if (this.hopFactor >= 1f)
			{
				this.lastZOffset = this.zOffset;
				this.zOffset = (float)Math.Sin((double)((float)(Engine.Frame - this.hopFrame) / (this.hopFactor * 0.3f))) * this.hopFactor;
				if (this.zOffset < 0f)
				{
					this.zOffset = 0f;
					this.hopFactor = 0f;
				}
			}
			if ((int)this.lastPosition.X != (int)this.position.X || (int)this.lastPosition.Y != (int)this.position.Y || (int)this.lastZOffset != (int)this.zOffset)
			{
				this.UpdateGraphics();
			}
		}

		public override void Collision(CollisionContext context)
		{
			if (!(context.Other is TriggerArea))
			{
				base.Collision(context);
				this.playerGraphic.Position = this.Position;
				this.playerGraphic.Depth = (int)this.Position.Y;
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					this.pipeline.Remove(this.playerGraphic);
					this.pipeline.Remove(this.shadowGraphic);
					this.playerGraphic.Dispose();
					this.shadowGraphic.Dispose();
				}
				InputManager.Instance.ButtonPressed -= this.ButtonPressed;
				InputManager.Instance.ButtonReleased -= this.ButtonReleased;
				TimerManager.Instance.OnTimerEnd -= this.CrouchTimerEnd;
				this.disposed = true;
			}
			base.Dispose(disposing);
		}

		private const float HOT_POINT_LENGTH = 11f;

		private const int SLIDE_DISTANCE_X = 10;

		private const int SLIDE_DISTANCE_Y = 8;

		private const float SPEED_WALK = 1f;

		private const float SPEED_RUN = 3f;

		private const float SPEED_CYCLE = 4f;

		private const int RUN_TIMER_DURATION = 10;

		private Vector2f moveVector;

		private Vector2f runVector;

		private Vector2f lastMoveVector;

		private RenderPipeline pipeline;

		private PartyTrain recorder;

		private Graphic shadowGraphic;

		private IndexedColorGraphic playerGraphic;

		private int direction;

		private float speed;

		private float lastSpeed;

		private CharacterType character;

		private Mover mover;

		private bool isShadowEnabled;

		private bool isDead;

		private bool isNauseous;

		private bool isCrouch;

		private bool isRunning;

		private bool isRunReady;

		private int crouchTimerIndex;

		private float hopFactor;

		private long hopFrame;

		private float lastZOffset;

		private Vector2f graphicOffset;

		private bool isRunButtonPressed;

		private bool isRunButtonReleased;

		private bool isRunTimerComplete;

		private bool isInputLocked;

		private IndexedColorGraphic effectGraphic;

		private AnimationControl animator;

		private int animationLoopCount;

		private int animationLoopCountTarget;

		private TerrainType terrainType;

		private Vector2f checkVector;

		public delegate void OnCollisionHanlder(Player sender, ICollidable[] collisionObjects);

		public delegate void OnTelepathyAnimationCompleteHanlder(Player sender);

		public delegate void OnRunningChangeHandler(Player sender);
	}
}
	