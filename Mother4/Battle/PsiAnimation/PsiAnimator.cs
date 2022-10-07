using System;
using System.Collections.Generic;
using Carbine.Graphics;
using Mother4.Battle.UI;
using Mother4.GUI.Modifiers;
using SFML.Graphics;
using SFML.System;

namespace Mother4.Battle.PsiAnimation
{
	internal class PsiAnimator
	{
		public bool Complete
		{
			get
			{
				return this.complete;
			}
		}

		public event PsiAnimator.AnimationCompleteHandler OnAnimationComplete;

		public PsiAnimator(RenderPipeline pipeline, List<IGraphicModifier> graphicModifiers, PsiElementList animation, Graphic senderGraphic, Graphic[] targetGraphics, CardBar cardBar, int[] targetCardIds)
		{
			this.pipeline = pipeline;
			this.graphicModifiers = graphicModifiers;
			this.animation = animation;
			this.senderGraphic = senderGraphic;
			this.targetGraphics = targetGraphics;
			this.cardBar = cardBar;
			this.targetCardIds = targetCardIds;
			this.screenShape = new RectangleShape(new Vector2f(320f, 180f));
			this.screenShape.FillColor = new Color(0, 0, 0, 0);
		}

		private void DarkenScreen(Color darkenColor, int depth)
		{
			if (this.screenDarkenShape != null)
			{
				this.pipeline.Remove(this.screenDarkenShape);
			}
			this.targetAlpha = darkenColor.A;
			this.sourceAlpha = this.screenShape.FillColor.A;
			this.alphaMultiplier = 0f;
			FloatRect localBounds = this.screenShape.GetLocalBounds();
			this.screenShape.FillColor = new Color(darkenColor.R, darkenColor.G, darkenColor.B, this.sourceAlpha);
			this.screenDarkenShape = new ShapeGraphic(this.screenShape, new Vector2f(0f, 0f), new Vector2f(0f, 0f), new Vector2f(localBounds.Width, localBounds.Height), depth);
			this.pipeline.Add(this.screenDarkenShape);
			if (this.sourceAlpha == 0)
			{
				if (this.depthMemory == null)
				{
					this.depthMemory = new Dictionary<Graphic, int>();
				}
				else
				{
					this.depthMemory.Clear();
				}
				for (int i = 0; i < this.targetGraphics.Length; i++)
				{
					if (this.targetCardIds[i] < 0)
					{
						Graphic graphic = this.targetGraphics[i];
						this.depthMemory.Add(graphic, graphic.Depth);
						graphic.Depth = 32677;
					}
				}
			}
			this.darkenedFlag = false;
		}

		private void UpdateDarkenColor()
		{
			Color fillColor = this.screenDarkenShape.Shape.FillColor;
			this.alphaMultiplier += 0.2f;
			fillColor.A = (byte)((float)this.sourceAlpha + (float)(this.targetAlpha - this.sourceAlpha) * this.alphaMultiplier);
			this.screenDarkenShape.Shape.FillColor = fillColor;
		}

