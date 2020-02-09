using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Dir { XY, ZY, XZ }

public class TextureAutoTiler : MonoBehaviour
{

    [Header("Scales the local copy of the material by the objects scale when the game runs.")]
    [Tooltip("Removes this script when the game starts.")]
    public bool destroyOnStart = false;
    [Tooltip("Sets which object-scale directions should be used.")]
    public Dir scaleDirection;
    [Tooltip("Tweaks the material scale further.")]
    public float additionalScaleFactor = 1f;

    // Start is called before the first frame update
    void Start()
    {
        ScaleTexture();

        if (destroyOnStart)
        {
            Destroy(this);
        }
    }

    private void OnValidate()
    {
        ScaleTexture();
    }

    void ScaleTexture()
    {
        if (Application.IsPlaying(gameObject))
        {
            Renderer renderer = transform.GetComponent<Renderer>();

            switch (scaleDirection)
            {
                case Dir.XY:
                    renderer.material.mainTextureScale = new Vector2(transform.localScale.x * additionalScaleFactor, transform.localScale.y * additionalScaleFactor);
                    break;
                case Dir.ZY:
                    renderer.material.mainTextureScale = new Vector2(transform.localScale.z * additionalScaleFactor, transform.localScale.y * additionalScaleFactor);
                    break;
                case Dir.XZ:
                    renderer.material.mainTextureScale = new Vector2(transform.localScale.x * additionalScaleFactor, transform.localScale.z * additionalScaleFactor);
                    break;
            }

            //Debug.Log("Rescaled texture on object");
        }
    }
}
