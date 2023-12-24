using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StarttoIntro : MonoBehaviour
{
    private Vector3 targetPosition = new Vector3(0f,19f,-16f); // Target camera location
    public float moveDuration = 5.0f; // time
    private Vector3 targetRotationEulerAngles = new Vector3(60f,0f,0f); // Target camera angle

    public void OnClick()
    {
        StartCoroutine(MoveAndRotateCamera());
    }


    private IEnumerator MoveAndRotateCamera()
    {
        Vector3 startPosition = Camera.main.transform.position;
        Quaternion startRotation = Camera.main.transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(targetRotationEulerAngles);

        for (float t = 0; t < 1; t += Time.deltaTime / moveDuration)
        {
            Camera.main.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            if (t >= 0.5f)
            {
                // 插值计算当前旋转
                Camera.main.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, (t - 0.5f) * 2); // t-0.5f 确保从 0 开始，乘以 2 以保持原来的旋转速度
            }
            yield return null;
        }

        Camera.main.transform.position = targetPosition;
        Camera.main.transform.rotation = targetRotation;

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(1);
    }
}
