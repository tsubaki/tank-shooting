using UnityEngine;
using System.Collections;

/// <summary>
/// このコンポーネントがアタッチされているオブジェクトは、 単一のオブジェクトに含まれる
/// 既にある場は削除される
/// </summary>
public class SingletonObject : MonoBehaviour
{
	/// <summary>
	/// 親オブジェクトの情報
	/// </summary>
	private readonly static string objectName = "gameController";
	/// <summary>
	/// The base object.
	/// </summary>
	private static GameObject baseObject;

	private static GameObject BaseObject {
		get {
			if (baseObject == null) {
				baseObject = GameObject.Find (objectName);
				if (baseObject == null) {
					baseObject = new GameObject (objectName);
				}
			}
			return baseObject;
		}
	}

	/// <summary>
	/// 登録済みオブジェクトの取得
	/// </summary>
	/// <returns> 名称に一致するオブジェクト </returns>
	/// <param name='objName'>
	/// オブジェクト名
	/// </param>
	public static GameObject GetGameObject (string objName)
	{
		return BaseObject.transform.FindChild (objName).gameObject;
	}

	/// <summary>
	/// 登録済みオブジェクトの取得
	/// </summary>
	/// <returns>最初に一致したオブジェクト</returns>
	/// <param name="type">探索する型情報</param>
	public static GameObject GetGameObject (System.Type type)
	{
		return BaseObject.transform.GetComponentInChildren (type).gameObject;
	}

	/// <summary>
	/// 既に同名のオブジェクトがある場合、自身を削除する
	/// </summary>
	bool Marge ()
	{
		Transform target = BaseObject.transform.FindChild (name);
		if (target != null && target != transform) {
			Destroy (gameObject);
			return true;
		} else {
			transform.parent = BaseObject.transform;
			return false;
		}
	}

	#region UnityDelegate

	void Awake ()
	{
		Marge ();
	}

	#endregion

}