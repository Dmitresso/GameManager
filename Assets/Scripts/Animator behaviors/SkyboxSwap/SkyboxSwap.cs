using UnityEngine;

public class SkyboxSwap : StateMachineBehaviour {
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        SkyboxManager.Instance.ChangeSkybox(SkyboxManager.Instance.SelectedSkyboxType);
        UIManager.Instance.SetTrigger(Data.Animator.Parameters.Id.Trigger.SwapCompleted);
    }
}