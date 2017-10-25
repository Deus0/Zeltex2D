using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Zeltex2D
{
    /// <summary>
    /// This generates an animation clip based on textures
    /// It will change the texture every x seconds
    /// </summary>
    public class AnimationGenerator : EditorWindow
    {
        public List<Sprite> MyTextures = new List<Sprite>();
        public string OutputPath = "Assets/Test.anim";
        public string PropertyPath = "CharacterSprite";
        public float TimePerFrame = 0.25f;

        // Add menu named "My Window" to the Window menu
        [MenuItem("Zeltex2D/GenerateAnimation")]
        static void Init()
        {
            // Get existing open window or if none, make a new one:
            AnimationGenerator window = (AnimationGenerator)EditorWindow.GetWindow(typeof(AnimationGenerator));
            window.name = "Animation Generator";
            window.Show();
        }

        void OnGUI()
        {
            if (GUILayout.Button("Generate"))
            {
                Debug.Log("Generating new animation!");
                GenerateAnimation();
            }
            GUILayout.Label("Output Path");
            OutputPath = EditorGUILayout.TextField(OutputPath);

            GUILayout.Label("Property Path:");
            PropertyPath = EditorGUILayout.TextField(PropertyPath);
            

            GUILayout.Label("Time Per Frame");
            TimePerFrame = float.Parse(EditorGUILayout.TextField(TimePerFrame.ToString()));

            GUILayout.Label("Textures Size");
            int OldTexturesCount = MyTextures.Count;
            int NewTexturesCount = int.Parse(EditorGUILayout.TextField(OldTexturesCount.ToString()));
            if (NewTexturesCount != OldTexturesCount)
            {
                if (NewTexturesCount > OldTexturesCount)
                {
                    for (int i = OldTexturesCount; i < NewTexturesCount; i++)
                    {
                        MyTextures.Add(null);
                    }
                }
                else
                {
                    for (int i = OldTexturesCount - 1; i >= NewTexturesCount; i--)
                    {
                        MyTextures.RemoveAt(i);
                    }
                }
            }

            GUILayout.Label("Textures");
            for (int i = 0; i < MyTextures.Count; i++)
            {
                MyTextures[i] = EditorGUILayout.ObjectField(i.ToString(), MyTextures[i], typeof(Sprite), false) as Sprite;
            }
        }

        private void GenerateAnimation()
        {
            AnimationClip clip = new AnimationClip();

            AnimationUtility.GetAnimationClipSettings(clip).loopTime = true;

            float TotalTime = (TimePerFrame * MyTextures.Count);// * clip.frameRate;

            EditorCurveBinding curveBinding = new EditorCurveBinding();
            curveBinding.type = typeof(SpriteRenderer);
            curveBinding.propertyName = "m_Sprite";
            curveBinding.path = PropertyPath;

            ObjectReferenceKeyframe[] keyFrames = new ObjectReferenceKeyframe[MyTextures.Count];

            for (int i = 0; i < MyTextures.Count; i++)
            {
                ObjectReferenceKeyframe kf = new ObjectReferenceKeyframe();
                kf.time = i * TimePerFrame;
                kf.value = MyTextures[i];
                keyFrames[i] = kf;
            }
            AnimationUtility.SetObjectReferenceCurve(clip, curveBinding, keyFrames);
            clip.wrapMode = WrapMode.Loop;

            AnimationClip outputAnimClip = AssetDatabase.LoadMainAssetAtPath(OutputPath) as AnimationClip;
            if (outputAnimClip != null)
            {
                EditorUtility.CopySerialized(clip, outputAnimClip);
                AssetDatabase.SaveAssets();
            }
            else
            {
                AssetDatabase.CreateAsset(clip, OutputPath);
                /*outputAnimClip = new AnimationClip();
                EditorUtility.CopySerialized(animClip, outputAnimClip);
                AssetDatabase.CreateAsset(outputAnimClip, path);*/
            }

            AnimationClip newClip = AssetDatabase.LoadAssetAtPath(OutputPath, typeof(AnimationClip)) as AnimationClip;
            AnimationClipSettings tSettings = AnimationUtility.GetAnimationClipSettings(newClip);
            tSettings.loopTime = true;
            AnimationUtility.SetAnimationClipSettings(newClip, tSettings);
        }
    }
}