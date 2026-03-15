using Animancer;
using System.Collections;
using UnityEngine;

public class CrumblingPlatform : MonoBehaviour
{
    [SerializeField]
    private AnimancerComponent _animancerComponent;
    [SerializeField]
    private AnimationClip _crumblingAnimation;

    private bool _crumbling = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if( !_crumbling && collision.gameObject.TryGetComponent<PlayerCharacter>(out PlayerCharacter character))
        {
            Crumble();
        }
    }

    private void Crumble()
    {
        _crumbling = true;
        AnimancerState state = _animancerComponent.Play(_crumblingAnimation);
        state.Events(this).OnEnd = OnCrumbleAnimationEnd;
    }

    private void OnCrumbleAnimationEnd()
    {
        _animancerComponent.Stop();
        _crumbling = false;
    }
}