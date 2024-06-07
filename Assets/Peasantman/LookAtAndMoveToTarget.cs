using UnityEngine;

public class LookAtAndMoveToTargets : MonoBehaviour
{
    Animator anim;
    public Transform[] targets; // Hedef noktalar dizisi
    public float rotationSpeed = 2f; // Dönme hýzý
    public float moveSpeed = 5f; // Ýlerleme hýzý
    public float waitTime = 2f; // Hedefe ulaþtýktan sonra beklenilecek süre

    private int currentTargetIndex = 0; // Þu anki hedef index'i
    private float waitTimer = 0f; // Bekleme süresi sayacý
    private bool isRotating = false; // Dönme durumu kontrolü

    void Start()
    {
        anim = GetComponent<Animator>();

    }

    void Update()
    {
        if (targets.Length == 0)
        {
            Debug.LogError("Hedef nokta bulunamadý.");
            return;
        }

        if (!isRotating)
        {
            MoveToTarget();
        }
        else
        {
            RotateToTarget();
        }
    }

    void MoveToTarget()
    {
        // Hedefe doðru bakma
        Vector3 lookAtPoint = new Vector3(targets[currentTargetIndex].position.x, transform.position.y, targets[currentTargetIndex].position.z);
        Quaternion targetRotation = Quaternion.LookRotation(lookAtPoint - transform.position);

        // Yumuþak dönüþ
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Hedefe doðru ilerleme
        Vector3 targetDirection = targets[currentTargetIndex].position - transform.position;
        targetDirection.y = 0f; // Y ekseninde yönlendirme yapma (yatay düzlemde)

        // Hedefe doðru ilerleme
        if (targetDirection.magnitude > 0.1f)
        {
            targetDirection.Normalize(); // Yönlendirme vektörünü normalize etme
            transform.Translate(targetDirection * moveSpeed * Time.deltaTime, Space.World);
            anim.SetBool("isWalking", true);
        }
        else
        {
            // Hedefe ulaþýldýðýnda dönme durumunu baþlat
            isRotating = true;
        }
    }

    void RotateToTarget()
    {
        // Bekleme süresi boyunca dönmeye devam et
        waitTimer += Time.deltaTime;
        anim.SetBool("isWalking", false);
        if (waitTimer >= waitTime)
        {
            // Bekleme süresi bittiðinde bir sonraki hedefe geçiþ
            isRotating = false;
            NextTarget();
            waitTimer = 0f; // Bekleme süresi sayacýný sýfýrla
        }
    }

    void NextTarget()
    {
        // Bir sonraki hedefe geçiþ
        currentTargetIndex = (currentTargetIndex + 1) % targets.Length;
    }
}
