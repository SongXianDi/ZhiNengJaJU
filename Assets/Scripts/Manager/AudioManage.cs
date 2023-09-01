using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Internal;
/// <summary>
/// 音乐管理器
/// </summary>
public class AudioManage : MonoBehaviour
{
    private static AudioManage instance;
    public static AudioManage Instance => instance;
    private bool isBoFang;
    //背景音乐播放器
    private AudioSource musicSource;

    //默认播放的背景音乐
    private AudioClip BGClip;
    //默认播放器的背景音乐大小
    private float mousicSize;


    private WaitForSeconds waitTime;
    float time;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            return;
        }

        //给播放器添加组件
        musicSource = this.gameObject.AddComponent<AudioSource>();

    }
    /// <summary>
    /// 播放音乐
    /// </summary>
    /// <param name="path">播放音乐的位置</param>
    /// <param name="size">播放音乐的大小</param>
    public void PlayMusicSource(string path, float size = 1)
    {
        AudioClip clip1 = Resources.Load<AudioClip>("Audio/" + path);
        BGClip = clip1;
        mousicSize = size;
        musicSource.clip = BGClip;
        musicSource.volume = mousicSize;
        musicSource.Play();
    }

    /// <summary>
    /// 声音播放，中间播放动画，完毕回调
    /// </summary>
    /// <param name="path"></param>
    /// <param name="action"></param>
    /// <param name="size"></param>
    public void PlayMusicSource(string path, UnityAction action, float size = 1)
    {
        AudioClip clip1 = Resources.Load<AudioClip>("Audio/" + path);
        BGClip = clip1;
        mousicSize = size;
        musicSource.clip = BGClip;
        musicSource.volume = mousicSize;
        musicSource.Play();
        StartCoroutine(AudioPlayFinished(clip1, action));

    }

    /// <summary>
    /// 声音播放，完毕回调
    /// </summary>
    /// <param name="path"></param>
    /// <param name="action"></param>
    /// <param name="action"></param>
    /// <param name="size"></param>
    public void PlayMusicSourceAnimator(string path, UnityAction animatorPlay, UnityAction action, float size = 1)
    {
        if (path == null)
        {
            musicSource.Stop();
            musicSource.clip = null;
        }
        else
        {
            AudioClip clip1 = Resources.Load<AudioClip>("Audio/" + path);
            BGClip = clip1;
            mousicSize = size;
            musicSource.clip = BGClip;
            musicSource.volume = mousicSize;
            musicSource.Play();
            StartCoroutine(AudioPlayAndAnimatorFinished(clip1, animatorPlay, action));
        }
    }
    /// <summary>
    /// 检测声音播放完毕携程
    /// </summary>
    /// <param name="time">声音时长</param>
    /// <param name="callBack">回调</param>
    /// <returns></returns>
    private IEnumerator AudioPlayFinished(AudioClip clip, UnityAction callBack)
    {
        waitTime = new WaitForSeconds(clip.length);
        yield return waitTime;
        callBack.Invoke();
    }
    private IEnumerator AudioPlayAndAnimatorFinished(AudioClip clip, UnityAction animatorPlay, UnityAction callBack)
    {
        float stopTime = 0.4f;
        waitTime = new WaitForSeconds(clip.length * stopTime);
        yield return waitTime;
        animatorPlay.Invoke();
        waitTime = new WaitForSeconds(clip.length * (1 - stopTime));
        yield return waitTime;
        callBack.Invoke();
    }

}
