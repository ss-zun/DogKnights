using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTarget : MonoBehaviour
{
    // ===== public =====
    [Tooltip("화살표 위치 설정")]
    public Vector3 arrowPosition = Vector3.zero;

    [Tooltip("화살표 방향 설정")]
    public Vector3 arrowDirection = Vector3.forward;

    [Tooltip("화살표 길이 설정")]
    public float arrowLength = 1.0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDrawGizmos()
    {
        // 화살표 방향
        Vector3 direction = transform.forward;
        // 화살표 시작점과 끝점 계산
        Vector3 arrowStart = transform.position + arrowPosition;
        Vector3 arrowEnd = transform.position + arrowPosition + direction.normalized * arrowLength;

        // 화살표 선 그리기
        Gizmos.color = Color.red; // 화살표 색상 설정
        Gizmos.DrawLine(arrowStart, arrowEnd);

        // 화살표 머리 그리기
        Vector3 arrowHead = arrowEnd - direction.normalized * 0.1f;
        // Gizmos.DrawLine(arrowEnd, arrowHead);
        Gizmos.DrawLine(arrowEnd, arrowEnd + Quaternion.Euler(direction.x, direction.y - 30, 0) * -direction * 0.05f);
        Gizmos.DrawLine(arrowEnd, arrowEnd + Quaternion.Euler(direction.x, direction.y + 30, 0) * -direction * 0.05f);
        // * Vector3.back * 0.05f
    }

    
}
