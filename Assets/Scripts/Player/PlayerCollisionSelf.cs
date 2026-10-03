using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CharacterController))]
public class PlayerCollisionSelf : MonoBehaviour
{

    /*
    **********************************************************************************************************************************
    *                                                                                                                                *
    *  Este script lida com a detecção de colisões do player (OnPlayerCollisionEnter, OnPlayerCollisionStay, OnPlayerCollisionExit)  *
    *                                                                                                                                *
    *  NAO MEXA NESSE SCRIPT ANAO SER QUE SAIBA O QUE ESTA FAZENDO!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!            *
    *                                                                                                                                *
    **********************************************************************************************************************************
    */

    private CharacterController characterController;
    private PlayerMovement playerMovement;
    private PlayerInteraction playerInteraction;

    private HashSet<PlayerCollisionObject> currentColliders = new HashSet<PlayerCollisionObject>();
    private HashSet<PlayerCollisionObject> previousColliders = new HashSet<PlayerCollisionObject>();


    private readonly Collider[] resultColl = new Collider[16]; // Resultado da OverlapCapsule
    private float contactTolerance = 0.03f; // Adicional de tamanho de colisao para checar colisao quando parado.

    public UnityEvent ccMoved; // Lembrando que isso e chamado depois do OnControllerColliderHit().

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerMovement = GetComponent<PlayerMovement>();
        playerInteraction = GetComponent<PlayerInteraction>();
        playerMovement.SetPlayerCollisionSelf(this);

        ccMoved.AddListener(RenewColliders);
    }

    // Ele e chamado depois de cada CharacterController.Move() nativamente.
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        RegisterCollision(hit.collider);
    }

    void RenewColliders()
    {
        CheckIdleCollisions();

        // 2. Processa EnterCollision e StayCollision
        foreach (var obj in currentColliders)
        {
            if (obj == null) continue;

            if (!previousColliders.Contains(obj))
            {
                obj.OnPlayerCollisionEnter(gameObject);
            }
            else
            {
                obj.OnPlayerCollisionStay(gameObject);
            }
        }

        // 3. Processa ExitCollision
        foreach (var prevObj in previousColliders)
        {
            if (prevObj == null) continue;

            if (!currentColliders.Contains(prevObj))
            {
                prevObj.OnPlayerCollisionExit(gameObject);
            }
        }

        HashSet<PlayerCollisionObject> temp = previousColliders;
        previousColliders = currentColliders;
        currentColliders = temp; // Isso e para evitar ficar aloprando a memoria de listas novas. Ele vai limpar a antiga lista que nao vai usar mais e criar a nova em cima dela
        currentColliders.Clear();
    }

    // Checa as colisoes quando o player esta parado.
    private void CheckIdleCollisions()
    {
        /*
        Quando o player esta parado, o CharacterController entende que o player nao esta colidindo com nada, porque nao ha "empurrao".
        Ele pode estar grudado, mas como um colisor nao bate no outro, a unity entende que nao esta colidindo com nada.
        Essa funcao vai "criar uma colisao" artifical maior que a do player para detectar esse problema.
        */

        // Assim como esta escrito no comeco do script, NAO MEXA ANAO SER QUE VOCE SAIBA O QUE ESTA FAZENDO
        
        GetCapsulePoints(out Vector3 p1, out Vector3 p2, out float radius);

        float checkRadius = radius + characterController.skinWidth + contactTolerance;

        // Colisao artificial
        int hitCount = Physics.OverlapCapsuleNonAlloc(
            p1, p2, checkRadius, resultColl, ~0, QueryTriggerInteraction.Ignore);

        for (int i = 0; i < hitCount; i++)
        {
            Collider col = resultColl[i];
            resultColl[i] = null; // Limpa referência para nao ficar lixo para tras

            // Ignora colisores do próprio jogador
            if (col == null || col.transform.root == transform.root) continue;

            RegisterCollision(col);
        }
    }
    
    // Atualiza a lista de colisores atuais
    private void RegisterCollision(Collider col)
    {
        if (col == null) return;
        if (col.TryGetComponent(out PlayerCollisionObject pc)) currentColliders.Add(pc);
    }

    // Pega os pontos e raio da capsula de colisor do player. Ele considera scale
    private void GetCapsulePoints(out Vector3 point1, out Vector3 point2, out float radius)
    {
        radius = characterController.radius * Mathf.Max(transform.lossyScale.x, transform.lossyScale.z);
        Vector3 center = transform.TransformPoint(characterController.center);

        float halfHeight = Mathf.Max(0f, (characterController.height * transform.lossyScale.y * 0.5f) - radius);
        point1 = center + transform.up * halfHeight;
        point2 = center - transform.up * halfHeight;
    }
}
