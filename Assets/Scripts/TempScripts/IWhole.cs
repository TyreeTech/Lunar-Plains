using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IWhole
{
	string name { get; set; }
	bool allowed { get; set; }
	bool running { get; }
	IWhole parent { get; set; }
	IList<IWhole> children { get; }
	IList<IPart> parts { get; }

	void AddChild (IWhole whole);
	void RemoveChild (IWhole whole);
	void RemoveChildren ();

	T AddPart<T> () where T : IPart, new();
	void RemovePart (IPart p);

	T GetPart<T>() where T : class, IPart;
	T GetPartInChildren<T>() where T : class, IPart;
	T GetPartInParent<T>() where T : class, IPart;
	List<T> GetParts<T>() where T : class, IPart;
	List<T> GetPartsInChildren<T>() where T : class, IPart;
	List<T> GetPartsInParent<T>() where T : class, IPart;

	void Check ();
	void Destroy ();
}
