using UnityEngine;

public class FallenBoden : FracturedWreckage {

    public override void CollisionEnter(string trigger)
    {
        base.CollisionEnter(trigger);
        Debug.Log("Enter");
        if (isMovable)
        {
            if (trigger == "Player")
            {
                isTriggered = true;
            }
        }
    }
    public override void CollisionExit(string trigger)
    {
        base.CollisionExit(trigger);
        if (trigger == "Player")
            if (isTriggered)
                Collapse(1);
    }

    public override void Collapse(int recursionTimes = 2)
    {
        if (recursionTimes-- <= 0)
            return;
        for (int i = 0; i < AdjacencyWreckages.Count; i++)
        {
            AdjacencyWreckages[i].Collapse(recursionTimes);
        }
        rg.isKinematic = false;
        rg.constraints = RigidbodyConstraints.None;
    }

}

