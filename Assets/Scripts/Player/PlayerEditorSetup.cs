
using Fusion;
using UnityEditor;

namespace Born.Player
{
    public class PlayerEditorSetup : NetworkBehaviour
    {
#if UNITY_EDITOR
        public override void Spawned()
        {
            if (Object.HasInputAuthority)
            {
                gameObject.name += " - Mine";
                Selection.activeObject = transform;
            }
        }
#endif
    }
}
