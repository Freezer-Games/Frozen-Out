using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class AnimatingNPCYarn : MonoBehaviour
{
	private Animator aldeanoAnimator;
	
    void Start()
    {
        aldeanoAnimator = GetComponent<Animator>();
    }
	
	[YarnCommand("setanim")]
	public void TriggerAnimation(string animationName) {
		
		Debug.Log("Animating by yarn");
		
		animationName = "Anim_" + animationName;
		
		aldeanoAnimator.SetTrigger(animationName);
		
	}
}
