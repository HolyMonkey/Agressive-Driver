using UnityEngine;
using System.Collections.Generic;
using System.Text;

public class PlayerMover : MonoBehaviour
{
    public bool activeControl = false;

    // Wheels Setting /////////////////////////////////

    public CarWheels carWheels;

    [System.Serializable]
    public class CarWheels
    {
        public ConnectWheel wheels;
        public WheelSetting setting;
    }

    [System.Serializable]
    public class ConnectWheel
    {
        public bool frontWheelDrive = true;
        public Transform frontRight;
        public Transform frontLeft;

        public bool backWheelDrive = true;
        public Transform backRight;
        public Transform backLeft;
    }

    [System.Serializable]
    public class WheelSetting
    {
        public float Radius = 0.4f;
        public float Weight = 1000.0f;
        public float Distance = 0.2f;
    }

    // Lights Setting ////////////////////////////////

    public CarLights carLights;

    [System.Serializable]
    public class CarLights
    {
        public Light[] brakeLights;
    }

    // Car sounds /////////////////////////////////

    public CarSounds carSounds;

    [System.Serializable]
    public class CarSounds
    {
        public AudioSource IdleEngine, LowEngine, HighEngine;
        public AudioSource nitro;
        public AudioSource switchGear;
    }

    // Car Particle /////////////////////////////////

    public CarParticles carParticles;

    [System.Serializable]
    public class CarParticles
    {
        public GameObject brakeParticlePerfab;
        public ParticleSystem shiftParticle1, shiftParticle2;
    }

    // Car Engine Setting /////////////////////////////////

    public CarSetting carSetting;

    [System.Serializable]
    public class CarSetting
    {
        public Transform carSteer;
        public List<Transform> cameraSwitchView;
        public Vector3 shiftCentre = new Vector3(0.0f, -0.8f, 0.0f);
        public bool showNormalGizmos = false;
        public bool automaticGear = true;
        public float springs = 25000.0f;
        public float dampers = 1500.0f;
        public float carPower = 120f;
        public float shiftPower = 150f;
        public float brakePower = 8000f;
        public float maxSteerAngle = 25.0f;
        public float shiftDownRPM = 1500.0f;
        public float shiftUpRPM = 2500.0f;
        public float idleRPM = 500.0f;
        public float stiffness = 2.0f;
        public float[] gears = { -10f, 9f, 6f, 4.5f, 3f, 2.5f };
        public float LimitForwardSpeed = 220.0f;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private float steer = 0;
    private float accel = 0.0f;
    [HideInInspector] public bool brake;

    private bool shifmotor;

    [HideInInspector] public float curTorque = 100f;
    [HideInInspector] public float powerShift = 100;
    private float torque = 100f;
    [HideInInspector] public float Speed = 0.0f;
    private float lastSpeed = -10.0f;
    private bool shifting = false;

    private float[] efficiencyTable =
    {
        0.6f, 0.65f, 0.7f, 0.75f, 0.8f, 0.85f, 0.9f, 1.0f, 1.0f, 0.95f, 0.80f, 0.70f, 0.60f, 0.5f, 0.45f, 0.40f, 0.36f,
        0.33f, 0.30f, 0.20f, 0.10f, 0.05f
    };

    private float efficiencyTableStep = 250.0f;
    private float Pitch;
    private float PitchDelay;
    private float shiftTime = 0.0f;
    private float shiftDelay = 0.0f;
    [HideInInspector] public int currentGear = 0;
    [HideInInspector] public bool NeutralGear = true;
    [HideInInspector] public float motorRPM = 0.0f;
    [HideInInspector] public bool Backward = false;
    private float wantedRPM = 0.0f;
    private float w_rotate;
    private GameObject[] Particle = new GameObject[4];
    private Vector3 steerCurAngle;
    private Rigidbody myRigidbody;
    private WheelComponent[] wheels;

