using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationoverrider : MonoBehaviour
{
    [System.Serializable]
    private class AnimationClipOverride
    {
        public string clipNamed;
        public AnimationClip overrideWith;
    }

    [SerializeField] AnimationClipOverride[] clipOverrides;

    // Use this for initialization
    public void Init(Animator animator, string name)
    {
        int i = 0;
        foreach(AnimationClip clip in animator.runtimeAnimatorController.animationClips)
        {
            Animation anim = Resources.Load<Animation>("character/" + name + "/" + clip.name);
            clipOverrides[i].clipNamed = clip.name;
            clipOverrides[i].overrideWith = anim.clip;

        }

        AnimatorOverrideController overrideController = new AnimatorOverrideController();
        overrideController.runtimeAnimatorController = animator.runtimeAnimatorController;

        foreach (AnimationClipOverride clipOverride in clipOverrides)
        {
            overrideController[clipOverride.clipNamed] = clipOverride.overrideWith;
        }

        animator.runtimeAnimatorController = overrideController;
    }

   
}
