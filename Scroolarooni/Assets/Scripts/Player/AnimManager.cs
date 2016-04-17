using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AnimManager : MonoBehaviour
{
    [System.Serializable]
    public class AnimPriority
    {
        public string Name;
        public int Priority;
    }

    private List<AnimPriority> animQueue;
    public Animator animator;
    public AnimPriority[] animationPriority;
    string forceAnimation;

    void Start()
    {
        this.animQueue = new List<AnimPriority>();
    }

    void Update()
    {
        var validAnim = this.animQueue.OrderBy(a => a.Priority).FirstOrDefault();
        if (validAnim != null)
        {
            if (!this.animator.GetCurrentAnimatorClipInfo(0).Any(c => c.clip.name == forceAnimation))
            {
                this.animator.Play(validAnim.Name);
            }
        }
        this.animQueue.Clear();
    }

    public void QueueAnimation(string animation)
    {
        AnimPriority priority = animationPriority.FirstOrDefault(anim => anim.Name == animation);
        if (priority == null)
        {
            priority = new AnimPriority() { Name = animation, Priority = int.MaxValue };
        }
        this.animQueue.Add(priority);
    }

    public void QueueAnimationWaitToFinish(string animation)
    {
        QueueAnimation(animation);
        forceAnimation = animation;
    }
}