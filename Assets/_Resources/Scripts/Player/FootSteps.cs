using UnityEngine;
using System.Collections;

public class FootSteps : MonoBehaviour
{
    public AudioClip[] concrete;
    public AudioClip[] grass;
    public AudioClip[] wood;
    public AudioClip[] dirt;
    public AudioClip[] metal;

    private float audioStepLengthCrouch = 0.75f;
    private float audioStepLengthWalk = 0.45f;
    private float audioStepLengthRun = 0.25f;
    private float minWalkSpeed = 5f;
    private float maxWalkSpeed = 9.0f;
    private float audioVolumeCrouch = 0.1f;
    private float audioVolumeWalk = 0.2f;
    private float audioVolumeRun = 0.3f;
    private bool step = true;
    public AudioSource soundsGO;
    public CharacterController cc;
    public FPSController fpscontroller;
    private int curMat;

    void OnEnable()
    {
        step = true;
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        float speed = cc.velocity.magnitude;

        Rigidbody body = hit.collider.attachedRigidbody;
        if (body != null && !body.isKinematic && body.mass < 10)
        {
            Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
            body.linearVelocity += pushDir * 5;
        }

        if (fpscontroller.state == 2 || !step) return;

        if (cc.isGrounded && hit.normal.y > 0.3f)
        {
            if (hit.collider.CompareTag("Untagged") || hit.collider.CompareTag("Concrete"))
            {
                if (speed > maxWalkSpeed) StartCoroutine(RunOnConcrete());
                else if (speed < maxWalkSpeed && speed > minWalkSpeed) StartCoroutine(WalkOnConcrete());
                else if (speed < minWalkSpeed && speed > 0.5f) StartCoroutine(CrouchOnConcrete());
				curMat = 0;
            }
            else if (hit.collider.CompareTag("Grass"))
            {
                if (speed > maxWalkSpeed) StartCoroutine(RunOnGrass());
                else if (speed < maxWalkSpeed && speed > minWalkSpeed) StartCoroutine(WalkOnGrass());
                else if (speed < minWalkSpeed && speed > 0.5f) StartCoroutine(CrouchOnGrass());
				curMat = 1;
            }
            else if (hit.collider.CompareTag("Wood"))
            {
                if (speed > maxWalkSpeed) StartCoroutine(RunOnWood());
                else if (speed < maxWalkSpeed && speed > minWalkSpeed) StartCoroutine(WalkOnWood());
                else if (speed < minWalkSpeed && speed > 0.5f) StartCoroutine(CrouchOnWood());
				curMat = 2;
            }
            else if (hit.collider.CompareTag("Dirt"))
            {
                if (speed > maxWalkSpeed) StartCoroutine(RunOnDirt());
                else if (speed < maxWalkSpeed && speed > minWalkSpeed) StartCoroutine(WalkOnDirt());
                else if (speed < minWalkSpeed && speed > 0.5f) StartCoroutine(CrouchOnDirt());
				curMat = 3;
            }
            else if (hit.collider.CompareTag("Metal"))
            {
                if (speed > maxWalkSpeed) StartCoroutine(RunOnMetal());
                else if (speed < maxWalkSpeed && speed > minWalkSpeed) StartCoroutine(WalkOnMetal());
                else if (speed < minWalkSpeed && speed > 0.5f) StartCoroutine(CrouchOnMetal());
				curMat = 4;
            }
        }
    }

    public IEnumerator JumpLand()
    {
        if (!soundsGO.enabled) yield break;

        if (curMat == 0)
        {
            soundsGO.PlayOneShot(concrete[Random.Range(0, concrete.Length)], 0.5f);
            yield return new WaitForSeconds(0.1f);
            soundsGO.PlayOneShot(concrete[Random.Range(0, concrete.Length)], 0.4f);
        }
        else if (curMat == 1)
        {
            soundsGO.PlayOneShot(grass[Random.Range(0, grass.Length)], 0.5f);
            yield return new WaitForSeconds(0.12f);
            soundsGO.PlayOneShot(grass[Random.Range(0, grass.Length)], 0.4f);
        }
        else if (curMat == 2)
        {
            soundsGO.PlayOneShot(wood[Random.Range(0, wood.Length)], 0.5f);
            yield return new WaitForSeconds(0.12f);
            soundsGO.PlayOneShot(wood[Random.Range(0, wood.Length)], 0.4f);
        }
        else if (curMat == 3)
        {
            soundsGO.PlayOneShot(dirt[Random.Range(0, dirt.Length)], 0.5f);
            yield return new WaitForSeconds(0.11f);
            soundsGO.PlayOneShot(dirt[Random.Range(0, dirt.Length)], 0.4f);
        }
        else if (curMat == 4)
        {
            soundsGO.PlayOneShot(metal[Random.Range(0, metal.Length)], 0.5f);
            yield return new WaitForSeconds(0.12f);
            soundsGO.PlayOneShot(metal[Random.Range(0, metal.Length)], 0.4f);
        }
    }
    // Concrete	or Untagged
    IEnumerator CrouchOnConcrete()
    {
        step = false;
        soundsGO.PlayOneShot(concrete[Random.Range(0, concrete.Length)], audioVolumeCrouch);
        yield return new WaitForSeconds(audioStepLengthCrouch);
        step = true;
    }

