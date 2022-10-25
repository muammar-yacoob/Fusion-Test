
using Fusion;
using UnityEditor;

namespace Born.Player
{
    [ScriptHelp(BackColor = EditorHeaderBackColor.Green)]
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
