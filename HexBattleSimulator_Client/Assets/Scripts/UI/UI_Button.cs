using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Button : UI_Popup
{
    enum Buttons
    {
        PointButton,
    }

    enum Texts
    {
        PointText,
        ScoreText,
    }

    enum GameObjects
    {
        TestObject,
    }

    enum Images
    {
        ItemIcon,
    }

    int _score;

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));

        GetButton((int)Buttons.PointButton).gameObject.AddUIEvent(OnButtonClicked);

        GameObject go = GetImage((int)Images.ItemIcon).gameObject;
        Action<PointerEventData> action = (PointerEventData data) => { go.transform.position = data.position; };
        go.AddUIEvent(action, Define.UIEvent.Drag);
    }

    public void OnButtonClicked(PointerEventData data)
    {
        _score += 10;
        GetText((int)Texts.ScoreText).text = $"점수 : {_score}";
    }

}
