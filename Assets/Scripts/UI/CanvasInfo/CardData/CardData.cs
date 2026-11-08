using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Scriptable Objects/CardData")]
public class CardData : ScriptableObject
{
    public string title; // Texto maior do card
    public string description; // Texto menor do card
    public float duration; // Tempo (em segundos) que o card vai ficar na tela antes de desaparecer
}
