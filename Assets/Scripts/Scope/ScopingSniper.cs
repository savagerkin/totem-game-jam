using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class ScopingSniper : MonoBehaviour
{
    private Volume volume;
    private DepthOfField depthOfField;
    private bool isScoped = false;
    [SerializeField] private Camera camera;

    void Start()
    {
        isScoped = false;
        volume = camera.GetComponent<Volume>();
        if (volume.profile.TryGet(out DepthOfField dof))
        {
            depthOfField = dof;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            depthOfField.active = true;
            
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            depthOfField.active = false;
            
        }
    }
}