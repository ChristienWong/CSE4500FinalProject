using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Heart : MonoBehaviour
{
    public int health;
    public int numOfHearts;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    public bool HasConfiguredHearts
    {
        get
        {
            return hearts != null && hearts.Length > 0 && System.Array.Exists(hearts, h => h != null);
        }
    }

    void Awake()
    {
        if (!HasConfiguredHearts)
        {
            AutoPopulateHearts();
        }
    }

    void AutoPopulateHearts()
    {
        Image[] candidates = GetComponentsInChildren<Image>(true);
        if (candidates == null || candidates.Length == 0)
        {
            return;
        }

        List<Image> discovered = new List<Image>();
        foreach (Image image in candidates)
        {
            if (image == null)
            {
                continue;
            }

            if ((fullHeart != null && image.sprite == fullHeart) ||
                (emptyHeart != null && image.sprite == emptyHeart))
            {
                discovered.Add(image);
            }
        }

        if (discovered.Count == 0)
        {
            return;
        }

        discovered.Sort((a, b) => a.transform.GetSiblingIndex().CompareTo(b.transform.GetSiblingIndex()));
        hearts = discovered.ToArray();
    }

    void Update()
    {
        if (health > numOfHearts)
        {
            health = numOfHearts;
        }

        if (health < 0)
        {
            health = 0;
        }

        for (int i = 0; i < hearts.Length; i++)
        {
            if (hearts[i] == null)
            {
                continue;
            }

            if (i < health)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }

            hearts[i].enabled = i < numOfHearts;
        }
    }
}
