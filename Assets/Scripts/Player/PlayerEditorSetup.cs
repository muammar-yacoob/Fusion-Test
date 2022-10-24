#if UNITY_EDITOR
using Fusion;
using UnityEditor;

namespace Born.Player
{
    public class PlayerEditorSetup : NetworkBehaviour
    {
        private void Start()
        {
            if (Object.HasInputAuthority)
            {
                gameObject.name += " - Mine";
                Selection.activeObject = transform;
            }
            else
            {
                gameObject.name += " - Other";
            }
        }
    }
}
#endif