using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Class <c>SlingShotHandler</c> Handles SlingShot
/// </summary>
public class SlingShotHandler : MonoBehaviour
{
    #region Variables

    [Header("Line Renders")]
    [SerializeField] private LineRenderer _leftLineRenderer;
    [SerializeField] private LineRenderer _rightLineRenderer;

    [Header("Transform References")]
    [SerializeField] private Transform _leftStartPosition;
    [SerializeField] private Transform _rightStartPosition;
    [SerializeField] private Transform _centerPosition;
    [SerializeField] private Transform _idlePosition;

    [Header("SlingShot Stats")]
    [SerializeField] private float _maxDistance = 3.5f;
    [SerializeField] private float _shotForce = 5f;
    [SerializeField] private float _timeBetweenBirdRespawns = 2f;

    [Header("Scripts")]
    [SerializeField] private SlingShotArea _slingShotArea;

    [Header("Bird")]
    [SerializeField] private AngryBird _angryBirdPrefab;
    [SerializeField] private float _angryBirdOffsetPosition = .275f;
    private AngryBird _angryBirdObject;

    private Vector2 _slingShotLinesPosition;
    private Vector2 _direction;
    private Vector2 _directionNormalized;

    private bool _clickedWithinArea;
    private bool _birdOnSlingShot;
    #endregion

    #region Event Functions

    /// <summary>
    /// Awake()
    /// </summary>
    private void Awake()
    {
        _leftLineRenderer.enabled = false;
        _rightLineRenderer.enabled = false;

        SpawnAngryBird();
    }

    /// <summary>
    /// Update()
    /// </summary>
    private void Update()
    {
        //if (Mouse.current.leftButton.wasPressedThisFrame && _slingShotArea.IsWithinSlingShotArea())
        if (InputManager.WasPrimaryPressed && _slingShotArea.IsWithinSlingShotArea())
        {
            _clickedWithinArea = true;
        }

        //if (Mouse.current.leftButton.isPressed && _clickedWithinArea && _birdOnSlingShot)
        if (InputManager.IsPrimaryPressed && _clickedWithinArea && _birdOnSlingShot)
        {
            DrawSlingShot();
            PositionAndRotateAngryBird();
        }

        //if (Mouse.current.leftButton.wasReleasedThisFrame && _birdOnSlingShot)
        if (InputManager.WasPrimaryReleased && _birdOnSlingShot)
        {
            // check if there are any birds left
            if (GameManager.Instance.HasEnoughShots())
            {
                _clickedWithinArea = false;
                _birdOnSlingShot = false;

                _angryBirdObject.LaunchBird(_direction, _shotForce);                
                GameManager.Instance.UseShot(); // use up a bird
                SetLines(_centerPosition.position); // move lines to center of slingshot position after shooting bird

                if (GameManager.Instance.HasEnoughShots())
                { 
                    // start coroutine to spawn a new angry bird
                    StartCoroutine(SpawnAngryBirdAfterTime());
                }
            }
        }
    }

    // slow time to 5%
    //Time.timeScale = .05f;
    //Debug.Log("mouse leftButton was clicked");
    //Debug.Log(Mouse.current.position.ReadValue());

    #endregion

    #region SlingShot Methods

    private void DrawSlingShot()
    {

        //Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector3 touchPosition = Camera.main.ScreenToWorldPoint(InputManager.MousePosition);

        _slingShotLinesPosition = _centerPosition.position + Vector3.ClampMagnitude(touchPosition - _centerPosition.position, _maxDistance);

        SetLines(_slingShotLinesPosition);

        // gotta cast from vector3 to vector2
        // set up direction
        _direction = (Vector2)_centerPosition.position - _slingShotLinesPosition;
        _directionNormalized = _direction.normalized;
    }

    /// <summary>
    /// Set Lines
    ///  - Sets the middle part of the slingshot elastic
    /// </summary>
    private void SetLines(Vector2 position)
    {
        if (!_leftLineRenderer.enabled && !_rightLineRenderer.enabled)
        {
            _leftLineRenderer.enabled = true;
            _rightLineRenderer.enabled = true;
        }

        _leftLineRenderer.SetPosition(0, position);
        _leftLineRenderer.SetPosition(1, _leftStartPosition.position);

        _rightLineRenderer.SetPosition(0, position);
        _rightLineRenderer.SetPosition(1, _rightStartPosition.position);
    }

    #endregion

    #region Angry Bird Methods

    /// <summary>
    /// Spawn Angry Bird
    /// </summary>
    private void SpawnAngryBird()
    {
        SetLines(_idlePosition.position);

        Vector2 dir = (_centerPosition.position - _idlePosition.position).normalized;
        Vector2 spawnPosition = (Vector2)_idlePosition.position + (dir * _angryBirdOffsetPosition);

        _angryBirdObject = Instantiate(_angryBirdPrefab, spawnPosition, Quaternion.identity);
        _angryBirdObject.transform.position = spawnPosition;

        _birdOnSlingShot = true;
    }

    /// <summary>
    /// Position and Rotate Angry Bird
    /// </summary>
    private void PositionAndRotateAngryBird()
    {
        _angryBirdObject.transform.position = _slingShotLinesPosition + (_directionNormalized * _angryBirdOffsetPosition);
        _angryBirdObject.transform.right = _directionNormalized;
    }

    // coroutine (execut some code after 2s)
    private IEnumerator SpawnAngryBirdAfterTime()
    {
        yield return new WaitForSeconds(_timeBetweenBirdRespawns);

        SpawnAngryBird();
    }
    #endregion
}
