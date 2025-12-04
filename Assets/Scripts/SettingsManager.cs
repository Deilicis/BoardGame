using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class SettingsManager : MonoBehaviour
{
    [Header("UI")]
    public TMP_Dropdown resolutionDropdown;   // assign your TMP dropdown in the Inspector
    public Slider bgmVolumeSlider;            // assign a UI Slider in the Inspector
    public Slider sfxVolumeSlider;            // assign a UI Slider for sound effects

    [Header("Optional - assign to avoid auto-finding")]
    public AudioSource bgmAudioSource;        // optional: assign your background music AudioSource

    // Preset resolutions shown in the dropdown
    readonly Vector2Int[] presetResolutions = new[]
    {
        new Vector2Int(1920,1080),
        new Vector2Int(1600,900),
        new Vector2Int(1366,768),
        new Vector2Int(1280,720),
        new Vector2Int(1024,768)
    };

    List<Vector2Int> resolutions = new List<Vector2Int>();

    // Collected AudioSources used by SoundEffectsScript
    AudioSource[] sfxAudioSources = new AudioSource[0];

    void Start()
    {
        // Fill resolution list with presets, ensure current resolution is present
        resolutions.AddRange(presetResolutions);
        Vector2Int current = new Vector2Int(Screen.width, Screen.height);
        if (!resolutions.Contains(current))
            resolutions.Insert(0, current);

        PopulateResolutionDropdown();

        // Auto-find BGM audio source if not assigned
        if (bgmAudioSource == null)
            bgmAudioSource = FindBgmAudioSource();

        // Auto-find SFX audio sources
        FindSfxAudioSources();

        // Load saved settings
        float savedBgmVolume = PlayerPrefs.GetFloat("BGMVolume", bgmAudioSource != null ? bgmAudioSource.volume : 1f);
        float savedSfxVolume = PlayerPrefs.GetFloat("SFXVolume", GetCurrentSfxVolume());
        int savedResIndex = PlayerPrefs.GetInt("ResolutionIndex", GetCurrentResolutionIndex());

        // Apply BGM volume
        if (bgmVolumeSlider != null)
        {
            bgmVolumeSlider.value = savedBgmVolume;
            bgmVolumeSlider.onValueChanged.RemoveAllListeners();
            bgmVolumeSlider.onValueChanged.AddListener(SetBgmVolume);
        }
        SetBgmVolume(savedBgmVolume);

        // Apply SFX volume
        if (sfxVolumeSlider != null)
        {
            sfxVolumeSlider.value = savedSfxVolume;
            sfxVolumeSlider.onValueChanged.RemoveAllListeners();
            sfxVolumeSlider.onValueChanged.AddListener(SetSfxVolume);
        }
        SetSfxVolume(savedSfxVolume);

        // Apply resolution
        if (resolutionDropdown != null)
        {
            resolutionDropdown.onValueChanged.RemoveAllListeners();
            resolutionDropdown.onValueChanged.AddListener(OnResolutionSelected);
            resolutionDropdown.value = Mathf.Clamp(savedResIndex, 0, resolutions.Count - 1);
        }
        ApplyResolutionIndex(savedResIndex);
    }

    void PopulateResolutionDropdown()
    {
        if (resolutionDropdown == null) return;

        resolutionDropdown.ClearOptions();
        var options = new List<string>();
        for (int i = 0; i < resolutions.Count; i++)
        {
            var r = resolutions[i];
            options.Add($"{r.x} x {r.y}");
        }
        resolutionDropdown.AddOptions(options);
    }

    AudioSource FindBgmAudioSource()
    {
        // First try a GameObject tagged "Music"
        try
        {
            var tagged = GameObject.FindGameObjectWithTag("Music");
            if (tagged != null)
            {
                var s = tagged.GetComponent<AudioSource>();
                if (s != null) return s;
            }
        }
        catch { /* Tag might not exist, ignore */ }

        // Next try to find a looping AudioSource (likely background music)
        var all = FindObjectsOfType<AudioSource>();
        foreach (var a in all)
        {
            if (a.loop) return a;
        }

        // Fallback to first AudioSource in scene
        if (all.Length > 0) return all[0];

        Debug.LogWarning("SettingsManager: No AudioSource found for background music. Assign one to the inspector or tag it 'Music'.");
        return null;
    }

    void FindSfxAudioSources()
    {
        var effects = FindObjectsOfType<SoundEffectsScript>();
        var list = new List<AudioSource>();
        foreach (var e in effects)
        {
            if (e == null) continue;
            if (e.audioSource != null) list.Add(e.audioSource);
        }
        sfxAudioSources = list.ToArray();
        if (sfxAudioSources.Length == 0)
        {
            // As a fallback, include non-looping AudioSources in the scene
            var all = FindObjectsOfType<AudioSource>();
            var fallback = new List<AudioSource>();
            foreach (var a in all)
            {
                if (!a.loop) fallback.Add(a);
            }
            sfxAudioSources = fallback.ToArray();
        }

        if (sfxAudioSources.Length == 0)
            Debug.LogWarning("SettingsManager: No SFX AudioSources found. Assign SoundEffectsScript references with audio sources.");
    }

    float GetCurrentSfxVolume()
    {
        if (sfxAudioSources == null || sfxAudioSources.Length == 0) return 1f;
        // return average of found volumes
        float sum = 0f;
        foreach (var a in sfxAudioSources) sum += a.volume;
        return sum / sfxAudioSources.Length;
    }

    int GetCurrentResolutionIndex()
    {
        var cur = new Vector2Int(Screen.width, Screen.height);
        for (int i = 0; i < resolutions.Count; i++)
            if (resolutions[i].Equals(cur)) return i;
        return 0;
    }

    void ApplyResolutionIndex(int index)
    {
        if (index < 0 || index >= resolutions.Count) return;
        var r = resolutions[index];
        bool fullscreen = PlayerPrefs.GetInt("Fullscreen", Screen.fullScreen ? 1 : 0) == 1;
        Screen.SetResolution(r.x, r.y, fullscreen);
        PlayerPrefs.SetInt("ResolutionIndex", index);
        PlayerPrefs.Save();
    }

    // UI callback for TMP Dropdown
    public void OnResolutionSelected(int index)
    {
        ApplyResolutionIndex(index);
    }

    // UI callback for volume slider
    public void SetBgmVolume(float volume)
    {
        if (bgmAudioSource != null)
            bgmAudioSource.volume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat("BGMVolume", Mathf.Clamp01(volume));
        PlayerPrefs.Save();
    }

    // UI callback for SFX volume slider
    public void SetSfxVolume(float volume)
    {
        float clamped = Mathf.Clamp01(volume);
        if (sfxAudioSources != null)
        {
            foreach (var a in sfxAudioSources)
            {
                if (a != null) a.volume = clamped;
            }
        }
        PlayerPrefs.SetFloat("SFXVolume", clamped);
        PlayerPrefs.Save();
    }

    // Optional: call from a Fullscreen toggle to persist fullscreen choice
    public void SetFullscreen(bool fullscreen)
    {
        Screen.fullScreen = fullscreen;
        PlayerPrefs.SetInt("Fullscreen", fullscreen ? 1 : 0);
        PlayerPrefs.Save();
    }
}