    private Transform _transform;
    private Vector3 _forward;
    private Vector3 _down;
    private Vector3 _up;
    private Quaternion _identity;
    private StringBuilder _wheelParticleText = new StringBuilder("WheelParticle");
    private StringBuilder _wheelCollider = new StringBuilder("WheelCollider");
    private Color _gizmosColor = new Color(1, 0, 0, 0.5f);
    private Vector3 _cubeSize = new Vector3(2.5f, 2.0f, 6);
    private Rigidbody _rigidBody;
    private KeyCode _w;
    private KeyCode _upArrow;
    private AudioRolloffMode _custom;
    private float _rad2Deg;
    private AudioSource _audioSource;

    public Rigidbody PlayerRigidbody => myRigidbody;

    private class WheelComponent
    {
        public Transform wheel;
        public WheelCollider collider;
        public Vector3 startPos;
        public float rotation = 0.0f;
        public float rotation2 = 0.0f;
        public float maxSteer;
        public bool drive;
        public float pos_y = 0.0f;
    }

    private WheelComponent SetWheelComponent(Transform wheel, float maxSteer, bool drive, float pos_y)
    {
        WheelComponent result = new WheelComponent();
        GameObject wheelCol = new GameObject(wheel.name + _wheelCollider);

        wheelCol.transform.parent = transform;
        wheelCol.transform.position = wheel.position;
        wheelCol.transform.eulerAngles = transform.eulerAngles;
        pos_y = wheelCol.transform.localPosition.y;
        WheelCollider col = (WheelCollider)wheelCol.AddComponent(typeof(WheelCollider));
        result.wheel = wheel;
        result.collider = wheelCol.GetComponent<WheelCollider>();
        result.drive = drive;
        result.pos_y = pos_y;
        result.maxSteer = maxSteer;
        result.startPos = wheelCol.transform.localPosition;
        return result;
    }

    private void Awake()
    {
        _rad2Deg = Mathf.Rad2Deg;
        _custom = AudioRolloffMode.Custom;
        _w = KeyCode.W;
        _upArrow = KeyCode.UpArrow;
        _transform = GetComponent<Transform>();
        _forward = Vector3.forward;
        _identity = Quaternion.identity;
        _down = Vector3.down;
        _up = Vector3.up;

        _limitSpeed = carSetting.LimitForwardSpeed;
        _player = GetComponent<Player>();
        if (carSetting.automaticGear) NeutralGear = false;
        myRigidbody = _transform.GetComponent<Rigidbody>();
        wheels = new WheelComponent[4];
        wheels[0] = SetWheelComponent(carWheels.wheels.frontRight, carSetting.maxSteerAngle,
            carWheels.wheels.frontWheelDrive, carWheels.wheels.frontRight.position.y);
        wheels[1] = SetWheelComponent(carWheels.wheels.frontLeft, carSetting.maxSteerAngle,
            carWheels.wheels.frontWheelDrive, carWheels.wheels.frontLeft.position.y);
        wheels[2] = SetWheelComponent(carWheels.wheels.backRight, 0, carWheels.wheels.backWheelDrive,
            carWheels.wheels.backRight.position.y);
        wheels[3] = SetWheelComponent(carWheels.wheels.backLeft, 0, carWheels.wheels.backWheelDrive,
            carWheels.wheels.backLeft.position.y);
        if (carSetting.carSteer)
            steerCurAngle = carSetting.carSteer.localEulerAngles;

        for (int i = 0; i < wheels.Length; i++)
        {
            WheelCollider col = wheels[i].collider;
            col.suspensionDistance = carWheels.setting.Distance;
            JointSpring js = col.suspensionSpring;
            js.spring = carSetting.springs;
            js.damper = carSetting.dampers;
            col.suspensionSpring = js;
            col.radius = carWheels.setting.Radius;
            col.mass = carWheels.setting.Weight;
            WheelFrictionCurve fc = col.forwardFriction;
            fc.asymptoteValue = 5000.0f;
            fc.extremumSlip = 2.0f;
            fc.asymptoteSlip = 20.0f;
            fc.stiffness = carSetting.stiffness;
            col.forwardFriction = fc;
            fc = col.sidewaysFriction;
            fc.asymptoteValue = 7500.0f;
            fc.asymptoteSlip = 2.0f;
            fc.stiffness = carSetting.stiffness;
            col.sidewaysFriction = fc;
        }
    }

