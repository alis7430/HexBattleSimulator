using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Inven : UI_Scene
{
    enum GameObjects
    {
        GridPanel,
    }
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<GameObject>(typeof(GameObjects));
        GameObject gridPanel = Get<GameObject>((int)GameObjects.GridPanel);
        foreach (Transform child in gridPanel.transform)
        {
            Managers.Resource.Destroy(child.gameObject);
        }

        for (int i = 0; i < 8; i++)
        {
            GameObject item = Managers.UI.MakeSubItem<UI_Inven_Item>(parent : gridPanel.transform).gameObject;

            UI_Inven_Item invenItem = item.AddMissingComponent<UI_Inven_Item>();
            invenItem.SetInfo($"집행검{i}번");
        }
    }
}
