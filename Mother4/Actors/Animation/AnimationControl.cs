using System;
using System.Collections.Generic;
using Carbine.Graphics;
using Carbine.Utility;
using Mother4.Overworld;
using SFML.System;

namespace Mother4.Actors.Animation
{
	// Token: 0x02000051 RID: 81
	internal class AnimationControl
	{
		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060001FE RID: 510 RVA: 0x0000C148 File Offset: 0x0000A348
		public bool Overriden
		{
			get
			{
				return this.isOverriden;
			}
		}

		// Token: 0x060001FF RID: 511 RVA: 0x0000C150 File Offset: 0x0000A350
		public AnimationControl(IndexedColorGraphic graphic, int initialDirection)
		{
			this.graphic = graphic;
			this.counts = new Dictionary<AnimationType, byte>();
			this.previousVelocity = VectorMath.ZERO_VECTOR;
			this.previousDirection = initialDirection;
			this.previousStance = AnimationType.INVALID;
			this.animationType = (AnimationType)263;
			this.overrideSubsprite = string.Empty;
			this.Initialize();
		}

		// Token: 0x06000200 RID: 512 RVA: 0x0000C1AC File Offset: 0x0000A3AC
		private void Initialize()
		{
            foreach (SpriteDefinition spriteDefinition in (IEnumerable<SpriteDefinition>)((IndexedTexture)this.graphic.Texture).GetSpriteDefinitions())
            {
                AnimationType animationType = AnimationNames.GetAnimationType(spriteDefinition.Name.ToLowerInvariant());
                if (animationType != AnimationType.INVALID)
                {
                    AnimationType key = animationType & AnimationType.STANCE_MASK;
                    if (this.defaultStance == AnimationType.INVALID)
                        this.defaultStance = key;
                    if (this.counts.ContainsKey(key))
                    {
                        Dictionary<AnimationType, byte> counts;
                        AnimationType index;
                        (counts = this.counts)[index = key] = (byte)((uint)counts[index] + 1U);
                    }
                    else
                        this.counts.Add(key, (byte)1);
                }
            }
            this.hasStand = this.counts.ContainsKey(AnimationType.STAND);
            this.hasWalk = this.counts.ContainsKey(AnimationType.WALK);
            this.hasRun = this.counts.ContainsKey(AnimationType.RUN);
            this.hasCrouch = this.counts.ContainsKey(AnimationType.CROUCH);
            this.hasDead = this.counts.ContainsKey(AnimationType.DEAD);
            this.hasIdle = this.counts.ContainsKey(AnimationType.IDLE);
            this.hasTalk = this.counts.ContainsKey(AnimationType.TALK);
            this.hasBlink = this.counts.ContainsKey(AnimationType.BLINK);
		}

		// Token: 0x06000201 RID: 513 RVA: 0x0000C328 File Offset: 0x0000A528
		public void ChangeGraphic(IndexedColorGraphic graphic)
		{
			this.graphic = graphic;
			this.counts.Clear();
			this.animationType = (AnimationType)263;
			this.overrideSubsprite = string.Empty;
			this.Initialize();
		}

		// Token: 0x06000202 RID: 514 RVA: 0x0000C358 File Offset: 0x0000A558
		public void OverrideSubsprite(string subsprite)
		{
			this.isOverriden = true;
			this.overrideSubsprite = subsprite;
			this.graphic.SpeedModifier = 1f;
		}

		// Token: 0x06000203 RID: 515 RVA: 0x0000C378 File Offset: 0x0000A578
		public void ClearOverride()
		{
			this.isOverriden = false;
			this.overrideSubsprite = string.Empty;
		}

		// Token: 0x06000204 RID: 516 RVA: 0x0000C38C File Offset: 0x0000A58C
		private AnimationType GetDirectionPart(Vector2f velocity, int direction, int dirCount)
		{
			int num = direction;
			if (dirCount == 4 && num % 2 == 1)
			{
				num = ((num > 4) ? 6 : 2);
			}
			AnimationType result;
			switch (num)
			{
				case 0:
					result = AnimationType.EAST;
					break;
				case 1:
					result = AnimationType.NORTHEAST;
					break;
				case 2:
					result = AnimationType.NORTH;
					break;
				case 3:
					result = AnimationType.NORTHWEST;
					break;
				case 4:
					result = AnimationType.WEST;
					break;
				case 5:
					result = AnimationType.SOUTHWEST;
					break;
				case 6:
					result = AnimationType.SOUTH;
					break;
				case 7:
					result = AnimationType.SOUTHEAST;
					break;
				default:
					result = AnimationType.SOUTH;
					break;
			}
			return result;
		}

