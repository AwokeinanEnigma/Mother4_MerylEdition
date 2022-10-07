using System;
using Carbine.Audio;
using Carbine.Graphics;
using Mother4.Battle.UI;
using SFML.Graphics;
using SFML.System;

namespace Mother4.Battle.PsiAnimation
{
	internal struct PsiElement
	{
		public int Timestamp;

		public MultipartAnimation Animation;

		public Vector2f Offset;

		public bool LockToTargetPosition;

		public int PositionIndex;

		public CarbineSound Sound;

		public int CardPop;

		public float CardPopSpeed;

		public int CardPopHangtime;

		public BattleCard.SpringMode CardSpringMode;

		public Vector2f CardSpringAmplitude;

		public Vector2f CardSpringSpeed;

		public Vector2f CardSpringDecay;

		public Color? TargetFlashColor;

		public ColorBlendMode TargetFlashBlendMode;

		public int TargetFlashCount;

		public int TargetFlashFrames;

		public Color? SenderFlashColor;

		public ColorBlendMode SenderFlashBlendMode;

		public int SenderFlashCount;

		public int SenderFlashFrames;

		public Color? ScreenDarkenColor;

		public int? ScreenDarkenDepth;
	}
}
