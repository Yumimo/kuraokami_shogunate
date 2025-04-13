using System;
using UnityEngine;

namespace Kuraokami
{
    [Serializable]
    public class AnimalFormClass
    {
        public AnimalFormData _data;
        public GameObject _formModel;

        public string Id => _data.Id;
        public Animator FormAnimator => _formModel.GetComponent<Animator>();

        private GameObject _humanForm;
        public void OnTransform(GameObject _human)
        {
            _humanForm = _human;
            _humanForm.SetActive(false);
            _formModel.SetActive(true);
        }

        public void ReturnToOriginalForm()
        {
            _humanForm.SetActive(true);
            _formModel.SetActive(false);
        }
    }
    
}
