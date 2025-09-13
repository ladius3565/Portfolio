using UnityEditor;
using UnityEngine.UI;
using UnityEngine;

public class CustomMenu
{
    static void CreateUI(MenuCommand menu, string path)
    {        
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

        if (prefab is null)
        {
            if (EditorUtility.DisplayDialog("Error", $"{path} is null\nRegenerate Custom Menu?", "Generate", "Cancel"))
            {
                CustomMenuGenerator.GenerateScript();
            }
            return;
        }

        GameObject target = null;
        if (menu.context is null)
        {
            var list = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
            var isCanvas = false;
            for (var i = 0; i < list.Length; i++)
            {
                if (list[i].TryGetComponent<Canvas>(out var canvas))
                {
                    target = list[i];
                    isCanvas = true;
                    break;
                }
            }

            if (!isCanvas)
            {
                var canvasObj = new GameObject("Canvas");
                var canvas = canvasObj.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvasObj.AddComponent<CanvasScaler>();
                canvasObj.AddComponent<GraphicRaycaster>();
                target = canvasObj;
            }
        }
        else
        {
            target = menu.context as GameObject;
        }
        
        var obj = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        GameObjectUtility.SetParentAndAlign(obj, target);

        Undo.RegisterCreatedObjectUndo(obj, $"Create {obj.name}");
        Selection.activeObject = obj;
    }
    static void CreateObj(MenuCommand menu, string path)
    {
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

        if (prefab is null)
        {
            if (EditorUtility.DisplayDialog("Error", $"{path} is null\nRegenerate Custom Menu?", "Generate", "Cancel"))
            {
                CustomMenuGenerator.GenerateScript();
            }
            return;
        }

        var obj = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        if (menu.context is not null)
        {
            GameObjectUtility.SetParentAndAlign(obj, menu.context as GameObject);
        }

        Undo.RegisterCreatedObjectUndo(obj, $"Create {obj.name}");
        Selection.activeObject = obj;
    }

}