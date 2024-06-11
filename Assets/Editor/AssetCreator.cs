using UnityEditor;
using UnityEngine;

public class AssetCreator 
{
    [MenuItem("Assets/Create/Conversation Data")]
    public static void CreateConversationData()
    {
        ScriptableObjectUtility.CreateAsset<ConversationData>();
    }

}
