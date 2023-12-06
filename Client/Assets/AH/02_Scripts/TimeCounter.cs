using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH {
    public class TimeCounter : MonoBehaviour {
        public void CountDown(Label countText) {
            int startCount = Random.Range(3, 11);

            countText.text = startCount.ToString(); // 시작값을 바꾸고
            StartCoroutine(RoutineCountDown(countText, startCount));
        }
        IEnumerator RoutineCountDown(Label countText, int time) {
            int count = 4;
            while(count > 0) {
                Debug.Log(count);
                countText.text = time.ToString();
                /*if(time <= 0){
                    break;
                } */
                count--;
                time--;
                yield return new WaitForSeconds(1);
            }
        }
    }
}