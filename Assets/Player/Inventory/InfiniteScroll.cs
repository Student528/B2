
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfiniteScroll : MonoBehaviour
{
    public ScrollRect scrollRect;
    public RectTransform viewPortTransform;
    public RectTransform contentPanelTransform;
    public VerticalLayoutGroup VLG;

    public RectTransform[] ItemList;

    // Start is called before the first frame update
    void Start()
    {
int ItemsToAdd = Mathf.CeilToInt(viewPortTransform.rect.height / (ItemList[0].rect.height + VLG.spacing));

for (int i = 0; i < ItemsToAdd; i++)
{
    RectTransform RT = Instantiate(ItemList[i % ItemList.Length], contentPanelTransform);
    RT.SetAsLastSibling();
}

for (int i = 0; i < ItemsToAdd; i++)

{
    int num = ItemList.Length - i - 1;
    while (num < 0)
    {
        num += ItemList.Length;
    }
    RectTransform RT = Instantiate(ItemList[num], contentPanelTransform);
    RT.SetAsFirstSibling();
}
contentPanelTransform.localPosition = new Vector3(contentPanelTransform.localPosition.x,
    (0 - (ItemList[0].rect.height + VLG.spacing) * ItemsToAdd),
    contentPanelTransform.localPosition.z);

    }
void Update()
{
    if (contentPanelTransform.localPosition.y > 0)
    {
        Canvas.ForceUpdateCanvases();
        contentPanelTransform.localPosition -= new Vector3(0, ItemList.Length * (ItemList[0].rect.height + VLG.spacing), 0);
    }

    if (contentPanelTransform.localPosition.y < 0 - (ItemList.Length * (ItemList[0].rect.height + VLG.spacing)))
    {
        Canvas.ForceUpdateCanvases();
        contentPanelTransform.localPosition += new Vector3(0, ItemList.Length * (ItemList[0].rect.height + VLG.spacing), 0);
    }
}

    // Update is called once per frame
}
