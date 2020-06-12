#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml;

using SceneType = defines.SceneType;

public class SceneInfo : Singleton<SceneInfo>
{
    [SerializeField]
    private SceneType scene_type = SceneType.NULL;

    public static SceneType SceneType
    {
        get{ return SceneInfo.Instance.scene_type; }
    }
}
