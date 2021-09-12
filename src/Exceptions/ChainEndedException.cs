using System;

namespace Kantaiko.Routing.Exceptions
{
    public class ChainEndedException : Exception
    {
        public ChainEndedException() : base("No handlers left") { }
    }
}
