using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    private Slider volumeSlider;

    private void Start()
    {
        volumeSlider = GetComponent<Slider>();
        volumeSlider.value = AudioListener.volume;
    }

    public void ChangeVolume()
    {
        SoundManager.Instance.ChangeMasterVolume(volumeSlider.value);
    }
}
