using DH;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH {
    public class TimeCounter : MonoBehaviour {
        IngameUIToolkit ingameToolkit;
        [SerializeField] private int runningTime;
        public int startCount = 3;

        private void Awake() {
            ingameToolkit = GetComponent<IngameUIToolkit>();
        }

        public void CountDown(Label countText, Action callback = null) {
            int startCount = UnityEngine.Random.Range(3, 11);

            countText.text = startCount.ToString(); // 시작값을 바꾸고
            StartCoroutine(RoutineCountDown(countText, startCount, 3, "", callback));
        }

        public void ResurrectionCountDown(Label countText, Action callback = null) {
            //countText.text = $"{startCount}초 뒤 부활"; // 시작값을 바꾸고
            Debug.Log(countText);
            Debug.Log(startCount);
            StartCoroutine(RoutineCountDown(countText, startCount, 3, "초 뒤 부활"));
        }
        public void InGameCountDown(Label countText) {
            int minutes = Mathf.FloorToInt(runningTime / 60);
            int seconds = Mathf.FloorToInt(runningTime % 60);
            countText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            StartCoroutine(RoutineInGameCountDown(countText, runningTime));
        }
        IEnumerator RoutineInGameCountDown(Label countText, float runningTime) {
            while(runningTime > 0) { // 기본값 180
                runningTime -= 1;

                int minutes = Mathf.FloorToInt(runningTime / 60);
                int seconds = Mathf.FloorToInt(runningTime % 60);
                countText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

                if(runningTime <= 3) {
                    SoundManager.Instance.Play("Effect/FinishTime");
                }

                yield return new WaitForSeconds(1f);
            }
            ingameToolkit.GameOver();
        }
        IEnumerator RoutineCountDown(Label countText, int time, int loopTime, string plusText = "", Action callback = null) {
            while(loopTime > 0) {
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