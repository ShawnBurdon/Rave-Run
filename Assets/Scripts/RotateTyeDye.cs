using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class RotateTyeDye : MonoBehaviour
{
	public float rotateSpeed;
	public float scaleSpeed;

	void OnEnable ()
	{
		transform.localScale = Vector3.one;
		transform.DOScale(0.75f, scaleSpeed).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
	}
	
	void Update ()
	{
		transform.Rotate(Vector3.forward * rotateSpeed);
	}
}