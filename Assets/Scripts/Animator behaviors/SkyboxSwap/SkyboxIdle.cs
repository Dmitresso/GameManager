using System.Collections;
using UnityEngine;

using Trigger = Data.Animator.Parameters.Id.Trigger;

public class SkyboxIdle : StateMachineBehaviour {
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        UIManager.Instance.StartCoroutine(WaitForSwap(SkyboxManager.Instance.SwapRate));
        UIManager.Instance.ResetTrigger(Trigger.SkyboxFadeInCompleted);
        UIManager.Instance.ResetTrigger(Trigger.SkyboxFadeOutCompleted);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        UIManager.Instance.StopCoroutine(nameof(WaitForSwap));
    }

    private IEnumerator WaitForSwap(float swapRate) {
        yield return new WaitForSeconds(swapRate);
        UIManager.Instance.SetTrigger(Trigger.StartSwap);
    }
}