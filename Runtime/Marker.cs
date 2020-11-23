using Excursion360_Builder.Runtime.Markers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Physical representation of some <see cref="StateItem"/>
/// </summary>
public abstract class Marker : MonoBehaviour
{
    public abstract string Title { get; }
    public abstract void HandleInteract();

    /// <summary>
    /// Affects animation varialbe "Hovered"
    /// </summary>
    public bool hovered
    {
        get
        {
            return _hovered;
        }
        set
        {
            if (_hovered == value)
                return;

            _hovered = value;

            if (_animator != null)
                _animator.SetBool("Hovered", _hovered);
        }
    }

    private Animator _animator;
    private bool _hovered;

    protected virtual void Start()
    {
        _animator = GetComponent<Animator>();
    }
}
