using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Reactions : MonoBehaviour
{
    public Reaction reactionPrefab;
    public void SetNewReactions(List<ReactionData> reactionData, bool hinted)
    {
        ClearContainer();
        for (int i = 0; i < reactionData.Count; i++)
        {
            Reaction reaction = Instantiate(reactionPrefab, transform);
            reaction.UpdateReaction(reactionData[i], hinted);
        }
    }
    public void AddListenerToReactions(UnityAction listener)
    {
        foreach (var item in transform.GetComponentsInChildren<Reaction>())
        {
            item.GetComponent<Button>().onClick.AddListener(listener);
        }
    }
    private void ClearContainer()
    {
        foreach (var item in transform.GetComponentsInChildren<Reaction>())
        {
            Destroy(item.gameObject);
        }
    }
}
