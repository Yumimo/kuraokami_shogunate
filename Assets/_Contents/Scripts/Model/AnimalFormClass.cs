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

        public Action OnTransformEvent;
        public void StartForm(GameObject _human)
        {
            _humanForm = _human;
            _humanForm.SetActive(false);
            _formModel.SetActive(true);
            OnTransformEvent?.Invoke();
        }

        public void EndForm()
        {
            _humanForm.SetActive(true);
            _formModel.SetActive(false);
        }
    }
    
}
