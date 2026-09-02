using System;
using System.Collections.Generic;
using UnityEngine;

namespace AMBehaviorSystem.Pipelines
{
    /// <summary>
    /// An abstract pipeline that invokes processors according to the pipeline sequence.
    /// </summary>
    /// <typeparam name="TSetting">The type of settings used by this processor.</typeparam>
    /// <typeparam name="TContext">The type of context accessed by this processor.</typeparam>
    /// <typeparam name="TProcessor">The type of processor managed by this controller.</typeparam>
    [Serializable]
    public abstract class BasePipeline<TProcessor, TSetting, TContext>
        where TProcessor : Processor
        where TSetting : ISetting
        where TContext : IContext
    {
        /// <summary>
        /// Initialize this pipeline.
        /// </summary>
        /// <param name="processors">The list of processors for the controller.</param>
        /// <param name="settings">The registry of settings for the controller.</param>
        /// <param name="contexts">The registry of contexts for the controller.</param>
        /// <param name="owner">The component that owns this pipeline.</param>
        public abstract void Initialize(IReadOnlyList<TProcessor> processors, IReadOnlyRegistry<TSetting> settings, IReadOnlyRegistry<TContext> contexts, Component owner);

        /// <summary>
        /// A method that implements the invocation of processors according to the pipeline sequence.
        /// </summary>
        /// <param name="timing">The timing of the current call.</param>
        public abstract void Invoke(InvokeTiming timing);

        /// <summary>
        /// A method that finds a processor of the specified type from the list of processors.
        /// </summary>
        /// <param name="processors">The list of processors to search.</param>
        /// <typeparam name="T">The type of processor to find.</typeparam>
        /// <returns>The found processor, or null if not found.</returns>
        protected static T Find<T>(IReadOnlyList<Processor> processors) where T : Processor
        {
            foreach (Processor processor in processors)
            {
                if (processor is T typed)
                    return typed;
            }

            return null;
        }

        /// <summary>
        /// A method that invokes a processor based on the specified timing.
        /// </summary>
        /// <param name="processor">The processor to be invoked.</param>
        /// <param name="timing">The timing at which to invoke the processor.</param>
        protected virtual void InvokeProcessor(Processor processor, InvokeTiming timing)
        {
            if (processor == null || (processor.InvokeTiming & timing) == 0) return;
            
            processor.CurrentTiming = timing;
            processor.Process();
        }
    }
}
