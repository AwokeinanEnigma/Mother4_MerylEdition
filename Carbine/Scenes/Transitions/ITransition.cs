namespace Carbine.Scenes.Transitions
{
    public interface ITransition
    {
        bool IsComplete { get; }

        float Progress { get; }

        bool ShowNewScene { get; }

        bool Blocking { get; set; }

        void Update();

        void Draw();

        void Reset();
    }
}
