using DH;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH
{
    public class TimeCounter : MonoBehaviour
    {
        IngameUIToolkit ingameToolkit;
        private int runningTime => NetworkGameManager.Instance.currentTime.Value;

        private void Awake()
        {
            ingameToolkit = GetComponent<IngameUIToolkit>();
        }

        public void CountDown(Label countText, Action callback = null)
        {
            int startCount = 3;

            countText.text = startCount.ToString(); // 시작값을 바꾸고
            StartCoroutine(RoutineCountDown(countText, startCount, 3, "", callback));
        }

        public int startCount = 3;
        public void ResurrectionCountDown(Label countText, Action callback = null)
        {
            countText.text = $"{startCount}초 뒤 부활"; // 시작값을 바꾸고
            StartCoroutine(RoutineCountDown(countText, startCount, 3, "초 뒤 부활", callback));
        }

        public void PlayTimeCountDown(Label countText)
        {
            if (runningTime <= 0) return;

            int minutes = Mathf.FloorToInt(runningTime / 60);
            int seconds = Mathf.FloorToInt(runningTime % 60);
            countText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            StartCoroutine(RoutinePlayCountDown(countText));
        }

        IEnumerator RoutinePlayCountDown(Label countText)
        {
            while (runningTime > 0)
            {
                int minutes = Mathf.FloorToInt(runningTime / 60);
                int seconds = Mathf.FloorToInt(runningTime % 60);
                countText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

                if (runningTime <= 3)
                {
                    SoundManager.Instance.Play("Effect/FinishTime");
                    yield return new WaitForSeconds(1f);
                }

                yield return null;
            }

            ingameToolkit.GameOver();
        }

        IEnumerator RoutineCountDown(Label countText, int time, int loopTime, string plusText = "", Action callback = null)
        {
            while (loopTime > 0)
            {
                countText.text = $"{time}{plusText}";

                loopTime--;
                time--;
                SoundManager.Instance.Play("Effect/CountDown");
                yield return new WaitForSeconds(1);
            }
            ingameToolkit.FinishCountDown();
            callback?.Invoke();
        }
    }
}