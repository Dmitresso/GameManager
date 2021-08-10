using UnityEngine;

public class MenuTransitions : StateMachineBehaviour {
    [SerializeField] private Transition from, to;

    public Transition[] Transitions {
        set { from = value[0]; to = value[1]; }
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        switch (from) {
            case Transition.Fade:
                animator.SetTrigger(Data.Animator.Parameters.Id.Trigger.TransitionFadeIn);
                break;
            case Transition.Slide:
                animator.SetTrigger(Data.Animator.Parameters.Id.Trigger.TransitionSlideIn);
                break;
        }

        switch (to) {
            case Transition.Fade:
                animator.SetTrigger(Data.Animator.Parameters.Id.Trigger.TransitionFadeOut);
                break;
            case Transition.Slide:
                animator.SetTrigger(Data.Animator.Parameters.Id.Trigger.TransitionSlideOut);
                break;
        }
    }
}