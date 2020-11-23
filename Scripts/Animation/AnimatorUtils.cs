using UnityEngine;
using System.Linq;

public static class AnimatorUtils
{
    public static bool HasParameter(this Animator animator, string param) => animator.gameObject.activeInHierarchy && animator.parameters.Any(x => x.name == param);
}
