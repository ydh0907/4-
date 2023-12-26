using DH;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH {
    public class TimeCounter : MonoBehaviour {
        IngameUIToolkit ingameToolkit;
        [SerializeField] private AudioClip countDown;

        private void Awake() {
            ingameToolkit = GetComponent<IngameUIToolkit>();
        }

        public void CountDown(Label countText, Action callback = null) {
            int startCount = UnityEngine.Random.Range(3, 11);

            countText.text = startCount.ToString(); // 시작값을 바꾸고
            StartCoroutine(RoutineCountDown(countText, startCount, 3, "", callback));
        }
        public void ResurrectionCountDown(Label countText, Action callback = null) {
            int startCount = 3;

            countText.text = $"{startCount}초 뒤 부활"; // 시작값을 바꾸고
            StartCoroutine(RoutineCountDown(countText, startCount, 3, "초 뒤 부활", callback));
        }
        public void PlayTimeCountDown(Label countText) {
            int runningTime = 180;

            int minutes = Mathf.FloorToInt(runningTime / 60);
            int seconds = Mathf.FloorToInt(runningTime % 60);
            countText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            StartCoroutine(RoutinePlayCountDown(countText, runningTime));
        }
        IEnumerator RoutinePlayCountDown(Label countText, float runningTime) {
            while(runningTime > 0) { // 기본값 180
                runningTime -= 1;

                int minutes = Mathf.FloorToInt(runningTime / 60);
                int seconds = Mathf.FloorToInt(runningTime % 60);
                countText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

                yield return new WaitForSeconds(1f);
            }

            NetworkGameManager.Instance.ServerGameEnd();
        }
        IEnumerator RoutineCountDown(Label countText, int time, int loopTime, string plusText = "", Action callback = null) {
            while(loopTime > 0) {
                //Debug.Log(loopTime);
                countText.text = $"{time}{plusText}";

                loopTime--;
                time--;

                SoundManager.Instance.Play(countDown);
                yield return new WaitForSeconds(1);
            }
            ingameToolkit.FinishCountDown();
            callback?.Invoke();
        }
    }
}