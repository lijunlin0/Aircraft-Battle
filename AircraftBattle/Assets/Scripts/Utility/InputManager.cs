using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class InputManager : MonoBehaviour
{
    private const int TouchOffsetY = 120;
    private const float MaxJoystickDistance = 150f;
    private const float MinMoveDistance = 50f;

    private bool mIsSwipe = false;
    private bool mIsFingerDown = false;
    private Vector3 mStartPosition;
    private Vector3 mCurrentTouchPosition;
    private Vector3 mLastDirection;

    public Joystick mJoystick;
    public GameObject mJoystickCenter;
    public Vector3 mJoystickBasePosition;

    public void Init(Joystick mJoystick)
    {
        this.mJoystick = mJoystick;
        mJoystickCenter = mJoystick.transform.Find("Center").gameObject;
        mJoystickBasePosition = mJoystick.transform.position;
    }

    public void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        Touch.onFingerDown += HandleFingerDown;
        Touch.onFingerMove += HandleFingerSwipe;
        Touch.onFingerUp += HandleFingerSwipeEnd;
    }

    public void OnDisable()
    {
        Touch.onFingerDown -= HandleFingerDown;
        Touch.onFingerMove -= HandleFingerSwipe;
        Touch.onFingerUp -= HandleFingerSwipeEnd;
    }

    private void HandleFingerDown(Finger finger)
    {
        if (!IsInputAllowed()) return;

        mIsFingerDown = true;
        mCurrentTouchPosition = finger.screenPosition;
        mStartPosition = mCurrentTouchPosition;

        if (mJoystick.gameObject.activeSelf)
        {
            UpdateJoystickPosition(mCurrentTouchPosition);
        }
    }

    private void HandleFingerSwipe(Finger finger)
    {
        if (!IsInputAllowed()) return;

        mCurrentTouchPosition = finger.screenPosition;
        Vector3 swipeDelta = mCurrentTouchPosition - mStartPosition;

        if (swipeDelta != Vector3.zero)
        {
            mLastDirection = swipeDelta.normalized;
            float distance = Mathf.Min(swipeDelta.magnitude, MaxJoystickDistance);

            if (mJoystick.gameObject.activeSelf)
            {
                UpdateJoystickCenterPosition(mLastDirection * distance);
            }
        }

        mIsSwipe = true;
    }

    private void HandleFingerSwipeEnd(Finger finger)
    {
        if (!IsInputAllowed()) return;

        ResetJoystick();
        mIsSwipe = false;
        mIsFingerDown = false;
    }

    private void MovePlayer()
    {
        Player player = Player.GetCurrent();
        if (player == null) return;

        if (MainScene.GetCurrent().GetControlButton().IsTouchControl() && mIsFingerDown)
        {
            HandleTouchControl(player);
        }
        else if (mIsSwipe)
        {
            player.Move(mLastDirection);
        }
    }

    private void HandleTouchControl(Player player)
    {
        Vector3 worldTouchPosition = Camera.main.ScreenToWorldPoint(new Vector3(mCurrentTouchPosition.x, mCurrentTouchPosition.y + TouchOffsetY, 0));
        if (Vector3.Distance(worldTouchPosition, player.transform.position) > MinMoveDistance)
        {
            mLastDirection = (worldTouchPosition - player.transform.position).normalized;
            mLastDirection.z = 0;
            player.Move(mLastDirection);
        }
    }

    private bool IsInputAllowed()
    {
        return mJoystick != null && !FightManager.GetCurrent().IsGameOver();
    }

    private void UpdateJoystickPosition(Vector3 position)
    {
        mJoystick.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(position.x, position.y, 0));
    }

    private void UpdateJoystickCenterPosition(Vector3 position)
    {
        mJoystickCenter.transform.localPosition = position;
    }

    private void ResetJoystick()
    {
        mJoystickCenter.transform.localPosition = Vector3.zero;
        mJoystick.transform.position = mJoystickBasePosition;
    }

    public void Update()
    {
        if (!FightManager.GetCurrent().IsGameOver())
        {
            MovePlayer();
        }
    }
}
