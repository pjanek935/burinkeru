using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler 
{
	List <Tweener> tweeners = new List<Tweener> ();

	public AnimationHandler (){}

	public void AddTweener (Tweener newTwneener)
	{
		tweeners.Add (newTwneener);
	}

	public void Cancel ()
	{
		for (int i = 0; i < tweeners.Count; i ++)
		{
			tweeners [i].Cancel ();
		}
	}

	public void Pause ()
	{
		for (int i = 0; i < tweeners.Count; i ++)
		{
			tweeners [i].Pause ();
		}
	}

	public void Resume ()
	{
		for (int i = 0; i < tweeners.Count; i ++)
		{
			tweeners [i].Resume ();
		}
	}
}