		public void Update()
		{
			List<PsiElement> elementsAtTime = this.animation.GetElementsAtTime(this.step);
			if (elementsAtTime != null && elementsAtTime.Count > 0)
			{
				foreach (PsiElement psiElement in elementsAtTime)
				{
					if (psiElement.Animation != null)
					{
						this.pipeline.Add(psiElement.Animation);
						psiElement.Animation.OnAnimationComplete += this.GraphicAnimationComplete;
						this.animatingCount++;
						if (psiElement.LockToTargetPosition)
						{
							psiElement.Animation.Position = this.targetGraphics[psiElement.PositionIndex].Position;
							psiElement.Animation.Position += psiElement.Offset;
						}
					}
					if (psiElement.Sound != null)
					{
						psiElement.Sound.Play();
					}
					Color? screenDarkenColor = psiElement.ScreenDarkenColor;
					if (screenDarkenColor != null)
					{
						Color? screenDarkenColor2 = psiElement.ScreenDarkenColor;
						this.DarkenScreen(screenDarkenColor2.Value, psiElement.ScreenDarkenDepth ?? 32667);
						this.animatingCount++;
					}
					Color? targetFlashColor = psiElement.TargetFlashColor;
					if (targetFlashColor != null)
					{
						foreach (Graphic graphic in this.targetGraphics)
						{
							if (graphic is IndexedColorGraphic)
							{
								IndexedColorGraphic graphic2 = graphic as IndexedColorGraphic;
								Color? targetFlashColor2 = psiElement.TargetFlashColor;
								GraphicFader item = new GraphicFader(graphic2, targetFlashColor2.Value, psiElement.TargetFlashBlendMode, psiElement.TargetFlashFrames, psiElement.TargetFlashCount);
								this.graphicModifiers.Add(item);
							}
						}
					}
					Color? senderFlashColor = psiElement.SenderFlashColor;
					if (senderFlashColor != null && this.senderGraphic is IndexedColorGraphic)
					{
						IndexedColorGraphic graphic3 = this.senderGraphic as IndexedColorGraphic;
						Color? senderFlashColor2 = psiElement.SenderFlashColor;
						GraphicFader item2 = new GraphicFader(graphic3, senderFlashColor2.Value, psiElement.SenderFlashBlendMode, psiElement.SenderFlashFrames, psiElement.SenderFlashCount);
						this.graphicModifiers.Add(item2);
					}
					foreach (int num in this.targetCardIds)
					{
						if (num >= 0)
						{
							this.cardBar.SetSpring(num, psiElement.CardSpringMode, psiElement.CardSpringAmplitude, psiElement.CardSpringSpeed, psiElement.CardSpringDecay);
						}
					}
				}
			}
			if (this.screenDarkenShape != null && !this.darkenedFlag)
			{
				if (Math.Abs((int)(this.targetAlpha - this.screenDarkenShape.Shape.FillColor.A)) > 1)
				{
					this.UpdateDarkenColor();
				}
				else
				{
					Color fillColor = this.screenDarkenShape.Shape.FillColor;
					fillColor.A = this.targetAlpha;
					this.screenDarkenShape.Shape.FillColor = fillColor;
					if (this.targetAlpha == 0)
					{
						foreach (Graphic graphic4 in this.targetGraphics)
						{
							if (this.depthMemory.ContainsKey(graphic4))
							{
								graphic4.Depth = this.depthMemory[graphic4];
							}
						}
					}
					this.animatingCount--;
					this.darkenedFlag = true;
				}
			}
			this.step++;
			this.complete = (!this.animation.HasElements && this.animatingCount == 0);
			if (this.complete && !this.completedFlag)
			{
				if (this.OnAnimationComplete != null)
				{
					this.OnAnimationComplete(this);
				}
				this.completedFlag = true;
			}
		}

		private void GraphicAnimationComplete(MultipartAnimation anim)
		{
			anim.Visible = false;
			this.pipeline.Remove(anim);
			anim.OnAnimationComplete -= this.GraphicAnimationComplete;
			this.animatingCount--;
		}

		private const int DARKEN_SHAPE_DEPTH = 32667;

		private const int DARKEN_GRAPHIC_DEPTH = 32677;

		private const float FADE_SPEED = 0.2f;

		private RenderPipeline pipeline;

		private PsiElementList animation;

		private Graphic senderGraphic;

		private Graphic[] targetGraphics;

		private CardBar cardBar;

		private Shape screenShape;

		private ShapeGraphic screenDarkenShape;

		private byte sourceAlpha;

		private byte targetAlpha;

		private float alphaMultiplier;

		private bool darkenedFlag;

		private Dictionary<Graphic, int> depthMemory;

		private List<IGraphicModifier> graphicModifiers;

		private int[] targetCardIds;

		private bool complete;

		private bool completedFlag;

		private int step;

		private int animatingCount;

		public delegate void AnimationCompleteHandler(PsiAnimator anim);
	}
}
