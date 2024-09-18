using UnityEngine;

public class SplitBlockAnimationManager : MonoBehaviour
{
    // ���R���|�[�l���g�擾
    private SplitManager splitManager;
    private AudioSource audioSource;

    [SerializeField] private Sprite[] sprites;
    [SerializeField] private SpriteRenderer overRenderer = null;

    bool isAvoid = false;

    void Start()
    {
        splitManager = GetComponent<SplitManager>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        bool preAvoid = isAvoid;
        isAvoid = splitManager.GetAVoid();

        if (isAvoid != preAvoid)
        {
            if (isAvoid == true)
            {
                overRenderer.sprite = sprites[1];
                audioSource.Play();
            }
            else
            {
                overRenderer.sprite = sprites[0];
            }
        }
    }
}
