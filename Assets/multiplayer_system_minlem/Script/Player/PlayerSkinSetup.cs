//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor;
//using UnityEngine;

//public class PlayerSkinSetup : MonoBehaviour
//{
//    public Sprite[] idleFrames;
//    public Sprite[] walkDownFrames;
//    public Sprite[] walkLeftFrames;
//    public Sprite[] walkRightFrames;
//    public Sprite[] walkUpFrames;

//    public AnimationClip idleClip;
//    public AnimationClip walkDownClip;
//    public AnimationClip walkLeftClip;
//    public AnimationClip walkRightClip;
//    public AnimationClip walkUpClip;

//    public RuntimeAnimatorController baseController;

//    private Animator animator;

//    void Start()
//    {
//        animator = GetComponent<Animator>();

//        if (baseController != null)
//        {
//            var overrideController = new AnimatorOverrideController(baseController);
//            animator.runtimeAnimatorController = overrideController;

//            overrideController["Idle"] = GenerateAnimationClip(idleFrames, idleClip.frameRate);
//            overrideController["WalkDown"] = GenerateAnimationClip(walkDownFrames, walkDownClip.frameRate);
//            overrideController["WalkLeft"] = GenerateAnimationClip(walkLeftFrames, walkLeftClip.frameRate);
//            overrideController["WalkRight"] = GenerateAnimationClip(walkRightFrames, walkRightClip.frameRate);
//            overrideController["WalkUp"] = GenerateAnimationClip(walkUpFrames, walkUpClip.frameRate);
//        }
//    }

//    AnimationClip GenerateAnimationClip(Sprite[] frames, float frameRate)
//    {
//        AnimationClip clip = new AnimationClip();
//        clip.frameRate = frameRate;

//        EditorCurveBinding spriteBinding = new EditorCurveBinding();
//        spriteBinding.type = typeof(SpriteRenderer);
//        spriteBinding.path = "";
//        spriteBinding.propertyName = "m_Sprite";

//        ObjectReferenceKeyframe[] keyFrames = new ObjectReferenceKeyframe[frames.Length];
//        for (int i = 0; i < frames.Length; i++)
//        {
//            keyFrames[i] = new ObjectReferenceKeyframe();
//            keyFrames[i].time = i / frameRate;
//            keyFrames[i].value = frames[i];
//        }

//        AnimationUtility.SetObjectReferenceCurve(clip, spriteBinding, keyFrames);
//        return clip;
//    }
//}
