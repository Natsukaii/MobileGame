using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VfxAnimatorUI : MonoBehaviour
{
    [System.Serializable]
    class VfxAnimationUI
    {
        public string name;
        [Tooltip("frame per second")] public float framerate = 20f;
        public List<Sprite> frames = new List<Sprite>();
    }
    
    [SerializeField] private List<VfxAnimationUI> animations = new List<VfxAnimationUI>();
    [SerializeField] private Image image;
    private VfxAnimationUI animchose;
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
                image.sprite = animchose.frames[currentFrameIndex];
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
    
    public void SetSprite(Sprite sprite)
    {
        image.sprite = sprite;
    }

}
