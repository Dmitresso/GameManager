using UnityEngine;


public class SkyboxFade : StateMachineBehaviour {
    [SerializeField] private Fade fade;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        SkyboxManager.Instance.StartCoroutine(SkyboxManager.Instance.FadeSkybox(fade));
    }
    
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        SkyboxManager.Instance.StopCoroutine(SkyboxManager.Instance.FadeSkybox(fade));
    }
}