using UnityEngine;
using System;
using System.Collections;

public class Demo : MonoBehaviour
{
	public static event EventHandler apocolypseEvent;

	void Start ()
	{
		CreateLocalScopedInstances();

		GC.Collect();
		GC.WaitForPendingFinalizers();
		GC.Collect();

		if (apocolypseEvent != null)
			apocolypseEvent(this, EventArgs.Empty);
	}

	void CreateLocalScopedInstances ()
	{
		IWhole whole = new Whole("Mortal");
		whole.AddPart<MyPart>();
	}
}

public class MyPart : Part
{
	public override void Resume ()
	{
		base.Resume ();
		Debug.Log("MyPart is now Enabled on " + whole.name);
		Demo.apocolypseEvent += OnApocolypticEvent;
	}

	public override void Suspend ()
	{
		base.Suspend ();
		Debug.Log("MyPart is now Disabled");
		Demo.apocolypseEvent -= OnApocolypticEvent;
	}

	~ MyPart ()
	{
		Debug.Log("MyPart has perished");
	}

	void OnApocolypticEvent (object sender, EventArgs e)
	{
		// This won't actually be reached
		Debug.Log("I'm alive!!!");
	}
}
