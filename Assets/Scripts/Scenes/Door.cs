using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    // 문 피벗 정보.
    public GameObject door_mesh_infor;

    // Start is called before the first frame update
    void Start()
    {
        // GameObject.FindGameObjectWithTag()
        // 씬 내에서 해당 컴포넌트의 태그를 가진 첫 번째 객체를 반환함.

        // GetComponents는 MonoBehavior나 Component 클래스를 상속받을 때 사용할 수 있음. 그러나, GameObject는 두 클래스를 상속받지 않으므로 사용할 수 없음.

        string tag_name = "";

        //foreach(GameObject temp_object in GetComponents<GameObject>())
        //{
        //    tag_name = temp_object.tag;

        //    if(tag_name.Equals("Pivot"))
        //    {
        //        door_mesh_infor = temp_object;
        //        Debug.Log(temp_object);
        //        break;
                
        //    }
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
