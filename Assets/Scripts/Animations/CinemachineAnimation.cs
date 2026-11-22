using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CinemachineAnimation : MonoBehaviour
{
    [Header("References")]
    
    [Tooltip("CinemachineCamera padrao do player")]
    [SerializeField] private GameObject CinemachaninePlayer;
    
    [Tooltip("Todos os objetos do CinemachineCamera ordenados do primeiro ao ultimo")]
    [SerializeField] private GameObject[] CinemachinesCameraAnimation;

    [Header("Settings")]
    
    [Tooltip("Determina se a variavel vai rodar assim que o jogo iniciar")]
    [SerializeField] private bool runAtStart;
    
    [Tooltip("Tempo de espera entre a animacao de um ponto a outro." +
             "Primeiro valor representa o tempo da camera do player ate o primeiro ponto." +
             "Esse array deve ter o mesmo tamanho do Cinemachines Camera Animation." +
             "Com excecao do primeiro valor, os demais nao devem ser 0.")]
    [SerializeField] private float[] animationDuration;
    
    [Tooltip("Tempo entre cada animacao (transicao de um ponto ao outro.")]
    [SerializeField] private float timeBetweenAnimations;
    
    [Tooltip("Tempo de espera entre o final da ultima animacao para voltar para a camera do player")]
    [SerializeField] private float timeToRestoreCamera;


    private CinemachineBrain _cinemachineBrain;
    private CinemachineBlendDefinition.Styles _defaultCinemachineBD;
    private PlayerMovement _playerMovement;
    
    // ------------------ Default Methods ------------------

    private void Start()
    {
        _cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
        _defaultCinemachineBD = _cinemachineBrain.DefaultBlend.Style;
        _playerMovement = GameManager.Instance.player.GetComponent<PlayerMovement>();
        
        if (runAtStart) RunAnimation();
       
    }
    
    // ------------------ Public Methods ------------------

    public void RunAnimation()
    {
        _playerMovement.SetMovementEnabled(false);
        
        if (animationDuration[0] <= 0) _cinemachineBrain.DefaultBlend.Style = CinemachineBlendDefinition.Styles.Cut;
        _cinemachineBrain.DefaultBlend.Time = animationDuration[0];
        
        CinemachinesCameraAnimation[0].SetActive(true);
        CinemachaninePlayer.SetActive(false);
        
        StartCoroutine(I_RunAnimation(animationDuration[0] <= 0));
    }
    
    // ------------------ Private Methods ------------------
    
    // Funcao auxiliar do RunAnimation
    private IEnumerator I_RunAnimation(bool resetAnimationStyle = true)
    {
        if (resetAnimationStyle)
        {
            _cinemachineBrain.DefaultBlend.Style = _defaultCinemachineBD;
        }
        
        for (int i = 1; i<CinemachinesCameraAnimation.Length; i++)
        {
            yield return new WaitForSeconds(animationDuration[i-1] + timeBetweenAnimations);
            
            CinemachinesCameraAnimation[i].SetActive(true);
            CinemachinesCameraAnimation[i-1].SetActive(false);
            _cinemachineBrain.DefaultBlend.Time = animationDuration[i];
        }
        
        yield return new WaitForSeconds(timeToRestoreCamera);
        _cinemachineBrain.DefaultBlend.Time = 2f;
        CinemachinesCameraAnimation[CinemachinesCameraAnimation.Length-1].SetActive(false);
        CinemachaninePlayer.SetActive(true);
        
        yield return new WaitForSeconds(2f);
        _playerMovement.SetMovementEnabled(true);

    }
}
