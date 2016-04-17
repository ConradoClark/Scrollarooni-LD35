using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BeamEffect : BulletEffect
{
    public float Strength;
    public GameObject PoofEffect;
    public float StunDuration;

    public override void Run(GameObject affected, Dictionary<string, object> extraParams)
    {
        base.Run(affected, extraParams);

        Debug.Log("Effect Ran!");

        Movement movement = affected.GetComponent<Movement>();
        object direction = null;
        extraParams.TryGetValue("Direction(Beam)", out direction);

        BaseEnemyBehaviour enemyBehaviour = affected.GetComponent<BaseEnemyBehaviour>();
        if (enemyBehaviour != null)
        {
            enemyBehaviour.Stun(this.StunDuration);
        }        

        if (movement != null && direction != null)
        {
            this.StartCoroutine(Push(movement, (Vector2)direction));
            this.StartCoroutine(Poof(movement));
        }
    }

    IEnumerator Poof(Movement movement)
    {
        if (PoofEffect == null) yield break;

        GameObject poof = GameObject.Instantiate(this.PoofEffect);
        poof.transform.SetParent(movement.transform, false);

        RectTransform rect = movement.GetComponent<RectTransform>();
        Vector3 localPos = Vector3.zero;
        if (rect != null)
        {
            poof.transform.localPosition = new Vector3(rect.sizeDelta.x/2 * Random.Range(-1f,1f), rect.sizeDelta.y/2 * Random.Range(-1f, 1f));
        }
    }

    IEnumerator Push(Movement movement, Vector2 direction)
    {
        var currentPosition = movement.transform.position;
        var pushDirection = -new Vector3(direction.x, direction.y).normalized;
        var push = new Vector3(direction.x, direction.y).normalized * this.Strength;
        var desiredPosition = movement.transform.position + push;
        float amountMoved = 0f;

        Vector3 velocity = Vector3.zero;

        while (amountMoved < push.magnitude)
        {
            Vector3 pushMovement = (Vector3.SmoothDamp(currentPosition, desiredPosition, ref velocity, 0.25f) - currentPosition);
            var pushMovementCorrect = new Vector3(pushMovement.x * Mathf.Abs(pushDirection.x), pushMovement.y * Mathf.Abs(pushDirection.y));
            movement.Push(pushMovementCorrect);
            amountMoved += pushMovementCorrect.magnitude;
            yield return 1;
        }
    }
}
