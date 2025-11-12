using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Rendering.Universal;

public class PostProcessingController : MonoBehaviour
{
    [SerializeField] Volume postProcess;

    private FilmGrain filmGrain;

    private void Awake()
    {
        if (postProcess == null)
            postProcess = GetComponent<Volume>();

        postProcess.profile.TryGet(out filmGrain);
    }

    public void FilmGrain(bool value)
        => filmGrain.active = value;
}
