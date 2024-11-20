using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public abstract class UIBase : InitBase
{

    protected Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();

    [SerializeField] protected float power = 0.1f;
    [SerializeField] protected float duration = 0.2f;

    [SerializeField] private Transform animationRoot;

    private void Awake()
    {
        Init();
    }

    public virtual void Open()
    {
        Open(Defines.UIAnimationType.None);
    }

    public virtual void Open(Defines.UIAnimationType type)
    {

        if (animationRoot == null)
        {
            return;
        }

        switch (type)
        {
            case Defines.UIAnimationType.Bounce:
                animationRoot
                    .DOScale(Vector3.one, 0.3f)
                    .ChangeStartValue(Vector3.zero)
                    .SetEase(Ease.OutBack);
                break;

            default:
              
                break;
        }

    }

    public virtual void Close()
    {
        Close(Defines.UIAnimationType.None);
    }


    public virtual void Close(Defines.UIAnimationType type)
    {


        if (animationRoot == null)
        {
        
            return;
        }

        switch (type)
        {
            case Defines.UIAnimationType.Bounce:
                animationRoot
                    .DOScale(Vector3.zero, 0.3f)
                    .SetEase(Ease.InBack);
                break;

            default:
      
                break;

        }


    }

      protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        string[] names = Enum.GetNames(type);
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        _objects.Add(typeof(T), objects);

        for (int i = 0; i < names.Length; i++)
        {
            if (typeof(T) == typeof(GameObject))
                objects[i] = Utils.FindChild(gameObject, names[i], true);
            else
                objects[i] = Utils.FindChild<T>(gameObject, names[i], true);

            if (objects[i] == null)
                Debug.LogWarning($"Failed to bind({names[i]})");
        }
    }

    protected void BindObject(Type type) { Bind<GameObject>(type); }
    protected void BindImage(Type type) { Bind<Image>(type); }
    protected void BindText(Type type) { Bind<TMP_Text>(type); }
    protected void BindButton(Type type) { Bind<Button>(type); }
    protected void BindToggle(Type type) { Bind<Toggle>(type); }

    protected void BindSlider(Type type) { Bind<Slider>(type); }

    protected T Get<T>(int idx) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;
        if (_objects.TryGetValue(typeof(T), out objects) == false)
            return null;

        return objects[idx] as T;
    }

    protected GameObject GetObject(Enum idx) { return Get<GameObject>((int)(object)idx); }
    protected TMP_Text GetText(Enum idx) { return Get<TMP_Text>((int)(object)idx); }
    protected Button GetButton(Enum idx) { return Get<Button>((int)(object)idx); }
    protected Image GetImage(Enum idx) { return Get<Image>((int)(object)idx); }
    protected Toggle GetToggle(Enum idx) { return Get<Toggle>((int)(object)idx); }

    protected Slider GetSlider(Enum idx) { return Get<Slider>((int)(object)idx); }



    public static void BindEvent(GameObject go, Action action = null, Action<BaseEventData> dragAction = null, Defines.UIEvent type = Defines.UIEvent.Click)
    {
        UIEventHandler evt = Utils.GetOrAddComponent<UIEventHandler>(go);

        switch (type)
        {
            case Defines.UIEvent.Click:
                evt.OnClickHandler -= action;
                evt.OnClickHandler += action;
                break;
            case Defines.UIEvent.Pressed:
                evt.OnPressedHandler -= action;
                evt.OnPressedHandler += action;
                break;
            case Defines.UIEvent.PointerDown:
                evt.OnPointerDownHandler -= action;
                evt.OnPointerDownHandler += action;
                break;
            case Defines.UIEvent.PointerUp:
                evt.OnPointerUpHandler -= action;
                evt.OnPointerUpHandler += action;
                break;
            case Defines.UIEvent.Drag:
                evt.OnDragHandler -= dragAction;
                evt.OnDragHandler += dragAction;
                break;
            case Defines.UIEvent.BeginDrag:
                evt.OnBeginDragHandler -= dragAction;
                evt.OnBeginDragHandler += dragAction;
                break;
            case Defines.UIEvent.EndDrag:
                evt.OnEndDragHandler -= dragAction;
                evt.OnEndDragHandler += dragAction;
                break;
        }
    }
    public static void UnBindEvent(GameObject go, Action action = null, Action<BaseEventData> dragAction = null, Defines.UIEvent type = Defines.UIEvent.Click)
    {
        UIEventHandler evt = go.GetComponent<UIEventHandler>();
        if (evt == null) return;
        
        switch (type)
        {
            case Defines.UIEvent.Click:
                evt.OnClickHandler -= action;
                break;
            case Defines.UIEvent.Pressed:
                evt.OnPressedHandler -= action;
                break;
            case Defines.UIEvent.PointerDown:
                evt.OnPointerDownHandler -= action;
                break;
            case Defines.UIEvent.PointerUp:
                evt.OnPointerUpHandler -= action;
                break;
            case Defines.UIEvent.Drag:
                evt.OnDragHandler -= dragAction;
                break;
            case Defines.UIEvent.BeginDrag:
                evt.OnBeginDragHandler -= dragAction;
                break;
            case Defines.UIEvent.EndDrag:
                evt.OnEndDragHandler -= dragAction;
                break;
        }
    }

 
}
