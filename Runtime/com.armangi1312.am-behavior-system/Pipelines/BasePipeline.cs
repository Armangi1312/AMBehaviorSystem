using System;
using System.Collections.Generic;
using UnityEngine;

namespace AMBehaviorSystem.Pipelines
{
    [Serializable]
    public abstract class BasePipeline<TProcessor, TSetting, TContext>
        where TProcessor : Processor
        where TSetting : ISetting
        where TContext : IContext
    {
        public abstract void Initialize(IReadOnlyList<TProcessor> processors, IReadOnlyRegistry<TSetting> settings, IReadOnlyRegistry<TContext> contexts, Component owner);

        public abstract void Invoke(InvokeTiming timing);

        protected static T Find<T>(IReadOnlyList<Processor> processors) where T : Processor
        {
            foreach (Processor processor in processors)
            {
                if (processor is T typed)
                    return typed;
            }

            return null;
        }

        protected virtual void InvokeProcessor(Processor processor, InvokeTiming timing)
        {
            if (processor == null || (processor.InvokeTiming & timing) == 0) return;

            processor.Process();
        }
    }
}
