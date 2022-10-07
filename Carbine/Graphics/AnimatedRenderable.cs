using System;

namespace Carbine.Graphics
{
    /// <summary>
    /// An abstract class for animated graphics
    /// </summary>
    public abstract class AnimatedRenderable : Renderable
    {
        /// <summary>
        /// How many frames this animated graphic has
        /// </summary>
        public int Frames
        {
            get
            {
                return this.frames;
            }
            protected set
            {
                this.frames = value;
            }
        }
        /// <summary>
        /// The frame this animated graphic is on
        /// </summary>
        public float Frame
        {
            get
            {
                return this.frame;
            }
            set
            {
                this.frame = Math.Max(0f, Math.Min(frames, value));
            }
        }
        /// <summary>
		/// Animation speeds
		/// </summary>
		public float[] Speeds
        {
            get
            {
                return this.speeds;
            }
            protected set
            {
                this.speeds = value;
            }
        }

        /// <summary>
		/// Speed modifier
		/// </summary>
		public float SpeedModifier
        {
            get
            {
                return this.speedModifier;
            }
            set
            {
                this.speedModifier = value;
            }
        }
        /// <summary>
        /// When this animated graphic has finished its animation.
        /// </summary>
        public event AnimatedRenderable.AnimationCompleteHandler OnAnimationComplete;

        protected void AnimationComplete()
        {
            OnAnimationComplete?.Invoke(this);
        }
        protected int frames;
        protected float frame;
        protected float speedIndex;
        protected float[] speeds;
        protected float speedModifier;
        public delegate void AnimationCompleteHandler(AnimatedRenderable renderable);
    }
}