    public void SetCarSteer(Transform newCarSetting)
    {
        carSetting.carSteer = newCarSetting.transform;
    }

    private void ShiftUp()
    {
        float now = Time.timeSinceLevelLoad;
        if (now < shiftDelay) return;
        if (currentGear < carSetting.gears.Length - 1)
        {
            if (!carSetting.automaticGear)
            {
                if (currentGear == 0)
                {
                    if (NeutralGear)
                    {
                        currentGear++;
                        NeutralGear = false;
                    }
                    else
                    {
                        NeutralGear = true;
                    }
                }
                else
                {
                    currentGear++;
                }
            }
            else
            {
                currentGear++;
            }

            shiftDelay = now + 1.0f;
            shiftTime = 1.5f;
        }
    }

    private void ShiftDown()
    {
        float now = Time.timeSinceLevelLoad;
        if (now < shiftDelay) return;
        if (currentGear > 0 || NeutralGear)
        {
            if (!carSetting.automaticGear)
            {
                if (currentGear == 1)
                {
                    if (!NeutralGear)
                    {
                        currentGear--;
                        NeutralGear = true;
                    }
                }
                else if (currentGear == 0)
                {
                    NeutralGear = false;
                }
                else
                {
                    currentGear--;
                }
            }
            else
            {
                currentGear--;
            }

            shiftDelay = now + 0.1f;
            shiftTime = 2.0f;
        }
    }

    public void PressingBrake(bool isPress)
    {
        brake = isPress;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.root.GetComponent<PlayerMover>())
        {
            myRigidbody.angularVelocity = new Vector3(-myRigidbody.angularVelocity.x * 0.5f,
                myRigidbody.angularVelocity.y * 0.5f,
                -myRigidbody.angularVelocity.z * 0.5f);
            myRigidbody.velocity =
                new Vector3(myRigidbody.velocity.x, myRigidbody.velocity.y * 0.5f, myRigidbody.velocity.z);
        }
    }

    [SerializeField] private float _minSpeed;
    [SerializeField] private float _stopDistance;
    [SerializeField] private float _pursuitDistance;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private SkidmarksEffect _skidmarksEffect;

    private bool _isPlayerDied;
    private Player _player;
    private Vector3 _targetPosition;
    private bool _isSkidmarks;
    private float _limitSpeed;
    public bool IsSkidmarks => _isSkidmarks;
    public bool IsPlayerDied => _isPlayerDied;

    private void OnEnable()
    {
        _player.PlayerDied += OnPlayerDied;
        _player.DelayRevived += OnPlayerDelayRevived;
    }

    private void OnDisable()
    {
        _player.PlayerDied -= OnPlayerDied;
        _player.DelayRevived -= OnPlayerDelayRevived;
    }

    private void OnPlayerDelayRevived()
    {
        _isPlayerDied = false;
    }

    private void OnPlayerDied()
    {
        _isPlayerDied = true;
    }

    public void SetInputs(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
    }

    public void SetIsPlayerDied()
    {
        _isPlayerDied = false;
    }

