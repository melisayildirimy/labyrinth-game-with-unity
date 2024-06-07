using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    // Oyuncunun hareket h�zlar� ve di�er ayarlar�
    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    public float crouchingSpeed = 3.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Camera playerCamera;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;
    bool isCrouching = false;
    float originalHeight;

    [HideInInspector]
    public bool canMove = true;

    // Stamina ile ilgili de�i�kenler
    public float stamina = 1.0f;
    public float maxStamina = 1.0f;
    public float staminaRecoveryRateWalk = 0.10f;
    public float staminaRecoveryRateOther = 0.15f;
    public float staminaConsumptionRate = 0.20f;
    public float sprintTime = 10.0f;
    public float crouchTime = 10.0f;
    public bool isSprinting = false;
    public bool isCrouchKey = false;
    public float sprintTimer = 0.0f;
    public float crouchTimer = 0.0f;

    public Slider staminaSlider; // Stamina g�rselle�tirmesi i�in UI Slider bile�eni

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        originalHeight = characterController.height;

        // Slider'�n ba�lang�� de�erini ayarla
        staminaSlider.minValue = 0.0f;
        staminaSlider.maxValue = 1.0f;
        staminaSlider.value = 1.0f;
    }

    void Update()
    {
        // Karakterin ileri ve yan hareket vekt�rleri hesaplan�r
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        // E�ilme tu�una bas�l�p bas�lmad���n� ve ko�ma tu�una bas�l�p bas�lmad���n� kontrol eder
        isCrouchKey = Input.GetKeyDown(KeyCode.LeftControl);
        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        // X ve Y eksenlerindeki hareket h�zlar� hesaplan�r
        float curSpeedX = canMove ? (isRunning ? runningSpeed : (isCrouching ? crouchingSpeed : walkingSpeed)) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runningSpeed : (isCrouching ? crouchingSpeed : walkingSpeed)) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        // Z�plama tu�una bas�l�p bas�lmad���n� kontrol eder ve karakter yerdeyse z�plat�r
        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpSpeed;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        // Karakter yerde de�ilse yer�ekimi etkisini uygular
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // Karakterin hareketi g�ncellenir
        characterController.Move(moveDirection * Time.deltaTime);

        // Kamera hareketi ve d�n��� kontrol edilir
        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }

        // E�ilme tu�una bas�l�p bas�lmad���n� kontrol eder
        if (isCrouchKey)
        {
            isCrouching = !isCrouching;
            crouchTimer = isCrouching ? crouchTime : 0.0f;

            // E�ilme durumuna g�re karakter y�ksekli�i ayarlan�r
            if (isCrouching)
            {
                characterController.height = originalHeight / 2;
            }
            else
            {
                characterController.height = originalHeight;
            }
        }

        // Ko�ma tu�una bas�l�p bas�lmad���n� ve hareket tu�lar�na bas�l�p bas�lmad���n� kontrol eder
        if (isRunning && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)))
        {
            // Stamina'n�n azalmas� ve s�re hesaplamas�
            if (stamina >= staminaConsumptionRate * Time.deltaTime)
            {
                isSprinting = true;
                stamina -= staminaConsumptionRate * Time.deltaTime;
                sprintTimer += Time.deltaTime;

                if (sprintTimer >= sprintTime)
                {
                    isSprinting = false;
                    sprintTimer = 0.0f;
                }
            }
            else
            {
                isSprinting = false;
            }
        }
        else
        {
            isSprinting = false;
            sprintTimer = 0.0f;
        }
        if (stamina <= 0.3f * maxStamina)
        {
            runningSpeed = 3.0f;
            walkingSpeed = 3.0f;

        }
        else
        {
            // Kondisyon %30'un �st�ndeyse normal h�z kullan
            walkingSpeed = 7.5f;
            runningSpeed = 11.5f;
        }


        // Stamina yeniden kazan�m�
        if (!isSprinting && !isRunning && isCrouching && stamina < maxStamina)
        {
            stamina += staminaRecoveryRateOther * Time.deltaTime;
        }
        if (!isSprinting && !isRunning && !isCrouching && stamina < maxStamina)
        {
            stamina += staminaRecoveryRateWalk * Time.deltaTime;
        }


        // Stamina de�eri s�n�rlan�r
        stamina = Mathf.Clamp(stamina, 0.0f, maxStamina);

        // Stamina de�eri Slider'a yans�t�l�r
        staminaSlider.value = stamina / maxStamina;
    }
}
