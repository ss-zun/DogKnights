using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeRoomFloorButton : MonoBehaviour
{
    // ===== public =====

    [Tooltip("버튼의 Material 정보를 담는 객체")]
    public Material buttonMaterial;

    [Tooltip("플레이어와 반응했을 때, 변경되는 색상")]
    public Color InteractToColor;

    // ===== private =====
    private MeshRenderer renderer;

    // Start is called before the first frame update
    void Start()
    {

        renderer = gameObject.GetComponent<MeshRenderer>();

        // renderer.material.color = InteractToColor;

        // renderer.material.SetColor("_EmissionColor", InteractToColor);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Box Collider와 Player가 충돌했을 때만 작동되도록 구현.
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag.Equals("Player"))
        {
            // Albedo와 Emissive Color를 동시에 변경.
            renderer.material.color = InteractToColor;

            renderer.material.SetColor("_EmissionColor", InteractToColor);
        }
    }
}
