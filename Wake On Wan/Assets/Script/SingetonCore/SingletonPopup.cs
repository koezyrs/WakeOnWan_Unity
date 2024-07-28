using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(CanvasScaler))]
[RequireComponent(typeof(GraphicRaycaster))]
public class SingletonPopup<T> : MonoBehaviour where T : Component
{
    private static T _instance;
    private static string _popupGuid = System.Guid.NewGuid().ToString();
    
    public PopupSortOrder popupOrder = PopupSortOrder.Information;
    public Canvas canvas;
    public CanvasScaler canvasScaler;
    public GraphicRaycaster graphicRaycaster;
    
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject prefab = PrefabManager.Instance.GetPrefab(typeof(T).Name);
                if (prefab != null)
                {
                    GameObject go = Instantiate(prefab);
                    _instance = go.GetComponent<T>();
                    _instance.gameObject.SetActive(false);
                }
                else
                {
                    Debug.LogError($"Prefab for {typeof(T).Name} not found.");
                }
            }

            return _instance;
        }
    }

    public static string Guid => _popupGuid;

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            LoadRequireComponent();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public virtual void Show()
    {
        gameObject.SetActive(true);
        // PopupManager.Instance.AddPopup(this as SingletonPopup<Component>);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
        // PopupManager.Instance.RemovePopup(this as SingletonPopup<Component>);
    }

    #region Private
    private void LoadRequireComponent()
    {
        if (canvas == null) canvas = GetComponent<Canvas>();
        if (canvasScaler == null) canvasScaler = GetComponent<CanvasScaler>();
        if (graphicRaycaster == null) graphicRaycaster = GetComponent<GraphicRaycaster>();
    }
    
    private void OnValidate()
    {
        LoadRequireComponent();
    }
    #endregion
}
