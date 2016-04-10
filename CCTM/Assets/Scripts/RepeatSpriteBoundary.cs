using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// @NOTE the attached sprite's position should be "top left" or the children will not align properly
// Strech out the image as you need in the sprite render, the following script will auto-correct it when rendered in the game
[RequireComponent(typeof(SpriteRenderer))]

// Generates a nice set of repeated sprites inside a streched sprite renderer
// @NOTE Vertical only, you can easily expand this to horizontal with a little tweaking
public class RepeatSpriteBoundary : MonoBehaviour
{
    SpriteRenderer sprite;
    public enum vertex { x, y }


    public vertex UseVertex = vertex.x;
    public string NamePrefix = "";

    const int maxSpritesToLoad = 3;

    void Awake()
    {
        // Get the current sprite with an unscaled size
        sprite = GetComponent<SpriteRenderer>();
        Vector2 spriteSize = new Vector2(sprite.bounds.size.x / transform.localScale.x, sprite.bounds.size.y / transform.localScale.y);

        // Generate a child prefab of the sprite renderer
        GameObject childPrefab = new GameObject();
        SpriteRenderer childSprite = childPrefab.AddComponent<SpriteRenderer>();
        childPrefab.transform.position = transform.position;

        List<Sprite> spriteList = GetSprites();
        Sprite lastSprite = null;

        // Loop through and spit out repeated tiles
        GameObject child;
        int extent = 0;

        Debug.Log("SpriteSize X: " + spriteSize.x);

        switch (UseVertex)
        {
            case vertex.x:
                extent = (int)Mathf.Round(sprite.bounds.size.x);
                break;
            case vertex.y:
                extent = (int)Mathf.Round(sprite.bounds.size.y);
                break;
        }

        Debug.Log("Extent: " + extent);

        for (int i = 1, l = extent; i < l; i++)
        {
            childSprite.sprite = spriteList[0];

            // CHANGE HERE FOR MULTIPLES
            if (spriteList.Count > 1)
            {
                while (childSprite.sprite != lastSprite)
                {
                    childSprite.sprite = spriteList[Random.Range(0, spriteList.Count)];
                }
            }
            child = Instantiate(childPrefab) as GameObject;
            Vector3 tempVec = Vector3.zero;

            switch (UseVertex)
            {
                case vertex.x:
                    tempVec = new Vector3(spriteSize.x - 0.1f, 0, 0);
                    break;
                case vertex.y:
                    tempVec = new Vector3(0, spriteSize.y - 1, 0);
                    break;
            }
            child.transform.position = transform.position - (tempVec * i);
            child.transform.parent = transform;
            lastSprite = childSprite.sprite;
        }

        // Set the parent last on the prefab to prevent transform displacement
        childPrefab.transform.parent = transform;

        // Disable the currently existing sprite component since its now a repeated image
        sprite.enabled = false;
    }

    List<Sprite> GetSprites()
    {
        List<Sprite> spriteList = new List<Sprite>();

        if ((NamePrefix == null) || (NamePrefix == string.Empty))
        {
            spriteList.Add(GetComponent<SpriteRenderer>().sprite);
        }
        else
        {
            /*
            for (int i = 1; i <= maxSpritesToLoad; i++)
            {
                Sprite mySprite = null;

                try
                {
                    Debug.Log("Attempting to load " + NamePrefix + i.ToString());
                    mySprite = Resources.Load<Sprite>(NamePrefix + i.ToString());
                }
                catch
                {
                    mySprite = null;
                }

                if (mySprite != null)
                {
                    spriteList.Add(mySprite);
                }
            }

            Debug.Log("Loaded " + spriteList.Count + " sprites.");
            */

            if (spriteList.Count == 0)
            {
                spriteList.Add(sprite.sprite);
            }
        }

        return spriteList;
    }
}