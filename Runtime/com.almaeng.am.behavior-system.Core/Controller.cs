using AMBehaviorSystem.Core.Pipelines;
using AMBehaviorSystem.Core.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AMBehaviorSystem.Core
{
    public partial class Controller<TSetting, TContext, TProcessor> : MonoBehaviour
        where TSetting : ISetting
        where TContext : IContext
        where TProcessor : Processor<TSetting, TContext>
    {
        //==== 공유 프로퍼티 ====//

        /// <summary>
        /// 런타임 준불변 데이터를 저장하는 객체입니다.
        /// </summary>
        public Registry<TSetting> Settings => settings;
        [SerializeField] private ObservableRegistry<TSetting> settings = new();

        /// <summary>
        /// 런타임 가변 데이터를 저장하는 객체입니다.
        /// </summary>
        public Registry<TContext> Contexts => contexts;
        [SerializeField] private ObservableRegistry<TContext> contexts = new();

        /// <summary>
        /// 게임 로직을 처리하는 모듈 단위의 프로세서를 저장하는 객체입니다.
        /// </summary>
        public List<TProcessor> Processors => processors;
        [SerializeField] private ObservableList<TProcessor> processors = new();

        //[SerializeField] protected PipelineGraph Graph;
        public Pipeline Pipeline { get; private set; }

        //==== 내부 프로퍼티 ====//

        protected bool IsInitialized;


        protected void Initialize()
        {
            if(IsInitialized) return;
            IsInitialized = true;


        }

        private void InitializeProcessors()
        {
            for(int i = 0; i < processors.Count; i++)
            {
                var processor = processors[i];

                if (processor == null)
                {
                    Debug.LogWarning($"{processor}은(는) NULL입니다.");
                    continue;
                }


                processor.Initialize(settings, contexts);
            }
        }

        private void InitializePipeline()
        {
            //Pipeline = PipelineFactory.CreatePipeline(Graph);
        }

        protected void ValidateDependencies()
        {

        }

        private void ValidateProcessorDependencies()
        {
            for (int i = 0; i < processors.Count; i++)
            {
                var processor = processors[i];

                (Type[] Context, Type[] Setting, Type[] Processor) dependencies = ProcessorDependencyValidator.GetRequiredTypes(processor.GetType());
            }
        }
    }
}
