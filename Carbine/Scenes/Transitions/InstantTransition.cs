namespace Carbine.Scenes.Transitions
{
    public class InstantTransition : ITransition
    {
        public bool IsComplete
        {
            get
            {
                return true;
            }
        }

        public float Progress
        {
            get
            {
                return 1f;
            }
        }

        public bool ShowNewScene
        {
            get
            {
                return true;
            }
        }

        public bool Blocking { get; set; }

        public void Update()
        {
        }

        public void Draw()
        {
        }

        public void Reset()
        {
        }
    }
}
