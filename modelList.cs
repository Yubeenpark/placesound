using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class modelList : MonoBehaviour
{
    ScrollRect scrollRect;
    // Start is called before the first frame update
    void Start()
    {
        // ��ũ�� ��� ���õ� ������ �ϱ� ���� ������ �ִ� ����

        scrollRect = GetComponent<ScrollRect>();    // ���� ������Ʈ�� ������ �ִ� ScrollRect�� �����´�.

    }

    // Update is called once per frame
    void Update()
    {

    }

    void SetContentSize()
    {
        float width = 100.0f;
        float height = 100.0f;
        // scrollRect.content�� ���ؼ� Hierachy �信�� �ô� Viewport ���� Content ���� ������Ʈ�� ������ �� �ִ�.
        // �׸��� sizeDelta ���� ���ؼ� Content�� ���̿� ���̸� ������ �� �ִ�.
        scrollRect.content.sizeDelta = new Vector2(width, height);
    }
}