		// Token: 0x06000205 RID: 517 RVA: 0x0000C3FC File Offset: 0x0000A5FC
		private void DetermineSubsprite(float velocityMagnitude, int direction, AnimationContext context)
		{
			AnimationType animationType = AnimationType.SOUTH;
			AnimationType animationType2;
			if (context.IsTalk)
			{
				animationType2 = AnimationType.TALK;
			}
			else if (context.IsDead)
			{
				animationType2 = AnimationType.DEAD;
			}
			else if (context.IsCrouch)
			{
				animationType2 = AnimationType.CROUCH;
			}
			else if (context.IsNauseous)
			{
				animationType2 = AnimationType.NAUSEA;
			}
			else if (velocityMagnitude >= 2f)
			{
				TerrainType terrainType = context.TerrainType;
				if (terrainType == TerrainType.Ocean)
				{
					animationType2 = AnimationType.SWIM;
				}
				else
				{
					animationType2 = (this.hasRun ? AnimationType.RUN : AnimationType.WALK);
				}
			}
			else if (velocityMagnitude > 0f)
			{
				TerrainType terrainType2 = context.TerrainType;
				if (terrainType2 == TerrainType.Ocean)
				{
					animationType2 = AnimationType.SWIM;
				}
				else
				{
					animationType2 = AnimationType.WALK;
				}
			}
			else
			{
				TerrainType terrainType3 = context.TerrainType;
				if (terrainType3 == TerrainType.Ocean)
				{
					animationType2 = AnimationType.FLOAT;
				}
				else
				{
					animationType2 = AnimationType.STAND;
				}
			}
			if (!this.counts.ContainsKey(animationType2) || this.counts[animationType2] == 0)
			{
				animationType2 = this.defaultStance;
			}
			if (animationType2 != AnimationType.INVALID)
			{
				animationType = this.GetDirectionPart(context.Velocity, direction, (int)this.counts[animationType2]);
			}
			this.animationType = (animationType2 | animationType);
		}

		// Token: 0x06000206 RID: 518 RVA: 0x0000C520 File Offset: 0x0000A720
		public void UpdateSubsprite(AnimationContext context)
		{
			bool flag = false;
			float num = VectorMath.Magnitude(context.Velocity);
			int num3;
			if (num > 0f)
			{
				double num2 = Math.Atan2((double)(-(double)context.Velocity.Y), (double)context.Velocity.X);
				num3 = (int)Math.Floor(num2 / 0.7853981633974483);
				if (num3 < 0)
				{
					num3 += 8;
				}
			}
			else
			{
				num3 = context.SuggestedDirection;
				flag = (!context.IsDead && !context.IsCrouch && !context.IsTalk);
			}
			bool reset = false;
			string @string;
			if (!this.isOverriden)
			{
				this.previousStance = (this.animationType & AnimationType.STANCE_MASK);
				this.DetermineSubsprite(num, num3, context);
				@string = AnimationNames.GetString(this.animationType);
				reset = (this.previousStance != (this.animationType & AnimationType.STANCE_MASK));
			}
			else
			{
				@string = this.overrideSubsprite;
			}
			this.graphic.SetSprite(@string, reset);
			if (!this.isOverriden)
			{
				if (flag && (!this.hasStand || this.animationType == AnimationType.NAUSEA))
				{
					this.graphic.SpeedModifier = 0f;
					this.graphic.Frame = 0f;
				}
				else
				{
					this.graphic.SpeedModifier = 1f;
				}
			}
			this.previousVelocity = context.Velocity;
			this.previousDirection = num3;
		}

		// Token: 0x040002CD RID: 717
		private IndexedColorGraphic graphic;

		// Token: 0x040002CE RID: 718
		private int previousDirection;

		// Token: 0x040002CF RID: 719
		private Vector2f previousVelocity;

		// Token: 0x040002D0 RID: 720
		private AnimationType previousStance;

		// Token: 0x040002D1 RID: 721
		private AnimationType defaultStance;

		// Token: 0x040002D2 RID: 722
		private Dictionary<AnimationType, byte> counts;

		// Token: 0x040002D3 RID: 723
		private bool hasStand;

		// Token: 0x040002D4 RID: 724
		private bool hasWalk;

		// Token: 0x040002D5 RID: 725
		private bool hasRun;

		// Token: 0x040002D6 RID: 726
		private bool hasCrouch;

		// Token: 0x040002D7 RID: 727
		private bool hasDead;

		// Token: 0x040002D8 RID: 728
		private bool hasIdle;

		// Token: 0x040002D9 RID: 729
		private bool hasTalk;

		// Token: 0x040002DA RID: 730
		private bool hasBlink;

		// Token: 0x040002DB RID: 731
		private bool isOverriden;

		// Token: 0x040002DC RID: 732
		private string overrideSubsprite;

		// Token: 0x040002DD RID: 733
		private AnimationType animationType;
	}
}
