using System;

namespace Carbine.Audio.fmod
{
    internal sealed class FmodException : Exception
    {
        public FmodException(string message) : base(message)
        {
        }
    }
}
