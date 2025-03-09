using System.Collections.Generic;
using UnityEngine;

public class Animal : Entity
{
    public float moveSpeed = 2.0f;
    public float rotationSpeed = 5.0f;
    private Vector3 targetPosition;

    private void Start()
    {
        SetRandomTargetPosition();
    }

    private void Update()
    {
        MoveTowardsTarget();
    }

    public override void Move(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }

    private void MoveTowardsTarget()
    {
        //Ha k�zel vagyunk a c�lponthoz, v�lasszunk egy �j c�lpontot
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            SetRandomTargetPosition();
        }

        //Mozg�s a c�lpont fel�
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        //Forg�s a mozg�s ir�ny�ba, ha van elmozdul�s)
        Vector3 direction = targetPosition - transform.position;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }


    private void SetRandomTargetPosition()
    {
        float randomX = Random.Range(0, 50);
        float randomZ = Random.Range(0, 50);
        targetPosition = new Vector3(randomX, 0, randomZ);
    }
}