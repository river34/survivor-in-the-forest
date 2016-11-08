using UnityEngine;
using System.Collections;

public class ColliderController : MonoBehaviour {

    private GameObject _parent;

    private int _strick_times = 0;

    private int _default_strick_times_for_fire = 2;

    void Start ()
    {
        _parent = transform.parent.gameObject;
    }

    public bool GetFireStatus ()
    {
        if (_parent)
        {
            if (_parent.CompareTag(Tags.LIGHTER))
            {
                return _parent.GetComponent<Lighter>().isLighterOn();
            }
            else if (_parent.CompareTag(Tags.TORCH))
            {
                return _parent.GetComponent<TorchController>().GetFireStatus();
            }
            else if (_parent.CompareTag(Tags.TREE_FIREPLACE))
            {
                return _parent.GetComponent<TreeFireplaceController>().GetFireStatus();
            }
            else if (_parent.CompareTag(Tags.CAMP_FIRE_COLLIDER))
            {
                return _parent.GetComponent<FireplaceController>().GetFireStatus();
            }
        }

        return false;
    }

    void OnTriggerEnter (Collider other)
    {
        if (_parent)
        {
            print("_parent : " + _parent.tag + " other : " + other.tag);

            // torch
            if (CompareTag(Tags.TORCH_CATCH_FIRE_COLLIDER) && other.CompareTag(Tags.FIREPLACE))
            {
                if (other.GetComponent<FireplaceController>().GetFireStatus())
                {
                    _parent.GetComponent<TorchController>().FireOn();
                }
            }
            else if (CompareTag(Tags.TORCH_CATCH_FIRE_COLLIDER) && other.CompareTag(Tags.CAMP_FIRE_COLLIDER))
            {
                if (other.GetComponent<ColliderController>().GetFireStatus())
                {
                    _parent.GetComponent<TorchController>().FireOn();
                }
            }
            else if (CompareTag(Tags.TORCH_CATCH_FIRE_COLLIDER) && other.CompareTag(Tags.TREE_FIREPLACE_COLLIDER))
            {
                if (other.GetComponent<ColliderController>().GetFireStatus())
                {
                    _parent.GetComponent<TorchController>().FireOn();
                }
            }
            else if (CompareTag(Tags.TORCH_CATCH_FIRE_COLLIDER) && other.CompareTag(Tags.TORCH_COLLIDER))
            {
                if (other.GetComponent<ColliderController>().GetFireStatus())
                {
                    _parent.GetComponent<TorchController>().FireOn();
                }
            }
            else if (CompareTag(Tags.TORCH_CATCH_FIRE_COLLIDER) && other.CompareTag(Tags.LIGHTER_COLLIDER))
            {
                if (other.GetComponent<ColliderController>().GetFireStatus())
                {
                    _parent.GetComponent<TorchController>().FireOn();
                }
            }
            else if (CompareTag(Tags.TORCH_CATCH_FIRE_COLLIDER) && other.CompareTag(Tags.TORCH_CATCH_FIRE_COLLIDER))
            {
                if (!other.GetComponent<ColliderController>().GetFireStatus())
                {
                    _strick_times++;

                    // play sound of stricking
                    // show sparks
                    _parent.GetComponent<TorchController>().Spark();

                    if (_strick_times >= _default_strick_times_for_fire)
                    {
                        _parent.GetComponent<TorchController>().FireOn();
                    }
                }
            }

            // wolf
            else if (CompareTag(Tags.WOLF_COLLIDER) && other.CompareTag(Tags.TORCH_COLLIDER))
            {
                if (other.GetComponent<ColliderController>().GetFireStatus())
                {
                    _parent.GetComponent<WolfController>().SetRetrieving();
                }
            }
        }
    }
}
