using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEditor;

public class EditorMenu : MonoBehaviour
{
    [MenuItem("Scenes/Game/GameScene")]
    static void EditorMenu_LoadInGameScene() // 게임 씬
    {
        EditorSceneManager.OpenScene("Assets/00.Scenes/SampleScene.unity"); 
    }
}
