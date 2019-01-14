using UnityEngine;
using UnityEngine.SceneManagement;

public class Spaceship : MonoBehaviour {

    [SerializeField] private float rcsThrust = 250f;
    [SerializeField] private float mainThrust = 1500f;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip winSound;
    [SerializeField] private AudioClip thrustSound;
    [SerializeField] private ParticleSystem deathParticles;
    [SerializeField] private ParticleSystem winParticles;
    [SerializeField] private ParticleSystem thrustParticles;

    private Rigidbody rigidBody;
    private AudioSource audioSource;
    private enum GameState { Playing, Dying, Winning };
    private GameState gameState;

	private void Start ()
    {
        gameState = GameState.Playing;
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
	}
	
	private void Update ()
    {
        if (gameState == GameState.Playing)
        {
            ApplyMainThrust();
            ApplyRCSThrust();
        }
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (gameState == GameState.Playing)
        {
            switch (collision.gameObject.tag)
            {
                case "Friendly":
                    break;
                case "Goal":
                    ProcessWinSequence();
                    break;
                default:
                    ProcessDeathSequence();
                    break;
            }
        }
    }

    private void ProcessWinSequence()
    {
        gameState = GameState.Winning;
        PlayWinSound();
        winParticles.Play();
        thrustParticles.Stop();
        Invoke("PlayNextLevel", 5f);
    }

    private void ProcessDeathSequence()
    {
        gameState = GameState.Dying;
        PlayDeathSound();
        deathParticles.Play();
        thrustParticles.Stop();
        Invoke("RestartCurrentLevel", 1f);
    }

    private void ApplyMainThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
            PlayThrustSound();
            thrustParticles.Play();
        }
        else
        {
            audioSource.Stop();
            thrustParticles.Stop();
        }
    }

    private void ApplyRCSThrust()
    {
        float frameRotation = rcsThrust * Time.deltaTime;
        rigidBody.freezeRotation = true; // Take manual control of rotation

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(Vector3.forward * frameRotation);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(-Vector3.forward * frameRotation);
        }

        rigidBody.freezeRotation = false; // Resume physics control of rotation
    }

    private void PlayNextLevel()
    {
        int currentBuildIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentBuildIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(currentBuildIndex + 1);
        }
    }

    private void RestartCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void PlayWinSound()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(winSound);
    }

    private void PlayDeathSound()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(deathSound);
    }

    private void PlayThrustSound()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(thrustSound);
        }
    }

}
