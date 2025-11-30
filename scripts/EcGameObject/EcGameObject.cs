using Godot;
using System;
using System.Collections.Generic;

public partial class EcGameObject : Node
{
	// Organizes all inherited class in tree
	// Make sure one and only one EcGameObject node is defined in tree
	// By trigger StoreAsGameObject() from inherited classes, it will be added at "<path of EcGameObject>/<class name>/<id>"

	static Dictionary<string, int> NextId = [];
	static EcGameObject GameObjectRoot;

	[Export]
	public int Id;

	public override void _EnterTree()
	{
		if (this.GetType().Name == "EcGameObject") {
			GameObjectRoot = this;
		}
	}

	public int StoreAsGameObject()
	{
		string type = this.GetType().Name;
		if (!NextId.ContainsKey(type)) NextId[type] = 0;
		int currentId = NextId[type];
		NextId[type] += 1;
		Name = currentId.ToString();
		Id = currentId;

		Node folderNode = GetOrCreateChildNode(GameObjectRoot, type);
		folderNode.AddChild(this);
		return currentId;
	}

	public void RemoveAsGameObject()
	{
		string type = this.GetType().Name;
		Node folderNode = GetOrCreateChildNode(GameObjectRoot, type);
		folderNode.RemoveChild(this);
	}

	public static T GetGameObject<T>(int id) where T: EcGameObject
	{
		string type = typeof(T).Name;
		return GameObjectRoot.GetNode<Node>(type).GetNode<T>(id.ToString());
	}

	public static Node GetOrCreateChildNode(Node parent, string nodeName)
	{
		Node childNode = parent.GetNodeOrNull<Node>(nodeName);
		if (childNode == null)
		{
			childNode = new Node
			{
				Name = nodeName
			};
			parent.AddChild(childNode);
		}
		return childNode;
	}

}
