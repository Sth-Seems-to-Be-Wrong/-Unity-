using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private static float MaxH = 200;
    private static float MinH = -25;
    private static float MaxV = -18;
    private static float MinV = -25;

    void Awake()
    {
    }

    void Update()
    {
        
    }

    //设置相机边界
    public static void SetLimit(float maxh,float minh,float maxV,float minv)
    {
        MaxH = maxh;
        MinH = minh;
        MaxV = maxV;
        MinV = minv;
    }

    public void MoveByPlayer(Vector3 position)
    {
        //相机跟随应该由玩家控制，在玩家每一帧结束调用
        float tmpx = position.x;
        float tmpy = position.y;
        float x = tmpx < MaxH ? (tmpx > MinH ? tmpx : MinH) : MaxH;
        float y = tmpy < MaxV ? (tmpy > MinV ? tmpy : MinV) : MaxV;
        Vector3 newPosition = new Vector3(x, y, transform.position.z);
        transform.position = newPosition;
    }

    public IEnumerator Shake(float duration, float magnitude)//摇晃时间、幅度
    {
        float elapsed = 0.0f;//摇晃进行时间
        while (elapsed < duration)
        {
            float x = Random.Range(-2f, 2f) * magnitude;//x轴随机抖动幅度
            float y = Random.Range(-2f, 2f) * magnitude;//y轴随机抖动幅度

            transform.position = new Vector3(x+transform.position.x, y+transform.position.y, transform.position.z);

            elapsed += Time.deltaTime;

            yield return null;
        }
    }

}
