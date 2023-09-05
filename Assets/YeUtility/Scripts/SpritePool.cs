using UnityEngine;

namespace CommonUnit
{
	public class SpritePool : MonoBehaviour
	{
		[SerializeField] Sprite[] Sprites;
		int index;
		public int SpriteIndex
		{
			get
			{
				return index;
			}
			set
			{

			}
		}
	}
}