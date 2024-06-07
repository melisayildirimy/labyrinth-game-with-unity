using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    // Oyuncunun hareket hýzlarý ve diðer ayarlarý
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

    // Stamina ile ilgili deðiþkenler
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

    public Slider staminaSlider; // Stamina görselleþtirmesi için UI Slider bileþeni

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        originalHeight = characterController.height;

        // Slider'ýn baþlangýç deðerini ayarla
        staminaSlider.minValue = 0.0f;
        staminaSlider.maxValue = 1.0f;
        staminaSlider.value = 1.0f;
    }

    void Update()
    {
        // Karakterin ileri ve yan hareket vektörleri hesaplanýr
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        // Eðilme tuþuna basýlýp basýlmadýðýný ve koþma tuþuna basýlýp basýlmadýðýný kontrol eder
        isCrouchKey = Input.GetKeyDown(KeyCode.LeftControl);
        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        // X ve Y eksenlerindeki hareket hýzlarý hesaplanýr
        float curSpeedX = canMove ? (isRunning ? runningSpeed : (isCrouching ? crouchingSpeed : walkingSpeed)) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runningSpeed : (isCrouching ? crouchingSpeed : walkingSpeed)) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        // Zýplama tuþuna basýlýp basýlmadýðýný kontrol eder ve karakter yerdeyse zýplatýr
        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpSpeed;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        // Karakter yerde deðilse yerçekimi etkisini uygular
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // Karakterin hareketi güncellenir
        characterController.Move(moveDirection * Time.deltaTime);

        // Kamera hareketi ve dönüþü kontrol edilir
        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }

        // Eðilme tuþuna basýlýp basýlmadýðýný kontrol eder
        if (isCrouchKey)
        {
            isCrouching = !isCrouching;
            crouchTimer = isCrouching ? crouchTime : 0.0f;

            // Eðilme durumuna göre karakter yüksekliði ayarlanýr
            if (isCrouching)
            {
                characterController.height = originalHeight / 2;
            }
            else
            {
                characterController.height = originalHeight;
            }
        }

        // Koþma tuþuna basýlýp basýlmadýðýný ve hareket tuþlarýna basýlýp basýlmadýðýný kontrol eder
        if (isRunning && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)))
        {
            // Stamina'nýn azalmasý ve süre hesaplamasý
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
            // Kondisyon %30'un üstündeyse normal hýz kullan
            walkingSpeed = 7.5f;
            runningSpeed = 11.5f;
        }


        // Stamina yeniden kazanýmý
        if (!isSprinting && !isRunning && isCrouching && stamina < maxStamina)
        {
            stamina += staminaRecoveryRateOther * Time.deltaTime;
        }
        if (!isSprinting && !isRunning && !isCrouching && stamina < maxStamina)
        {
            stamina += staminaRecoveryRateWalk * Time.deltaTime;
        }


        // Stamina deðeri sýnýrlanýr
        stamina = Mathf.Clamp(stamina, 0.0f, maxStamina);

        // Stamina deðeri Slider'a yansýtýlýr
        staminaSlider.value = stamina / maxStamina;
    }
}
