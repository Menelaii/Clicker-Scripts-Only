using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MagicBook : IBodyChangesHandler
{
    private Button _button;
    private Animator _animator;
    private SpellUser _spellUser;
    private Spell _selected;
    private Coroutine _task;
    private bool _isCasting;

    public MagicBook(Button button, SpellUser spellUser, Spell selected)
    {
        _button = button;
        _spellUser = spellUser;
        _selected = selected;
        _animator = _button.GetComponent<Animator>();

        if (_selected == null)
            _button.interactable = false;
    }

    public void OnButtonClick()
    {
        if (_isCasting)
        {
            StopCast();
        }
        else
        {
            TryStartCast();
        }
    }

    public void OnBodyChanged(Body body)
    {
        StopCast();

        if(body.Head.Spells != null && body.Head.Spells.Count > 0)
        {
            _selected = body.Head.Spells[0];
        }
        else
        {
            _selected = null;
        }

        _button.interactable = _selected != null;
    }

    private void StopCast()
    {
        if (_task != null)
            CoroutineMaster.Instance.StopCoroutine(_task);

        _isCasting = false;
        _animator.SetBool("isCasting", _isCasting);
    }

    private void TryStopCast()
    {
        if (_spellUser.Mana.IsEnoughFor(_selected) == false)
            StopCast();
    }

    private void TryStartCast()
    {
        if (_selected == null)
            return;

        StopCast();

        _isCasting = true;
        _animator.SetBool("isCasting", _isCasting);
        _task = CoroutineMaster.Instance.StartCoroutine(CastWithInterval(_selected));
    }

    private IEnumerator CastWithInterval(Spell ability)
    {
        WaitForSeconds waitForInterval = new WaitForSeconds(ability.Cooldown);
        while (true)
        {
            if (_spellUser.TryUse(ability) == false)
                TryStopCast();

            yield return waitForInterval;
        }
    }
}
