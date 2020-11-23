using Excursion360_Builder.Shared.States.Items.Field;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class fieldItemContent : Marker
{
    private FieldItem fieldItem;

    public GameObject imageSource;
    public GameObject videoSource;

    public override string Title => "";

    public void Init(FieldItem fieldItem)
    {
        this.fieldItem = fieldItem;

        GetComponentInChildren<Canvas>().worldCamera = Camera.current;

        videoSource.SetActive(false);
        imageSource.SetActive(false);

        switch (fieldItem.contentType)
        {
            case FieldItem.ContentType.Photo:
                StartCoroutine(CreatePhoto());
                break;

            case FieldItem.ContentType.Video:
                StartCoroutine(CreateVideo());
                break;
        }
    }

    private IEnumerator CreatePhoto()
    {
        videoSource.gameObject.SetActive(false);
        imageSource.gameObject.SetActive(true);

        FileImageSource textureSource = imageSource.GetComponent<FileImageSource>();
        textureSource.texture = fieldItem.texture;
        yield return StartCoroutine(textureSource.LoadTexture());

        var rawImage = GetComponentInChildren<RawImage>();
        rawImage.texture = textureSource.LoadedTexture;
        imageSource.GetComponent<RectTransform>().sizeDelta = new Vector2(textureSource.LoadedTexture.width, textureSource.LoadedTexture.height);

        var collider = gameObject.AddComponent<BoxCollider>();
        collider.size = new Vector3(textureSource.LoadedTexture.width * 0.001f, textureSource.LoadedTexture.height * 0.001f);
    }

    private IEnumerator CreateVideo()
    {
        imageSource.gameObject.SetActive(false);
        videoSource.gameObject.SetActive(true);

        FileVideoSource textureSource = videoSource.GetComponent<FileVideoSource>();
        textureSource.videoClip = fieldItem.videoClip;
        yield return StartCoroutine(textureSource.LoadTexture());

        var videoPlayer = GetComponentInChildren<VideoPlayer>();
        videoPlayer.clip = textureSource.videoClip;
        videoSource.GetComponent<RectTransform>().sizeDelta = new Vector2(textureSource.videoClip.width, textureSource.videoClip.height);
        RenderTexture texture = new RenderTexture((int) textureSource.videoClip.width, (int) textureSource.videoClip.height, 16);
        videoSource.GetComponent<RawImage>().texture = texture;
        videoPlayer.targetTexture = texture;

        var collider = gameObject.AddComponent<BoxCollider>();
        collider.size = new Vector3(textureSource.videoClip.width * 0.001f, textureSource.videoClip.height * 0.001f);

        videoPlayer.Play();
    }

    public override void HandleInteract()
    {
        Destroy(gameObject);
    }
}
