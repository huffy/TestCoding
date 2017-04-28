
//功能：
//创建者: 胡海辉
//创建时间：


using Assets.Script.Base;
using Assets.Script.Tools;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.AudioMgr
{

    public class AudioControl : TSingleton<AudioControl>, IDisposable
    {

        private struct AudioParam
        {
            public AudioSource mAudioSource;
            public bool bMute;
        }

        #region menber
        private Dictionary<int, AudioClip> mAudioDic = new Dictionary<int, AudioClip>();
        private AudioSource[] mAudioSourceArr;
        private AudioSource[] allAudioSource;
        private List<AudioParam> AudioParamList = new List<AudioParam>();
        private bool mMute = false;
        private float tempVolume = 0;
        //private AudioListener mAudioListener;
        #endregion

        #region Init
        public override void Init()
        {
            base.Init();
            GetSceneAudioSoure();
            audioBGMVolume = mAudioSourceArr[0].volume;
            audioVolume = mAudioSourceArr[1].volume;
            tempVolume = audioVolume;
            InitSoundResource();
        }

        public override void Dispose()
        {
            base.Dispose();
            if (mAudioDic != null) mAudioDic.Clear();
            mAudioDic = null;
            if (AudioParamList != null) AudioParamList.Clear();
            AudioParamList = null;
        }

        public override void Update(float time)
        {
            base.Update(time);
            if (mMute == false)
            {
                if (tempVolume < audioVolume)
                {
                    tempVolume += Time.deltaTime * time;
                    SetTempVolume();
                }
            }
            else
            {
                if (tempVolume > 0)
                {
                    tempVolume -= Time.deltaTime * time;
                    SetTempVolume();
                }
            }
        }

        private void InitSoundResource()
        {
            string filePath = "common";
            LoadAudioData((int)SoundEnum.PickUpTrash, filePath, "10_General_pickup an item");
            LoadAudioData((int)SoundEnum.EndMagicParticle, filePath, "13_General_magicsound");
            filePath = "minigames/minigame18recycletrash";
            LoadAudioData((int)SoundEnum.TrashintheBinFood, filePath, "75_Recycling_TrashintheBin food");
            LoadAudioData((int)SoundEnum.TrashintheBinGlass, filePath, "75_Recycling_TrashintheBin glass");
            LoadAudioData((int)SoundEnum.TrashintheBinPaper, filePath, "75_Recycling_TrashintheBin paper");
            LoadAudioData((int)SoundEnum.TrashintheBinPlastics, filePath, "75_Recycling_TrashintheBin plastics");
            LoadAudioData((int)SoundEnum.Trashbinmoving, filePath, "77A_Recycling_Trashbinmoving");
            LoadAudioData((int)SoundEnum.OpenBin, filePath, "76_Recycling_OpenBin");
            LoadAudioData((int)SoundEnum.CloseBin, filePath, "77_Recycling_CloseBin");
        }

        private void SetTempVolume()
        {
            for (int i = 0; i < AudioParamList.Count; i++)
            {
                if (AudioParamList[i].mAudioSource == null || AudioParamList[i].mAudioSource.clip == null)
                    continue;
                if (AudioParamList[i].bMute == false)
                    continue;

                allAudioSource[i].volume = tempVolume;
            }
        }

        #endregion

        #region  public

        /// <summary>
        /// 音效音量
        /// </summary>
        public float audioVolume;

        /// <summary>
        /// 背景音量
        /// </summary>
        public float audioBGMVolume;



        private float mAllVolume = 1;
        /// <summary>
        /// 全局音量
        /// </summary>
        public float allVolume
        {
            get { return mAllVolume; }
            set
            {

                mAllVolume = value;
                AudioListener.volume = mAllVolume;
            }
        }

        /// <summary>
        /// 播放音效
        /// </summary>
        public void PlayAudio(int ID, bool loop = false)
        {
            PlayAudioDataByID(false, ID, loop);
        }

        /// <summary>
        /// 播放背景音乐
        /// </summary>
        public void PlayBGMAudio(int ID)
        {
            PlayAudioDataByID(true, ID, true);
        }

        /// <summary>
        /// 播放特定声音源的音效
        /// </summary>
        /// <param name="mAudioSource"></param>
        /// <param name="ID"></param>
        /// <param name="loop"></param>
        public void PlayAudioBySource(AudioSource mAudioSource, int ID, bool bLoop)
        {
            if (mAudioSource == null)
            {
                Debug.LogError("mAudioSource is null");
                return;
            }
            AudioClip mAudioClip = GetAudioClipByID(ID);
            if (mAudioClip == null)
            {
                DebugHelper.DebugLogError("PlayAudioBySource  Invaild ID === " + ID);
            }
            SetAudioScoure(ref mAudioSource, mAudioClip, audioVolume, bLoop);
        }

        /// <summary>
        /// 停止某个音效
        /// </summary>
        /// <param name="ID"></param>
        public void StopAudioByID(int ID)
        {
            if (mAudioDic.ContainsKey(ID) == false) return;
            for (int i = 1; i < mAudioSourceArr.Length; i++)
            {
                if (mAudioSourceArr[i].clip == mAudioDic[ID])
                {
                    mAudioSourceArr[i].Stop();
                    return;
                }
            }
        }

        /// <summary>
        /// 停止全部音效
        /// </summary>
        /// <param name="ID"></param>
        public void StopAllAudio()
        {
            for (int i = 1; i < mAudioSourceArr.Length; i++)
            {
                if (mAudioSourceArr[i].isPlaying)
                {
                    mAudioSourceArr[i].Stop();
                }
            }
        }

        /// <summary>
        /// 得到音频长度
        /// </summary>
        /// <param name="ID"></param>
        public float GetAudioLength(int ID)
        {
            AudioClip clip = GetAudioClipByID(ID);
            if (clip != null)
            {
                return clip.length;
            }
            else
            {
                DebugHelper.DebugLogError("GetAudioLength  Invaild ID === " + ID);
                return 0;
            }
        }

        /// <summary>
        /// 所有声音静音
        /// </summary>
        public void MuteAllAudio(bool bMute, bool needFindAll = true)
        {
            mMute = bMute;
            if (bMute)
            {
                tempVolume = audioVolume;
            }

            if (needFindAll && mMute == true)
            {
                allAudioSource = GameObject.FindObjectsOfType<AudioSource>();
                AudioParamList.Clear();
                for (int i = 0; i < allAudioSource.Length; i++)
                {
                    AudioParam param;
                    param.mAudioSource = allAudioSource[i];
                    param.bMute = true;
                    AudioParamList.Add(param);
                }
            }
        }


        #endregion

        #region  privte

        /// <summary>
        /// 通过ID播放相应的音效
        /// </summary>
        private void PlayAudioDataByID(bool isBGM, int ID, bool bLoop)
        {
            AudioSource mAudioSource = GetAudioSoure(isBGM);
            float volume = 1;
            if (mAudioSource == null) return;
            AudioClip mAudioClip = GetAudioClipByID(ID);
            if (mAudioClip == null)
            {
                DebugHelper.DebugLogError("PlayAudioDataByID  Invaild ID === " + ID);
            }
            volume = isBGM ? audioBGMVolume : audioVolume;
            SetAudioScoure(ref mAudioSource, mAudioClip, volume, bLoop);
        }

        /// <summary>
        /// 设置AuidoScoure属性
        /// </summary>
        private void SetAudioScoure(ref AudioSource mAudioSource, AudioClip mAudioClip, float volume, bool bLoop)
        {
            mAudioSource.loop = bLoop;
            SetAudioScoure(ref mAudioSource, mAudioClip, volume);
        }
        private void SetAudioScoure(ref AudioSource mAudioSource, AudioClip mAudioClip, float volume)
        {
            mAudioSource.volume = volume;
            tempVolume = volume;
            SetAudioScoure(ref mAudioSource, mAudioClip);
        }
        private void SetAudioScoure(ref AudioSource mAudioSource, AudioClip mAudioClip)
        {
            mAudioSource.clip = mAudioClip;

            for (int i = 0; i < AudioParamList.Count; i++)
            {
                if (AudioParamList[i].mAudioSource == mAudioSource)
                {
                    AudioParam param;
                    param.mAudioSource = mAudioSource;
                    param.bMute = false;
                    AudioParamList[i] = param;
                    break;
                }
            }
            mAudioSource.mute = false;
            mAudioSource.Play();
        }

        /// <summary>
        /// 通过ID获取音效资源
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        private AudioClip GetAudioClipByID(int ID)
        {
            if (mAudioDic.ContainsKey(ID) == false)
            {
                return null;
            }

            return mAudioDic[ID];
        }


        /// <summary>
        /// 加载音效
        /// </summary>
        private void LoadAudioData(int ID, string fileName, string soundName)
        {
            string pathStr = string.Format(StaticMemberMgr.SOUND_RESOURCE_PATH, fileName, soundName);
            AudioClip clip = (AudioClip)Resources.Load(pathStr, typeof(AudioClip));
            mAudioDic.Add(ID, clip);
        }

        /// <summary>
        /// 获取场景中audioSoure
        /// </summary>
        private void GetSceneAudioSoure()
        {
            GameObject soundObj = GameObject.Find(StaticMemberMgr.GAME_SOUND_OBJ_NAME);
            if (soundObj == null)
            {
                Debug.LogError("SoundObj  name is  wrong");
            }
            mAudioSourceArr = soundObj.GetComponents<AudioSource>();
            //Debug.Log("scene audioSource num is " + mAudioSourceArr.Length);
            allAudioSource = GameObject.FindObjectsOfType<AudioSource>();
        }

        /// <summary>
        /// 获取声音源
        /// </summary>
        private AudioSource GetAudioSoure(bool isBGM)
        {
            //if (isBGM)
            //{
            //    return mAudioSourceArr[0];
            //}
            //else
            {
                for (int i = 0; i < mAudioSourceArr.Length; i++)
                {
                    if (mAudioSourceArr[i].isPlaying == false)
                    {
                        return mAudioSourceArr[i];
                    }
                }
            }
            return null;
        }
        #endregion
    }
}
