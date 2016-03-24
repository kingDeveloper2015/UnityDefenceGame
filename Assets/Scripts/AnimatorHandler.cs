using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimatorHandler : MonoBehaviour 
{
	private List <SpriteRenderer>_sprite = new List<SpriteRenderer>();
	[SerializeField]	Sprite image;
	[SerializeField] List<Animator> _anim;

	#if UNITY_EDITOR
	public List<Animator> GetList(){
		return _anim;
	}
	#endif

    void Start()
    {
        setSprites();
    }
	void OnEnable ()
	{
		AnimManagement (true);
	}
    void setSprites()
    {
        if(_anim.Count > 0)
        {
            for (int i = 0; i < _anim.Count; i++)
            {
                _sprite.Add(_anim[i].gameObject.GetComponent<SpriteRenderer>());
            }
        }
    }
	
	void OnDisable () 
	{
		for (int j=0; j<_sprite.Count; j++) 
		{
            if (_sprite[j] != null)
            {
                _sprite[j].sprite = image;
            }
		}
		AnimManagement (false); 
	}

	void AnimManagement(bool boolValue)
	{
		for(int i= 0; i< _anim.Count; i++)
		{
            if (_anim[i] != null)
            {
                _anim[i].enabled = boolValue;
            }
		}
	} 
}
