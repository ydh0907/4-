using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH {
    public class TimeCounter : MonoBehaviour {
        IngameUIToolkit ingameToolkit;

        private void Awake() {
            ingameToolkit = GetComponent<IngameUIToolkit>();
        }

        public void CountDown(Label countText) {
            int startCount = Random.Range(3, 11);

            countText.text = startCount.ToString(); // ���۰��� �ٲٰ�
            StartCoroutine(RoutineCountDown(countText, startCount, 4));
        }
        public void ResurrectionCountDown(Label countText) {
            int startCount = 3;

            countText.text = $"{startCount}�� �� ��Ȱ"; // ���۰��� �ٲٰ�
            StartCoroutine(RoutineCountDown(countText, startCount, 4, "�� �� ��Ȱ"));
        }

        IEnumerator RoutineCountDown(Label countText, int time, int loopTime, string plusText = "") {
            while(loopTime > 0) {
                //Debug.Log(loopTime);
                countText.text = $"{time}{plusText}";

                loopTime--;
                time--;
                yield return new WaitForSeconds(1);
            }
            ingameToolkit.FinishCountDown();
        }
    }
}