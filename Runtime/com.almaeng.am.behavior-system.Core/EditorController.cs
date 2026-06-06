#if UNITY_EDITOR
using System;

namespace AMBehaviorSystem.Core
{
    public partial class Controller<TSetting, TContext, TProcessor>
    {
        private void OnValidate()
        {
            try
            {
                ValidateDependencies();
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError($"An error occurred while validating of {name}: {e}");
            }
        }
    }
}
#endif