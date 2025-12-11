using System.Collections;
using UnityEngine;

public class CoroutineExample1 : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Test1();
            StartCoroutine(Test2());
            // 코루틴 : 협력 + 루틴의 합성어 : 협력 동작
            // 1초 시간이 걸린다. (렉, 프리징, 블로킹)
            // -> 코드를 '동기'적으로 실행하는 게 아니라 '비동기'적으로 실행하고 싶다.
            // 동기 : 이전 코드가 실행 완료된 다음에 그 다음 코드를 실행하는 것
            // 비동기 : 이전 코드가 실행 완료 여부와 상관 없이 다음 코드를 실행하는 것
            // 유니티에서는 '비동기' 방식을 지원하기 위해 코루틴이라는 기능을 제공한다.
            Test3();
        }

    }

    private void Test1()
    {
        Debug.Log("Test1");
    }

    private IEnumerator Test2()
    {
        int sum = 0;
        for (int i = 0; i < 1000; i++)
        {
            for (int j = 0; j < 100; j++)
            {
                sum += (i * j);
            }
        }
        yield return null;
        Debug.Log("Test2");
    }

    private void Test3()
    {
        Debug.Log("Test3");
    }
}