    IEnumerator WalkOnConcrete()
    {
        step = false;
        soundsGO.PlayOneShot(concrete[Random.Range(0, concrete.Length)], audioVolumeWalk);
        yield return new  WaitForSeconds (audioStepLengthWalk);
        step = true;
    }

    IEnumerator RunOnConcrete()
    {
        step = false;
        soundsGO.PlayOneShot(concrete[Random.Range(0, concrete.Length)], audioVolumeRun);
        yield return new  WaitForSeconds (audioStepLengthRun);
        step = true;
    }

    // Grass
    IEnumerator CrouchOnGrass()
    {
        step = false;
        soundsGO.PlayOneShot(grass[Random.Range(0, grass.Length)], audioVolumeCrouch);
        yield return new  WaitForSeconds (audioStepLengthCrouch);
        step = true;
    }

    IEnumerator WalkOnGrass()
    {
        step = false;
        soundsGO.PlayOneShot(grass[Random.Range(0, grass.Length)], audioVolumeWalk);
        yield return new  WaitForSeconds (audioStepLengthWalk);
        step = true;
    }

    IEnumerator RunOnGrass()
    {
        step = false;
        soundsGO.PlayOneShot(grass[Random.Range(0, grass.Length)], audioVolumeRun);
        yield return new WaitForSeconds (audioStepLengthRun);
        step = true;
    }

    // Wood
    IEnumerator CrouchOnWood()
    {
        step = false;
        soundsGO.PlayOneShot(wood[Random.Range(0, wood.Length)], audioVolumeCrouch);
        yield return new WaitForSeconds (audioStepLengthCrouch);
        step = true;
    }

    IEnumerator WalkOnWood()
    {
        step = false;
        soundsGO.PlayOneShot(wood[Random.Range(0, wood.Length)], audioVolumeWalk);
        yield return new WaitForSeconds (audioStepLengthWalk);
        step = true;
    }

    IEnumerator RunOnWood()
    {
        step = false;
        soundsGO.PlayOneShot(wood[Random.Range(0, wood.Length)], audioVolumeRun);
        yield return new WaitForSeconds (audioStepLengthRun);
        step = true;
    }

    // Dirt
    IEnumerator CrouchOnDirt()
    {
        step = false;
        soundsGO.PlayOneShot(dirt[Random.Range(0, dirt.Length)], audioVolumeCrouch);
        yield return new WaitForSeconds (audioStepLengthCrouch);
        step = true;
    }

    IEnumerator WalkOnDirt()
    {
        step = false;
        soundsGO.PlayOneShot(dirt[Random.Range(0, dirt.Length)], audioVolumeWalk);
        yield return new WaitForSeconds (audioStepLengthWalk);
        step = true;
    }

    IEnumerator RunOnDirt()
    {
        step = false;
        soundsGO.PlayOneShot(dirt[Random.Range(0, dirt.Length)], audioVolumeRun);
        yield return new WaitForSeconds (audioStepLengthRun);
        step = true;
    }

    // Metal
    IEnumerator CrouchOnMetal()
    {
        step = false;
        soundsGO.PlayOneShot(metal[Random.Range(0, metal.Length)], audioVolumeCrouch);
        yield return new WaitForSeconds (audioStepLengthCrouch);
        step = true;
    }

    IEnumerator WalkOnMetal()
    {
        step = false;
        soundsGO.PlayOneShot(metal[Random.Range(0, metal.Length)], audioVolumeWalk);
        yield return new WaitForSeconds (audioStepLengthWalk);
        step = true;
    }

    IEnumerator RunOnMetal()
    {
        step = false;
        soundsGO.PlayOneShot(metal[Random.Range(0, metal.Length)], audioVolumeRun);
        yield return new WaitForSeconds (audioStepLengthRun);
        step = true;
    }
}