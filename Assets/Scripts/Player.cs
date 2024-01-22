using PGGE.Patterns;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [HideInInspector]
    public FSM mFsm = new FSM();
    public Animator mAnimator;
    public PlayerMovement mPlayerMovement;

    // This is the maximum number of bullets that the player 
    // needs to fire before reloading.
    public int mMaxAmunitionBeforeReload = 40;

    // This is the total number of bullets that the 
    // player has.
    [HideInInspector]
    public int mAmunitionCount = 100;

    // This is the count of bullets in the magazine.
    [HideInInspector]
    public int mBulletsInMagazine = 40;

    [HideInInspector]
    public bool[] mAttackButtons = new bool[3];

    public Transform mGunTransform;
    public LayerMask mPlayerMask;
    public Canvas mCanvas;
    public RectTransform mCrossHair;


    public GameObject mBulletPrefab;
    public float mBulletSpeed = 10.0f;

    public int[] RoundsPerSecond = new int[3];
    bool[] mFiring = new bool[3];


    // Start is called before the first frame update
    void Start()
    {
        mFsm.Add(new PlayerState_MOVEMENT(this));
        mFsm.Add(new PlayerState_ATTACK(this));
        mFsm.Add(new PlayerState_RELOAD(this));
        mFsm.SetCurrentState((int)PlayerStateType.MOVEMENT);
    }

    void Update()
    {
        mFsm.Update();
        Aim();

        // For Student ----------------------------------------------------//
        // Implement the logic of button clicks for shooting. 
        //-----------------------------------------------------------------//

        //like the original code,
        //ensure that the fire3 btn is prioritised when determining
        //the final update of the values of the array for which mouse button is clicked

        //argument of HandleFireBtnInput changes based on the button that is pressed
        if (Input.GetButton("Fire3"))
        {
            HandleFireBtnInput(FiringState.Fire3);
        }
        else if (Input.GetButton("Fire2"))
        {
            HandleFireBtnInput(FiringState.Fire2);
        }
        else if (Input.GetButton("Fire1"))
        {
            HandleFireBtnInput(FiringState.Fire1);
        }
        else
        {
            HandleFireBtnInput(FiringState.NoFire);
        }

    }
    //change the values of the attack button array based on the firebutton pressed
    private void HandleFireBtnInput(FiringState firingState)
    {
        //reset values in mAttackButtons array
        mAttackButtons[0] = false;
        mAttackButtons[1] = false;
        mAttackButtons[2] = false;

        //reaasign a true value to an element in mAttackButtons depending on button pressed
        switch (firingState)
        {
            case FiringState.NoFire:
                break;
            case FiringState.Fire1:
                mAttackButtons[0] = true;
                break;
            case FiringState.Fire2:
                mAttackButtons[1] = true;
                break;
            case FiringState.Fire3:
                mAttackButtons[2] = true;
                break;
        }
    }

    private enum FiringState
    {
        NoFire,
        Fire1,
        Fire2,
        Fire3
    }

    public void Aim()
    {
        // For Student ----------------------------------------------------------//
        // Implement the logic of aiming and showing the crosshair
        // if there is an intersection.
        //
        // Hints:
        // Find the direction of fire.
        // Find gunpoint as mentioned in the worksheet.
        // Find the layer mask for objects that you want to intersect with.
        //
        // Do the Raycast
        // if (intersected)
        // {
        //     // Draw a line as debug to show the aim of fire in scene view.
        //     // Find the transformed intersected point to screenspace
        //     // and then transform the crosshair position to this
        //     // new position.
        //     // Enable or set active the crosshair gameobject.
        // }
        // else
        // {
        //     // Hide or set inactive the crosshair gameobject.
        // }
        //-----------------------------------------------------------------------//

        Vector3 dir = -mGunTransform.right.normalized;
        // Find gunpoint as mentioned in the worksheet.
        Vector3 gunpoint = mGunTransform.transform.position +
                           dir * 1.2f -
                           mGunTransform.forward * 0.1f;
        // Fine the layer mask for objects that you want to intersect with.
        LayerMask objectsMask = ~mPlayerMask;

        // Do the Raycast
        RaycastHit hit;
        bool flag = Physics.Raycast(gunpoint, dir,
                        out hit, 50.0f, objectsMask);
        if (flag)
        {
            // Draw a line as debug to show the aim of fire in scene view.
            Debug.DrawLine(gunpoint, gunpoint +
                (dir * hit.distance), Color.red, 0.0f);

            // Find the transformed intersected point to screenspace
            // and then transform the crosshair position to this
            // new position.
            // first you need the RectTransform component of your mCanvas
            RectTransform CanvasRect = mCanvas.GetComponent<RectTransform>();

            // then you calculate the position of the UI element.
            // Remember that 0,0 for the mCanvas is at the centre of the screen. 
            // But WorldToViewPortPoint treats the lower left corner as 0,0. 
            // Because of this, you need to subtract the height / width 
            // of the mCanvas * 0.5 to get the correct position.

            Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(hit.point);
            Vector2 WorldObject_ScreenPosition = new Vector2(
            ((ViewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
            ((ViewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));

            //now you can set the position of the UI element
            mCrossHair.anchoredPosition = WorldObject_ScreenPosition;


            // Enable or set active the crosshair gameobject.
            mCrossHair.gameObject.SetActive(true);
        }
        else
        {
            // Hide or set inactive the crosshair gameobject.
            mCrossHair.gameObject.SetActive(false);
        }
    }

    public void Move()
    {
        mPlayerMovement.HandleInputs();
        mPlayerMovement.Move();
    }

    public void NoAmmo()
    {

    }

    public void Reload()
    {

    }

    public void Fire(int id)
    {
        if (mFiring[id] == false)
        {
            StartCoroutine(Coroutine_Firing(id));
        }
    }

    public void FireBullet()
    {
        if (mBulletPrefab == null) return;

        Vector3 dir = -mGunTransform.right.normalized;
        Vector3 firePoint = mGunTransform.transform.position + dir *
            1.2f - mGunTransform.forward * 0.1f;
        GameObject bullet = Instantiate(mBulletPrefab, firePoint,
            Quaternion.LookRotation(dir) * Quaternion.AngleAxis(90.0f, Vector3.right));

        bullet.GetComponent<Rigidbody>().AddForce(dir * mBulletSpeed, ForceMode.Impulse);
    }

    IEnumerator Coroutine_Firing(int id)
    {
        mFiring[id] = true;
        FireBullet();
        yield return new WaitForSeconds(1.0f / RoundsPerSecond[id]);
        mFiring[id] = false;
        mBulletsInMagazine -= 1;
    }
}
