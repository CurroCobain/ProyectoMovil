using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Networking;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class PortraitController : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public SpriteRenderer posterSprite;
    public AnimationCurve _curve;
    public Vector3 _rotationGoal;
    public float _speed = 0.5f;
    private float _current, _target;
    private NewControls inputSystem;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetImg());
        inputSystem = new NewControls();
    }

    IEnumerator GetImg()
    {
        UnityWebRequest data = UnityWebRequest.Get("https://www.omdbapi.com/?s=glory&apikey=bec0dc5f");
        yield return data.SendWebRequest();

        if (data.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.LogError(data.error);
        }
        else
        {
            Debug.Log(data.downloadHandler.text);
            SearchData mySearch = JsonUtility.FromJson<SearchData>(data.downloadHandler.text);
            Debug.Log(mySearch.Search[0].Poster);
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(mySearch.Search[1].Poster);
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                Texture2D loadedTexture = DownloadHandlerTexture.GetContent(www);
                posterSprite.sprite = Sprite.Create(loadedTexture, new Rect(0f, 0f, loadedTexture.width, loadedTexture.height), Vector2.zero);
                //posterSprite.SetNativeSize();
                inputSystem.UI.Enable();
                inputSystem.UI.Newaction.started += ScreenTouch;
                inputSystem.UI.Newaction.canceled += ScreenRelease;
            }
        }
        yield break;
        
    }

    void ScreenTouch(CallbackContext cb)
    {
        Debug.Log("touch");
    }
    void ScreenTouchAlternative(CallbackContext cb)
    {
        Debug.Log(cb.);
    }
    void ScreenRelease(CallbackContext cb)
    {
        Debug.Log("Release");
    }

    // Update is called once per frame
    void Update()
    {
        _current = Mathf.MoveTowards(_current, 1.0f, _speed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(Quaternion.Euler(Vector3.zero), Quaternion.Euler(_rotationGoal), _curve.Evaluate(Mathf.PingPong(_current, 0.5f)));
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        throw new NotImplementedException();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        throw new NotImplementedException();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        throw new NotImplementedException();
    }
}
[Serializable]
public class SearchData
{
    public List<MovieData> Search;
    public string totalResults;
    public string Response;
}
[Serializable]
public class MovieData
{
    public string Title;
    public string Year;
    public string imdbID;
    public string Type;
    public string Poster;
}