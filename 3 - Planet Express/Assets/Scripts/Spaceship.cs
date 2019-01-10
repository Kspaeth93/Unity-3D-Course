using UnityEngine;
using UnityEngine.SceneManagement;

public class Spaceship : MonoBehaviour {

    [SerializeField]
    private float rcsThrust = 250f;
    [SerializeField]
    private float mainThrust = 1500f;

    private Rigidbody rigidBody;
    private AudioSource[] audioSources;
    private enum GameState { Playing, Dying, Winning };
    private GameState gameState;

	private void Start ()
    {
        gameState = GameState.Playing;
        rigidBody = GetComponent<Rigidbody>();
        audioSources = GetComponents<AudioSource>();
	}
	
	private void Update ()
    {
        if (gameState == GameState.Playing)
        {
            ApplyThrust();
            RotateShip();
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
                    gameState = GameState.Winning;
                    PlayLevelCompletedSound();
                    Invoke("PlayNextLevel", 5f);
                    break;
                default:
                    gameState = GameState.Dying;
                    PlayDeathSound();
                    Invoke("RestartCurrentLevel", 1f);
                    break;
            }
        }
    }

    private void RotateShip()
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

    private void PlayLevelCompletedSound()
    {
        if (!audioSources[2].isPlaying)
        {
            audioSources[2].Play();
        }
    }

    private void PlayNextLevel()
    {
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 0:
                SceneManager.LoadScene(1);
                break;
            default:
                break;
        }
    }

    private void PlayDeathSound()
    {
        if (!audioSources[1].isPlaying)
        {
            audioSources[1].Play();
        }
    }

    private void RestartCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void ApplyThrust()
    {
        float frameThrust = mainThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * frameThrust);

            if (!audioSources[0].isPlaying)
            {
                audioSources[0].Play();
            }
        }
        else
        {
            audioSources[0].Stop();
        }
    }

}
