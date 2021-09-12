using System.Collections.Generic;
using Kantaiko.Routing.Handlers;

namespace Kantaiko.Routing
{
    public static class Handler
    {
        public static IHandler<TInput, TOutput> Split<TInput, TOutput>(
            SplitHandler<TInput, TOutput>.SplitPredicate splitPredicate,
            IHandler<TInput, TOutput> firstHandler,
            IHandler<TInput, TOutput> secondHandler)
        {
            return new SplitHandler<TInput, TOutput>(splitPredicate, firstHandler, secondHandler);
        }

        public static IHandler<TInput, TOutput> Function<TInput, TOutput>(
            FunctionHandler<TInput, TOutput>.FunctionDelegate functionDelegate)
        {
            return new FunctionHandler<TInput, TOutput>(functionDelegate);
        }

        public static IChainedHandler<TInput, TOutput> Function<TInput, TOutput>(
            ChainedFunctionHandler<TInput, TOutput>.FunctionDelegate functionDelegate)
        {
            return new ChainedFunctionHandler<TInput, TOutput>(functionDelegate);
        }

        public static IHandler<TInput, TOutput> Chain<TInput, TOutput>(
            IEnumerable<IHandler<TInput, TOutput>> handlers)
        {
            return new ChainHandler<TInput, TOutput>(handlers);
        }
    }
}
