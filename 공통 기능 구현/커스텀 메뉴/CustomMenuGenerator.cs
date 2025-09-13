using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class CustomMenuGenerator
{
    const string PATH = "Assets/lLibrary";
    const string FOLDER = "GameObject";    

    class Data
    {
        public string fileName;        
        public string path;
        public bool isUI;
    }

    [MenuItem("Custom Menu/Generate Menu")]
    public static void GenerateScript()
    {        
        var list = new List<Data>();

        var path = $"{PATH}/{FOLDER}";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        var files = Directory.GetFiles(path, "*.prefab", SearchOption.AllDirectories);
        foreach (var file in files)
        {
            var index = file.LastIndexOf(FOLDER);
            var localPath = file.Substring(index).Replace("\\", "/").Replace(".prefab", "");

            index = localPath.LastIndexOf("/");
            var name = localPath.Substring(index + 1);

            var data = new Data();
            data.path = localPath;
            data.fileName = name;
            data.isUI = localPath.Contains("UI");

            list.Add(data);
        }
        #region Static Code
        var script = @"using UnityEditor;
using UnityEngine.UI;
using UnityEngine;

public class CustomMenu
{
    static void CreateUI(MenuCommand menu, string path)
    {        
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

        if (prefab is null)
        {
            if (EditorUtility.DisplayDialog(""Error"", $""{path} is null\nRegenerate Custom Menu?"", ""Generate"", ""Cancel""))
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
                var canvasObj = new GameObject(""Canvas"");
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

        Undo.RegisterCreatedObjectUndo(obj, $""Create {obj.name}"");
        Selection.activeObject = obj;
    }
    static void CreateObj(MenuCommand menu, string path)
    {
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

        if (prefab is null)
        {
            if (EditorUtility.DisplayDialog(""Error"", $""{path} is null\nRegenerate Custom Menu?"", ""Generate"", ""Cancel""))
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

        Undo.RegisterCreatedObjectUndo(obj, $""Create {obj.name}"");
        Selection.activeObject = obj;
    }

";
        #endregion

        var top = @"    [MenuItem(""{0}"")]
    static void CreateObject{1}(MenuCommand menu)";

        var uiMethod = @"       CreateUI(menu, ""{0}"");";
        var objMethod = @"      CreateObj(menu, ""{0}"");";

        var idx = 0;
        foreach (var item in list)
        {
            var method = item.isUI ? uiMethod : objMethod;
            var line = $"{string.Format(top, item.path, idx++)}\n\t{{\n{string.Format(method, $"{PATH}/{item.path}.prefab")}\n\t}}\n";
            script += line;
        }
        script += "}";
        var scriptPath = $"{PATH}/Editor/CustomMenu.cs";
        File.WriteAllText(scriptPath, script, System.Text.Encoding.UTF8);

        EditorUtility.DisplayDialog("UI Menu", "UI Menu Generate Successed", "Ok");
        AssetDatabase.Refresh();
    }
}