using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class Mano : MonoBehaviour
{
    public XRPokeInteractor pokeInteractor;

    void OnEnable()
    {
        pokeInteractor.hoverEntered.AddListener(OnHoverEntered);
    }

    void OnDisable()
    {
        pokeInteractor.hoverEntered.RemoveListener(OnHoverEntered);
    }

    void OnHoverEntered(HoverEnterEventArgs args)
    {
        GameObject gameObject = args.interactableObject.transform.gameObject;
       
        if (gameObject.CompareTag("Cubo"))
        {
            Destroy(gameObject);
            Marcador.Instance.score++;
            Marcador.Instance.scoreText.text = Marcador.Instance.score.ToString();
        }
    }
}