    private void FixedUpdate()
    {
        Speed = myRigidbody.velocity.magnitude;
        lastSpeed = Speed;
        myRigidbody.centerOfMass = carSetting.shiftCentre;

        if (activeControl)
        {
            if (carWheels.wheels.frontWheelDrive || carWheels.wheels.backWheelDrive)
            {
                Vector3 localTarget = _transform.InverseTransformPoint(_targetPosition);
                float targetAngle = Mathf.Atan2(localTarget.x, localTarget.z) * _rad2Deg;
                steer = Mathf.Clamp(targetAngle * 0.05f, -1, 1) * 1.5f;
                if (Input.GetKey(_w) || Input.GetKey(_upArrow) || Input.touchCount > 0)
                {
                    accel = 1f;
                }
                else if (Speed > _minSpeed)
                {
                    accel = -1f;
                }
                else
                {
                    accel = 0f;
                }
            }
            RaycastHit raycastHit;

            if (Physics.Raycast(new Vector3(_transform.position.x, _transform.position.y + 0.3f, _transform.position.z),
                transform.TransformDirection(_forward), out raycastHit, _layerMask) && _targetPosition.x < 1f)
            {
                if (raycastHit.distance <= _stopDistance)
                {
                    if (Speed > _minSpeed)
                    {
                        accel = -1f;
                        _limitSpeed = _minSpeed;
                        _isSkidmarks = true;
                    }
                    else
                    {
                        _stopDistance = _pursuitDistance;
                    }
                }
            }
            else
            {
                _stopDistance = 9f;
                _limitSpeed = carSetting.LimitForwardSpeed;
            }

            if (carSetting.automaticGear && (currentGear == 1) && (accel < 0.0f))
            {
                if (Speed < 5.0f)
                    ShiftDown();
            }
            else if (brake)
            {
                ShiftUp();
            }
            else if (carSetting.automaticGear && (currentGear == 0) && (accel > 0.0f))
            {
                if (Speed < 5.0f)
                    ShiftUp();
            }
            else if (carSetting.automaticGear && (motorRPM > carSetting.shiftUpRPM) && (accel > 0.0f) && Speed > 10.0f && !brake)
            {
                ShiftUp();
            }
            else if (carSetting.automaticGear && (motorRPM < carSetting.shiftDownRPM) && (currentGear > 1))
            {
                ShiftDown();
            }

            if (Speed < 1.0f) Backward = true;

            if (currentGear == 0 && Backward == true)
            {
                if (Speed < carSetting.gears[0] * -10)
                    accel = -accel;
            }
            else
            {
                Backward = false;
            }

            if (_isPlayerDied)
            {
                accel = 0f;
            }

            wantedRPM = (5500.0f * accel) * 0.1f + wantedRPM * 0.9f;

            float rpm = 0.0f;
            int motorizedWheels = 0;
            bool floorContact = false;
            int currentWheel = 0;

            for (int i = 0; i < wheels.Length; i++)
            {
                WheelHit hit;
                WheelCollider col = wheels[i].collider;

                if (wheels[i].drive)
                {
                    if (!NeutralGear)
                    {
                        rpm += col.rpm;
                    }
                    else
                    {
                        rpm += carSetting.idleRPM * accel;
                    }

                    motorizedWheels++;
                }

                if (brake || accel < 0.0f)
                {
                    if (accel < 0.0f || brake && (wheels[i] == wheels[2] || wheels[i] == wheels[3]))
                    {
                        wantedRPM = 0f;
                        if (myRigidbody.velocity.magnitude > 20.2f)
                        {
                            col.brakeTorque = carSetting.brakePower;
                        }
                        else if (myRigidbody.velocity.magnitude >= 19.8f && myRigidbody.velocity.magnitude < 20.1f)
                        {
                            col.brakeTorque = 1000f;
                        }
                        else
                        {
                            col.brakeTorque = 0;
                        }
                        wheels[i].rotation = w_rotate;
                    }
                }
                else
                {
                    col.brakeTorque = accel == 0 || NeutralGear ? col.brakeTorque = 1000 : col.brakeTorque = 0;
                    w_rotate = wheels[i].rotation;
                }

                if (currentGear > 1 && Speed > 0f)
                {
                    if (powerShift == 0)
                    {
                        shifmotor = false;
                    }
                    powerShift = Mathf.MoveTowards(powerShift, 0.0f, Time.deltaTime * 10.0f);
                    carSounds.nitro.volume = Mathf.Lerp(carSounds.nitro.volume, 1.0f, Time.deltaTime * 10.0f);
                    if (!carSounds.nitro.isPlaying)
                    {
                        carSounds.nitro.GetComponent<AudioSource>().Play();
                    }

                    curTorque = powerShift > 0 ? carSetting.shiftPower : carSetting.carPower;
                    carParticles.shiftParticle1.emissionRate = Mathf.Lerp(carParticles.shiftParticle1.emissionRate,
                        powerShift > 0 ? 50 : 0, Time.deltaTime * 10.0f);
                    carParticles.shiftParticle2.emissionRate = Mathf.Lerp(carParticles.shiftParticle2.emissionRate,
                        powerShift > 0 ? 50 : 0, Time.deltaTime * 10.0f);
                }
                else
                {
                    if (powerShift > 20)
                    {
                        shifmotor = true;
                    }

                    carSounds.nitro.volume = Mathf.MoveTowards(carSounds.nitro.volume, 0.0f, Time.deltaTime * 2.0f);
                    if (carSounds.nitro.volume == 0)
                        carSounds.nitro.Stop();

                    powerShift = Mathf.MoveTowards(powerShift, 100.0f, Time.deltaTime * 5.0f);
                    curTorque = carSetting.carPower;
                    carParticles.shiftParticle1.emissionRate = Mathf.Lerp(carParticles.shiftParticle1.emissionRate, 0,
                        Time.deltaTime * 10.0f);
                    carParticles.shiftParticle2.emissionRate = Mathf.Lerp(carParticles.shiftParticle2.emissionRate, 0,
                        Time.deltaTime * 10.0f);
                }


                wheels[i].rotation = Mathf.Repeat(wheels[i].rotation + Time.deltaTime * col.rpm * 360.0f / 60.0f, 360.0f);
                wheels[i].rotation2 = Mathf.Lerp(wheels[i].rotation2, col.steerAngle, 0.1f);
                wheels[i].wheel.localRotation = Quaternion.Euler(wheels[i].rotation, wheels[i].rotation2, 0.0f);

                Vector3 lp = wheels[i].wheel.localPosition;

                if (col.GetGroundHit(out hit))
                {
                    if (carParticles.brakeParticlePerfab)
                    {
                        if (Particle[currentWheel] == null)
                        {
                            Particle[currentWheel] =
                                Instantiate(carParticles.brakeParticlePerfab, wheels[i].wheel.position,
                                    _identity);
                            Particle[currentWheel].name = _wheelParticleText.ToString();
                            Particle[currentWheel].transform.parent = _transform;
                            _audioSource = Particle[currentWheel].AddComponent<AudioSource>();
                            var audio = _audioSource;
                            audio.maxDistance = 50;
                            audio.spatialBlend = 1;
                            audio.dopplerLevel = 5;
                            audio.rolloffMode = _custom;
                        }

                        var pc = Particle[currentWheel].GetComponent<ParticleSystem>();
                        bool WGrounded = false;

                        if (WGrounded && Speed > 5 && !brake)
                        {
                            pc.enableEmission = true;

                            Particle[currentWheel].GetComponent<AudioSource>().volume = 0.5f;

                            if (!Particle[currentWheel].GetComponent<AudioSource>().isPlaying)
                                Particle[currentWheel].GetComponent<AudioSource>().Play();
                        }
                        else if ((brake || accel < 0f || Mathf.Abs(hit.sidewaysSlip) > 0.6f) && Speed > 22)
                        {
                            if ((accel < 0.0f) || ((brake || Mathf.Abs(hit.sidewaysSlip) > 0.6f) &&
                                                   (wheels[i] == wheels[2] || wheels[i] == wheels[3])))
                            {
                                if (!Particle[currentWheel].GetComponent<AudioSource>().isPlaying)
                                    Particle[currentWheel].GetComponent<AudioSource>().Play();
                                pc.enableEmission = true;
                                Particle[currentWheel].GetComponent<AudioSource>().volume = 10;
                            }
                        }
                        else
                        {
                            pc.enableEmission = false;
                            _audioSource.volume = Mathf.Lerp(
                                _audioSource.volume, 0, Time.deltaTime * 10.0f);
                        }
                    }

                    lp.y -= Vector3.Dot(wheels[i].wheel.position - hit.point,
                        _transform.TransformDirection(0, 1, 0) / _transform.lossyScale.x) - (col.radius);
                    lp.y = Mathf.Clamp(lp.y, -10.0f, wheels[i].pos_y);
                    floorContact = floorContact || (wheels[i].drive);
                }

                else
                {
                    if (Particle[currentWheel] != null)
                    {
                        var pc = Particle[currentWheel].GetComponent<ParticleSystem>();
                        pc.enableEmission = false;
                    }

                    lp.y = wheels[i].startPos.y - carWheels.setting.Distance;

                    myRigidbody.AddForce(_down * 5000);
                }

                currentWheel++;
                wheels[i].wheel.localPosition = lp;
            }

            if (motorizedWheels > 1)
            {
                rpm /= motorizedWheels;
            }

            motorRPM = 0.95f * motorRPM + 0.05f * Mathf.Abs(rpm * carSetting.gears[currentGear]);
            if (motorRPM > 5500.0f) motorRPM = 5200.0f;

            int index = (int)(motorRPM / efficiencyTableStep);
            if (index >= efficiencyTable.Length) index = efficiencyTable.Length - 1;
            if (index < 0) index = 0;

            float newTorque = curTorque * carSetting.gears[currentGear] * efficiencyTable[index];

            for (int i = 0; i < wheels.Length; i++)
            {
                WheelCollider col = wheels[i].collider;

                if (wheels[i].drive)
                {
                    if (Mathf.Abs(col.rpm) > Mathf.Abs(wantedRPM))
                    {
                        col.motorTorque = 100f;
                    }
                    else
                    {
                        float curTorqueCol = col.motorTorque;
                        if (!brake && accel != 0 && NeutralGear == false)
                        {
                            if ((Speed < _limitSpeed && currentGear > 0) ||
                                (Speed < _limitSpeed && currentGear == 0))
                            {
                                col.motorTorque = curTorqueCol * 0.9f + newTorque * 1.0f;
                            }
                            else
                            {
                                col.motorTorque = 0;
                            }
                        }
                        else if (brake == true)
                        {
                            col.motorTorque = curTorqueCol * 1.2f + newTorque * 4f;
                            col.motorTorque = 300f;
                        }
                    }
                }
                float SteerAngle = Mathf.Clamp(Speed / carSetting.maxSteerAngle, 1.0f, carSetting.maxSteerAngle);
                col.steerAngle = steer * (wheels[i].maxSteer / SteerAngle);
            }

            Pitch = Mathf.Clamp(1.2f + ((motorRPM - carSetting.idleRPM) / (carSetting.shiftUpRPM - carSetting.idleRPM)),
                1.0f, 10.0f);

            shiftTime = Mathf.MoveTowards(shiftTime, 0.0f, 0.1f);

            if (Pitch == 1)
            {
                carSounds.IdleEngine.volume = Mathf.Lerp(carSounds.IdleEngine.volume, 1.0f, 0.1f);
                carSounds.LowEngine.volume = Mathf.Lerp(carSounds.LowEngine.volume, 0.5f, 0.1f);
                carSounds.HighEngine.volume = Mathf.Lerp(carSounds.HighEngine.volume, 0.0f, 0.1f);
            }
            else
            {
                carSounds.IdleEngine.volume = Mathf.Lerp(carSounds.IdleEngine.volume, 1.8f - Pitch, 0.1f);

                if ((Pitch > PitchDelay || accel > 0) && shiftTime == 0.0f)
                {
                    carSounds.LowEngine.volume = Mathf.Lerp(carSounds.LowEngine.volume, 0.0f, 0.2f);
                    carSounds.HighEngine.volume = Mathf.Lerp(carSounds.HighEngine.volume, 1.0f, 0.1f);
                }
                else
                {
                    carSounds.LowEngine.volume = Mathf.Lerp(carSounds.LowEngine.volume, 0.5f, 0.1f);
                    carSounds.HighEngine.volume = Mathf.Lerp(carSounds.HighEngine.volume, 0.0f, 0.2f);
                }

                carSounds.HighEngine.pitch = Pitch;
                carSounds.LowEngine.pitch = Pitch;

                PitchDelay = Pitch;
            }
        }
    }
}