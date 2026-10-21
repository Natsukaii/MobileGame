using System.Collections.Generic;
using UnityEngine;

public class VfxAnimator : MonoBehaviour
{
    [System.Serializable]
    class VfxAnimation
    {
        public string name;
        [Tooltip("frame per second")] public float framerate = 20f;
        public List<Sprite> frames = new List<Sprite>();
    }
    
    [SerializeField] private List<VfxAnimation> animations = new List<VfxAnimation>();
    [SerializeField] private SpriteRenderer spriteRenderer;
    private VfxAnimation animchose;
    private bool isPlaying = false;
    private int currentFrameIndex = 0;
    private float timeSinceLastFrame = 0f;
    
    void Update()
    {
        if (isPlaying)
        {
            timeSinceLastFrame += Time.deltaTime;
            if (timeSinceLastFrame >= 1f / animchose.framerate)
            {
                currentFrameIndex = (currentFrameIndex + 1) % animchose.frames.Count;
                timeSinceLastFrame = 0f;
                spriteRenderer.sprite = animchose.frames[currentFrameIndex];
            }
            if (currentFrameIndex == animchose.frames.Count - 1)
            {
                isPlaying = false;
            }
        }
    }
    
    public void PlayAnimation(string animationName)
    {
        animchose = animations.Find(x => x.name == animationName);
        if (animchose != null)
        {
            isPlaying = true;
            currentFrameIndex = 0;
            timeSinceLastFrame = 0f;
        }
    }
}
