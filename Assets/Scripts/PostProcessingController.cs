using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Rendering.Universal;

public class PostProcessingController : MonoBehaviour
{
    [SerializeField] Volume postProcess;

    private FilmGrain filmGrain;
    private static float toggleFilmGrain = 1f;

    private void Awake()
    {
        if (postProcess == null)
            postProcess = GetComponent<Volume>();

        postProcess.profile.TryGet(out filmGrain);

        SetToggleFilmGrain();
    }

    public void ToggleFilmGrain()
    {
        toggleFilmGrain = filmGrain.intensity == 1f ? 0f : 1f;
        filmGrain.intensity.Override(toggleFilmGrain);
    }

    public void SetToggleFilmGrain()
        => filmGrain.intensity.Override(toggleFilmGrain);
}
