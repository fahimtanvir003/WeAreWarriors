using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VfxPool : MonoBehaviour
{
    public static VfxPool instance;

    [SerializeField] private Transform _vfxPoolTrans;
    [SerializeField] private List<VfxPools> _vfxPools;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (var vfx in _vfxPools)
        {
            vfx.InstantiateEffects(_vfxPoolTrans);
        }
    }

    public void PlayVfx(string _vfxName, Vector3 _position, Quaternion _rotation)
    {
        foreach(var VFXs in _vfxPools)
        {
            foreach (var vfx in VFXs.vfxList)
            {
                if (vfx.name == _vfxName && !vfx.activeInHierarchy)
                {
                    vfx.transform.SetLocalPositionAndRotation(_position, _rotation);
                    vfx.SetActive(true);

                    StartCoroutine(TurnOffVfx(vfx));
                    break;
                }
            }
        }
    }

    private IEnumerator TurnOffVfx(GameObject obj)
    {
        yield return new WaitForSeconds(2f);
        obj.SetActive(false);
    }

    [Serializable]
    private class VfxPools
    {
        public GameObject effectPrefab;
        public string effectName;
        [SerializeField] private float _spawnAmount;

        public List<GameObject> vfxList;

        public void InstantiateEffects(Transform parent)
        {
            for (int i = 0; i < _spawnAmount; i++)
            {
                GameObject obj = Instantiate(effectPrefab, parent);
                obj.transform.name = effectName;
                vfxList.Add(obj);

                obj.SetActive(false);
            }
        }
    }
}
