using UnityEngine;

public class LookAtAndMoveToTargets : MonoBehaviour
{
    Animator anim;
    public Transform[] targets; // Hedef noktalar dizisi
    public float rotationSpeed = 2f; // D�nme h�z�
    public float moveSpeed = 5f; // �lerleme h�z�
    public float waitTime = 2f; // Hedefe ula�t�ktan sonra beklenilecek s�re

    private int currentTargetIndex = 0; // �u anki hedef index'i
    private float waitTimer = 0f; // Bekleme s�resi sayac�
    private bool isRotating = false; // D�nme durumu kontrol�

    void Start()
    {
        anim = GetComponent<Animator>();

    }

    void Update()
    {
        if (targets.Length == 0)
        {
            Debug.LogError("Hedef nokta bulunamad�.");
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
        // Hedefe do�ru bakma
        Vector3 lookAtPoint = new Vector3(targets[currentTargetIndex].position.x, transform.position.y, targets[currentTargetIndex].position.z);
        Quaternion targetRotation = Quaternion.LookRotation(lookAtPoint - transform.position);

        // Yumu�ak d�n��
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Hedefe do�ru ilerleme
        Vector3 targetDirection = targets[currentTargetIndex].position - transform.position;
        targetDirection.y = 0f; // Y ekseninde y�nlendirme yapma (yatay d�zlemde)

        // Hedefe do�ru ilerleme
        if (targetDirection.magnitude > 0.1f)
        {
            targetDirection.Normalize(); // Y�nlendirme vekt�r�n� normalize etme
            transform.Translate(targetDirection * moveSpeed * Time.deltaTime, Space.World);
            anim.SetBool("isWalking", true);
        }
        else
        {
            // Hedefe ula��ld���nda d�nme durumunu ba�lat
            isRotating = true;
        }
    }

    void RotateToTarget()
    {
        // Bekleme s�resi boyunca d�nmeye devam et
        waitTimer += Time.deltaTime;
        anim.SetBool("isWalking", false);
        if (waitTimer >= waitTime)
        {
            // Bekleme s�resi bitti�inde bir sonraki hedefe ge�i�
            isRotating = false;
            NextTarget();
            waitTimer = 0f; // Bekleme s�resi sayac�n� s�f�rla
        }
    }

    void NextTarget()
    {
        // Bir sonraki hedefe ge�i�
        currentTargetIndex = (currentTargetIndex + 1) % targets.Length;
    }
}
