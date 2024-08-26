using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UIElements;
using UnityEngine.XR;

public class CustomerMovement
{
    CustomerBot CustomerBot;
    GameObject CustomerObject;
    public CustomerMovement(CustomerBot customer) 
    {
        CustomerBot = customer;
    }

    private IEnumerator MoveToPosition(Vector3 destination, float moveSpeed)
    {
        while (Vector3.Distance(CustomerObject.transform.position, destination) > 0.1f)
        {
            Vector3 newPosition = Vector3.MoveTowards(CustomerObject.transform.position, destination, moveSpeed * Time.deltaTime);
            CustomerObject.transform.position = new Vector3(newPosition.x, CustomerObject.transform.position.y, newPosition.z);
            RotateTowardsDirection(destination);
            yield return null;
        }
        CustomerObject.transform.position = new Vector3(destination.x, CustomerObject.transform.position.y, destination.z);
    }

    private IEnumerator SearchForShelf()
    {
        if (!CustomerBot.firstSearchDone)
        {
            yield return new WaitForSeconds(0.5f);
            CustomerBot.firstSearchDone = true;
        }

        GameObject shelf = CustomerBot.shelfList.GetRandomSection();
        if (shelf != null)
        {
            Vector3 sectionPosition = shelf.transform.position;
            CustomerBot.SetTargetPosition(sectionPosition + targetOffset);
            yield return MoveToPosition(targetPosition, speed);

            yield return LookAtSection();

            SpawnerController spawner = shelf.GetComponent<SpawnerController>();
            if (spawner != null && spawner.ObjectStack.Count > 0)
            {
                walking = true;
                Debug.Log("Misión Cumplida :)");
                GameObject cereal = spawner.PopStack();
                if (cereal != null)
                {
                    Animator animator = cereal.GetComponent<Animator>();
                    if (animator != null)
                    {
                        animator.SetBool("despawn", false);
                    }
                    done = true;
                    cereal.transform.parent = parentForFlakes;
                    cereal.transform.position = parentForFlakes.transform.position + new Vector3(0, 1, 1);
                }
            }
            else
            {
                Debug.Log("Intentando una sección aleatoria :(");
                yield return new WaitForSeconds(waitTime);
                StartCoroutine(NextSection());
            }
        }
    }

    private IEnumerator LookAtSection()
    {
        while (Mathf.Abs(CustomerObject.transform.rotation.eulerAngles.y) > 0.1f)
        {
            Quaternion targetRotation = Quaternion.Euler(CustomerObject.transform.rotation.eulerAngles.x, 0, CustomerObject.transform.rotation.eulerAngles.z);
            CustomerObject.transform.rotation = Quaternion.Slerp(CustomerObject.transform.rotation, targetRotation, CustomerBot.rotationSpeed * Time.deltaTime);
            yield return null;
        }
        CustomerObject.transform.rotation = Quaternion.Euler(CustomerObject.transform.rotation.eulerAngles.x, 0, CustomerObject.transform.rotation.eulerAngles.z);
    }

    private IEnumerator NextSection()
    {
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(SearchForShelf());
    }

    private void RotateTowardsDirection(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - transform.position;
        direction.y = 0;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);
        }
    }
}
