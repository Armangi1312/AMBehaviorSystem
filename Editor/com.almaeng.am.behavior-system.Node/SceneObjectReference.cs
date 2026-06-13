using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AMBehaviorSystem.Node
{
    [Serializable]
    public sealed class SceneObjectReference<T> where T : Object
    {
        [SerializeField, HideInInspector] private string targetID;
        [SerializeField, HideInInspector] private T target;

        public T Value
        {
            get
            {
                if (target == null)
                    Resolve();
                return target;
            }
            set
            {
                target = value;
                SyncID();
            }
        }

        public void Resolve()
        {
            if (target != null)
                return;

            if (string.IsNullOrEmpty(targetID))
                return;

            if (GlobalObjectId.TryParse(targetID, out var id))
                target = GlobalObjectId.GlobalObjectIdentifierToObjectSlow(id) as T;
            else
                target = null;
        }

        private void SyncID()
        {
            if (target == null)
            {
                targetID = null;
                return;
            }

            var id = GlobalObjectId.GetGlobalObjectIdSlow(target);
            targetID = id.identifierType != 0 ? id.ToString() : null;
        }
    }
}
