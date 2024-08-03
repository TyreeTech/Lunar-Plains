using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public sealed class Whole : IWhole
{
	#region Fields / Properties
	public string name { get; set; }

	public bool allowed
	{
		get { return _allowed; }
		set
		{
			if (_allowed == value)
				return;
			_allowed = value;
			Check();
		}
	}
	bool _allowed = true;

	public bool running
	{
		get { return _running; }
		private set
		{
			if (_running == value)
				return;
			_running = value;
			for (int i = _parts.Count - 1; i >= 0; --i)
				_parts[i].Check();
		}
	}
	bool _running = true;

	public IWhole parent
	{
		get { return _parent; }
		set
		{
			if (_parent == value)
				return;

			if (_parent != null)
				_parent.RemoveChild(this);

			_parent = value;

			if (_parent != null)
				_parent.AddChild(this);

			Check ();
		}
	}
	IWhole _parent = null;

	public IList<IWhole> children { get { return _children.AsReadOnly(); }}
	public IList<IPart> parts { get { return _parts.AsReadOnly(); }}

	List<IWhole> _children = new List<IWhole>();
	List<IPart> _parts = new List<IPart>();
	bool _didDestroy;
	#endregion

	#region Constructor & Destructor
	public Whole ()
	{

	}

	public Whole (string name) : this ()
	{
		this.name = name;
	}

	~ Whole ()
	{
		Destroy();
	}
	#endregion

	#region Public
	public void Check ()
	{
		CheckEnabledInParent();
		CheckEnabledInChildren();
	}

	public void AddChild (IWhole whole)
	{
		if (_children.Contains(whole))
			return;

		_children.Add(whole);
		whole.parent = this;
	}

	public void RemoveChild (IWhole whole)
	{
		int index = _children.IndexOf(whole);
		if (index != -1)
		{
			_children.RemoveAt(index);
			whole.parent = null;
		}
	}

	public void RemoveChildren ()
	{
		for (int i = _children.Count - 1; i >= 0; --i)
			_children[i].parent = null;
	}

	public T AddPart<T> () where T : IPart, new()
	{
		T t = new T();
		t.whole = this;
		_parts.Add(t);
		return t;
	}

	public void RemovePart (IPart p)
	{
		int index = _parts.IndexOf(p);
		if (index != -1)
		{
			_parts.RemoveAt(index);
			p.whole = null;
			p.Disassemble();
		}
	}

	public T GetPart<T>() where T : class, IPart
	{
		for (int i = 0; i < _parts.Count; ++i)
			if (_parts[i] is T)
				return _parts[i] as T;
		return null;
	}

	public T GetPartInChildren<T>() where T : class, IPart
	{
		T retValue = GetPart<T>();
		if (retValue == null)
		{
			for (int i = 0; i < _children.Count; ++i)
			{
				retValue = _children[i].GetPartInChildren<T>();
				if (retValue != null)
					break;
			}
		}
		return retValue;
	}

	public T GetPartInParent<T>() where T : class, IPart
	{
		T retValue = GetPart<T>();
		if (retValue == null && parent != null)
			retValue = parent.GetPartInParent<T>();
		return retValue;
	}

	public List<T> GetParts<T>() where T : class, IPart
	{
		List<T> list = new List<T>();
		AppendParts<T>(this, list);
		return list;
	}

	public List<T> GetPartsInChildren<T>() where T : class, IPart
	{
		List<T> list = GetParts<T>();
		AppendPartsInChildren<T>(this, list);
		return list;
	}

	public List<T> GetPartsInParent<T>() where T : class, IPart
	{
		List<T> list = new List<T>();
		AppendPartsInParent<T>(this, list);
		return list;
	}

	public void Destroy ()
	{
		if (_didDestroy)
			return;

		_didDestroy = true;
		allowed = false;
		parent = null;

		for (int i = _parts.Count - 1; i >= 0; --i)
			_parts[i].Disassemble();

		for (int i = _children.Count - 1; i >= 0; --i)
			_children[i].Destroy();
	}
	#endregion

	#region Private
	void CheckEnabledInParent ()
	{
		bool shouldEnable = allowed;
		IWhole next = parent;
		while (shouldEnable && next != null)
		{
			shouldEnable = next.allowed;
			next = next.parent;
		}
		running = shouldEnable;
	}

	void CheckEnabledInChildren ()
	{
		for (int i = _children.Count - 1; i >= 0; --i)
			_children[i].Check();
	}

	void AppendParts<T> (IWhole target, List<T> list) where T : class, IPart
	{
		for (int i = 0; i < target.parts.Count; ++i)
			if (target.parts[i] is T)
				list.Add(target.parts[i] as T);
	}

	void AppendPartsInChildren<T>( IWhole target, List<T> list ) where T : class, IPart
	{
		AppendParts<T>(target, list);
		for (int i = 0; i < target.children.Count; ++i)
			AppendPartsInChildren<T>(target.children[i], list);
	}

	void AppendPartsInParent<T>( IWhole target, List<T> list ) where T : class, IPart
	{
		AppendParts<T>(target, list);
		if (target.parent != null)
			AppendPartsInParent<T>(target.parent, list);
	}
	#endregion
}
