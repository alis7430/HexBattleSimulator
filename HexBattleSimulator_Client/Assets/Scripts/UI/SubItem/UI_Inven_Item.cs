using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Inven_Item : UI_Base
{
    string _name;

    enum GameObjects
    {
        ItemIcon,
        TxtName,
    }
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
        GameObject txtName = Get<GameObject>((int)GameObjects.TxtName);
        txtName.GetComponent<Text>().text = _name;

        Get<GameObject>((int)GameObjects.ItemIcon).AddUIEvent((PointerEventData data) => { Debug.Log($"아이템 클릭: {_name}"); });
    }

    public void SetInfo(string name)
    {
        _name = name;
    }
}
