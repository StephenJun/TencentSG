using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class MovePointEffect : MonoBehaviour {
    [SerializeField]
    SpriteRenderer sp;
	void Start () {
        transform.position += Vector3.up * 0.1f;
        transform.localScale = Vector3.zero;
        transform.DOScale(1, 0.8f).OnStart(()=>sp.DOFade(0,0.4f).SetDelay(0.4f)).OnComplete(()=>Destroy(transform.parent.gameObject));
	}

}
